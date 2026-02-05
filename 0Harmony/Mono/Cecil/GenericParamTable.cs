using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000ED RID: 237
	internal sealed class GenericParamTable : MetadataTable<Row<ushort, GenericParameterAttributes, uint, uint>>
	{
		// Token: 0x060005C4 RID: 1476 RVA: 0x0001A124 File Offset: 0x00018324
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
