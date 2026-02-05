using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000EB RID: 235
	internal struct Row<T1, T2, T3, T4, T5, T6>
	{
		// Token: 0x06000982 RID: 2434 RVA: 0x0001EC17 File Offset: 0x0001CE17
		public Row(T1 col1, T2 col2, T3 col3, T4 col4, T5 col5, T6 col6)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
			this.Col5 = col5;
			this.Col6 = col6;
		}

		// Token: 0x040003DE RID: 990
		internal T1 Col1;

		// Token: 0x040003DF RID: 991
		internal T2 Col2;

		// Token: 0x040003E0 RID: 992
		internal T3 Col3;

		// Token: 0x040003E1 RID: 993
		internal T4 Col4;

		// Token: 0x040003E2 RID: 994
		internal T5 Col5;

		// Token: 0x040003E3 RID: 995
		internal T6 Col6;
	}
}
