using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil.PE
{
	// Token: 0x020000D4 RID: 212
	internal sealed class ImageWriter : BinaryStreamWriter
	{
		// Token: 0x06000906 RID: 2310 RVA: 0x0001CBFC File Offset: 0x0001ADFC
		private ImageWriter(ModuleDefinition module, string runtime_version, MetadataBuilder metadata, Disposable<Stream> stream, bool metadataOnly = false)
			: base(stream.value)
		{
			this.module = module;
			this.runtime_version = runtime_version;
			this.text_map = metadata.text_map;
			this.stream = stream;
			this.metadata = metadata;
			if (metadataOnly)
			{
				return;
			}
			this.pe64 = module.Architecture == TargetArchitecture.AMD64 || module.Architecture == TargetArchitecture.IA64 || module.Architecture == TargetArchitecture.ARM64;
			this.has_reloc = module.Architecture == TargetArchitecture.I386;
			this.GetDebugHeader();
			this.GetWin32Resources();
			this.BuildTextMap();
			this.sections = (this.has_reloc ? 2 : 1);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001CCAC File Offset: 0x0001AEAC
		private void GetDebugHeader()
		{
			ISymbolWriter symbol_writer = this.metadata.symbol_writer;
			if (symbol_writer != null)
			{
				this.debug_header = symbol_writer.GetDebugHeader();
			}
			if (this.module.HasDebugHeader)
			{
				if (this.module.GetDebugHeader().GetDeterministicEntry() == null)
				{
					return;
				}
				this.debug_header = this.debug_header.AddDeterministicEntry();
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001CD08 File Offset: 0x0001AF08
		private void GetWin32Resources()
		{
			if (!this.module.HasImage)
			{
				return;
			}
			DataDirectory win32Resources = this.module.Image.Win32Resources;
			uint size = win32Resources.Size;
			if (size > 0U)
			{
				this.win32_resources = this.module.Image.GetReaderAt<uint, ByteBuffer>(win32Resources.VirtualAddress, size, (uint s, BinaryStreamReader reader) => new ByteBuffer(reader.ReadBytes((int)s)));
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001CD7B File Offset: 0x0001AF7B
		public static ImageWriter CreateWriter(ModuleDefinition module, MetadataBuilder metadata, Disposable<Stream> stream)
		{
			ImageWriter imageWriter = new ImageWriter(module, module.runtime_version, metadata, stream, false);
			imageWriter.BuildSections();
			return imageWriter;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001CD94 File Offset: 0x0001AF94
		public static ImageWriter CreateDebugWriter(ModuleDefinition module, MetadataBuilder metadata, Disposable<Stream> stream)
		{
			ImageWriter imageWriter = new ImageWriter(module, "PDB v1.0", metadata, stream, true);
			uint length = metadata.text_map.GetLength();
			imageWriter.text = new Section
			{
				SizeOfRawData = length,
				VirtualSize = length
			};
			return imageWriter;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001CDD4 File Offset: 0x0001AFD4
		private void BuildSections()
		{
			bool flag = this.win32_resources != null;
			if (flag)
			{
				this.sections += 1;
			}
			this.text = this.CreateSection(".text", this.text_map.GetLength(), null);
			Section section = this.text;
			if (flag)
			{
				this.rsrc = this.CreateSection(".rsrc", (uint)this.win32_resources.length, section);
				this.PatchWin32Resources(this.win32_resources);
				section = this.rsrc;
			}
			if (this.has_reloc)
			{
				this.reloc = this.CreateSection(".reloc", 12U, section);
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001CE70 File Offset: 0x0001B070
		private Section CreateSection(string name, uint size, Section previous)
		{
			return new Section
			{
				Name = name,
				VirtualAddress = ((previous != null) ? (previous.VirtualAddress + ImageWriter.Align(previous.VirtualSize, 8192U)) : 8192U),
				VirtualSize = size,
				PointerToRawData = ((previous != null) ? (previous.PointerToRawData + previous.SizeOfRawData) : ImageWriter.Align(this.GetHeaderSize(), 512U)),
				SizeOfRawData = ImageWriter.Align(size, 512U)
			};
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0001CEF0 File Offset: 0x0001B0F0
		private static uint Align(uint value, uint align)
		{
			align -= 1U;
			return (value + align) & ~align;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0001CEFD File Offset: 0x0001B0FD
		private void WriteDOSHeader()
		{
			this.Write(new byte[]
			{
				77, 90, 144, 0, 3, 0, 0, 0, 4, 0,
				0, 0, byte.MaxValue, byte.MaxValue, 0, 0, 184, 0, 0, 0,
				0, 0, 0, 0, 64, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				128, 0, 0, 0, 14, 31, 186, 14, 0, 180,
				9, 205, 33, 184, 1, 76, 205, 33, 84, 104,
				105, 115, 32, 112, 114, 111, 103, 114, 97, 109,
				32, 99, 97, 110, 110, 111, 116, 32, 98, 101,
				32, 114, 117, 110, 32, 105, 110, 32, 68, 79,
				83, 32, 109, 111, 100, 101, 46, 13, 13, 10,
				36, 0, 0, 0, 0, 0, 0, 0
			});
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0001CF1A File Offset: 0x0001B11A
		private ushort SizeOfOptionalHeader()
		{
			return (!this.pe64) ? 224 : 240;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001CF34 File Offset: 0x0001B134
		private void WritePEFileHeader()
		{
			base.WriteUInt32(17744U);
			base.WriteUInt16((ushort)this.module.Architecture);
			base.WriteUInt16(this.sections);
			base.WriteUInt32(this.metadata.timestamp);
			base.WriteUInt32(0U);
			base.WriteUInt32(0U);
			base.WriteUInt16(this.SizeOfOptionalHeader());
			ushort num = (ushort)(2 | ((!this.pe64) ? 256 : 32));
			if (this.module.Kind == ModuleKind.Dll || this.module.Kind == ModuleKind.NetModule)
			{
				num |= 8192;
			}
			base.WriteUInt16(num);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001CFD5 File Offset: 0x0001B1D5
		private Section LastSection()
		{
			if (this.reloc != null)
			{
				return this.reloc;
			}
			if (this.rsrc != null)
			{
				return this.rsrc;
			}
			return this.text;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001CFFC File Offset: 0x0001B1FC
		private void WriteOptionalHeaders()
		{
			base.WriteUInt16((!this.pe64) ? 267 : 523);
			base.WriteUInt16(this.module.linker_version);
			base.WriteUInt32(this.text.SizeOfRawData);
			base.WriteUInt32(((this.reloc != null) ? this.reloc.SizeOfRawData : 0U) + ((this.rsrc != null) ? this.rsrc.SizeOfRawData : 0U));
			base.WriteUInt32(0U);
			Range range = this.text_map.GetRange(TextSegment.StartupStub);
			base.WriteUInt32((range.Length > 0U) ? range.Start : 0U);
			base.WriteUInt32(8192U);
			if (!this.pe64)
			{
				base.WriteUInt32(0U);
				base.WriteUInt32(4194304U);
			}
			else
			{
				base.WriteUInt64(4194304UL);
			}
			base.WriteUInt32(8192U);
			base.WriteUInt32(512U);
			base.WriteUInt16(4);
			base.WriteUInt16(0);
			base.WriteUInt16(0);
			base.WriteUInt16(0);
			base.WriteUInt16(this.module.subsystem_major);
			base.WriteUInt16(this.module.subsystem_minor);
			base.WriteUInt32(0U);
			Section section = this.LastSection();
			base.WriteUInt32(section.VirtualAddress + ImageWriter.Align(section.VirtualSize, 8192U));
			base.WriteUInt32(this.text.PointerToRawData);
			base.WriteUInt32(0U);
			base.WriteUInt16(this.GetSubSystem());
			base.WriteUInt16((ushort)this.module.Characteristics);
			if (!this.pe64)
			{
				base.WriteUInt32(1048576U);
				base.WriteUInt32(4096U);
				base.WriteUInt32(1048576U);
				base.WriteUInt32(4096U);
			}
			else
			{
				base.WriteUInt64(4194304UL);
				base.WriteUInt64(16384UL);
				base.WriteUInt64(1048576UL);
				base.WriteUInt64(8192UL);
			}
			base.WriteUInt32(0U);
			base.WriteUInt32(16U);
			this.WriteZeroDataDirectory();
			base.WriteDataDirectory(this.text_map.GetDataDirectory(TextSegment.ImportDirectory));
			if (this.rsrc != null)
			{
				base.WriteUInt32(this.rsrc.VirtualAddress);
				base.WriteUInt32(this.rsrc.VirtualSize);
			}
			else
			{
				this.WriteZeroDataDirectory();
			}
			this.WriteZeroDataDirectory();
			this.WriteZeroDataDirectory();
			base.WriteUInt32((this.reloc != null) ? this.reloc.VirtualAddress : 0U);
			base.WriteUInt32((this.reloc != null) ? this.reloc.VirtualSize : 0U);
			if (this.text_map.GetLength(TextSegment.DebugDirectory) > 0)
			{
				base.WriteUInt32(this.text_map.GetRVA(TextSegment.DebugDirectory));
				base.WriteUInt32((uint)(this.debug_header.Entries.Length * 28));
			}
			else
			{
				this.WriteZeroDataDirectory();
			}
			this.WriteZeroDataDirectory();
			this.WriteZeroDataDirectory();
			this.WriteZeroDataDirectory();
			this.WriteZeroDataDirectory();
			this.WriteZeroDataDirectory();
			base.WriteDataDirectory(this.text_map.GetDataDirectory(TextSegment.ImportAddressTable));
			this.WriteZeroDataDirectory();
			base.WriteDataDirectory(this.text_map.GetDataDirectory(TextSegment.CLIHeader));
			this.WriteZeroDataDirectory();
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001D321 File Offset: 0x0001B521
		private void WriteZeroDataDirectory()
		{
			base.WriteUInt32(0U);
			base.WriteUInt32(0U);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001D334 File Offset: 0x0001B534
		private ushort GetSubSystem()
		{
			switch (this.module.Kind)
			{
			case ModuleKind.Dll:
			case ModuleKind.Console:
			case ModuleKind.NetModule:
				return 3;
			case ModuleKind.Windows:
				return 2;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001D370 File Offset: 0x0001B570
		private void WriteSectionHeaders()
		{
			this.WriteSection(this.text, 1610612768U);
			if (this.rsrc != null)
			{
				this.WriteSection(this.rsrc, 1073741888U);
			}
			if (this.reloc != null)
			{
				this.WriteSection(this.reloc, 1107296320U);
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001D3C0 File Offset: 0x0001B5C0
		private void WriteSection(Section section, uint characteristics)
		{
			byte[] array = new byte[8];
			string name = section.Name;
			for (int i = 0; i < name.Length; i++)
			{
				array[i] = (byte)name[i];
			}
			base.WriteBytes(array);
			base.WriteUInt32(section.VirtualSize);
			base.WriteUInt32(section.VirtualAddress);
			base.WriteUInt32(section.SizeOfRawData);
			base.WriteUInt32(section.PointerToRawData);
			base.WriteUInt32(0U);
			base.WriteUInt32(0U);
			base.WriteUInt16(0);
			base.WriteUInt16(0);
			base.WriteUInt32(characteristics);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001D451 File Offset: 0x0001B651
		private void MoveTo(uint pointer)
		{
			this.BaseStream.Seek((long)((ulong)pointer), SeekOrigin.Begin);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001D462 File Offset: 0x0001B662
		private void MoveToRVA(Section section, uint rva)
		{
			this.BaseStream.Seek((long)((ulong)(section.PointerToRawData + rva - section.VirtualAddress)), SeekOrigin.Begin);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001D481 File Offset: 0x0001B681
		private void MoveToRVA(TextSegment segment)
		{
			this.MoveToRVA(this.text, this.text_map.GetRVA(segment));
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0001D49B File Offset: 0x0001B69B
		private void WriteRVA(uint rva)
		{
			if (!this.pe64)
			{
				base.WriteUInt32(rva);
				return;
			}
			base.WriteUInt64((ulong)rva);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001D4B8 File Offset: 0x0001B6B8
		private void PrepareSection(Section section)
		{
			this.MoveTo(section.PointerToRawData);
			if (section.SizeOfRawData <= 4096U)
			{
				this.Write(new byte[section.SizeOfRawData]);
				this.MoveTo(section.PointerToRawData);
				return;
			}
			int num = 0;
			byte[] array = new byte[4096];
			while ((long)num != (long)((ulong)section.SizeOfRawData))
			{
				int num2 = Math.Min((int)(section.SizeOfRawData - (uint)num), 4096);
				this.Write(array, 0, num2);
				num += num2;
			}
			this.MoveTo(section.PointerToRawData);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001D544 File Offset: 0x0001B744
		private void WriteText()
		{
			this.PrepareSection(this.text);
			if (this.has_reloc)
			{
				this.WriteRVA(this.text_map.GetRVA(TextSegment.ImportHintNameTable));
				this.WriteRVA(0U);
			}
			base.WriteUInt32(72U);
			base.WriteUInt16(2);
			base.WriteUInt16((this.module.Runtime <= TargetRuntime.Net_1_1) ? 0 : 5);
			base.WriteUInt32(this.text_map.GetRVA(TextSegment.MetadataHeader));
			base.WriteUInt32(this.GetMetadataLength());
			base.WriteUInt32((uint)this.module.Attributes);
			base.WriteUInt32(this.metadata.entry_point.ToUInt32());
			base.WriteDataDirectory(this.text_map.GetDataDirectory(TextSegment.Resources));
			base.WriteDataDirectory(this.text_map.GetDataDirectory(TextSegment.StrongNameSignature));
			this.WriteZeroDataDirectory();
			this.WriteZeroDataDirectory();
			this.WriteZeroDataDirectory();
			this.WriteZeroDataDirectory();
			this.MoveToRVA(TextSegment.Code);
			base.WriteBuffer(this.metadata.code);
			this.MoveToRVA(TextSegment.Resources);
			base.WriteBuffer(this.metadata.resources);
			if (this.metadata.data.length > 0)
			{
				this.MoveToRVA(TextSegment.Data);
				base.WriteBuffer(this.metadata.data);
			}
			this.MoveToRVA(TextSegment.MetadataHeader);
			this.WriteMetadataHeader();
			this.WriteMetadata();
			if (this.text_map.GetLength(TextSegment.DebugDirectory) > 0)
			{
				this.MoveToRVA(TextSegment.DebugDirectory);
				this.WriteDebugDirectory();
			}
			if (!this.has_reloc)
			{
				return;
			}
			this.MoveToRVA(TextSegment.ImportDirectory);
			this.WriteImportDirectory();
			this.MoveToRVA(TextSegment.StartupStub);
			this.WriteStartupStub();
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001D6D9 File Offset: 0x0001B8D9
		private uint GetMetadataLength()
		{
			return this.text_map.GetRVA(TextSegment.DebugDirectory) - this.text_map.GetRVA(TextSegment.MetadataHeader);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0001D6F8 File Offset: 0x0001B8F8
		public void WriteMetadataHeader()
		{
			base.WriteUInt32(1112167234U);
			base.WriteUInt16(1);
			base.WriteUInt16(1);
			base.WriteUInt32(0U);
			byte[] zeroTerminatedString = ImageWriter.GetZeroTerminatedString(this.runtime_version);
			base.WriteUInt32((uint)zeroTerminatedString.Length);
			base.WriteBytes(zeroTerminatedString);
			base.WriteUInt16(0);
			base.WriteUInt16(this.GetStreamCount());
			uint num = this.text_map.GetRVA(TextSegment.TableHeap) - this.text_map.GetRVA(TextSegment.MetadataHeader);
			this.WriteStreamHeader(ref num, TextSegment.TableHeap, "#~");
			this.WriteStreamHeader(ref num, TextSegment.StringHeap, "#Strings");
			this.WriteStreamHeader(ref num, TextSegment.UserStringHeap, "#US");
			this.WriteStreamHeader(ref num, TextSegment.GuidHeap, "#GUID");
			this.WriteStreamHeader(ref num, TextSegment.BlobHeap, "#Blob");
			this.WriteStreamHeader(ref num, TextSegment.PdbHeap, "#Pdb");
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0001D7C8 File Offset: 0x0001B9C8
		private ushort GetStreamCount()
		{
			return (ushort)(2 + (this.metadata.user_string_heap.IsEmpty ? 0 : 1) + (this.metadata.guid_heap.IsEmpty ? 0 : 1) + (this.metadata.blob_heap.IsEmpty ? 0 : 1) + ((this.metadata.pdb_heap == null) ? 0 : 1));
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001D830 File Offset: 0x0001BA30
		private void WriteStreamHeader(ref uint offset, TextSegment heap, string name)
		{
			uint length = (uint)this.text_map.GetLength(heap);
			if (length == 0U)
			{
				return;
			}
			base.WriteUInt32(offset);
			base.WriteUInt32(length);
			base.WriteBytes(ImageWriter.GetZeroTerminatedString(name));
			offset += length;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001D86F File Offset: 0x0001BA6F
		private static int GetZeroTerminatedStringLength(string @string)
		{
			return (@string.Length + 1 + 3) & -4;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001D87E File Offset: 0x0001BA7E
		private static byte[] GetZeroTerminatedString(string @string)
		{
			return ImageWriter.GetString(@string, ImageWriter.GetZeroTerminatedStringLength(@string));
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001D88C File Offset: 0x0001BA8C
		private static byte[] GetSimpleString(string @string)
		{
			return ImageWriter.GetString(@string, @string.Length);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001D89C File Offset: 0x0001BA9C
		private static byte[] GetString(string @string, int length)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < @string.Length; i++)
			{
				array[i] = (byte)@string[i];
			}
			return array;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001D8D0 File Offset: 0x0001BAD0
		public void WriteMetadata()
		{
			this.WriteHeap(TextSegment.TableHeap, this.metadata.table_heap);
			this.WriteHeap(TextSegment.StringHeap, this.metadata.string_heap);
			this.WriteHeap(TextSegment.UserStringHeap, this.metadata.user_string_heap);
			this.WriteHeap(TextSegment.GuidHeap, this.metadata.guid_heap);
			this.WriteHeap(TextSegment.BlobHeap, this.metadata.blob_heap);
			this.WriteHeap(TextSegment.PdbHeap, this.metadata.pdb_heap);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001D94D File Offset: 0x0001BB4D
		private void WriteHeap(TextSegment heap, HeapBuffer buffer)
		{
			if (buffer == null || buffer.IsEmpty)
			{
				return;
			}
			this.MoveToRVA(heap);
			base.WriteBuffer(buffer);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001D96C File Offset: 0x0001BB6C
		private void WriteDebugDirectory()
		{
			int num = (int)this.BaseStream.Position + this.debug_header.Entries.Length * 28;
			for (int i = 0; i < this.debug_header.Entries.Length; i++)
			{
				ImageDebugHeaderEntry imageDebugHeaderEntry = this.debug_header.Entries[i];
				ImageDebugDirectory directory = imageDebugHeaderEntry.Directory;
				base.WriteInt32(directory.Characteristics);
				base.WriteInt32(directory.TimeDateStamp);
				base.WriteInt16(directory.MajorVersion);
				base.WriteInt16(directory.MinorVersion);
				base.WriteInt32((int)directory.Type);
				base.WriteInt32(directory.SizeOfData);
				base.WriteInt32(directory.AddressOfRawData);
				base.WriteInt32(num);
				num += imageDebugHeaderEntry.Data.Length;
			}
			for (int j = 0; j < this.debug_header.Entries.Length; j++)
			{
				ImageDebugHeaderEntry imageDebugHeaderEntry2 = this.debug_header.Entries[j];
				base.WriteBytes(imageDebugHeaderEntry2.Data);
			}
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001DA68 File Offset: 0x0001BC68
		private void WriteImportDirectory()
		{
			base.WriteUInt32(this.text_map.GetRVA(TextSegment.ImportDirectory) + 40U);
			base.WriteUInt32(0U);
			base.WriteUInt32(0U);
			base.WriteUInt32(this.text_map.GetRVA(TextSegment.ImportHintNameTable) + 14U);
			base.WriteUInt32(this.text_map.GetRVA(TextSegment.ImportAddressTable));
			base.Advance(20);
			base.WriteUInt32(this.text_map.GetRVA(TextSegment.ImportHintNameTable));
			this.MoveToRVA(TextSegment.ImportHintNameTable);
			base.WriteUInt16(0);
			base.WriteBytes(this.GetRuntimeMain());
			base.WriteByte(0);
			base.WriteBytes(ImageWriter.GetSimpleString("mscoree.dll"));
			base.WriteUInt16(0);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001DB15 File Offset: 0x0001BD15
		private byte[] GetRuntimeMain()
		{
			if (this.module.Kind != ModuleKind.Dll && this.module.Kind != ModuleKind.NetModule)
			{
				return ImageWriter.GetSimpleString("_CorExeMain");
			}
			return ImageWriter.GetSimpleString("_CorDllMain");
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001DB48 File Offset: 0x0001BD48
		private void WriteStartupStub()
		{
			TargetArchitecture architecture = this.module.Architecture;
			if (architecture == TargetArchitecture.I386)
			{
				base.WriteUInt16(9727);
				base.WriteUInt32(4194304U + this.text_map.GetRVA(TextSegment.ImportAddressTable));
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001DB92 File Offset: 0x0001BD92
		private void WriteRsrc()
		{
			this.PrepareSection(this.rsrc);
			base.WriteBuffer(this.win32_resources);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001DBAC File Offset: 0x0001BDAC
		private void WriteReloc()
		{
			this.PrepareSection(this.reloc);
			uint num = this.text_map.GetRVA(TextSegment.StartupStub);
			num += ((this.module.Architecture == TargetArchitecture.IA64) ? 32U : 2U);
			uint num2 = num & 4294963200U;
			base.WriteUInt32(num2);
			base.WriteUInt32(12U);
			TargetArchitecture architecture = this.module.Architecture;
			if (architecture == TargetArchitecture.I386)
			{
				base.WriteUInt32(12288U + num - num2);
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001DC30 File Offset: 0x0001BE30
		public void WriteImage()
		{
			this.WriteDOSHeader();
			this.WritePEFileHeader();
			this.WriteOptionalHeaders();
			this.WriteSectionHeaders();
			this.WriteText();
			if (this.rsrc != null)
			{
				this.WriteRsrc();
			}
			if (this.reloc != null)
			{
				this.WriteReloc();
			}
			this.Flush();
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001DC80 File Offset: 0x0001BE80
		private void BuildTextMap()
		{
			TextMap textMap = this.text_map;
			textMap.AddMap(TextSegment.Code, this.metadata.code.length, (!this.pe64) ? 4 : 16);
			textMap.AddMap(TextSegment.Resources, this.metadata.resources.length, 8);
			textMap.AddMap(TextSegment.Data, this.metadata.data.length, 4);
			if (this.metadata.data.length > 0)
			{
				this.metadata.table_heap.FixupData(textMap.GetRVA(TextSegment.Data));
			}
			textMap.AddMap(TextSegment.StrongNameSignature, this.GetStrongNameLength(), 4);
			this.BuildMetadataTextMap();
			int num = 0;
			if (this.debug_header != null && this.debug_header.HasEntries)
			{
				int num2 = this.debug_header.Entries.Length * 28;
				int num3 = (int)(textMap.GetNextRVA(TextSegment.BlobHeap) + (uint)num2);
				int num4 = 0;
				for (int i = 0; i < this.debug_header.Entries.Length; i++)
				{
					ImageDebugHeaderEntry imageDebugHeaderEntry = this.debug_header.Entries[i];
					ImageDebugDirectory directory = imageDebugHeaderEntry.Directory;
					directory.AddressOfRawData = ((imageDebugHeaderEntry.Data.Length == 0) ? 0 : num3);
					imageDebugHeaderEntry.Directory = directory;
					num4 += imageDebugHeaderEntry.Data.Length;
					num3 += num4;
				}
				num = num2 + num4;
			}
			textMap.AddMap(TextSegment.DebugDirectory, num, 4);
			if (!this.has_reloc)
			{
				uint nextRVA = textMap.GetNextRVA(TextSegment.DebugDirectory);
				textMap.AddMap(TextSegment.ImportDirectory, new Range(nextRVA, 0U));
				textMap.AddMap(TextSegment.ImportHintNameTable, new Range(nextRVA, 0U));
				textMap.AddMap(TextSegment.StartupStub, new Range(nextRVA, 0U));
				return;
			}
			uint nextRVA2 = textMap.GetNextRVA(TextSegment.DebugDirectory);
			uint num5 = nextRVA2 + 48U;
			num5 = (num5 + 15U) & 4294967280U;
			uint num6 = num5 - nextRVA2 + 27U;
			uint num7 = nextRVA2 + num6;
			num7 = ((this.module.Architecture == TargetArchitecture.IA64) ? ((num7 + 15U) & 4294967280U) : (2U + ((num7 + 3U) & 4294967292U)));
			textMap.AddMap(TextSegment.ImportDirectory, new Range(nextRVA2, num6));
			textMap.AddMap(TextSegment.ImportHintNameTable, new Range(num5, 0U));
			textMap.AddMap(TextSegment.StartupStub, new Range(num7, this.GetStartupStubLength()));
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001DEA0 File Offset: 0x0001C0A0
		public void BuildMetadataTextMap()
		{
			TextMap textMap = this.text_map;
			textMap.AddMap(TextSegment.MetadataHeader, this.GetMetadataHeaderLength(this.module.RuntimeVersion));
			textMap.AddMap(TextSegment.TableHeap, this.metadata.table_heap.length, 4);
			textMap.AddMap(TextSegment.StringHeap, this.metadata.string_heap.length, 4);
			textMap.AddMap(TextSegment.UserStringHeap, this.metadata.user_string_heap.IsEmpty ? 0 : this.metadata.user_string_heap.length, 4);
			textMap.AddMap(TextSegment.GuidHeap, this.metadata.guid_heap.length, 4);
			textMap.AddMap(TextSegment.BlobHeap, this.metadata.blob_heap.IsEmpty ? 0 : this.metadata.blob_heap.length, 4);
			textMap.AddMap(TextSegment.PdbHeap, (this.metadata.pdb_heap == null) ? 0 : this.metadata.pdb_heap.length, 4);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001DF98 File Offset: 0x0001C198
		private uint GetStartupStubLength()
		{
			TargetArchitecture architecture = this.module.Architecture;
			if (architecture == TargetArchitecture.I386)
			{
				return 6U;
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001DFC0 File Offset: 0x0001C1C0
		private int GetMetadataHeaderLength(string runtimeVersion)
		{
			return 20 + ImageWriter.GetZeroTerminatedStringLength(runtimeVersion) + 12 + 20 + (this.metadata.user_string_heap.IsEmpty ? 0 : 12) + 16 + (this.metadata.blob_heap.IsEmpty ? 0 : 16) + ((this.metadata.pdb_heap == null) ? 0 : 16);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001E024 File Offset: 0x0001C224
		private int GetStrongNameLength()
		{
			if (this.module.Assembly == null)
			{
				return 0;
			}
			byte[] publicKey = this.module.Assembly.Name.PublicKey;
			if (publicKey.IsNullOrEmpty<byte>())
			{
				return 0;
			}
			int num = publicKey.Length;
			if (num > 32)
			{
				return num - 32;
			}
			return 128;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001E073 File Offset: 0x0001C273
		public DataDirectory GetStrongNameSignatureDirectory()
		{
			return this.text_map.GetDataDirectory(TextSegment.StrongNameSignature);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001E081 File Offset: 0x0001C281
		public uint GetHeaderSize()
		{
			return (uint)(152 + this.SizeOfOptionalHeader() + this.sections * 40);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001E099 File Offset: 0x0001C299
		private void PatchWin32Resources(ByteBuffer resources)
		{
			this.PatchResourceDirectoryTable(resources);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001E0A4 File Offset: 0x0001C2A4
		private void PatchResourceDirectoryTable(ByteBuffer resources)
		{
			resources.Advance(12);
			int num = (int)(resources.ReadUInt16() + resources.ReadUInt16());
			for (int i = 0; i < num; i++)
			{
				this.PatchResourceDirectoryEntry(resources);
			}
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001E0DC File Offset: 0x0001C2DC
		private void PatchResourceDirectoryEntry(ByteBuffer resources)
		{
			resources.Advance(4);
			uint num = resources.ReadUInt32();
			int position = resources.position;
			resources.position = (int)(num & 2147483647U);
			if ((num & 2147483648U) != 0U)
			{
				this.PatchResourceDirectoryTable(resources);
			}
			else
			{
				this.PatchResourceDataEntry(resources);
			}
			resources.position = position;
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0001E12C File Offset: 0x0001C32C
		private void PatchResourceDataEntry(ByteBuffer resources)
		{
			uint num = resources.ReadUInt32();
			resources.position -= 4;
			resources.WriteUInt32(num - this.module.Image.Win32Resources.VirtualAddress + this.rsrc.VirtualAddress);
		}

		// Token: 0x0400035C RID: 860
		private readonly ModuleDefinition module;

		// Token: 0x0400035D RID: 861
		private readonly MetadataBuilder metadata;

		// Token: 0x0400035E RID: 862
		private readonly TextMap text_map;

		// Token: 0x0400035F RID: 863
		internal readonly Disposable<Stream> stream;

		// Token: 0x04000360 RID: 864
		private readonly string runtime_version;

		// Token: 0x04000361 RID: 865
		private ImageDebugHeader debug_header;

		// Token: 0x04000362 RID: 866
		private ByteBuffer win32_resources;

		// Token: 0x04000363 RID: 867
		private const uint pe_header_size = 152U;

		// Token: 0x04000364 RID: 868
		private const uint section_header_size = 40U;

		// Token: 0x04000365 RID: 869
		private const uint file_alignment = 512U;

		// Token: 0x04000366 RID: 870
		private const uint section_alignment = 8192U;

		// Token: 0x04000367 RID: 871
		private const ulong image_base = 4194304UL;

		// Token: 0x04000368 RID: 872
		internal const uint text_rva = 8192U;

		// Token: 0x04000369 RID: 873
		private readonly bool pe64;

		// Token: 0x0400036A RID: 874
		private readonly bool has_reloc;

		// Token: 0x0400036B RID: 875
		internal Section text;

		// Token: 0x0400036C RID: 876
		internal Section rsrc;

		// Token: 0x0400036D RID: 877
		internal Section reloc;

		// Token: 0x0400036E RID: 878
		private ushort sections;
	}
}
