using System;

namespace System.Net.Cache
{
	// Token: 0x02000317 RID: 791
	internal enum CacheValidationStatus
	{
		// Token: 0x04001B6E RID: 7022
		DoNotUseCache,
		// Token: 0x04001B6F RID: 7023
		Fail,
		// Token: 0x04001B70 RID: 7024
		DoNotTakeFromCache,
		// Token: 0x04001B71 RID: 7025
		RetryResponseFromCache,
		// Token: 0x04001B72 RID: 7026
		RetryResponseFromServer,
		// Token: 0x04001B73 RID: 7027
		ReturnCachedResponse,
		// Token: 0x04001B74 RID: 7028
		CombineCachedAndServerResponse,
		// Token: 0x04001B75 RID: 7029
		CacheResponse,
		// Token: 0x04001B76 RID: 7030
		UpdateResponseInformation,
		// Token: 0x04001B77 RID: 7031
		RemoveFromCache,
		// Token: 0x04001B78 RID: 7032
		DoNotUpdateCache,
		// Token: 0x04001B79 RID: 7033
		Continue
	}
}
