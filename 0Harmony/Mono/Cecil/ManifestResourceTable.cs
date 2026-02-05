using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000EB RID: 235
	internal sealed class ManifestResourceTable : MetadataTable<Row<uint, ManifestResourceAttributes, uint, uint>>
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x0001A050 File Offset: 0x00018250
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32(this.rows[i].Col1);
				buffer.WriteUInt32((uint)this.rows[i].Col2);
				buffer.WriteString(this.rows[i].Col3);
				buffer.WriteCodedRID(this.rows[i].Col4, CodedIndex.Implementation);
			}
		}
	}
}
