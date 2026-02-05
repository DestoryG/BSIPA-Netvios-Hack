using System;

namespace System.Threading
{
	// Token: 0x020003D7 RID: 983
	public class ThreadExceptionEventArgs : EventArgs
	{
		// Token: 0x060025D9 RID: 9689 RVA: 0x000AFD76 File Offset: 0x000ADF76
		public ThreadExceptionEventArgs(Exception t)
		{
			this.exception = t;
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x060025DA RID: 9690 RVA: 0x000AFD85 File Offset: 0x000ADF85
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x04002066 RID: 8294
		private Exception exception;
	}
}
