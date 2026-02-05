using System;

namespace System.Net.Cache
{
	// Token: 0x02000310 RID: 784
	internal class RequestCacheBinding
	{
		// Token: 0x06001C0A RID: 7178 RVA: 0x00085B14 File Offset: 0x00083D14
		internal RequestCacheBinding(RequestCache requestCache, RequestCacheValidator cacheValidator, RequestCachePolicy policy)
		{
			this.m_RequestCache = requestCache;
			this.m_CacheValidator = cacheValidator;
			this.m_Policy = policy;
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x00085B31 File Offset: 0x00083D31
		internal RequestCache Cache
		{
			get
			{
				return this.m_RequestCache;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x00085B39 File Offset: 0x00083D39
		internal RequestCacheValidator Validator
		{
			get
			{
				return this.m_CacheValidator;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x00085B41 File Offset: 0x00083D41
		internal RequestCachePolicy Policy
		{
			get
			{
				return this.m_Policy;
			}
		}

		// Token: 0x04001B46 RID: 6982
		private RequestCache m_RequestCache;

		// Token: 0x04001B47 RID: 6983
		private RequestCacheValidator m_CacheValidator;

		// Token: 0x04001B48 RID: 6984
		private RequestCachePolicy m_Policy;
	}
}
