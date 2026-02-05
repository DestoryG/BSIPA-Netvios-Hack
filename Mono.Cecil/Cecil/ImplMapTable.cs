using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000036 RID: 54
	internal sealed class ImplMapTable : SortedTable<Row<PInvokeAttributes, uint, uint, uint>>
	{
		// Token: 0x06000240 RID: 576 RVA: 0x0000B7FC File Offset: 0x000099FC
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

		// Token: 0x06000241 RID: 577 RVA: 0x0000B879 File Offset: 0x00009A79
		public override int Compare(Row<PInvokeAttributes, uint, uint, uint> x, Row<PInvokeAttributes, uint, uint, uint> y)
		{
			return SortedTable<Row<PInvokeAttributes, uint, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
