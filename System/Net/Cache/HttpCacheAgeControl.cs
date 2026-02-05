using System;

namespace System.Net.Cache
{
	// Token: 0x02000314 RID: 788
	public enum HttpCacheAgeControl
	{
		// Token: 0x04001B5D RID: 7005
		None,
		// Token: 0x04001B5E RID: 7006
		MinFresh,
		// Token: 0x04001B5F RID: 7007
		MaxAge,
		// Token: 0x04001B60 RID: 7008
		MaxStale = 4,
		// Token: 0x04001B61 RID: 7009
		MaxAgeAndMinFresh = 3,
		// Token: 0x04001B62 RID: 7010
		MaxAgeAndMaxStale = 6
	}
}
