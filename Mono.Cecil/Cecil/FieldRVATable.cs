using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000037 RID: 55
	internal sealed class FieldRVATable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x06000243 RID: 579 RVA: 0x0000B894 File Offset: 0x00009A94
		public override void Write(TableHeapBuffer buffer)
		{
			this.position = buffer.position;
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Field);
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000B4FD File Offset: 0x000096FD
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col2, y.Col2);
		}

		// Token: 0x04000044 RID: 68
		internal int position;
	}
}
