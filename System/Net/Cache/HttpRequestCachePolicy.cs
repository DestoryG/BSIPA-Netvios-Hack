using System;
using System.Globalization;

namespace System.Net.Cache
{
	// Token: 0x02000315 RID: 789
	public class HttpRequestCachePolicy : RequestCachePolicy
	{
		// Token: 0x06001C12 RID: 7186 RVA: 0x00085B99 File Offset: 0x00083D99
		public HttpRequestCachePolicy()
			: this(HttpRequestCacheLevel.Default)
		{
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x00085BA4 File Offset: 0x00083DA4
		public HttpRequestCachePolicy(HttpRequestCacheLevel level)
			: base(HttpRequestCachePolicy.MapLevel(level))
		{
			this.m_Level = level;
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x00085BF0 File Offset: 0x00083DF0
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan ageOrFreshOrStale)
			: this(HttpRequestCacheLevel.Default)
		{
			switch (cacheAgeControl)
			{
			case HttpCacheAgeControl.MinFresh:
				this.m_MinFresh = ageOrFreshOrStale;
				return;
			case HttpCacheAgeControl.MaxAge:
				this.m_MaxAge = ageOrFreshOrStale;
				return;
			case HttpCacheAgeControl.MaxStale:
				this.m_MaxStale = ageOrFreshOrStale;
				return;
			}
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "HttpCacheAgeControl" }), "cacheAgeControl");
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x00085C58 File Offset: 0x00083E58
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale)
			: this(HttpRequestCacheLevel.Default)
		{
			switch (cacheAgeControl)
			{
			case HttpCacheAgeControl.MinFresh:
				this.m_MinFresh = freshOrStale;
				return;
			case HttpCacheAgeControl.MaxAge:
				this.m_MaxAge = maxAge;
				return;
			case HttpCacheAgeControl.MaxAgeAndMinFresh:
				this.m_MaxAge = maxAge;
				this.m_MinFresh = freshOrStale;
				return;
			case HttpCacheAgeControl.MaxStale:
				this.m_MaxStale = freshOrStale;
				return;
			case HttpCacheAgeControl.MaxAgeAndMaxStale:
				this.m_MaxAge = maxAge;
				this.m_MaxStale = freshOrStale;
				return;
			}
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "HttpCacheAgeControl" }), "cacheAgeControl");
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x00085CE6 File Offset: 0x00083EE6
		public HttpRequestCachePolicy(DateTime cacheSyncDate)
			: this(HttpRequestCacheLevel.Default)
		{
			this.m_LastSyncDateUtc = cacheSyncDate.ToUniversalTime();
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x00085CFC File Offset: 0x00083EFC
		public HttpRequestCachePolicy(HttpCacheAgeControl cacheAgeControl, TimeSpan maxAge, TimeSpan freshOrStale, DateTime cacheSyncDate)
			: this(cacheAgeControl, maxAge, freshOrStale)
		{
			this.m_LastSyncDateUtc = cacheSyncDate.ToUniversalTime();
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001C18 RID: 7192 RVA: 0x00085D14 File Offset: 0x00083F14
		public new HttpRequestCacheLevel Level
		{
			get
			{
				return this.m_Level;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001C19 RID: 7193 RVA: 0x00085D1C File Offset: 0x00083F1C
		public DateTime CacheSyncDate
		{
			get
			{
				if (this.m_LastSyncDateUtc == DateTime.MinValue || this.m_LastSyncDateUtc == DateTime.MaxValue)
				{
					return this.m_LastSyncDateUtc;
				}
				return this.m_LastSyncDateUtc.ToLocalTime();
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x00085D54 File Offset: 0x00083F54
		internal DateTime InternalCacheSyncDateUtc
		{
			get
			{
				return this.m_LastSyncDateUtc;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06001C1B RID: 7195 RVA: 0x00085D5C File Offset: 0x00083F5C
		public TimeSpan MaxAge
		{
			get
			{
				return this.m_MaxAge;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06001C1C RID: 7196 RVA: 0x00085D64 File Offset: 0x00083F64
		public TimeSpan MinFresh
		{
			get
			{
				return this.m_MinFresh;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x00085D6C File Offset: 0x00083F6C
		public TimeSpan MaxStale
		{
			get
			{
				return this.m_MaxStale;
			}
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x00085D74 File Offset: 0x00083F74
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Level:",
				this.m_Level.ToString(),
				(this.m_MaxAge == TimeSpan.MaxValue) ? string.Empty : (" MaxAge:" + this.m_MaxAge.ToString()),
				(this.m_MinFresh == TimeSpan.MinValue) ? string.Empty : (" MinFresh:" + this.m_MinFresh.ToString()),
				(this.m_MaxStale == TimeSpan.MinValue) ? string.Empty : (" MaxStale:" + this.m_MaxStale.ToString()),
				(this.CacheSyncDate == DateTime.MinValue) ? string.Empty : (" CacheSyncDate:" + this.CacheSyncDate.ToString(CultureInfo.CurrentCulture))
			});
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x00085E86 File Offset: 0x00084086
		private static RequestCacheLevel MapLevel(HttpRequestCacheLevel level)
		{
			if (level <= HttpRequestCacheLevel.NoCacheNoStore)
			{
				return (RequestCacheLevel)level;
			}
			if (level == HttpRequestCacheLevel.CacheOrNextCacheOnly)
			{
				return RequestCacheLevel.CacheOnly;
			}
			if (level == HttpRequestCacheLevel.Refresh)
			{
				return RequestCacheLevel.Reload;
			}
			throw new ArgumentOutOfRangeException("level");
		}

		// Token: 0x04001B63 RID: 7011
		internal static readonly HttpRequestCachePolicy BypassCache = new HttpRequestCachePolicy(HttpRequestCacheLevel.BypassCache);

		// Token: 0x04001B64 RID: 7012
		private HttpRequestCacheLevel m_Level;

		// Token: 0x04001B65 RID: 7013
		private DateTime m_LastSyncDateUtc = DateTime.MinValue;

		// Token: 0x04001B66 RID: 7014
		private TimeSpan m_MaxAge = TimeSpan.MaxValue;

		// Token: 0x04001B67 RID: 7015
		private TimeSpan m_MinFresh = TimeSpan.MinValue;

		// Token: 0x04001B68 RID: 7016
		private TimeSpan m_MaxStale = TimeSpan.MinValue;
	}
}
