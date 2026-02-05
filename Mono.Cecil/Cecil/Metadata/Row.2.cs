using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000E8 RID: 232
	internal struct Row<T1, T2, T3>
	{
		// Token: 0x0600097F RID: 2431 RVA: 0x0001EBBA File Offset: 0x0001CDBA
		public Row(T1 col1, T2 col2, T3 col3)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
		}

		// Token: 0x040003D2 RID: 978
		internal T1 Col1;

		// Token: 0x040003D3 RID: 979
		internal T2 Col2;

		// Token: 0x040003D4 RID: 980
		internal T3 Col3;
	}
}
