using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F6 RID: 246
	internal sealed class StateMachineMethodTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005D6 RID: 1494 RVA: 0x0001A4E4 File Offset: 0x000186E4
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.Method);
				buffer.WriteRID(this.rows[i].Col2, Table.Method);
			}
		}
	}
}
