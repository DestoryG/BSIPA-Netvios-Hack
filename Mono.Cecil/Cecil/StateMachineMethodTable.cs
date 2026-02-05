using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000047 RID: 71
	internal sealed class StateMachineMethodTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0000C03C File Offset: 0x0000A23C
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
