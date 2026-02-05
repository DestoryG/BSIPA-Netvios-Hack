using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000020 RID: 32
	internal sealed class TypeRefTable : MetadataTable<Row<uint, uint, uint>>
	{
		// Token: 0x0600020D RID: 525 RVA: 0x0000AECC File Offset: 0x000090CC
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
