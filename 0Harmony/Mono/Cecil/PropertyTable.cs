using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E0 RID: 224
	internal sealed class PropertyTable : MetadataTable<Row<PropertyAttributes, uint, uint>>
	{
		// Token: 0x060005A6 RID: 1446 RVA: 0x00019AF8 File Offset: 0x00017CF8
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
