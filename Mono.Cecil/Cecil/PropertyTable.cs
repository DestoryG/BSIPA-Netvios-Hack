using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000031 RID: 49
	internal sealed class PropertyTable : MetadataTable<Row<PropertyAttributes, uint, uint>>
	{
		// Token: 0x06000235 RID: 565 RVA: 0x0000B650 File Offset: 0x00009850
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
			}
		}
	}
}
