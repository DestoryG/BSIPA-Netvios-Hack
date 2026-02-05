using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000038 RID: 56
	internal sealed class AssemblyTable : OneRowTable<Row<AssemblyHashAlgorithm, ushort, ushort, ushort, ushort, AssemblyAttributes, uint, uint, uint>>
	{
		// Token: 0x06000246 RID: 582 RVA: 0x0000B8F0 File Offset: 0x00009AF0
		public override void Write(TableHeapBuffer buffer)
		{
			buffer.WriteUInt32((uint)this.row.Col1);
			buffer.WriteUInt16(this.row.Col2);
			buffer.WriteUInt16(this.row.Col3);
			buffer.WriteUInt16(this.row.Col4);
			buffer.WriteUInt16(this.row.Col5);
			buffer.WriteUInt32((uint)this.row.Col6);
			buffer.WriteBlob(this.row.Col7);
			buffer.WriteString(this.row.Col8);
			buffer.WriteString(this.row.Col9);
		}
	}
}
