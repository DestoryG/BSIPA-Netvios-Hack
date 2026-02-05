using System;

namespace System.Net
{
	// Token: 0x020001E5 RID: 485
	internal class RequestLifetimeSetter
	{
		// Token: 0x060012DC RID: 4828 RVA: 0x00063E4A File Offset: 0x0006204A
		internal RequestLifetimeSetter(long requestStartTimestamp)
		{
			this.m_RequestStartTimestamp = requestStartTimestamp;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00063E59 File Offset: 0x00062059
		internal static void Report(RequestLifetimeSetter tracker)
		{
			if (tracker != null)
			{
				NetworkingPerfCounters.Instance.IncrementAverage(NetworkingPerfCounterName.HttpWebRequestAvgLifeTime, tracker.m_RequestStartTimestamp);
			}
		}

		// Token: 0x04001528 RID: 5416
		private long m_RequestStartTimestamp;
	}
}
