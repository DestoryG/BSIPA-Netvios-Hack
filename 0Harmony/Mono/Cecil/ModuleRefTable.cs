using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E3 RID: 227
	internal sealed class ModuleRefTable : MetadataTable<uint>
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x00019C4C File Offset: 0x00017E4C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteString(this.rows[i]);
			}
		}
	}
}
