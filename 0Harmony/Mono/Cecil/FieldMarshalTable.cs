using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D8 RID: 216
	internal sealed class FieldMarshalTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x000197F0 File Offset: 0x000179F0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasFieldMarshal);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001983D File Offset: 0x00017A3D
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
