using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000E7 RID: 231
	internal struct Row<T1, T2>
	{
		// Token: 0x0600097E RID: 2430 RVA: 0x0001EBAA File Offset: 0x0001CDAA
		public Row(T1 col1, T2 col2)
		{
			this.Col1 = col1;
			this.Col2 = col2;
		}

		// Token: 0x040003D0 RID: 976
		internal T1 Col1;

		// Token: 0x040003D1 RID: 977
		internal T2 Col2;
	}
}
