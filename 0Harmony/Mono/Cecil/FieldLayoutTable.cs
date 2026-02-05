using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DB RID: 219
	internal sealed class FieldLayoutTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x0600059B RID: 1435 RVA: 0x00019958 File Offset: 0x00017B58
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Field);
			}
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x000199A5 File Offset: 0x00017BA5
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
