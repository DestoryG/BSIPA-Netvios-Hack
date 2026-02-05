using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DC RID: 220
	internal sealed class StandAloneSigTable : MetadataTable<uint>
	{
		// Token: 0x0600059E RID: 1438 RVA: 0x000199B8 File Offset: 0x00017BB8
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteBlob(this.rows[i]);
			}
		}
	}
}
