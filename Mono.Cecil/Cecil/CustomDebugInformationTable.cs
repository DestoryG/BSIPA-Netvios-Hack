using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000048 RID: 72
	internal sealed class CustomDebugInformationTable : SortedTable<Row<uint, uint, uint>>
	{
		// Token: 0x06000267 RID: 615 RVA: 0x0000C08C File Offset: 0x0000A28C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasCustomDebugInformation);
				buffer.WriteGuid(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000B32A File Offset: 0x0000952A
		public override int Compare(Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return SortedTable<Row<uint, uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
