using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000DF RID: 223
	internal sealed class PropertyMapTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005A4 RID: 1444 RVA: 0x00019AA8 File Offset: 0x00017CA8
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteRID(this.rows[i].Col2, Table.Property);
			}
		}
	}
}
