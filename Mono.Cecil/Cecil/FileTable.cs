using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200003A RID: 58
	internal sealed class FileTable : MetadataTable<Row<FileAttributes, uint, uint>>
	{
		// Token: 0x0600024A RID: 586 RVA: 0x0000BA9C File Offset: 0x00009C9C
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
