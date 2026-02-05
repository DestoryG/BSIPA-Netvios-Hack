using System;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000044 RID: 68
	internal sealed class LocalVariableTable : MetadataTable<Row<VariableAttributes, ushort, uint>>
	{
		// Token: 0x0600025F RID: 607 RVA: 0x0000BF34 File Offset: 0x0000A134
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
