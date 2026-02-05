using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F4 RID: 244
	internal sealed class LocalConstantTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005D2 RID: 1490 RVA: 0x0001A448 File Offset: 0x00018648
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
