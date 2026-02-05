using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F1 RID: 241
	internal sealed class MethodDebugInformationTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005CC RID: 1484 RVA: 0x0001A2CC File Offset: 0x000184CC
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.Document);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}
	}
}
