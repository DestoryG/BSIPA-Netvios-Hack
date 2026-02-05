using System;

namespace System.Timers
{
	// Token: 0x0200006B RID: 107
	public class ElapsedEventArgs : EventArgs
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x0001EF7C File Offset: 0x0001D17C
		internal ElapsedEventArgs(int low, int high)
		{
			long num = ((long)high << 32) | ((long)low & (long)((ulong)(-1)));
			this.signalTime = DateTime.FromFileTime(num);
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x0001EFA7 File Offset: 0x0001D1A7
		public DateTime SignalTime
		{
			get
			{
				return this.signalTime;
			}
		}

		// Token: 0x04000BD0 RID: 3024
		private DateTime signalTime;
	}
}
