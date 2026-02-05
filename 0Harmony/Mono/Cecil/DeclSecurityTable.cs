using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D9 RID: 217
	internal sealed class DeclSecurityTable : SortedTable<Row<SecurityAction, uint, uint>>
	{
		// Token: 0x06000595 RID: 1429 RVA: 0x00019858 File Offset: 0x00017A58
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.HasDeclSecurity);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x000198BC File Offset: 0x00017ABC
		public override int Compare(Row<SecurityAction, uint, uint> x, Row<SecurityAction, uint, uint> y)
		{
			return SortedTable<Row<SecurityAction, uint, uint>>.Compare(x.Col2, y.Col2);
		}
	}
}
