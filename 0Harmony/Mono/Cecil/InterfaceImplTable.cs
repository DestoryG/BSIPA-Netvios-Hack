using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D4 RID: 212
	internal sealed class InterfaceImplTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000588 RID: 1416 RVA: 0x00019630 File Offset: 0x00017830
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.TypeDef);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.TypeDefOrRef);
			}
		}
	}
}
