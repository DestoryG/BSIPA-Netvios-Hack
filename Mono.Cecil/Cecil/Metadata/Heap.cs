using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000E5 RID: 229
	internal abstract class Heap
	{
		// Token: 0x0600097B RID: 2427 RVA: 0x0001EB87 File Offset: 0x0001CD87
		protected Heap(byte[] data)
		{
			this.data = data;
		}

		// Token: 0x040003CA RID: 970
		public int IndexSize;

		// Token: 0x040003CB RID: 971
		internal readonly byte[] data;
	}
}
