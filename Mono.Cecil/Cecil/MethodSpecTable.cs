using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200003F RID: 63
	internal sealed class MethodSpecTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000255 RID: 597 RVA: 0x0000BD00 File Offset: 0x00009F00
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteCodedRID(this.rows[i].Col1, CodedIndex.MethodDefOrRef);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}
	}
}
