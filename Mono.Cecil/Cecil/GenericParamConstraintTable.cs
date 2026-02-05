using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000040 RID: 64
	internal sealed class GenericParamConstraintTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000BD50 File Offset: 0x00009F50
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.GenericParam);
				buffer.WriteCodedRID(this.rows[i].Col2, CodedIndex.TypeDefOrRef);
			}
		}
	}
}
