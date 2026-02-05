using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000EC RID: 236
	internal struct Row<T1, T2, T3, T4, T5, T6, T7, T8, T9>
	{
		// Token: 0x06000983 RID: 2435 RVA: 0x0001EC48 File Offset: 0x0001CE48
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

		// Token: 0x040003E4 RID: 996
		internal T1 Col1;

		// Token: 0x040003E5 RID: 997
		internal T2 Col2;

		// Token: 0x040003E6 RID: 998
		internal T3 Col3;

		// Token: 0x040003E7 RID: 999
		internal T4 Col4;

		// Token: 0x040003E8 RID: 1000
		internal T5 Col5;

		// Token: 0x040003E9 RID: 1001
		internal T6 Col6;

		// Token: 0x040003EA RID: 1002
		internal T7 Col7;

		// Token: 0x040003EB RID: 1003
		internal T8 Col8;

		// Token: 0x040003EC RID: 1004
		internal T9 Col9;
	}
}
