using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DE RID: 222
	internal sealed class EventTable : MetadataTable<Row<EventAttributes, uint, uint>>
	{
		// Token: 0x060005A2 RID: 1442 RVA: 0x00019A3C File Offset: 0x00017C3C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteCodedRID(this.rows[i].Col3, CodedIndex.TypeDefOrRef);
			}
		}
	}
}
