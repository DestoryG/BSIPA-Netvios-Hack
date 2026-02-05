using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000CE RID: 206
	internal sealed class ModuleTable : OneRowTable<Row<uint, uint>>
	{
		// Token: 0x0600057C RID: 1404 RVA: 0x00019330 File Offset: 0x00017530
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
