using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000176 RID: 374
	public class UploadFileCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000DF5 RID: 3573 RVA: 0x0004991A File Offset: 0x00047B1A
		internal UploadFileCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0004992D File Offset: 0x00047B2D
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04001218 RID: 4632
		private byte[] m_Result;
	}
}
