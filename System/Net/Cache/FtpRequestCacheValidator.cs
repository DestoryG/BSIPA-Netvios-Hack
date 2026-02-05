using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x0200030C RID: 780
	internal class FtpRequestCacheValidator : HttpRequestCacheValidator
	{
		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001BCE RID: 7118 RVA: 0x000844BD File Offset: 0x000826BD
		private bool HttpProxyMode
		{
			get
			{
				return this.m_HttpProxyMode;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x000844C5 File Offset: 0x000826C5
		internal new RequestCachePolicy Policy
		{
			get
			{
				return base.Policy;
			}
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x000844CD File Offset: 0x000826CD
		private void ZeroPrivateVars()
		{
			this.m_LastModified = DateTime.MinValue;
			this.m_HttpProxyMode = false;
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x000844E1 File Offset: 0x000826E1
		internal override RequestCacheValidator CreateValidator()
		{
			return new FtpRequestCacheValidator(base.StrictCacheErrors, base.UnspecifiedMaxAge);
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x000844F4 File Offset: 0x000826F4
		internal FtpRequestCacheValidator(bool strictCacheErrors, TimeSpan unspecifiedMaxAge)
			: base(strictCacheErrors, unspecifiedMaxAge)
		{
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x00084500 File Offset: 0x00082700
		protected internal override CacheValidationStatus ValidateRequest()
		{
			this.ZeroPrivateVars();
			if (base.Request is HttpWebRequest)
			{
				this.m_HttpProxyMode = true;
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_ftp_proxy_doesnt_support_partial"));
				}
				return base.ValidateRequest();
			}
			if (this.Policy.Level == RequestCacheLevel.BypassCache)
			{
				return CacheValidationStatus.DoNotUseCache;
			}
			string text = base.Request.Method.ToUpper(CultureInfo.InvariantCulture);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_ftp_method", new object[] { text }));
			}
			if (!(text == "RETR"))
			{
				if (!(text == "STOR"))
				{
					if (!(text == "APPE"))
					{
						if (!(text == "RENAME"))
						{
							if (!(text == "DELE"))
							{
								base.RequestMethod = HttpMethod.Other;
							}
							else
							{
								base.RequestMethod = HttpMethod.Delete;
							}
						}
						else
						{
							base.RequestMethod = HttpMethod.Put;
						}
					}
					else
					{
						base.RequestMethod = HttpMethod.Put;
					}
				}
				else
				{
					base.RequestMethod = HttpMethod.Put;
				}
			}
			else
			{
				base.RequestMethod = HttpMethod.Get;
			}
			if ((base.RequestMethod != HttpMethod.Get || !((FtpWebRequest)base.Request).UseBinary) && this.Policy.Level == RequestCacheLevel.CacheOnly)
			{
				this.FailRequest(WebExceptionStatus.RequestProhibitedByCachePolicy);
			}
			if (text != "RETR")
			{
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (!((FtpWebRequest)base.Request).UseBinary)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_ftp_supports_bin_only"));
				}
				return CacheValidationStatus.DoNotUseCache;
			}
			if (this.Policy.Level >= RequestCacheLevel.Reload)
			{
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			return CacheValidationStatus.Continue;
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0008468C File Offset: 0x0008288C
		protected internal override CacheFreshnessStatus ValidateFreshness()
		{
			if (this.HttpProxyMode)
			{
				if (base.CacheStream != Stream.Null)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_replacing_entry_with_HTTP_200"));
					}
					if (base.CacheEntry.EntryMetadata == null)
					{
						base.CacheEntry.EntryMetadata = new StringCollection();
					}
					base.CacheEntry.EntryMetadata.Clear();
					base.CacheEntry.EntryMetadata.Add("HTTP/1.1 200 OK");
				}
				return base.ValidateFreshness();
			}
			DateTime utcNow = DateTime.UtcNow;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_now_time", new object[] { utcNow.ToString("r", CultureInfo.InvariantCulture) }));
			}
			if (base.CacheEntry.ExpiresUtc != DateTime.MinValue)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_max_age_absolute", new object[] { base.CacheEntry.ExpiresUtc.ToString("r", CultureInfo.InvariantCulture) }));
				}
				if (base.CacheEntry.ExpiresUtc < utcNow)
				{
					return CacheFreshnessStatus.Stale;
				}
				return CacheFreshnessStatus.Fresh;
			}
			else
			{
				TimeSpan timeSpan = TimeSpan.MaxValue;
				if (base.CacheEntry.LastSynchronizedUtc != DateTime.MinValue)
				{
					timeSpan = utcNow - base.CacheEntry.LastSynchronizedUtc;
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_age1", new object[]
						{
							((int)timeSpan.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo),
							base.CacheEntry.LastSynchronizedUtc.ToString("r", CultureInfo.InvariantCulture)
						}));
					}
				}
				if (base.CacheEntry.LastModifiedUtc != DateTime.MinValue)
				{
					int num = (int)((utcNow - base.CacheEntry.LastModifiedUtc).TotalSeconds / 10.0);
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_no_max_age_use_10_percent", new object[]
						{
							num.ToString(NumberFormatInfo.InvariantInfo),
							base.CacheEntry.LastModifiedUtc.ToString("r", CultureInfo.InvariantCulture)
						}));
					}
					if (timeSpan.TotalSeconds < (double)num)
					{
						return CacheFreshnessStatus.Fresh;
					}
					return CacheFreshnessStatus.Stale;
				}
				else
				{
					if (Logging.On)
					{
						Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_no_max_age_use_default", new object[] { ((int)base.UnspecifiedMaxAge.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo) }));
					}
					if (base.UnspecifiedMaxAge >= timeSpan)
					{
						return CacheFreshnessStatus.Fresh;
					}
					return CacheFreshnessStatus.Stale;
				}
			}
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x00084938 File Offset: 0x00082B38
		protected internal override CacheValidationStatus ValidateCache()
		{
			if (this.HttpProxyMode)
			{
				return base.ValidateCache();
			}
			if (this.Policy.Level >= RequestCacheLevel.Reload)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_validator_invalid_for_policy", new object[] { this.Policy.ToString() }));
				}
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (base.CacheStream == Stream.Null || base.CacheEntry.IsPartialEntry)
			{
				if (this.Policy.Level == RequestCacheLevel.CacheOnly)
				{
					this.FailRequest(WebExceptionStatus.CacheEntryNotFound);
				}
				if (base.CacheStream == Stream.Null)
				{
					return CacheValidationStatus.DoNotTakeFromCache;
				}
			}
			base.CacheStreamOffset = 0L;
			base.CacheStreamLength = base.CacheEntry.StreamSize;
			if (this.Policy.Level == RequestCacheLevel.Revalidate || base.CacheEntry.IsPartialEntry)
			{
				return this.TryConditionalRequest();
			}
			long num = ((base.Request is FtpWebRequest) ? ((FtpWebRequest)base.Request).ContentOffset : 0L);
			if (base.CacheFreshnessStatus == CacheFreshnessStatus.Fresh || this.Policy.Level == RequestCacheLevel.CacheOnly || this.Policy.Level == RequestCacheLevel.CacheIfAvailable)
			{
				if (num != 0L)
				{
					if (num >= base.CacheStreamLength)
					{
						if (this.Policy.Level == RequestCacheLevel.CacheOnly)
						{
							this.FailRequest(WebExceptionStatus.CacheEntryNotFound);
						}
						return CacheValidationStatus.DoNotTakeFromCache;
					}
					base.CacheStreamOffset = num;
				}
				return CacheValidationStatus.ReturnCachedResponse;
			}
			return CacheValidationStatus.DoNotTakeFromCache;
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x00084A84 File Offset: 0x00082C84
		protected internal override CacheValidationStatus RevalidateCache()
		{
			if (this.HttpProxyMode)
			{
				return base.RevalidateCache();
			}
			if (this.Policy.Level >= RequestCacheLevel.Reload)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_validator_invalid_for_policy", new object[] { this.Policy.ToString() }));
				}
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (base.CacheStream == Stream.Null)
			{
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			FtpWebResponse ftpWebResponse = base.Response as FtpWebResponse;
			if (ftpWebResponse == null)
			{
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			CacheValidationStatus cacheValidationStatus;
			if (ftpWebResponse.StatusCode == FtpStatusCode.FileStatus)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_response_last_modified", new object[]
					{
						ftpWebResponse.LastModified.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture),
						ftpWebResponse.ContentLength
					}));
				}
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_cache_last_modified", new object[]
					{
						base.CacheEntry.LastModifiedUtc.ToString("r", CultureInfo.InvariantCulture),
						base.CacheEntry.StreamSize
					}));
				}
				if (base.CacheStreamOffset != 0L && base.CacheEntry.IsPartialEntry)
				{
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_partial_and_non_zero_content_offset", new object[] { base.CacheStreamOffset.ToString(CultureInfo.InvariantCulture) }));
					}
				}
				if (ftpWebResponse.LastModified.ToUniversalTime() == base.CacheEntry.LastModifiedUtc)
				{
					if (base.CacheEntry.IsPartialEntry)
					{
						if (ftpWebResponse.ContentLength > 0L)
						{
							base.CacheStreamLength = ftpWebResponse.ContentLength;
						}
						else
						{
							base.CacheStreamLength = -1L;
						}
						cacheValidationStatus = CacheValidationStatus.CombineCachedAndServerResponse;
					}
					else if (ftpWebResponse.ContentLength == base.CacheEntry.StreamSize)
					{
						cacheValidationStatus = CacheValidationStatus.ReturnCachedResponse;
					}
					else
					{
						cacheValidationStatus = CacheValidationStatus.DoNotTakeFromCache;
					}
				}
				else
				{
					cacheValidationStatus = CacheValidationStatus.DoNotTakeFromCache;
				}
			}
			else
			{
				cacheValidationStatus = CacheValidationStatus.DoNotTakeFromCache;
			}
			return cacheValidationStatus;
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x00084C78 File Offset: 0x00082E78
		protected internal override CacheValidationStatus ValidateResponse()
		{
			if (this.HttpProxyMode)
			{
				return base.ValidateResponse();
			}
			if (this.Policy.Level != RequestCacheLevel.Default && this.Policy.Level != RequestCacheLevel.Revalidate)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_response_valid_based_on_policy", new object[] { this.Policy.ToString() }));
				}
				return CacheValidationStatus.Continue;
			}
			FtpWebResponse ftpWebResponse = base.Response as FtpWebResponse;
			if (ftpWebResponse == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_null_response_failure"));
				}
				return CacheValidationStatus.Continue;
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_ftp_response_status", new object[]
				{
					((int)ftpWebResponse.StatusCode).ToString(CultureInfo.InvariantCulture),
					ftpWebResponse.StatusCode.ToString()
				}));
			}
			if (base.ResponseCount > 1)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_resp_valid_based_on_retry", new object[] { base.ResponseCount }));
				}
				return CacheValidationStatus.Continue;
			}
			if (ftpWebResponse.StatusCode != FtpStatusCode.OpeningData && ftpWebResponse.StatusCode != FtpStatusCode.FileStatus)
			{
				return CacheValidationStatus.RetryResponseFromServer;
			}
			return CacheValidationStatus.Continue;
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x00084DB8 File Offset: 0x00082FB8
		protected internal override CacheValidationStatus UpdateCache()
		{
			if (this.HttpProxyMode)
			{
				return base.UpdateCache();
			}
			base.CacheStreamOffset = 0L;
			if (base.RequestMethod == HttpMethod.Other)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_not_updated_based_on_policy", new object[] { base.Request.Method }));
				}
				return CacheValidationStatus.DoNotUpdateCache;
			}
			if (base.ValidationStatus == CacheValidationStatus.RemoveFromCache)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_removed_existing_invalid_entry"));
				}
				return CacheValidationStatus.RemoveFromCache;
			}
			if (this.Policy.Level == RequestCacheLevel.CacheOnly)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_not_updated_based_on_policy", new object[] { this.Policy.ToString() }));
				}
				return CacheValidationStatus.DoNotUpdateCache;
			}
			FtpWebResponse ftpWebResponse = base.Response as FtpWebResponse;
			if (ftpWebResponse == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_not_updated_because_no_response"));
				}
				return CacheValidationStatus.DoNotUpdateCache;
			}
			if (base.RequestMethod == HttpMethod.Delete || base.RequestMethod == HttpMethod.Put)
			{
				if (base.RequestMethod == HttpMethod.Delete || ftpWebResponse.StatusCode == FtpStatusCode.OpeningData || ftpWebResponse.StatusCode == FtpStatusCode.DataAlreadyOpen || ftpWebResponse.StatusCode == FtpStatusCode.FileActionOK || ftpWebResponse.StatusCode == FtpStatusCode.ClosingData)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_removed_existing_based_on_method", new object[] { base.Request.Method }));
					}
					return CacheValidationStatus.RemoveFromCache;
				}
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_existing_not_removed_because_unexpected_response_status", new object[]
					{
						(int)ftpWebResponse.StatusCode,
						ftpWebResponse.StatusCode.ToString()
					}));
				}
				return CacheValidationStatus.DoNotUpdateCache;
			}
			else
			{
				if (this.Policy.Level == RequestCacheLevel.NoCacheNoStore)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_removed_existing_based_on_policy", new object[] { this.Policy.ToString() }));
					}
					return CacheValidationStatus.RemoveFromCache;
				}
				if (base.ValidationStatus == CacheValidationStatus.ReturnCachedResponse)
				{
					return this.UpdateCacheEntryOnRevalidate();
				}
				if (ftpWebResponse.StatusCode != FtpStatusCode.OpeningData && ftpWebResponse.StatusCode != FtpStatusCode.DataAlreadyOpen && ftpWebResponse.StatusCode != FtpStatusCode.ClosingData)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_not_updated_based_on_ftp_response_status", new object[]
						{
							string.Concat(new string[]
							{
								FtpStatusCode.OpeningData.ToString(),
								"|",
								FtpStatusCode.DataAlreadyOpen.ToString(),
								"|",
								FtpStatusCode.ClosingData.ToString()
							}),
							ftpWebResponse.StatusCode.ToString()
						}));
					}
					return CacheValidationStatus.DoNotUpdateCache;
				}
				if (((FtpWebRequest)base.Request).ContentOffset == 0L)
				{
					return this.UpdateCacheEntryOnStore();
				}
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_update_not_supported_for_ftp_restart", new object[] { ((FtpWebRequest)base.Request).ContentOffset.ToString(CultureInfo.InvariantCulture) }));
				}
				if (base.CacheEntry.LastModifiedUtc != DateTime.MinValue && ftpWebResponse.LastModified.ToUniversalTime() != base.CacheEntry.LastModifiedUtc)
				{
					if (Logging.On)
					{
						Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_removed_entry_because_ftp_restart_response_changed", new object[]
						{
							base.CacheEntry.LastModifiedUtc.ToString("r", CultureInfo.InvariantCulture),
							ftpWebResponse.LastModified.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture)
						}));
					}
					return CacheValidationStatus.RemoveFromCache;
				}
				return CacheValidationStatus.DoNotUpdateCache;
			}
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x00085190 File Offset: 0x00083390
		private CacheValidationStatus UpdateCacheEntryOnStore()
		{
			base.CacheEntry.EntryMetadata = null;
			base.CacheEntry.SystemMetadata = null;
			FtpWebResponse ftpWebResponse = base.Response as FtpWebResponse;
			if (ftpWebResponse.LastModified != DateTime.MinValue)
			{
				base.CacheEntry.LastModifiedUtc = ftpWebResponse.LastModified.ToUniversalTime();
			}
			base.ResponseEntityLength = base.Response.ContentLength;
			base.CacheEntry.StreamSize = base.ResponseEntityLength;
			base.CacheEntry.LastSynchronizedUtc = DateTime.UtcNow;
			return CacheValidationStatus.CacheResponse;
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x00085220 File Offset: 0x00083420
		private CacheValidationStatus UpdateCacheEntryOnRevalidate()
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_last_synchronized", new object[] { base.CacheEntry.LastSynchronizedUtc.ToString("r", CultureInfo.InvariantCulture) }));
			}
			DateTime utcNow = DateTime.UtcNow;
			if (base.CacheEntry.LastSynchronizedUtc + TimeSpan.FromMinutes(1.0) >= utcNow)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_suppress_update_because_synched_last_minute"));
				}
				return CacheValidationStatus.DoNotUpdateCache;
			}
			base.CacheEntry.EntryMetadata = null;
			base.CacheEntry.SystemMetadata = null;
			base.CacheEntry.LastSynchronizedUtc = utcNow;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_updating_last_synchronized", new object[] { base.CacheEntry.LastSynchronizedUtc.ToString("r", CultureInfo.InvariantCulture) }));
			}
			return CacheValidationStatus.UpdateResponseInformation;
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x00085320 File Offset: 0x00083520
		private CacheValidationStatus TryConditionalRequest()
		{
			FtpWebRequest ftpWebRequest = base.Request as FtpWebRequest;
			if (ftpWebRequest == null || !ftpWebRequest.UseBinary)
			{
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (ftpWebRequest.ContentOffset != 0L)
			{
				if (base.CacheEntry.IsPartialEntry || ftpWebRequest.ContentOffset >= base.CacheStreamLength)
				{
					return CacheValidationStatus.DoNotTakeFromCache;
				}
				base.CacheStreamOffset = ftpWebRequest.ContentOffset;
			}
			return CacheValidationStatus.Continue;
		}

		// Token: 0x04001B30 RID: 6960
		private DateTime m_LastModified;

		// Token: 0x04001B31 RID: 6961
		private bool m_HttpProxyMode;
	}
}
