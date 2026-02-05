using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000034 RID: 52
	internal sealed class ModuleRefTable : MetadataTable<uint>
	{
		// Token: 0x0600023C RID: 572 RVA: 0x0000B7A4 File Offset: 0x000099A4
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteString(this.rows[i]);
			}
		}
	}
}
