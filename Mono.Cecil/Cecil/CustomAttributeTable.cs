using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000028 RID: 40
	internal sealed class CustomAttributeTable : SortedTable<Row<uint, uint, uint>>
	{
		// Token: 0x0600021E RID: 542 RVA: 0x0000B2C4 File Offset: 0x000094C4
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasCustomAttribute);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.CustomAttributeType);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000B32A File Offset: 0x0000952A
		public override int Compare(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return SortedTable<Row<uint, uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
