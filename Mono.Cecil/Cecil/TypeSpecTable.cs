using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000035 RID: 53
	internal sealed class TypeSpecTable : MetadataTable<uint>
	{
		// Token: 0x0600023E RID: 574 RVA: 0x0000B7D0 File Offset: 0x000099D0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteBlob(this.rows[i]);
			}
		}
	}
}
