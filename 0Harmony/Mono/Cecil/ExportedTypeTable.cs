using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000EA RID: 234
	internal sealed class ExportedTypeTable : MetadataTable<Row<TypeAttributes, uint, uint, uint, uint>>
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x00019FB0 File Offset: 0x000181B0
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32((uint)this.rows[i].Col1);
				buffer.WriteUInt32(this.rows[i].Col2);
				buffer.WriteString(this.rows[i].Col3);
				buffer.WriteString(this.rows[i].Col4);
				buffer.WriteCodedRID(this.rows[i].Col5, CodedIndex.Implementation);
			}
		}
	}
}
