using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x0200017C RID: 380
	public class UploadProgressChangedEventArgs : ProgressChangedEventArgs
	{
		// Token: 0x06000E08 RID: 3592 RVA: 0x00049985 File Offset: 0x00047B85
		internal UploadProgressChangedEventArgs(int progressPercentage, object userToken, long bytesSent, long totalBytesToSend, long bytesReceived, long totalBytesToReceive)
			: base(progressPercentage, userToken)
		{
			this.m_BytesReceived = bytesReceived;
			this.m_TotalBytesToReceive = totalBytesToReceive;
			this.m_BytesSent = bytesSent;
			this.m_TotalBytesToSend = totalBytesToSend;
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x000499AE File Offset: 0x00047BAE
		public long BytesReceived
		{
			get
			{
				return this.m_BytesReceived;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x000499B6 File Offset: 0x00047BB6
		public long TotalBytesToReceive
		{
			get
			{
				return this.m_TotalBytesToReceive;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x000499BE File Offset: 0x00047BBE
		public long BytesSent
		{
			get
			{
				return this.m_BytesSent;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x000499C6 File Offset: 0x00047BC6
		public long TotalBytesToSend
		{
			get
			{
				return this.m_TotalBytesToSend;
			}
		}

		// Token: 0x0400121C RID: 4636
		private long m_BytesReceived;

		// Token: 0x0400121D RID: 4637
		private long m_TotalBytesToReceive;

		// Token: 0x0400121E RID: 4638
		private long m_BytesSent;

		// Token: 0x0400121F RID: 4639
		private long m_TotalBytesToSend;
	}
}
