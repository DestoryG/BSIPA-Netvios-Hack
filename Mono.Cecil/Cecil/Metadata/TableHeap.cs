using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000F1 RID: 241
	internal sealed class TableHeap : Heap
	{
		// Token: 0x170002B4 RID: 692
		public TableInformation this[Table table]
		{
			get
			{
				return this.Tables[(int)table];
			}
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0001EE32 File Offset: 0x0001D032
		public TableHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0001EE48 File Offset: 0x0001D048
		public bool HasTable(Table table)
		{
			return (this.Valid & (1L << (int)table)) != 0L;
		}

		// Token: 0x04000427 RID: 1063
		public long Valid;

		// Token: 0x04000428 RID: 1064
		public long Sorted;

		// Token: 0x04000429 RID: 1065
		public readonly TableInformation[] Tables = new TableInformation[58];
	}
}
