using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000042 RID: 66
	internal sealed class MethodDebugInformationTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x0600025B RID: 603 RVA: 0x0000BE24 File Offset: 0x0000A024
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
