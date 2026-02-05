using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AA RID: 426
	internal sealed class PdbHeap : Heap
	{
		// Token: 0x06000D60 RID: 3424 RVA: 0x0002D3FF File Offset: 0x0002B5FF
		public PdbHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0002DD2E File Offset: 0x0002BF2E
		public bool HasTable(Table table)
		{
			return (this.TypeSystemTables & (1L << (int)table)) != 0L;
		}

		// Token: 0x0400062B RID: 1579
		public byte[] Id;

		// Token: 0x0400062C RID: 1580
		public uint EntryPoint;

		// Token: 0x0400062D RID: 1581
		public long TypeSystemTables;

		// Token: 0x0400062E RID: 1582
		public uint[] TypeSystemTableRows;
	}
}
