using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E6 RID: 230
	internal sealed class FieldRVATable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x060005B4 RID: 1460 RVA: 0x00019D3C File Offset: 0x00017F3C
		public override void Write(TableHeapBuffer buffer)
		{
			this.position = buffer.position;
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Field);
			}
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x000199A5 File Offset: 0x00017BA5
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col2, y.Col2);
		}

		// Token: 0x0400024C RID: 588
		internal int position;
	}
}
