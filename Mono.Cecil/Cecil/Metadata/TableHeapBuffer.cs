using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000D9 RID: 217
	internal sealed class TableHeapBuffer : HeapBuffer
	{
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x000026DB File Offset: 0x000008DB
		public override bool IsEmpty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0001E394 File Offset: 0x0001C594
		public TableHeapBuffer(ModuleDefinition module, MetadataBuilder metadata)
			: base(24)
		{
			this.module = module;
			this.metadata = metadata;
			this.counter = new Func<Table, int>(this.GetTableLength);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0001E3F0 File Offset: 0x0001C5F0
		private int GetTableLength(Table table)
		{
			return (int)this.table_infos[(int)table].Length;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0001E404 File Offset: 0x0001C604
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

		// Token: 0x0600094D RID: 2381 RVA: 0x0001E43E File Offset: 0x0001C63E
		public void WriteBySize(uint value, int size)
		{
			if (size == 4)
			{
				base.WriteUInt32(value);
				return;
			}
			base.WriteUInt16((ushort)value);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0001E454 File Offset: 0x0001C654
		public void WriteBySize(uint value, bool large)
		{
			if (large)
			{
				base.WriteUInt32(value);
				return;
			}
			base.WriteUInt16((ushort)value);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0001E469 File Offset: 0x0001C669
		public void WriteString(uint @string)
		{
			this.WriteBySize(this.string_offsets[(int)@string], this.large_string);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0001E47F File Offset: 0x0001C67F
		public void WriteBlob(uint blob)
		{
			this.WriteBySize(blob, this.large_blob);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0001E48E File Offset: 0x0001C68E
		public void WriteGuid(uint guid)
		{
			this.WriteBySize(guid, this.large_guid);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0001E49D File Offset: 0x0001C69D
		public void WriteRID(uint rid, Table table)
		{
			this.WriteBySize(rid, this.table_infos[(int)table].IsLarge);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0001E4B8 File Offset: 0x0001C6B8
		private int GetCodedIndexSize(CodedIndex coded_index)
		{
			int num = this.coded_index_sizes[(int)coded_index];
			if (num != 0)
			{
				return num;
			}
			return this.coded_index_sizes[(int)coded_index] = coded_index.GetSize(this.counter);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0001E4EC File Offset: 0x0001C6EC
		public void WriteCodedRID(uint rid, CodedIndex coded_index)
		{
			this.WriteBySize(rid, this.GetCodedIndexSize(coded_index));
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0001E4FC File Offset: 0x0001C6FC
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

		// Token: 0x06000956 RID: 2390 RVA: 0x0001E560 File Offset: 0x0001C760
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

		// Token: 0x06000957 RID: 2391 RVA: 0x0001E5A0 File Offset: 0x0001C7A0
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

		// Token: 0x06000958 RID: 2392 RVA: 0x0001E5DC File Offset: 0x0001C7DC
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

		// Token: 0x06000959 RID: 2393 RVA: 0x0001E624 File Offset: 0x0001C824
		public void ComputeTableInformations()
		{
			if (this.metadata.metadata_builder != null)
			{
				this.ComputeTableInformations(this.metadata.metadata_builder.table_heap);
			}
			this.ComputeTableInformations(this.metadata.table_heap);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0001E65C File Offset: 0x0001C85C
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

		// Token: 0x0600095B RID: 2395 RVA: 0x0001E6A8 File Offset: 0x0001C8A8
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

		// Token: 0x0600095C RID: 2396 RVA: 0x0001E714 File Offset: 0x0001C914
		private byte GetTableHeapVersion()
		{
			TargetRuntime runtime = this.module.Runtime;
			if (runtime == TargetRuntime.Net_1_0 || runtime == TargetRuntime.Net_1_1)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0001E738 File Offset: 0x0001C938
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

		// Token: 0x04000387 RID: 903
		private readonly ModuleDefinition module;

		// Token: 0x04000388 RID: 904
		private readonly MetadataBuilder metadata;

		// Token: 0x04000389 RID: 905
		internal readonly TableInformation[] table_infos = new TableInformation[58];

		// Token: 0x0400038A RID: 906
		internal readonly MetadataTable[] tables = new MetadataTable[58];

		// Token: 0x0400038B RID: 907
		private bool large_string;

		// Token: 0x0400038C RID: 908
		private bool large_blob;

		// Token: 0x0400038D RID: 909
		private bool large_guid;

		// Token: 0x0400038E RID: 910
		private readonly int[] coded_index_sizes = new int[14];

		// Token: 0x0400038F RID: 911
		private readonly Func<Table, int> counter;

		// Token: 0x04000390 RID: 912
		internal uint[] string_offsets;
	}
}
