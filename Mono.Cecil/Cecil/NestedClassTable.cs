using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200003D RID: 61
	internal sealed class NestedClassTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x06000250 RID: 592 RVA: 0x0000BC2C File Offset: 0x00009E2C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteRID(this.rows[i].Col2, Table.TypeDef);
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000B395 File Offset: 0x00009595
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
