using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000EC RID: 236
	internal sealed class NestedClassTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x060005C1 RID: 1473 RVA: 0x0001A0D4 File Offset: 0x000182D4
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteRID(this.rows[i].Col2, Table.TypeDef);
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001983D File Offset: 0x00017A3D
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
