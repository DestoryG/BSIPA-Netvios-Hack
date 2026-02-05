using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DD RID: 221
	internal sealed class EventMapTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005A0 RID: 1440 RVA: 0x000199EC File Offset: 0x00017BEC
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
