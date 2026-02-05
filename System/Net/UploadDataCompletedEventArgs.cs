using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000174 RID: 372
	public class UploadDataCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000DEF RID: 3567 RVA: 0x000498F9 File Offset: 0x00047AF9
		internal UploadDataCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x0004990C File Offset: 0x00047B0C
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04001217 RID: 4631
		private byte[] m_Result;
	}
}
