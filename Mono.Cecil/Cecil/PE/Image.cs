using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil.PE
{
	// Token: 0x020000D2 RID: 210
	internal sealed class Image : IDisposable
	{
		// Token: 0x060008E3 RID: 2275 RVA: 0x0001BA49 File Offset: 0x00019C49
		public Image()
		{
			this.counter = new Func<Table, int>(this.GetTableLength);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001BA70 File Offset: 0x00019C70
		public bool HasTable(Table table)
		{
			return this.GetTableLength(table) > 0;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001BA7C File Offset: 0x00019C7C
		public int GetTableLength(Table table)
		{
			return (int)this.TableHeap[table].Length;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0001BA8F File Offset: 0x00019C8F
		public int GetTableIndexSize(Table table)
		{
			if (this.GetTableLength(table) >= 65536)
			{
				return 4;
			}
			return 2;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001BAA4 File Offset: 0x00019CA4
		public int GetCodedIndexSize(CodedIndex coded_index)
		{
			int num = this.coded_index_sizes[(int)coded_index];
			if (num != 0)
			{
				return num;
			}
			return this.coded_index_sizes[(int)coded_index] = coded_index.GetSize(this.counter);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001BAD8 File Offset: 0x00019CD8
		public uint ResolveVirtualAddress(uint rva)
		{
			Section sectionAtVirtualAddress = this.GetSectionAtVirtualAddress(rva);
			if (sectionAtVirtualAddress == null)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.ResolveVirtualAddressInSection(rva, sectionAtVirtualAddress);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001BAFE File Offset: 0x00019CFE
		public uint ResolveVirtualAddressInSection(uint rva, Section section)
		{
			return rva + section.PointerToRawData - section.VirtualAddress;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001BB10 File Offset: 0x00019D10
		public Section GetSection(string name)
		{
			foreach (Section section in this.Sections)
			{
				if (section.Name == name)
				{
					return section;
				}
			}
			return null;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001BB48 File Offset: 0x00019D48
		public Section GetSectionAtVirtualAddress(uint rva)
		{
			foreach (Section section in this.Sections)
			{
				if (rva >= section.VirtualAddress && rva < section.VirtualAddress + section.SizeOfRawData)
				{
					return section;
				}
			}
			return null;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001BB8C File Offset: 0x00019D8C
		private BinaryStreamReader GetReaderAt(uint rva)
		{
			Section sectionAtVirtualAddress = this.GetSectionAtVirtualAddress(rva);
			if (sectionAtVirtualAddress == null)
			{
				return null;
			}
			BinaryStreamReader binaryStreamReader = new BinaryStreamReader(this.Stream.value);
			binaryStreamReader.MoveTo(this.ResolveVirtualAddressInSection(rva, sectionAtVirtualAddress));
			return binaryStreamReader;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001BBC4 File Offset: 0x00019DC4
		public TRet GetReaderAt<TItem, TRet>(uint rva, TItem item, Func<TItem, BinaryStreamReader, TRet> read) where TRet : class
		{
			long position = this.Stream.value.Position;
			TRet tret;
			try
			{
				BinaryStreamReader readerAt = this.GetReaderAt(rva);
				if (readerAt == null)
				{
					tret = default(TRet);
					tret = tret;
				}
				else
				{
					tret = read(item, readerAt);
				}
			}
			finally
			{
				this.Stream.value.Position = position;
			}
			return tret;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001BC28 File Offset: 0x00019E28
		public bool HasDebugTables()
		{
			return this.HasTable(Table.Document) || this.HasTable(Table.MethodDebugInformation) || this.HasTable(Table.LocalScope) || this.HasTable(Table.LocalVariable) || this.HasTable(Table.LocalConstant) || this.HasTable(Table.StateMachineMethod) || this.HasTable(Table.CustomDebugInformation);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001BC7B File Offset: 0x00019E7B
		public void Dispose()
		{
			this.Stream.Dispose();
		}

		// Token: 0x0400033D RID: 829
		public Disposable<Stream> Stream;

		// Token: 0x0400033E RID: 830
		public string FileName;

		// Token: 0x0400033F RID: 831
		public ModuleKind Kind;

		// Token: 0x04000340 RID: 832
		public string RuntimeVersion;

		// Token: 0x04000341 RID: 833
		public TargetArchitecture Architecture;

		// Token: 0x04000342 RID: 834
		public ModuleCharacteristics Characteristics;

		// Token: 0x04000343 RID: 835
		public ushort LinkerVersion;

		// Token: 0x04000344 RID: 836
		public ushort SubSystemMajor;

		// Token: 0x04000345 RID: 837
		public ushort SubSystemMinor;

		// Token: 0x04000346 RID: 838
		public ImageDebugHeader DebugHeader;

		// Token: 0x04000347 RID: 839
		public Section[] Sections;

		// Token: 0x04000348 RID: 840
		public Section MetadataSection;

		// Token: 0x04000349 RID: 841
		public uint EntryPointToken;

		// Token: 0x0400034A RID: 842
		public uint Timestamp;

		// Token: 0x0400034B RID: 843
		public ModuleAttributes Attributes;

		// Token: 0x0400034C RID: 844
		public DataDirectory Win32Resources;

		// Token: 0x0400034D RID: 845
		public DataDirectory Debug;

		// Token: 0x0400034E RID: 846
		public DataDirectory Resources;

		// Token: 0x0400034F RID: 847
		public DataDirectory StrongName;

		// Token: 0x04000350 RID: 848
		public StringHeap StringHeap;

		// Token: 0x04000351 RID: 849
		public BlobHeap BlobHeap;

		// Token: 0x04000352 RID: 850
		public UserStringHeap UserStringHeap;

		// Token: 0x04000353 RID: 851
		public GuidHeap GuidHeap;

		// Token: 0x04000354 RID: 852
		public TableHeap TableHeap;

		// Token: 0x04000355 RID: 853
		public PdbHeap PdbHeap;

		// Token: 0x04000356 RID: 854
		private readonly int[] coded_index_sizes = new int[14];

		// Token: 0x04000357 RID: 855
		private readonly Func<Table, int> counter;
	}
}
