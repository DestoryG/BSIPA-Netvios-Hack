using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000074 RID: 116
	internal class IntRef
	{
		// Token: 0x06000896 RID: 2198 RVA: 0x000280AB File Offset: 0x000262AB
		public IntRef(int value)
		{
			this.value = value;
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x000280BA File Offset: 0x000262BA
		public int Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04000322 RID: 802
		private int value;
	}
}
