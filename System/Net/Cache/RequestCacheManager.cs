using System;
using System.Net.Configuration;

namespace System.Net.Cache
{
	// Token: 0x0200030F RID: 783
	internal sealed class RequestCacheManager
	{
		// Token: 0x06001C04 RID: 7172 RVA: 0x0008592C File Offset: 0x00083B2C
		private RequestCacheManager()
		{
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x00085934 File Offset: 0x00083B34
		internal static RequestCacheBinding GetBinding(string internedScheme)
		{
			if (internedScheme == null)
			{
				throw new ArgumentNullException("uriScheme");
			}
			if (RequestCacheManager.s_CacheConfigSettings == null)
			{
				RequestCacheManager.LoadConfigSettings();
			}
			if (RequestCacheManager.s_CacheConfigSettings.DisableAllCaching)
			{
				return RequestCacheManager.s_BypassCacheBinding;
			}
			if (internedScheme.Length == 0)
			{
				return RequestCacheManager.s_DefaultGlobalBinding;
			}
			if (internedScheme == Uri.UriSchemeHttp || internedScheme == Uri.UriSchemeHttps)
			{
				return RequestCacheManager.s_DefaultHttpBinding;
			}
			if (internedScheme == Uri.UriSchemeFtp)
			{
				return RequestCacheManager.s_DefaultFtpBinding;
			}
			return RequestCacheManager.s_BypassCacheBinding;
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001C06 RID: 7174 RVA: 0x000859AE File Offset: 0x00083BAE
		internal static bool IsCachingEnabled
		{
			get
			{
				if (RequestCacheManager.s_CacheConfigSettings == null)
				{
					RequestCacheManager.LoadConfigSettings();
				}
				return !RequestCacheManager.s_CacheConfigSettings.DisableAllCaching;
			}
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000859D0 File Offset: 0x00083BD0
		internal static void SetBinding(string uriScheme, RequestCacheBinding binding)
		{
			if (uriScheme == null)
			{
				throw new ArgumentNullException("uriScheme");
			}
			if (RequestCacheManager.s_CacheConfigSettings == null)
			{
				RequestCacheManager.LoadConfigSettings();
			}
			if (RequestCacheManager.s_CacheConfigSettings.DisableAllCaching)
			{
				return;
			}
			if (uriScheme.Length == 0)
			{
				RequestCacheManager.s_DefaultGlobalBinding = binding;
				return;
			}
			if (uriScheme == Uri.UriSchemeHttp || uriScheme == Uri.UriSchemeHttps)
			{
				RequestCacheManager.s_DefaultHttpBinding = binding;
				return;
			}
			if (uriScheme == Uri.UriSchemeFtp)
			{
				RequestCacheManager.s_DefaultFtpBinding = binding;
			}
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x00085A54 File Offset: 0x00083C54
		private static void LoadConfigSettings()
		{
			RequestCacheBinding requestCacheBinding = RequestCacheManager.s_BypassCacheBinding;
			lock (requestCacheBinding)
			{
				if (RequestCacheManager.s_CacheConfigSettings == null)
				{
					RequestCachingSectionInternal section = RequestCachingSectionInternal.GetSection();
					RequestCacheManager.s_DefaultGlobalBinding = new RequestCacheBinding(section.DefaultCache, section.DefaultHttpValidator, section.DefaultCachePolicy);
					RequestCacheManager.s_DefaultHttpBinding = new RequestCacheBinding(section.DefaultCache, section.DefaultHttpValidator, section.DefaultHttpCachePolicy);
					RequestCacheManager.s_DefaultFtpBinding = new RequestCacheBinding(section.DefaultCache, section.DefaultFtpValidator, section.DefaultFtpCachePolicy);
					RequestCacheManager.s_CacheConfigSettings = section;
				}
			}
		}

		// Token: 0x04001B41 RID: 6977
		private static volatile RequestCachingSectionInternal s_CacheConfigSettings;

		// Token: 0x04001B42 RID: 6978
		private static readonly RequestCacheBinding s_BypassCacheBinding = new RequestCacheBinding(null, null, new RequestCachePolicy(RequestCacheLevel.BypassCache));

		// Token: 0x04001B43 RID: 6979
		private static volatile RequestCacheBinding s_DefaultGlobalBinding;

		// Token: 0x04001B44 RID: 6980
		private static volatile RequestCacheBinding s_DefaultHttpBinding;

		// Token: 0x04001B45 RID: 6981
		private static volatile RequestCacheBinding s_DefaultFtpBinding;
	}
}
