using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200003C RID: 60
	internal sealed class ManifestResourceTable : MetadataTable<Row<uint, ManifestResourceAttributes, uint, uint>>
	{
		// Token: 0x0600024E RID: 590 RVA: 0x0000BBA8 File Offset: 0x00009DA8
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
