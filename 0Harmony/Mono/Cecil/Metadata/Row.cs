using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AB RID: 427
	internal struct Row<T1, T2>
	{
		// Token: 0x06000D62 RID: 3426 RVA: 0x0002DD42 File Offset: 0x0002BF42
		public Row(T1 col1, T2 col2)
		{
			this.Col1 = col1;
			this.Col2 = col2;
		}

		// Token: 0x0400062F RID: 1583
		internal T1 Col1;

		// Token: 0x04000630 RID: 1584
		internal T2 Col2;
	}
}
