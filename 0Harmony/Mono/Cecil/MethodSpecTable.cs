using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000EE RID: 238
	internal sealed class MethodSpecTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005C6 RID: 1478 RVA: 0x0001A1A8 File Offset: 0x000183A8
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
