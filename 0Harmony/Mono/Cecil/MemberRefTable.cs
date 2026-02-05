using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D5 RID: 213
	internal sealed class MemberRefTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x0600058A RID: 1418 RVA: 0x00019688 File Offset: 0x00017888
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.MemberRefParent);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}
	}
}
