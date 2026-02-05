using System;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x02000318 RID: 792
	internal abstract class RequestCacheValidator
	{
		// Token: 0x06001C21 RID: 7201
		internal abstract RequestCacheValidator CreateValidator();

		// Token: 0x06001C22 RID: 7202 RVA: 0x00085EB1 File Offset: 0x000840B1
		protected RequestCacheValidator(bool strictCacheErrors, TimeSpan unspecifiedMaxAge)
		{
			this._StrictCacheErrors = strictCacheErrors;
			this._UnspecifiedMaxAge = unspecifiedMaxAge;
			this._ValidationStatus = CacheValidationStatus.DoNotUseCache;
			this._CacheFreshnessStatus = CacheFreshnessStatus.Undefined;
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001C23 RID: 7203 RVA: 0x00085ED5 File Offset: 0x000840D5
		internal bool StrictCacheErrors
		{
			get
			{
				return this._StrictCacheErrors;
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001C24 RID: 7204 RVA: 0x00085EDD File Offset: 0x000840DD
		internal TimeSpan UnspecifiedMaxAge
		{
			get
			{
				return this._UnspecifiedMaxAge;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001C25 RID: 7205 RVA: 0x00085EE5 File Offset: 0x000840E5
		protected internal Uri Uri
		{
			get
			{
				return this._Uri;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x00085EED File Offset: 0x000840ED
		protected internal WebRequest Request
		{
			get
			{
				return this._Request;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001C27 RID: 7207 RVA: 0x00085EF5 File Offset: 0x000840F5
		protected internal WebResponse Response
		{
			get
			{
				return this._Response;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06001C28 RID: 7208 RVA: 0x00085EFD File Offset: 0x000840FD
		protected internal RequestCachePolicy Policy
		{
			get
			{
				return this._Policy;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001C29 RID: 7209 RVA: 0x00085F05 File Offset: 0x00084105
		protected internal int ResponseCount
		{
			get
			{
				return this._ResponseCount;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001C2A RID: 7210 RVA: 0x00085F0D File Offset: 0x0008410D
		protected internal CacheValidationStatus ValidationStatus
		{
			get
			{
				return this._ValidationStatus;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001C2B RID: 7211 RVA: 0x00085F15 File Offset: 0x00084115
		protected internal CacheFreshnessStatus CacheFreshnessStatus
		{
			get
			{
				return this._CacheFreshnessStatus;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x00085F1D File Offset: 0x0008411D
		protected internal RequestCacheEntry CacheEntry
		{
			get
			{
				return this._CacheEntry;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06001C2D RID: 7213 RVA: 0x00085F25 File Offset: 0x00084125
		// (set) Token: 0x06001C2E RID: 7214 RVA: 0x00085F2D File Offset: 0x0008412D
		protected internal Stream CacheStream
		{
			get
			{
				return this._CacheStream;
			}
			set
			{
				this._CacheStream = value;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001C2F RID: 7215 RVA: 0x00085F36 File Offset: 0x00084136
		// (set) Token: 0x06001C30 RID: 7216 RVA: 0x00085F3E File Offset: 0x0008413E
		protected internal long CacheStreamOffset
		{
			get
			{
				return this._CacheStreamOffset;
			}
			set
			{
				this._CacheStreamOffset = value;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001C31 RID: 7217 RVA: 0x00085F47 File Offset: 0x00084147
		// (set) Token: 0x06001C32 RID: 7218 RVA: 0x00085F4F File Offset: 0x0008414F
		protected internal long CacheStreamLength
		{
			get
			{
				return this._CacheStreamLength;
			}
			set
			{
				this._CacheStreamLength = value;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001C33 RID: 7219 RVA: 0x00085F58 File Offset: 0x00084158
		protected internal string CacheKey
		{
			get
			{
				return this._CacheKey;
			}
		}

		// Token: 0x06001C34 RID: 7220
		protected internal abstract CacheValidationStatus ValidateRequest();

		// Token: 0x06001C35 RID: 7221
		protected internal abstract CacheFreshnessStatus ValidateFreshness();

		// Token: 0x06001C36 RID: 7222
		protected internal abstract CacheValidationStatus ValidateCache();

		// Token: 0x06001C37 RID: 7223
		protected internal abstract CacheValidationStatus ValidateResponse();

		// Token: 0x06001C38 RID: 7224
		protected internal abstract CacheValidationStatus RevalidateCache();

		// Token: 0x06001C39 RID: 7225
		protected internal abstract CacheValidationStatus UpdateCache();

		// Token: 0x06001C3A RID: 7226 RVA: 0x00085F60 File Offset: 0x00084160
		protected internal virtual void FailRequest(WebExceptionStatus webStatus)
		{
			if (Logging.On)
			{
				Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_failing_request_with_exception", new object[] { webStatus.ToString() }));
			}
			if (webStatus == WebExceptionStatus.CacheEntryNotFound)
			{
				throw ExceptionHelper.CacheEntryNotFoundException;
			}
			if (webStatus == WebExceptionStatus.RequestProhibitedByCachePolicy)
			{
				throw ExceptionHelper.RequestProhibitedByCachePolicyException;
			}
			throw new WebException(NetRes.GetWebStatusString("net_requestaborted", webStatus), webStatus);
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x00085FC8 File Offset: 0x000841C8
		internal void FetchRequest(Uri uri, WebRequest request)
		{
			this._Request = request;
			this._Policy = request.CachePolicy;
			this._Response = null;
			this._ResponseCount = 0;
			this._ValidationStatus = CacheValidationStatus.DoNotUseCache;
			this._CacheFreshnessStatus = CacheFreshnessStatus.Undefined;
			this._CacheStream = null;
			this._CacheStreamOffset = 0L;
			this._CacheStreamLength = 0L;
			if (!uri.Equals(this._Uri))
			{
				this._CacheKey = uri.GetParts(UriComponents.AbsoluteUri, UriFormat.Unescaped);
			}
			this._Uri = uri;
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x0008603F File Offset: 0x0008423F
		internal void FetchCacheEntry(RequestCacheEntry fetchEntry)
		{
			this._CacheEntry = fetchEntry;
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x00086048 File Offset: 0x00084248
		internal void FetchResponse(WebResponse fetchResponse)
		{
			this._ResponseCount++;
			this._Response = fetchResponse;
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x0008605F File Offset: 0x0008425F
		internal void SetFreshnessStatus(CacheFreshnessStatus status)
		{
			this._CacheFreshnessStatus = status;
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x00086068 File Offset: 0x00084268
		internal void SetValidationStatus(CacheValidationStatus status)
		{
			this._ValidationStatus = status;
		}

		// Token: 0x04001B7A RID: 7034
		internal WebRequest _Request;

		// Token: 0x04001B7B RID: 7035
		internal WebResponse _Response;

		// Token: 0x04001B7C RID: 7036
		internal Stream _CacheStream;

		// Token: 0x04001B7D RID: 7037
		private RequestCachePolicy _Policy;

		// Token: 0x04001B7E RID: 7038
		private Uri _Uri;

		// Token: 0x04001B7F RID: 7039
		private string _CacheKey;

		// Token: 0x04001B80 RID: 7040
		private RequestCacheEntry _CacheEntry;

		// Token: 0x04001B81 RID: 7041
		private int _ResponseCount;

		// Token: 0x04001B82 RID: 7042
		private CacheValidationStatus _ValidationStatus;

		// Token: 0x04001B83 RID: 7043
		private CacheFreshnessStatus _CacheFreshnessStatus;

		// Token: 0x04001B84 RID: 7044
		private long _CacheStreamOffset;

		// Token: 0x04001B85 RID: 7045
		private long _CacheStreamLength;

		// Token: 0x04001B86 RID: 7046
		private bool _StrictCacheErrors;

		// Token: 0x04001B87 RID: 7047
		private TimeSpan _UnspecifiedMaxAge;
	}
}
