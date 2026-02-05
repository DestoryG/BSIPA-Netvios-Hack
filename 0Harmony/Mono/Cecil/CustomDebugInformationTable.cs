using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F7 RID: 247
	internal sealed class CustomDebugInformationTable : SortedTable<Row<uint, uint, uint>>
	{
		// Token: 0x060005D8 RID: 1496 RVA: 0x0001A534 File Offset: 0x00018734
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasCustomDebugInformation);
				buffer.WriteGuid(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x000197D2 File Offset: 0x000179D2
		public override int Compare(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return SortedTable<Row<uint, uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
