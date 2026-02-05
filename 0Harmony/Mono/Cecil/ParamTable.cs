using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D3 RID: 211
	internal sealed class ParamTable : MetadataTable<Row<ParameterAttributes, ushort, uint>>
	{
		// Token: 0x06000586 RID: 1414 RVA: 0x000195C4 File Offset: 0x000177C4
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
