using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000F5 RID: 245
	internal sealed class ImportScopeTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x060005D4 RID: 1492 RVA: 0x0001A494 File Offset: 0x00018694
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteRID(this.rows[i].Col1, Table.ImportScope);
				buffer.WriteBlob(this.rows[i].Col2);
			}
		}
	}
}
