using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E1 RID: 225
	internal sealed class MethodSemanticsTable : SortedTable<Row<MethodSemanticsAttributes, uint, uint>>
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x00019B64 File Offset: 0x00017D64
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Method);
				buffer.WriteCodedRID(this.rows[i].Col3, CodedIndex.HasSemantics);
			}
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00019BC9 File Offset: 0x00017DC9
		public override int Compare(Row<MethodSemanticsAttributes, uint, uint> x, Row<MethodSemanticsAttributes, uint, uint> y)
		{
			return SortedTable<Row<MethodSemanticsAttributes, uint, uint>>.Compare(x.Col3, y.Col3);
		}
	}
}
