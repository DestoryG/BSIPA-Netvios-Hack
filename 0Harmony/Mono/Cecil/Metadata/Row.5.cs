using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AF RID: 431
	internal struct Row<T1, T2, T3, T4, T5, T6>
	{
		// Token: 0x06000D66 RID: 3430 RVA: 0x0002DDAF File Offset: 0x0002BFAF
		public Row(T1 col1, T2 col2, T3 col3, T4 col4, T5 col5, T6 col6)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
			this.Col5 = col5;
			this.Col6 = col6;
		}

		// Token: 0x0400063D RID: 1597
		internal T1 Col1;

		// Token: 0x0400063E RID: 1598
		internal T2 Col2;

		// Token: 0x0400063F RID: 1599
		internal T3 Col3;

		// Token: 0x04000640 RID: 1600
		internal T4 Col4;

		// Token: 0x04000641 RID: 1601
		internal T5 Col5;

		// Token: 0x04000642 RID: 1602
		internal T6 Col6;
	}
}
