using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200002C RID: 44
	internal sealed class FieldLayoutTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x0600022A RID: 554 RVA: 0x0000B4B0 File Offset: 0x000096B0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Field);
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000B4FD File Offset: 0x000096FD
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
