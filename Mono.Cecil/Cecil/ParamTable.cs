using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000024 RID: 36
	internal sealed class ParamTable : MetadataTable<Row<ParameterAttributes, ushort, uint>>
	{
		// Token: 0x06000215 RID: 533 RVA: 0x0000B11C File Offset: 0x0000931C
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
