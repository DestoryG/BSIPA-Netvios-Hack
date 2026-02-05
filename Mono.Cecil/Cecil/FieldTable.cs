using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000022 RID: 34
	internal sealed class FieldTable : MetadataTable<Row<FieldAttributes, uint, uint>>
	{
		// Token: 0x06000211 RID: 529 RVA: 0x0000AFF8 File Offset: 0x000091F8
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}
	}
}
