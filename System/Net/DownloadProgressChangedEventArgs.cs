using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x0200017A RID: 378
	public class DownloadProgressChangedEventArgs : ProgressChangedEventArgs
	{
		// Token: 0x06000E01 RID: 3585 RVA: 0x0004995C File Offset: 0x00047B5C
		internal DownloadProgressChangedEventArgs(int progressPercentage, object userToken, long bytesReceived, long totalBytesToReceive)
			: base(progressPercentage, userToken)
		{
			this.m_BytesReceived = bytesReceived;
			this.m_TotalBytesToReceive = totalBytesToReceive;
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x00049975 File Offset: 0x00047B75
		public long BytesReceived
		{
			get
			{
				return this.m_BytesReceived;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x0004997D File Offset: 0x00047B7D
		public long TotalBytesToReceive
		{
			get
			{
				return this.m_TotalBytesToReceive;
			}
		}

		// Token: 0x0400121A RID: 4634
		private long m_BytesReceived;

		// Token: 0x0400121B RID: 4635
		private long m_TotalBytesToReceive;
	}
}
