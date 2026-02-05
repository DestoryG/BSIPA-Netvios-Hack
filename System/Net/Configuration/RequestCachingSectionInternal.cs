using System;
using System.Configuration;
using System.Net.Cache;
using System.Threading;
using Microsoft.Win32;

namespace System.Net.Configuration
{
	// Token: 0x0200033E RID: 830
	internal sealed class RequestCachingSectionInternal
	{
		// Token: 0x06001D9E RID: 7582 RVA: 0x0008C304 File Offset: 0x0008A504
		private RequestCachingSectionInternal()
		{
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x0008C30C File Offset: 0x0008A50C
		internal RequestCachingSectionInternal(RequestCachingSection section)
		{
			if (!section.DisableAllCaching)
			{
				this.defaultCachePolicy = new RequestCachePolicy(section.DefaultPolicyLevel);
				this.isPrivateCache = section.IsPrivateCache;
				this.unspecifiedMaximumAge = section.UnspecifiedMaximumAge;
			}
			else
			{
				this.disableAllCaching = true;
			}
			this.httpRequestCacheValidator = new HttpRequestCacheValidator(false, this.UnspecifiedMaximumAge);
			this.ftpRequestCacheValidator = new FtpRequestCacheValidator(false, this.UnspecifiedMaximumAge);
			this.defaultCache = new WinInetCache(this.IsPrivateCache, true, true);
			if (section.DisableAllCaching)
			{
				return;
			}
			HttpCachePolicyElement httpCachePolicyElement = section.DefaultHttpCachePolicy;
			if (httpCachePolicyElement.WasReadFromConfig)
			{
				if (httpCachePolicyElement.PolicyLevel == HttpRequestCacheLevel.Default)
				{
					HttpCacheAgeControl httpCacheAgeControl = ((httpCachePolicyElement.MinimumFresh != TimeSpan.MinValue) ? HttpCacheAgeControl.MaxAgeAndMinFresh : HttpCacheAgeControl.MaxAgeAndMaxStale);
					this.defaultHttpCachePolicy = new HttpRequestCachePolicy(httpCacheAgeControl, httpCachePolicyElement.MaximumAge, (httpCachePolicyElement.MinimumFresh != TimeSpan.MinValue) ? httpCachePolicyElement.MinimumFresh : httpCachePolicyElement.MaximumStale);
				}
				else
				{
					this.defaultHttpCachePolicy = new HttpRequestCachePolicy(httpCachePolicyElement.PolicyLevel);
				}
			}
			FtpCachePolicyElement ftpCachePolicyElement = section.DefaultFtpCachePolicy;
			if (ftpCachePolicyElement.WasReadFromConfig)
			{
				this.defaultFtpCachePolicy = new RequestCachePolicy(ftpCachePolicyElement.PolicyLevel);
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001DA0 RID: 7584 RVA: 0x0008C42C File Offset: 0x0008A62C
		internal static object ClassSyncObject
		{
			get
			{
				if (RequestCachingSectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref RequestCachingSectionInternal.classSyncObject, obj, null);
				}
				return RequestCachingSectionInternal.classSyncObject;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x0008C458 File Offset: 0x0008A658
		internal bool DisableAllCaching
		{
			get
			{
				return this.disableAllCaching;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001DA2 RID: 7586 RVA: 0x0008C460 File Offset: 0x0008A660
		internal RequestCache DefaultCache
		{
			get
			{
				return this.defaultCache;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x0008C468 File Offset: 0x0008A668
		internal RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				return this.defaultCachePolicy;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001DA4 RID: 7588 RVA: 0x0008C470 File Offset: 0x0008A670
		internal bool IsPrivateCache
		{
			get
			{
				return this.isPrivateCache;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06001DA5 RID: 7589 RVA: 0x0008C478 File Offset: 0x0008A678
		internal TimeSpan UnspecifiedMaximumAge
		{
			get
			{
				return this.unspecifiedMaximumAge;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06001DA6 RID: 7590 RVA: 0x0008C480 File Offset: 0x0008A680
		internal HttpRequestCachePolicy DefaultHttpCachePolicy
		{
			get
			{
				return this.defaultHttpCachePolicy;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x0008C488 File Offset: 0x0008A688
		internal RequestCachePolicy DefaultFtpCachePolicy
		{
			get
			{
				return this.defaultFtpCachePolicy;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x0008C490 File Offset: 0x0008A690
		internal HttpRequestCacheValidator DefaultHttpValidator
		{
			get
			{
				return this.httpRequestCacheValidator;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06001DA9 RID: 7593 RVA: 0x0008C498 File Offset: 0x0008A698
		internal FtpRequestCacheValidator DefaultFtpValidator
		{
			get
			{
				return this.ftpRequestCacheValidator;
			}
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x0008C4A0 File Offset: 0x0008A6A0
		internal static RequestCachingSectionInternal GetSection()
		{
			object obj = RequestCachingSectionInternal.ClassSyncObject;
			RequestCachingSectionInternal requestCachingSectionInternal;
			lock (obj)
			{
				RequestCachingSection requestCachingSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.RequestCachingSectionPath) as RequestCachingSection;
				if (requestCachingSection == null)
				{
					requestCachingSectionInternal = null;
				}
				else
				{
					try
					{
						requestCachingSectionInternal = new RequestCachingSectionInternal(requestCachingSection);
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_requestcaching"), ex);
					}
				}
			}
			return requestCachingSectionInternal;
		}

		// Token: 0x04001C55 RID: 7253
		private static object classSyncObject;

		// Token: 0x04001C56 RID: 7254
		private RequestCache defaultCache;

		// Token: 0x04001C57 RID: 7255
		private HttpRequestCachePolicy defaultHttpCachePolicy;

		// Token: 0x04001C58 RID: 7256
		private RequestCachePolicy defaultFtpCachePolicy;

		// Token: 0x04001C59 RID: 7257
		private RequestCachePolicy defaultCachePolicy;

		// Token: 0x04001C5A RID: 7258
		private bool disableAllCaching;

		// Token: 0x04001C5B RID: 7259
		private HttpRequestCacheValidator httpRequestCacheValidator;

		// Token: 0x04001C5C RID: 7260
		private FtpRequestCacheValidator ftpRequestCacheValidator;

		// Token: 0x04001C5D RID: 7261
		private bool isPrivateCache;

		// Token: 0x04001C5E RID: 7262
		private TimeSpan unspecifiedMaximumAge;
	}
}
