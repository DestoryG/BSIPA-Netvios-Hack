using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001AC RID: 428
	internal struct Row<T1, T2, T3>
	{
		// Token: 0x06000D63 RID: 3427 RVA: 0x0002DD52 File Offset: 0x0002BF52
		public Row(T1 col1, T2 col2, T3 col3)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
		}

		// Token: 0x04000631 RID: 1585
		internal T1 Col1;

		// Token: 0x04000632 RID: 1586
		internal T2 Col2;

		// Token: 0x04000633 RID: 1587
		internal T3 Col3;
	}
}
