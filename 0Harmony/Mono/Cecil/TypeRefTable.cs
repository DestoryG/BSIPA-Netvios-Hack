using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000CF RID: 207
	internal sealed class TypeRefTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x00019374 File Offset: 0x00017574
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.ResolutionScope);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteString(this.rows[i].Col3);
			}
		}
	}
}
