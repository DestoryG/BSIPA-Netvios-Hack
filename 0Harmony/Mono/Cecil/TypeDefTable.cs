using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000D0 RID: 208
	internal sealed class TypeDefTable : MetadataTable<Row<TypeAttributes, uint, uint, uint, uint, uint>>
	{
		// Token: 0x06000580 RID: 1408 RVA: 0x000193E4 File Offset: 0x000175E4
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt32((uint)this.rows[i].Col1);
				buffer.WriteString(this.rows[i].Col2);
				buffer.WriteString(this.rows[i].Col3);
				buffer.WriteCodedRID(this.rows[i].Col4, CodedIndex.TypeDefOrRef);
				buffer.WriteRID(this.rows[i].Col5, Table.Field);
				buffer.WriteRID(this.rows[i].Col6, Table.Method);
			}
		}
	}
}
