using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000045 RID: 69
	internal sealed class LocalConstantTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000261 RID: 609 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteString(this.rows[i].Col1);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}
	}
}
