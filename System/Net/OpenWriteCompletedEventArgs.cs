using System;
using System.ComponentModel;
using System.IO;

namespace System.Net
{
	// Token: 0x0200016C RID: 364
	public class OpenWriteCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000DD7 RID: 3543 RVA: 0x00049875 File Offset: 0x00047A75
		internal OpenWriteCompletedEventArgs(Stream result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x00049888 File Offset: 0x00047A88
		public Stream Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04001213 RID: 4627
		private Stream m_Result;
	}
}
