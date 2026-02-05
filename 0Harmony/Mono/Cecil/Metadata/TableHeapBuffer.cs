using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x0200019C RID: 412
	internal sealed class TableHeapBuffer : HeapBuffer
	{
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00010910 File Offset: 0x0000EB10
		public override bool IsEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0002D4AC File Offset: 0x0002B6AC
		public TableHeapBuffer(ModuleDefinition module, MetadataBuilder metadata)
			: base(24)
		{
			this.module = module;
			this.metadata = metadata;
			this.counter = new Func<Table, int>(this.GetTableLength);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0002D508 File Offset: 0x0002B708
		private int GetTableLength(Table table)
		{
			return (int)this.table_infos[(int)table].Length;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0002D51C File Offset: 0x0002B71C
		public TTable GetTable<TTable>(Table table) where TTable : MetadataTable, new()
		{
			TTable ttable = (TTable)((object)this.tables[(int)table]);
			if (ttable != null)
			{
				return ttable;
			}
			ttable = new TTable();
			this.tables[(int)table] = ttable;
			return ttable;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0002D556 File Offset: 0x0002B756
		public void WriteBySize(uint value, int size)
		{
			if (size == 4)
			{
				base.WriteUInt32(value);
				return;
			}
			base.WriteUInt16((ushort)value);
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0002D56C File Offset: 0x0002B76C
		public void WriteBySize(uint value, bool large)
		{
			if (large)
			{
				base.WriteUInt32(value);
				return;
			}
			base.WriteUInt16((ushort)value);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0002D581 File Offset: 0x0002B781
		public void WriteString(uint @string)
		{
			this.WriteBySize(this.string_offsets[(int)@string], this.large_string);
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0002D597 File Offset: 0x0002B797
		public void WriteBlob(uint blob)
		{
			this.WriteBySize(blob, this.large_blob);
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0002D5A6 File Offset: 0x0002B7A6
		public void WriteGuid(uint guid)
		{
			this.WriteBySize(guid, this.large_guid);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0002D5B5 File Offset: 0x0002B7B5
		public void WriteRID(uint rid, Table table)
		{
			this.WriteBySize(rid, this.table_infos[(int)table].IsLarge);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0002D5D0 File Offset: 0x0002B7D0
		private int GetCodedIndexSize(CodedIndex coded_index)
		{
			int num = this.coded_index_sizes[(int)coded_index];
			if (num != 0)
			{
				return num;
			}
			return this.coded_index_sizes[(int)coded_index] = coded_index.GetSize(this.counter);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0002D604 File Offset: 0x0002B804
		public void WriteCodedRID(uint rid, CodedIndex coded_index)
		{
			this.WriteBySize(rid, this.GetCodedIndexSize(coded_index));
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0002D614 File Offset: 0x0002B814
		public void WriteTableHeap()
		{
			base.WriteUInt32(0U);
			base.WriteByte(this.GetTableHeapVersion());
			base.WriteByte(0);
			base.WriteByte(this.GetHeapSizes());
			base.WriteByte(10);
			base.WriteUInt64(this.GetValid());
			base.WriteUInt64(55193285546867200UL);
			this.WriteRowCount();
			this.WriteTables();
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0002D678 File Offset: 0x0002B878
		private void WriteRowCount()
		{
			for (int i = 0; i < this.tables.Length; i++)
			{
				MetadataTable metadataTable = this.tables[i];
				if (metadataTable != null && metadataTable.Length != 0)
				{
					base.WriteUInt32((uint)metadataTable.Length);
				}
			}
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0002D6B8 File Offset: 0x0002B8B8
		private void WriteTables()
		{
			for (int i = 0; i < this.tables.Length; i++)
			{
				MetadataTable metadataTable = this.tables[i];
				if (metadataTable != null && metadataTable.Length != 0)
				{
					metadataTable.Write(this);
				}
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0002D6F4 File Offset: 0x0002B8F4
		private ulong GetValid()
		{
			ulong num = 0UL;
			for (int i = 0; i < this.tables.Length; i++)
			{
				MetadataTable metadataTable = this.tables[i];
				if (metadataTable != null && metadataTable.Length != 0)
				{
					metadataTable.Sort();
					num |= 1UL << i;
				}
			}
			return num;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0002D73C File Offset: 0x0002B93C
		public void ComputeTableInformations()
		{
			if (this.metadata.metadata_builder != null)
			{
				this.ComputeTableInformations(this.metadata.metadata_builder.table_heap);
			}
			this.ComputeTableInformations(this.metadata.table_heap);
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0002D774 File Offset: 0x0002B974
		private void ComputeTableInformations(TableHeapBuffer table_heap)
		{
			MetadataTable[] array = table_heap.tables;
			for (int i = 0; i < array.Length; i++)
			{
				MetadataTable metadataTable = array[i];
				if (metadataTable != null && metadataTable.Length > 0)
				{
					this.table_infos[i].Length = (uint)metadataTable.Length;
				}
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0002D7C0 File Offset: 0x0002B9C0
		private byte GetHeapSizes()
		{
			byte b = 0;
			if (this.metadata.string_heap.IsLarge)
			{
				this.large_string = true;
				b |= 1;
			}
			if (this.metadata.guid_heap.IsLarge)
			{
				this.large_guid = true;
				b |= 2;
			}
			if (this.metadata.blob_heap.IsLarge)
			{
				this.large_blob = true;
				b |= 4;
			}
			return b;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0002D82C File Offset: 0x0002BA2C
		private byte GetTableHeapVersion()
		{
			TargetRuntime runtime = this.module.Runtime;
			if (runtime <= TargetRuntime.Net_1_1)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0002D84C File Offset: 0x0002BA4C
		public void FixupData(uint data_rva)
		{
			FieldRVATable table = this.GetTable<FieldRVATable>(Table.FieldRVA);
			if (table.length == 0)
			{
				return;
			}
			int num = (this.GetTable<FieldTable>(Table.Field).IsLarge ? 4 : 2);
			int position = this.position;
			this.position = table.position;
			for (int i = 0; i < table.length; i++)
			{
				uint num2 = base.ReadUInt32();
				this.position -= 4;
				base.WriteUInt32(num2 + data_rva);
				this.position += num;
			}
			this.position = position;
		}

		// Token: 0x040005E6 RID: 1510
		private readonly ModuleDefinition module;

		// Token: 0x040005E7 RID: 1511
		private readonly MetadataBuilder metadata;

		// Token: 0x040005E8 RID: 1512
		internal readonly TableInformation[] table_infos = new TableInformation[58];

		// Token: 0x040005E9 RID: 1513
		internal readonly MetadataTable[] tables = new MetadataTable[58];

		// Token: 0x040005EA RID: 1514
		private bool large_string;

		// Token: 0x040005EB RID: 1515
		private bool large_blob;

		// Token: 0x040005EC RID: 1516
		private bool large_guid;

		// Token: 0x040005ED RID: 1517
		private readonly int[] coded_index_sizes = new int[14];

		// Token: 0x040005EE RID: 1518
		private readonly Func<Table, int> counter;

		// Token: 0x040005EF RID: 1519
		internal uint[] string_offsets;
	}
}
