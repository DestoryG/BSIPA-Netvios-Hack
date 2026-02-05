using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E4 RID: 228
	internal sealed class TypeSpecTable : MetadataTable<uint>
	{
		// Token: 0x060005AF RID: 1455 RVA: 0x00019C78 File Offset: 0x00017E78
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteBlob(this.rows[i]);
			}
		}
	}
}
