using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B5 RID: 437
	internal sealed class TableHeap : Heap
	{
		// Token: 0x170002D4 RID: 724
		public TableInformation this[Table table]
		{
			get
			{
				return this.Tables[(int)table];
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0002DFCA File Offset: 0x0002C1CA
		public TableHeap(byte[] data)
			: base(data)
		{
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0002DFE0 File Offset: 0x0002C1E0
		public bool HasTable(Table table)
		{
			return (this.Valid & (1L << (int)table)) != 0L;
		}

		// Token: 0x04000686 RID: 1670
		public long Valid;

		// Token: 0x04000687 RID: 1671
		public long Sorted;

		// Token: 0x04000688 RID: 1672
		public readonly TableInformation[] Tables = new TableInformation[58];
	}
}
