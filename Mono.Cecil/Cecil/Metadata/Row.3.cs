using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000E9 RID: 233
	internal struct Row<T1, T2, T3, T4>
	{
		// Token: 0x06000980 RID: 2432 RVA: 0x0001EBD1 File Offset: 0x0001CDD1
		public Row(T1 col1, T2 col2, T3 col3, T4 col4)
		{
			this.Col1 = col1;
			this.Col2 = col2;
			this.Col3 = col3;
			this.Col4 = col4;
		}

		// Token: 0x040003D5 RID: 981
		internal T1 Col1;

		// Token: 0x040003D6 RID: 982
		internal T2 Col2;

		// Token: 0x040003D7 RID: 983
		internal T3 Col3;

		// Token: 0x040003D8 RID: 984
		internal T4 Col4;
	}
}
