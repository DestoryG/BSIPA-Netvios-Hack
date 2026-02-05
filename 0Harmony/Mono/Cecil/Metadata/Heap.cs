using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A9 RID: 425
	internal abstract class Heap
	{
		// Token: 0x06000D5F RID: 3423 RVA: 0x0002DD1F File Offset: 0x0002BF1F
		protected Heap(byte[] data)
		{
			this.data = data;
		}

		// Token: 0x04000629 RID: 1577
		public int IndexSize;

		// Token: 0x0400062A RID: 1578
		internal readonly byte[] data;
	}
}
