using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200003E RID: 62
	internal sealed class GenericParamTable : MetadataTable<Row<ushort, GenericParameterAttributes, uint, uint>>
	{
		// Token: 0x06000253 RID: 595 RVA: 0x0000BC7C File Offset: 0x00009E7C
		public override void Write(TableHeapBuffer buffer)
		{
			for (int i = 0; i < this.length; i++)
			{
				buffer.WriteUInt16(this.rows[i].Col1);
				buffer.WriteUInt16((ushort)this.rows[i].Col2);
				buffer.WriteCodedRID(this.rows[i].Col3, CodedIndex.TypeOrMethodDef);
				buffer.WriteString(this.rows[i].Col4);
			}
		}
	}
}
