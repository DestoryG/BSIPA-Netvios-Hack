using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000026 RID: 38
	internal sealed class MemberRefTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x06000219 RID: 537 RVA: 0x0000B1E0 File Offset: 0x000093E0
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
