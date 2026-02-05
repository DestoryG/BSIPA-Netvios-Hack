using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000032 RID: 50
	internal sealed class MethodSemanticsTable : SortedTable<Row<MethodSemanticsAttributes, uint, uint>>
	{
		// Token: 0x06000237 RID: 567 RVA: 0x0000B6BC File Offset: 0x000098BC
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteRID(this.rows[i].Col2, Table.Method);
				buffer.WriteCodedRID(this.rows[i].Col3, CodedIndex.HasSemantics);
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000B721 File Offset: 0x00009921
		public override int Compare(Row<MethodSemanticsAttributes, uint, uint> x, Row<MethodSemanticsAttributes, uint, uint> y)
		{
			return SortedTable<Row<MethodSemanticsAttributes, uint, uint>>.Compare(x.Col3, y.Col3);
		}
	}
}
