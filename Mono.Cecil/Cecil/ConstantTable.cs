using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000027 RID: 39
	internal sealed class ConstantTable : SortedTable<Row<ElementType, uint, uint>>
	{
		// Token: 0x0600021B RID: 539 RVA: 0x0000B244 File Offset: 0x00009444
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.HasConstant);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000B2A8 File Offset: 0x000094A8
		public override int Compare(Row<ElementType, uint, uint> x, Row<ElementType, uint, uint> y)
		{
			return SortedTable<Row<ElementType, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
