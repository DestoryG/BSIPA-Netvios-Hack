using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000046 RID: 70
	internal sealed class ImportScopeTable : MetadataTable<Row<uint, uint>>
	{
		// Token: 0x06000263 RID: 611 RVA: 0x0000BFEC File Offset: 0x0000A1EC
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
