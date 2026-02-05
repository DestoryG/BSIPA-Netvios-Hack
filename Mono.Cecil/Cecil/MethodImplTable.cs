using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000033 RID: 51
	internal sealed class MethodImplTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x0600023A RID: 570 RVA: 0x0000B73C File Offset: 0x0000993C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.MethodDefOrRef);
				buffer.WriteCodedRID(this.rows[i].Col3, CodedIndex.MethodDefOrRef);
			}
		}
	}
}
