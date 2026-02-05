using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D7 RID: 215
	internal sealed class CustomAttributeTable : SortedTable<Row<uint, uint, uint>>
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x0001976C File Offset: 0x0001796C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasCustomAttribute);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.CustomAttributeType);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x000197D2 File Offset: 0x000179D2
		public override int Compare(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return SortedTable<Row<uint, uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
