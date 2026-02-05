using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200002D RID: 45
	internal sealed class StandAloneSigTable : MetadataTable<uint>
	{
		// Token: 0x0600022D RID: 557 RVA: 0x0000B510 File Offset: 0x00009710
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteBlob(this.rows[i]);
			}
		}
	}
}
