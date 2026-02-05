using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000E2 RID: 226
	internal sealed class MethodImplTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x060005AB RID: 1451 RVA: 0x00019BE4 File Offset: 0x00017DE4
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
