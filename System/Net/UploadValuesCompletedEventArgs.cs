using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000178 RID: 376
	public class UploadValuesCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000DFB RID: 3579 RVA: 0x0004993B File Offset: 0x00047B3B
		internal UploadValuesCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0004994E File Offset: 0x00047B4E
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04001219 RID: 4633
		private byte[] m_Result;
	}
}
