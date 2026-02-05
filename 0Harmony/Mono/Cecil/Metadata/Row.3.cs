using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AD RID: 429
	internal struct Row<T1, T2, T3, T4>
	{
		// Token: 0x06000D64 RID: 3428 RVA: 0x0002DD69 File Offset: 0x0002BF69
		public Row(T1 col1, T2 col2, T3 col3, T4 col4)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
		}

		// Token: 0x04000634 RID: 1588
		internal T1 Col1;

		// Token: 0x04000635 RID: 1589
		internal T2 Col2;

		// Token: 0x04000636 RID: 1590
		internal T3 Col3;

		// Token: 0x04000637 RID: 1591
		internal T4 Col4;
	}
}
