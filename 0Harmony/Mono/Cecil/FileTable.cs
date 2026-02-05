using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E9 RID: 233
	internal sealed class FileTable : MetadataTable<Row<FileAttributes, uint, uint>>
	{
		// Token: 0x060005BB RID: 1467 RVA: 0x00019F44 File Offset: 0x00018144
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32((uint)this.rows[i].Col1);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}
	}
}
