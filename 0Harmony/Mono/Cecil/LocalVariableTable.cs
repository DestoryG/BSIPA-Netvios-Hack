using System;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F3 RID: 243
	internal sealed class LocalVariableTable : MetadataTable<Row<VariableAttributes, ushort, uint>>
	{
		// Token: 0x060005D0 RID: 1488 RVA: 0x0001A3DC File Offset: 0x000185DC
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16((ushort)this.rows[i].Col1);
				buffer.WriteUInt16(this.rows[i].Col2);
				buffer.WriteString(this.rows[i].Col3);
			}
		}
	}
}
