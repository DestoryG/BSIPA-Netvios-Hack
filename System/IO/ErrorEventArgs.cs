using System;

namespace System.IO
{
	// Token: 0x020003FB RID: 1019
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x06002651 RID: 9809 RVA: 0x000B0A6E File Offset: 0x000AEC6E
		public ErrorEventArgs(Exception exception)
		{
			this.exception = exception;
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x000B0A7D File Offset: 0x000AEC7D
		public virtual Exception GetException()
		{
			return this.exception;
		}

		// Token: 0x040020AC RID: 8364
		private Exception exception;
	}
}
