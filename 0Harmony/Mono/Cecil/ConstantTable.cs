using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D6 RID: 214
	internal sealed class ConstantTable : SortedTable<Row<ElementType, uint, uint>>
	{
		// Token: 0x0600058C RID: 1420 RVA: 0x000196EC File Offset: 0x000178EC
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.HasConstant);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00019750 File Offset: 0x00017950
		public override int Compare(Row<ElementType, uint, uint> x, Row<ElementType, uint, uint> y)
		{
			return SortedTable<Row<ElementType, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
