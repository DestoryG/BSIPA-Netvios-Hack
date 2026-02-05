using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001B0 RID: 432
	internal struct Row<T1, T2, T3, T4, T5, T6, T7, T8, T9>
	{
		// Token: 0x06000D67 RID: 3431 RVA: 0x0002DDE0 File Offset: 0x0002BFE0
		public Row(T1 col1, T2 col2, T3 col3, T4 col4, T5 col5, T6 col6, T7 col7, T8 col8, T9 col9)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
			this.Col5 = col5;
			this.Col6 = col6;
			this.Col7 = col7;
			this.Col8 = col8;
			this.Col9 = col9;
		}

		// Token: 0x04000643 RID: 1603
		internal T1 Col1;

		// Token: 0x04000644 RID: 1604
		internal T2 Col2;

		// Token: 0x04000645 RID: 1605
		internal T3 Col3;

		// Token: 0x04000646 RID: 1606
		internal T4 Col4;

		// Token: 0x04000647 RID: 1607
		internal T5 Col5;

		// Token: 0x04000648 RID: 1608
		internal T6 Col6;

		// Token: 0x04000649 RID: 1609
		internal T7 Col7;

		// Token: 0x0400064A RID: 1610
		internal T8 Col8;

		// Token: 0x0400064B RID: 1611
		internal T9 Col9;
	}
}
