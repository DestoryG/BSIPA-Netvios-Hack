using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000041 RID: 65
	internal sealed class DocumentTable : MetadataTable<Row<uint, uint, uint, uint>>
	{
		// Token: 0x06000259 RID: 601 RVA: 0x0000BDA0 File Offset: 0x00009FA0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteBlob(this.rows[i].Col1);
				buffer.WriteGuid(this.rows[i].Col2);
				buffer.WriteBlob(this.rows[i].Col3);
				buffer.WriteGuid(this.rows[i].Col4);
			}
		}
	}
}
