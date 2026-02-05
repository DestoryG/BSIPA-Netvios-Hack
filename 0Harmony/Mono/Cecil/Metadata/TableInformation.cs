using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B4 RID: 436
	internal struct TableInformation
	{
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0002DFAD File Offset: 0x0002C1AD
		public bool IsLarge
		{
			get
			{
				return this.Length > 65535U;
			}
		}

		// Token: 0x04000683 RID: 1667
		public uint Offset;

		// Token: 0x04000684 RID: 1668
		public uint Length;

		// Token: 0x04000685 RID: 1669
		public uint RowSize;
	}
}
