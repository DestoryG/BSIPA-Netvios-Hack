using System;
using System.ComponentModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002E9 RID: 745
	public class PingCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06001A19 RID: 6681 RVA: 0x0007EA68 File Offset: 0x0007CC68
		internal PingCompletedEventArgs(PingReply reply, Exception error, bool cancelled, object userToken)
			: base(error, cancelled, userToken)
		{
			this.reply = reply;
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x0007EA7B File Offset: 0x0007CC7B
		public PingReply Reply
		{
			get
			{
				return this.reply;
			}
		}

		// Token: 0x04001A64 RID: 6756
		private PingReply reply;
	}
}
