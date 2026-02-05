using System;
using System.Net.Http;

namespace BeatSaverSharp.Exceptions
{
	// Token: 0x0200001B RID: 27
	public class RateLimitExceededException : Exception
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00003B3C File Offset: 0x00001D3C
		public RateLimitExceededException(RateLimitInfo info)
		{
			this.RateLimit = info;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003B4C File Offset: 0x00001D4C
		public RateLimitExceededException(HttpResponseMessage resp)
		{
			RateLimitInfo? rateLimitInfo = RateLimitInfo.FromHttp(resp);
			if (rateLimitInfo == null)
			{
				throw new Exception("Could not parse rate limit info");
			}
			this.RateLimit = rateLimitInfo.Value;
		}

		// Token: 0x0400005E RID: 94
		public readonly RateLimitInfo RateLimit;
	}
}
