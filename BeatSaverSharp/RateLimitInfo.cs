using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace BeatSaverSharp
{
	// Token: 0x02000008 RID: 8
	public struct RateLimitInfo
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002C59 File Offset: 0x00000E59
		public RateLimitInfo(int remaining, DateTime reset, int total)
		{
			this.Remaining = remaining;
			this.Reset = reset;
			this.Total = total;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002C70 File Offset: 0x00000E70
		internal static RateLimitInfo? FromHttp(HttpResponseMessage resp)
		{
			DateTime? dateTime = null;
			IEnumerable<string> enumerable;
			if (!resp.Headers.TryGetValues("Rate-Limit-Remaining", out enumerable))
			{
				return null;
			}
			string text = enumerable.FirstOrDefault<string>();
			if (text == null)
			{
				return null;
			}
			int num;
			if (!int.TryParse(text, out num))
			{
				return null;
			}
			int num2 = num;
			IEnumerable<string> enumerable2;
			if (!resp.Headers.TryGetValues("Rate-Limit-Total", out enumerable2))
			{
				return null;
			}
			string text2 = enumerable2.FirstOrDefault<string>();
			if (text2 == null)
			{
				return null;
			}
			int num3;
			if (!int.TryParse(text2, out num3))
			{
				return null;
			}
			int num4 = num3;
			IEnumerable<string> enumerable3;
			if (!resp.Headers.TryGetValues("Rate-Limit-Reset", out enumerable3))
			{
				return null;
			}
			string text3 = enumerable3.FirstOrDefault<string>();
			if (text3 != null)
			{
				ulong num5;
				if (!ulong.TryParse(text3, out num5))
				{
					return null;
				}
				dateTime = new DateTime?(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
				dateTime = ((dateTime != null) ? new DateTime?(dateTime.GetValueOrDefault().AddSeconds(num5).ToLocalTime()) : null);
			}
			if (dateTime == null)
			{
				return null;
			}
			return new RateLimitInfo?(new RateLimitInfo(num2, dateTime.Value, num4));
		}

		// Token: 0x0400000F RID: 15
		public readonly int Remaining;

		// Token: 0x04000010 RID: 16
		public readonly DateTime Reset;

		// Token: 0x04000011 RID: 17
		public readonly int Total;
	}
}
