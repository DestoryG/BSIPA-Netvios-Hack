using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200002B RID: 43
	internal sealed class ClassLayoutTable : SortedTable<Row<ushort, uint, uint>>
	{
		// Token: 0x06000227 RID: 551 RVA: 0x0000B430 File Offset: 0x00009630
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16(this.rows[i].Col1);
				buffer.WriteUInt32(this.rows[i].Col2);
				buffer.WriteRID(this.rows[i].Col3, Table.TypeDef);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000B494 File Offset: 0x00009694
		public override int Compare(Row<ushort, uint, uint> x, Row<ushort, uint, uint> y)
		{
			return SortedTable<Row<ushort, uint, uint>>.Compare(x.Col3, y.Col3);
		}
	}
}
