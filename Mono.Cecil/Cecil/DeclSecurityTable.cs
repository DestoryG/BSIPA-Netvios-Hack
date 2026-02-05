using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200002A RID: 42
	internal sealed class DeclSecurityTable : SortedTable<Row<SecurityAction, uint, uint>>
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000B3B0 File Offset: 0x000095B0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.HasDeclSecurity);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B414 File Offset: 0x00009614
		public override int Compare(Row<SecurityAction, uint, uint> x, Row<SecurityAction, uint, uint> y)
		{
			return SortedTable<Row<SecurityAction, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
