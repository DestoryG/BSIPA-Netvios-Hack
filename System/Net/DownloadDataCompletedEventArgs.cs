using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000170 RID: 368
	public class DownloadDataCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000DE3 RID: 3555 RVA: 0x000498B7 File Offset: 0x00047AB7
		internal DownloadDataCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x000498CA File Offset: 0x00047ACA
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04001215 RID: 4629
		private byte[] m_Result;
	}
}
