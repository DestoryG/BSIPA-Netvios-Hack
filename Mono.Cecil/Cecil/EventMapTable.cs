using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200002E RID: 46
	internal sealed class EventMapTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x0600022F RID: 559 RVA: 0x0000B544 File Offset: 0x00009744
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteRID(this.rows[i].Col2, Table.Event);
			}
		}
	}
}
