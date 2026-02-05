using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000029 RID: 41
	internal sealed class FieldMarshalTable : SortedTable<Row<uint, uint>>
	{
		// Token: 0x06000221 RID: 545 RVA: 0x0000B348 File Offset: 0x00009548
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.HasFieldMarshal);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B395 File Offset: 0x00009595
		public override int Compare(Row<uint, uint> x, Row<uint, uint> y)
		{
			return SortedTable<Row<uint, uint>>.Compare(x.Col1, y.Col1);
		}
	}
}
