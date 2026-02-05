using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AE RID: 430
	internal struct Row<T1, T2, T3, T4, T5>
	{
		// Token: 0x06000D65 RID: 3429 RVA: 0x0002DD88 File Offset: 0x0002BF88
		public Row(T1 col1, T2 col2, T3 col3, T4 col4, T5 col5)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
			this.Col5 = col5;
		}

		// Token: 0x04000638 RID: 1592
		internal T1 Col1;

		// Token: 0x04000639 RID: 1593
		internal T2 Col2;

		// Token: 0x0400063A RID: 1594
		internal T3 Col3;

		// Token: 0x0400063B RID: 1595
		internal T4 Col4;

		// Token: 0x0400063C RID: 1596
		internal T5 Col5;
	}
}
