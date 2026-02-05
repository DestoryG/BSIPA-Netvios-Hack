using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200002F RID: 47
	internal sealed class EventTable : MetadataTable<Row<EventAttributes, uint, uint>>
	{
		// Token: 0x06000231 RID: 561 RVA: 0x0000B594 File Offset: 0x00009794
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
