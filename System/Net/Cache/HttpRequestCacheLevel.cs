using System;

namespace System.Net.Cache
{
	// Token: 0x02000313 RID: 787
	public enum HttpRequestCacheLevel
	{
		// Token: 0x04001B53 RID: 6995
		Default,
		// Token: 0x04001B54 RID: 6996
		BypassCache,
		// Token: 0x04001B55 RID: 6997
		CacheOnly,
		// Token: 0x04001B56 RID: 6998
		CacheIfAvailable,
		// Token: 0x04001B57 RID: 6999
		Revalidate,
		// Token: 0x04001B58 RID: 7000
		Reload,
		// Token: 0x04001B59 RID: 7001
		NoCacheNoStore,
		// Token: 0x04001B5A RID: 7002
		CacheOrNextCacheOnly,
		// Token: 0x04001B5B RID: 7003
		Refresh
	}
}
