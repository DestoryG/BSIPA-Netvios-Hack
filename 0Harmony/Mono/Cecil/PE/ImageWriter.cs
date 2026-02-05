using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil.PE
{
	// Token: 0x02000196 RID: 406
	internal sealed class ImageWriter : BinaryStreamWriter
	{
		// Token: 0x06000CE4 RID: 3300 RVA: 0x0002BD0C File Offset: 0x00029F0C
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

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0002BDBC File Offset: 0x00029FBC
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

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0002BE18 File Offset: 0x0002A018
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

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0002BE8B File Offset: 0x0002A08B
		public static ImageWriter CreateWriter(ModuleDefinition module, MetadataBuilder metadata, Disposable<Stream> stream)
		{
			ImageWriter imageWriter = new ImageWriter(module, module.runtime_version, metadata, stream, false);
			imageWriter.BuildSections();
			return imageWriter;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0002BEA4 File Offset: 0x0002A0A4
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

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0002BEE4 File Offset: 0x0002A0E4
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

		// Token: 0x06000CEA RID: 3306 RVA: 0x0002BF80 File Offset: 0x0002A180
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

		// Token: 0x06000CEB RID: 3307 RVA: 0x0002C000 File Offset: 0x0002A200
		private static uint Align(uint value, uint align)
		{
			align -= 1U;
			return (value + align) & ~align;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0002C00D File Offset: 0x0002A20D
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

		// Token: 0x06000CED RID: 3309 RVA: 0x0002C02A File Offset: 0x0002A22A
		private ushort SizeOfOptionalHeader()
		{
			return (!this.pe64) ? 224 : 240;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0002C044 File Offset: 0x0002A244
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

		// Token: 0x06000CEF RID: 3311 RVA: 0x0002C0E5 File Offset: 0x0002A2E5
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

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0002C10C File Offset: 0x0002A30C
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

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0002C431 File Offset: 0x0002A631
		private void WriteZeroDataDirectory()
		{
			base.WriteUInt32(0U);
			base.WriteUInt32(0U);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0002C444 File Offset: 0x0002A644
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

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002C480 File Offset: 0x0002A680
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

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0002C4D0 File Offset: 0x0002A6D0
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

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0002C561 File Offset: 0x0002A761
		private uint GetRVAFileOffset(Section section, uint rva)
		{
			return section.PointerToRawData + rva - section.VirtualAddress;
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0002C572 File Offset: 0x0002A772
		private void MoveTo(uint pointer)
		{
			this.BaseStream.Seek((long)((ulong)pointer), SeekOrigin.Begin);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0002C583 File Offset: 0x0002A783
		private void MoveToRVA(Section section, uint rva)
		{
			this.BaseStream.Seek((long)((ulong)this.GetRVAFileOffset(section, rva)), SeekOrigin.Begin);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0002C59B File Offset: 0x0002A79B
		private void MoveToRVA(TextSegment segment)
		{
			this.MoveToRVA(this.text, this.text_map.GetRVA(segment));
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0002C5B5 File Offset: 0x0002A7B5
		private void WriteRVA(uint rva)
		{
			if (!this.pe64)
			{
				base.WriteUInt32(rva);
				return;
			}
			base.WriteUInt64((ulong)rva);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0002C5D0 File Offset: 0x0002A7D0
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

		// Token: 0x06000CFB RID: 3323 RVA: 0x0002C65C File Offset: 0x0002A85C
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

		// Token: 0x06000CFC RID: 3324 RVA: 0x0002C7F1 File Offset: 0x0002A9F1
		private uint GetMetadataLength()
		{
			return this.text_map.GetRVA(TextSegment.DebugDirectory) - this.text_map.GetRVA(TextSegment.MetadataHeader);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0002C810 File Offset: 0x0002AA10
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

		// Token: 0x06000CFE RID: 3326 RVA: 0x0002C8E0 File Offset: 0x0002AAE0
		private ushort GetStreamCount()
		{
			return (ushort)(2 + (this.metadata.user_string_heap.IsEmpty ? 0 : 1) + (this.metadata.guid_heap.IsEmpty ? 0 : 1) + (this.metadata.blob_heap.IsEmpty ? 0 : 1) + ((this.metadata.pdb_heap == null) ? 0 : 1));
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0002C948 File Offset: 0x0002AB48
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

		// Token: 0x06000D00 RID: 3328 RVA: 0x0002C987 File Offset: 0x0002AB87
		private static int GetZeroTerminatedStringLength(string @string)
		{
			return (@string.Length + 1 + 3) & -4;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0002C996 File Offset: 0x0002AB96
		private static byte[] GetZeroTerminatedString(string @string)
		{
			return ImageWriter.GetString(@string, ImageWriter.GetZeroTerminatedStringLength(@string));
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0002C9A4 File Offset: 0x0002ABA4
		private static byte[] GetSimpleString(string @string)
		{
			return ImageWriter.GetString(@string, @string.Length);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0002C9B4 File Offset: 0x0002ABB4
		private static byte[] GetString(string @string, int length)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < @string.Length; i++)
			{
				array[i] = (byte)@string[i];
			}
			return array;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0002C9E8 File Offset: 0x0002ABE8
		public void WriteMetadata()
		{
			this.WriteHeap(TextSegment.TableHeap, this.metadata.table_heap);
			this.WriteHeap(TextSegment.StringHeap, this.metadata.string_heap);
			this.WriteHeap(TextSegment.UserStringHeap, this.metadata.user_string_heap);
			this.WriteHeap(TextSegment.GuidHeap, this.metadata.guid_heap);
			this.WriteHeap(TextSegment.BlobHeap, this.metadata.blob_heap);
			this.WriteHeap(TextSegment.PdbHeap, this.metadata.pdb_heap);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0002CA65 File Offset: 0x0002AC65
		private void WriteHeap(TextSegment heap, HeapBuffer buffer)
		{
			if (buffer == null || buffer.IsEmpty)
			{
				return;
			}
			this.MoveToRVA(heap);
			base.WriteBuffer(buffer);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0002CA84 File Offset: 0x0002AC84
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

		// Token: 0x06000D07 RID: 3335 RVA: 0x0002CB80 File Offset: 0x0002AD80
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

		// Token: 0x06000D08 RID: 3336 RVA: 0x0002CC2D File Offset: 0x0002AE2D
		private byte[] GetRuntimeMain()
		{
			if (this.module.Kind != ModuleKind.Dll && this.module.Kind != ModuleKind.NetModule)
			{
				return ImageWriter.GetSimpleString("_CorExeMain");
			}
			return ImageWriter.GetSimpleString("_CorDllMain");
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0002CC5F File Offset: 0x0002AE5F
		private void WriteStartupStub()
		{
			if (this.module.Architecture == TargetArchitecture.I386)
			{
				base.WriteUInt16(9727);
				base.WriteUInt32(4194304U + this.text_map.GetRVA(TextSegment.ImportAddressTable));
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0002CC9C File Offset: 0x0002AE9C
		private void WriteRsrc()
		{
			this.PrepareSection(this.rsrc);
			base.WriteBuffer(this.win32_resources);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0002CCB8 File Offset: 0x0002AEB8
		private void WriteReloc()
		{
			this.PrepareSection(this.reloc);
			uint num = this.text_map.GetRVA(TextSegment.StartupStub);
			num += ((this.module.Architecture == TargetArchitecture.IA64) ? 32U : 2U);
			uint num2 = num & 4294963200U;
			base.WriteUInt32(num2);
			base.WriteUInt32(12U);
			if (this.module.Architecture == TargetArchitecture.I386)
			{
				base.WriteUInt32(12288U + num - num2);
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0002CD38 File Offset: 0x0002AF38
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

		// Token: 0x06000D0D RID: 3341 RVA: 0x0002CD88 File Offset: 0x0002AF88
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

		// Token: 0x06000D0E RID: 3342 RVA: 0x0002CFA8 File Offset: 0x0002B1A8
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

		// Token: 0x06000D0F RID: 3343 RVA: 0x0002D0A0 File Offset: 0x0002B2A0
		private uint GetStartupStubLength()
		{
			if (this.module.Architecture == TargetArchitecture.I386)
			{
				return 6U;
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0002D0BC File Offset: 0x0002B2BC
		private int GetMetadataHeaderLength(string runtimeVersion)
		{
			return 20 + ImageWriter.GetZeroTerminatedStringLength(runtimeVersion) + 12 + 20 + (this.metadata.user_string_heap.IsEmpty ? 0 : 12) + 16 + (this.metadata.blob_heap.IsEmpty ? 0 : 16) + ((this.metadata.pdb_heap == null) ? 0 : 16);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0002D120 File Offset: 0x0002B320
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

		// Token: 0x06000D12 RID: 3346 RVA: 0x0002D16F File Offset: 0x0002B36F
		public DataDirectory GetStrongNameSignatureDirectory()
		{
			return this.text_map.GetDataDirectory(TextSegment.StrongNameSignature);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0002D17D File Offset: 0x0002B37D
		public uint GetHeaderSize()
		{
			return (uint)(152 + this.SizeOfOptionalHeader() + this.sections * 40);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0002D195 File Offset: 0x0002B395
		private void PatchWin32Resources(ByteBuffer resources)
		{
			this.PatchResourceDirectoryTable(resources);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0002D1A0 File Offset: 0x0002B3A0
		private void PatchResourceDirectoryTable(ByteBuffer resources)
		{
			resources.Advance(12);
			int num = (int)(resources.ReadUInt16() + resources.ReadUInt16());
			for (int i = 0; i < num; i++)
			{
				this.PatchResourceDirectoryEntry(resources);
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0002D1D8 File Offset: 0x0002B3D8
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

		// Token: 0x06000D17 RID: 3351 RVA: 0x0002D228 File Offset: 0x0002B428
		private void PatchResourceDataEntry(ByteBuffer resources)
		{
			uint num = resources.ReadUInt32();
			resources.position -= 4;
			resources.WriteUInt32(num - this.module.Image.Win32Resources.VirtualAddress + this.rsrc.VirtualAddress);
		}

		// Token: 0x040005B9 RID: 1465
		private readonly ModuleDefinition module;

		// Token: 0x040005BA RID: 1466
		private readonly MetadataBuilder metadata;

		// Token: 0x040005BB RID: 1467
		private readonly TextMap text_map;

		// Token: 0x040005BC RID: 1468
		internal readonly Disposable<Stream> stream;

		// Token: 0x040005BD RID: 1469
		private readonly string runtime_version;

		// Token: 0x040005BE RID: 1470
		private ImageDebugHeader debug_header;

		// Token: 0x040005BF RID: 1471
		private ByteBuffer win32_resources;

		// Token: 0x040005C0 RID: 1472
		private const uint pe_header_size = 152U;

		// Token: 0x040005C1 RID: 1473
		private const uint section_header_size = 40U;

		// Token: 0x040005C2 RID: 1474
		private const uint file_alignment = 512U;

		// Token: 0x040005C3 RID: 1475
		private const uint section_alignment = 8192U;

		// Token: 0x040005C4 RID: 1476
		private const ulong image_base = 4194304UL;

		// Token: 0x040005C5 RID: 1477
		internal const uint text_rva = 8192U;

		// Token: 0x040005C6 RID: 1478
		private readonly bool pe64;

		// Token: 0x040005C7 RID: 1479
		private readonly bool has_reloc;

		// Token: 0x040005C8 RID: 1480
		internal Section text;

		// Token: 0x040005C9 RID: 1481
		internal Section rsrc;

		// Token: 0x040005CA RID: 1482
		internal Section reloc;

		// Token: 0x040005CB RID: 1483
		private ushort sections;
	}
}
