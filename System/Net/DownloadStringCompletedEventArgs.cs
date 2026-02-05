using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x0200016E RID: 366
	public class DownloadStringCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000DDD RID: 3549 RVA: 0x00049896 File Offset: 0x00047A96
		internal DownloadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000DDE RID: 3550 RVA: 0x000498A9 File Offset: 0x00047AA9
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04001214 RID: 4628
		private string m_Result;
	}
}
