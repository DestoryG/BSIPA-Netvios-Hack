using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E5 RID: 229
	internal sealed class ImplMapTable : SortedTable<Row<PInvokeAttributes, uint, uint, uint>>
	{
		// Token: 0x060005B1 RID: 1457 RVA: 0x00019CA4 File Offset: 0x00017EA4
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.MemberForwarded);
				buffer.WriteString(this.rows[i].Col3);
				buffer.WriteRID(this.rows[i].Col4, Table.ModuleRef);
			}
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00019D21 File Offset: 0x00017F21
		public override int Compare(Row<PInvokeAttributes, uint, uint, uint> x, Row<PInvokeAttributes, uint, uint, uint> y)
		{
			return SortedTable<Row<PInvokeAttributes, uint, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
