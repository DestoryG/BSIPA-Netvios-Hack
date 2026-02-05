using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200001F RID: 31
	internal sealed class ModuleTable : OneRowTable<Row<uint, uint>>
	{
		// Token: 0x0600020B RID: 523 RVA: 0x0000AE88 File Offset: 0x00009088
		public override void Write(TableHeapBuffer buffer)
		{
			buffer.WriteUInt16(0);
			buffer.WriteString(this.row.Col1);
			buffer.WriteGuid(this.row.Col2);
			buffer.WriteUInt16(0);
			buffer.WriteUInt16(0);
		}
	}
}
