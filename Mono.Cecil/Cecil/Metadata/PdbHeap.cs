using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000E6 RID: 230
	internal sealed class PdbHeap : Heap
	{
		// Token: 0x0600097C RID: 2428 RVA: 0x0001E2E7 File Offset: 0x0001C4E7
		public PdbHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0001EB96 File Offset: 0x0001CD96
		public bool HasTable(Table table)
		{
			return (this.TypeSystemTables & (1L << (int)table)) != 0L;
		}

		// Token: 0x040003CC RID: 972
		public byte[] Id;

		// Token: 0x040003CD RID: 973
		public uint EntryPoint;

		// Token: 0x040003CE RID: 974
		public long TypeSystemTables;

		// Token: 0x040003CF RID: 975
		public uint[] TypeSystemTableRows;
	}
}
