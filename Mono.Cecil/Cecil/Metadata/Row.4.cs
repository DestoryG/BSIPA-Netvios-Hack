using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000EA RID: 234
	internal struct Row<T1, T2, T3, T4, T5>
	{
		// Token: 0x06000981 RID: 2433 RVA: 0x0001EBF0 File Offset: 0x0001CDF0
		public Row(T1 col1, T2 col2, T3 col3, T4 col4, T5 col5)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
			this.Col5 = col5;
		}

		// Token: 0x040003D9 RID: 985
		internal T1 Col1;

		// Token: 0x040003DA RID: 986
		internal T2 Col2;

		// Token: 0x040003DB RID: 987
		internal T3 Col3;

		// Token: 0x040003DC RID: 988
		internal T4 Col4;

		// Token: 0x040003DD RID: 989
		internal T5 Col5;
	}
}
