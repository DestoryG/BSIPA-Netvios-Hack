using System;

namespace System.Net.Cache
{
	// Token: 0x02000311 RID: 785
	public enum RequestCacheLevel
	{
		// Token: 0x04001B4A RID: 6986
		Default,
		// Token: 0x04001B4B RID: 6987
		BypassCache,
		// Token: 0x04001B4C RID: 6988
		CacheOnly,
		// Token: 0x04001B4D RID: 6989
		CacheIfAvailable,
		// Token: 0x04001B4E RID: 6990
		Revalidate,
		// Token: 0x04001B4F RID: 6991
		Reload,
		// Token: 0x04001B50 RID: 6992
		NoCacheNoStore
	}
}
