using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D1 RID: 209
	internal sealed class FieldTable : MetadataTable<Row<FieldAttributes, uint, uint>>
	{
		// Token: 0x06000582 RID: 1410 RVA: 0x000194A0 File Offset: 0x000176A0
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
