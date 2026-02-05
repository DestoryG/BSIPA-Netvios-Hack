using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DA RID: 218
	internal sealed class ClassLayoutTable : SortedTable<Row<ushort, uint, uint>>
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x000198D8 File Offset: 0x00017AD8
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16(this.rows[i].Col1);
				buffer.WriteUInt32(this.rows[i].Col2);
				buffer.WriteRID(this.rows[i].Col3, Table.TypeDef);
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001993C File Offset: 0x00017B3C
		public override int Compare(Row<ushort, uint, uint> x, Row<ushort, uint, uint> y)
		{
			return SortedTable<Row<ushort, uint, uint>>.Compare(x.Col3, y.Col3);
		}
	}
}
