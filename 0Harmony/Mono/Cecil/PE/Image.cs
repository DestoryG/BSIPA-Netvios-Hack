using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil.PE
{
	// Token: 0x02000194 RID: 404
	internal sealed class Image : IDisposable
	{
		// Token: 0x06000CC1 RID: 3265 RVA: 0x0002AB49 File Offset: 0x00028D49
		public Image()
		{
			this.counter = new Func<Table, int>(this.GetTableLength);
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0002AB70 File Offset: 0x00028D70
		public bool HasTable(Table table)
		{
			return this.GetTableLength(table) > 0;
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0002AB7C File Offset: 0x00028D7C
		public int GetTableLength(Table table)
		{
			return (int)this.TableHeap[table].Length;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0002AB8F File Offset: 0x00028D8F
		public int GetTableIndexSize(Table table)
		{
			if (this.GetTableLength(table) >= 65536)
			{
				return 4;
			}
			return 2;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0002ABA4 File Offset: 0x00028DA4
		public int GetCodedIndexSize(CodedIndex coded_index)
		{
			int num = this.coded_index_sizes[(int)coded_index];
			if (num != 0)
			{
				return num;
			}
			return this.coded_index_sizes[(int)coded_index] = coded_index.GetSize(this.counter);
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0002ABD8 File Offset: 0x00028DD8
		public uint ResolveVirtualAddress(uint rva)
		{
			Section sectionAtVirtualAddress = this.GetSectionAtVirtualAddress(rva);
			if (sectionAtVirtualAddress == null)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.ResolveVirtualAddressInSection(rva, sectionAtVirtualAddress);
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0002ABFE File Offset: 0x00028DFE
		public uint ResolveVirtualAddressInSection(uint rva, Section section)
		{
			return rva + section.PointerToRawData - section.VirtualAddress;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0002AC10 File Offset: 0x00028E10
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

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0002AC48 File Offset: 0x00028E48
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

		// Token: 0x06000CCA RID: 3274 RVA: 0x0002AC8C File Offset: 0x00028E8C
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

		// Token: 0x06000CCB RID: 3275 RVA: 0x0002ACC4 File Offset: 0x00028EC4
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

		// Token: 0x06000CCC RID: 3276 RVA: 0x0002AD28 File Offset: 0x00028F28
		public bool HasDebugTables()
		{
			return this.HasTable(Table.Document) || this.HasTable(Table.MethodDebugInformation) || this.HasTable(Table.LocalScope) || this.HasTable(Table.LocalVariable) || this.HasTable(Table.LocalConstant) || this.HasTable(Table.StateMachineMethod) || this.HasTable(Table.CustomDebugInformation);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0002AD7B File Offset: 0x00028F7B
		public void Dispose()
		{
			this.Stream.Dispose();
		}

		// Token: 0x0400059A RID: 1434
		public Disposable<Stream> Stream;

		// Token: 0x0400059B RID: 1435
		public string FileName;

		// Token: 0x0400059C RID: 1436
		public ModuleKind Kind;

		// Token: 0x0400059D RID: 1437
		public string RuntimeVersion;

		// Token: 0x0400059E RID: 1438
		public TargetArchitecture Architecture;

		// Token: 0x0400059F RID: 1439
		public ModuleCharacteristics Characteristics;

		// Token: 0x040005A0 RID: 1440
		public ushort LinkerVersion;

		// Token: 0x040005A1 RID: 1441
		public ushort SubSystemMajor;

		// Token: 0x040005A2 RID: 1442
		public ushort SubSystemMinor;

		// Token: 0x040005A3 RID: 1443
		public ImageDebugHeader DebugHeader;

		// Token: 0x040005A4 RID: 1444
		public Section[] Sections;

		// Token: 0x040005A5 RID: 1445
		public Section MetadataSection;

		// Token: 0x040005A6 RID: 1446
		public uint EntryPointToken;

		// Token: 0x040005A7 RID: 1447
		public uint Timestamp;

		// Token: 0x040005A8 RID: 1448
		public ModuleAttributes Attributes;

		// Token: 0x040005A9 RID: 1449
		public DataDirectory Win32Resources;

		// Token: 0x040005AA RID: 1450
		public DataDirectory Debug;

		// Token: 0x040005AB RID: 1451
		public DataDirectory Resources;

		// Token: 0x040005AC RID: 1452
		public DataDirectory StrongName;

		// Token: 0x040005AD RID: 1453
		public StringHeap StringHeap;

		// Token: 0x040005AE RID: 1454
		public BlobHeap BlobHeap;

		// Token: 0x040005AF RID: 1455
		public UserStringHeap UserStringHeap;

		// Token: 0x040005B0 RID: 1456
		public GuidHeap GuidHeap;

		// Token: 0x040005B1 RID: 1457
		public TableHeap TableHeap;

		// Token: 0x040005B2 RID: 1458
		public PdbHeap PdbHeap;

		// Token: 0x040005B3 RID: 1459
		private readonly int[] coded_index_sizes = new int[14];

		// Token: 0x040005B4 RID: 1460
		private readonly Func<Table, int> counter;
	}
}
