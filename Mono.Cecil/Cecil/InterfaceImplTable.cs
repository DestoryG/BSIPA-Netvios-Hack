using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000025 RID: 37
	internal sealed class InterfaceImplTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000217 RID: 535 RVA: 0x0000B188 File Offset: 0x00009388
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
