using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000EF RID: 239
	internal sealed class GenericParamConstraintTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005C8 RID: 1480 RVA: 0x0001A1F8 File Offset: 0x000183F8
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
