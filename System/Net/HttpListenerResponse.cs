using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Net
{
	// Token: 0x02000100 RID: 256
	public sealed class HttpListenerResponse : IDisposable
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x000349E0 File Offset: 0x00032BE0
		internal HttpListenerResponse()
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", "");
			}
			this.m_NativeResponse = default(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE);
			this.m_WebHeaders = new WebHeaderCollection(WebHeaderCollectionType.HttpListenerResponse);
			this.m_BoundaryType = BoundaryType.None;
			this.m_NativeResponse.StatusCode = 200;
			this.m_NativeResponse.Version.MajorVersion = 1;
			this.m_NativeResponse.Version.MinorVersion = 1;
			this.m_KeepAlive = true;
			this.m_ResponseState = HttpListenerResponse.ResponseState.Created;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00034A6E File Offset: 0x00032C6E
		internal HttpListenerResponse(HttpListenerContext httpContext)
			: this()
		{
			if (Logging.On)
			{
				Logging.Associate(Logging.HttpListener, this, httpContext);
			}
			this.m_HttpContext = httpContext;
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x00034A90 File Offset: 0x00032C90
		private HttpListenerContext HttpListenerContext
		{
			get
			{
				return this.m_HttpContext;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x00034A98 File Offset: 0x00032C98
		private HttpListenerRequest HttpListenerRequest
		{
			get
			{
				return this.HttpListenerContext.Request;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x00034AA5 File Offset: 0x00032CA5
		// (set) Token: 0x06000969 RID: 2409 RVA: 0x00034AAD File Offset: 0x00032CAD
		public Encoding ContentEncoding
		{
			get
			{
				return this.m_ContentEncoding;
			}
			set
			{
				this.m_ContentEncoding = value;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x00034AB6 File Offset: 0x00032CB6
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x00034AC5 File Offset: 0x00032CC5
		public string ContentType
		{
			get
			{
				return this.Headers[HttpResponseHeader.ContentType];
			}
			set
			{
				this.CheckDisposed();
				if (string.IsNullOrEmpty(value))
				{
					this.Headers.Remove(HttpResponseHeader.ContentType);
					return;
				}
				this.Headers.Set(HttpResponseHeader.ContentType, value);
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x00034AF1 File Offset: 0x00032CF1
		public Stream OutputStream
		{
			get
			{
				this.CheckDisposed();
				this.EnsureResponseStream();
				return this.m_ResponseStream;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x00034B05 File Offset: 0x00032D05
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x00034B14 File Offset: 0x00032D14
		public string RedirectLocation
		{
			get
			{
				return this.Headers[HttpResponseHeader.Location];
			}
			set
			{
				this.CheckDisposed();
				if (string.IsNullOrEmpty(value))
				{
					this.Headers.Remove(HttpResponseHeader.Location);
					return;
				}
				this.Headers.Set(HttpResponseHeader.Location, value);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x00034B40 File Offset: 0x00032D40
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x00034B4D File Offset: 0x00032D4D
		public int StatusCode
		{
			get
			{
				return (int)this.m_NativeResponse.StatusCode;
			}
			set
			{
				this.CheckDisposed();
				if (value < 100 || value > 999)
				{
					throw new ProtocolViolationException(SR.GetString("net_invalidstatus"));
				}
				this.m_NativeResponse.StatusCode = (ushort)value;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x00034B7F File Offset: 0x00032D7F
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x00034BB4 File Offset: 0x00032DB4
		public string StatusDescription
		{
			get
			{
				if (this.m_StatusDescription == null)
				{
					this.m_StatusDescription = HttpStatusDescription.Get(this.StatusCode);
				}
				if (this.m_StatusDescription == null)
				{
					this.m_StatusDescription = string.Empty;
				}
				return this.m_StatusDescription;
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				for (int i = 0; i < value.Length; i++)
				{
					char c = 'ÿ' & value[i];
					if ((c <= '\u001f' && c != '\t') || c == '\u007f')
					{
						throw new ArgumentException(SR.GetString("net_WebHeaderInvalidControlChars"), "name");
					}
				}
				this.m_StatusDescription = value;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x00034C20 File Offset: 0x00032E20
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x00034C3C File Offset: 0x00032E3C
		public CookieCollection Cookies
		{
			get
			{
				if (this.m_Cookies == null)
				{
					this.m_Cookies = new CookieCollection(false);
				}
				return this.m_Cookies;
			}
			set
			{
				this.m_Cookies = value;
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00034C48 File Offset: 0x00032E48
		public void CopyFrom(HttpListenerResponse templateResponse)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "CopyFrom", "templateResponse#" + ValidationHelper.HashString(templateResponse));
			}
			this.m_NativeResponse = default(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE);
			this.m_ResponseState = HttpListenerResponse.ResponseState.Created;
			this.m_WebHeaders = templateResponse.m_WebHeaders;
			this.m_BoundaryType = templateResponse.m_BoundaryType;
			this.m_ContentLength = templateResponse.m_ContentLength;
			this.m_NativeResponse.StatusCode = templateResponse.m_NativeResponse.StatusCode;
			this.m_NativeResponse.Version.MajorVersion = templateResponse.m_NativeResponse.Version.MajorVersion;
			this.m_NativeResponse.Version.MinorVersion = templateResponse.m_NativeResponse.Version.MinorVersion;
			this.m_StatusDescription = templateResponse.m_StatusDescription;
			this.m_KeepAlive = templateResponse.m_KeepAlive;
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x00034D21 File Offset: 0x00032F21
		// (set) Token: 0x06000977 RID: 2423 RVA: 0x00034D2C File Offset: 0x00032F2C
		public bool SendChunked
		{
			get
			{
				return this.EntitySendFormat == EntitySendFormat.Chunked;
			}
			set
			{
				if (value)
				{
					this.EntitySendFormat = EntitySendFormat.Chunked;
					return;
				}
				this.EntitySendFormat = EntitySendFormat.ContentLength;
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00034D40 File Offset: 0x00032F40
		private bool CanSendResponseBody(int responseCode)
		{
			for (int i = 0; i < HttpListenerResponse.s_NoResponseBody.Length; i++)
			{
				if (responseCode == HttpListenerResponse.s_NoResponseBody[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x00034D6C File Offset: 0x00032F6C
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x00034D74 File Offset: 0x00032F74
		internal EntitySendFormat EntitySendFormat
		{
			get
			{
				return (EntitySendFormat)this.m_BoundaryType;
			}
			set
			{
				this.CheckDisposed();
				if (this.m_ResponseState >= HttpListenerResponse.ResponseState.SentHeaders)
				{
					throw new InvalidOperationException(SR.GetString("net_rspsubmitted"));
				}
				if (value == EntitySendFormat.Chunked && this.HttpListenerRequest.ProtocolVersion.Minor == 0)
				{
					throw new ProtocolViolationException(SR.GetString("net_nochunkuploadonhttp10"));
				}
				this.m_BoundaryType = (BoundaryType)value;
				if (value != EntitySendFormat.ContentLength)
				{
					this.m_ContentLength = -1L;
				}
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x00034DD8 File Offset: 0x00032FD8
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x00034DE0 File Offset: 0x00032FE0
		public bool KeepAlive
		{
			get
			{
				return this.m_KeepAlive;
			}
			set
			{
				this.CheckDisposed();
				this.m_KeepAlive = value;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x00034DEF File Offset: 0x00032FEF
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x00034DF8 File Offset: 0x00032FF8
		public WebHeaderCollection Headers
		{
			get
			{
				return this.m_WebHeaders;
			}
			set
			{
				this.m_WebHeaders.Clear();
				foreach (string text in value.AllKeys)
				{
					this.m_WebHeaders.Add(text, value[text]);
				}
			}
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00034E3C File Offset: 0x0003303C
		public void AddHeader(string name, string value)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "AddHeader", " name=" + name + " value=" + value);
			}
			this.Headers.SetInternal(name, value);
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00034E73 File Offset: 0x00033073
		public void AppendHeader(string name, string value)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "AppendHeader", " name=" + name + " value=" + value);
			}
			this.Headers.Add(name, value);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00034EAC File Offset: 0x000330AC
		public void Redirect(string url)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "Redirect", " url=" + url);
			}
			this.Headers.SetInternal(HttpResponseHeader.Location, url);
			this.StatusCode = 302;
			this.StatusDescription = HttpStatusDescription.Get(this.StatusCode);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00034F08 File Offset: 0x00033108
		public void AppendCookie(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "AppendCookie", " cookie#" + ValidationHelper.HashString(cookie));
			}
			this.Cookies.Add(cookie);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00034F58 File Offset: 0x00033158
		public void SetCookie(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			Cookie cookie2 = cookie.Clone();
			int num = this.Cookies.InternalAdd(cookie2, true);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "SetCookie", " cookie#" + ValidationHelper.HashString(cookie));
			}
			if (num != 1)
			{
				throw new ArgumentException(SR.GetString("net_cookie_exists"), "cookie");
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x00034FC8 File Offset: 0x000331C8
		// (set) Token: 0x06000985 RID: 2437 RVA: 0x00034FD0 File Offset: 0x000331D0
		public long ContentLength64
		{
			get
			{
				return this.m_ContentLength;
			}
			set
			{
				this.CheckDisposed();
				if (this.m_ResponseState >= HttpListenerResponse.ResponseState.SentHeaders)
				{
					throw new InvalidOperationException(SR.GetString("net_rspsubmitted"));
				}
				if (value >= 0L)
				{
					this.m_ContentLength = value;
					this.m_BoundaryType = BoundaryType.ContentLength;
					return;
				}
				throw new ArgumentOutOfRangeException("value", SR.GetString("net_clsmall"));
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x00035024 File Offset: 0x00033224
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x0003504C File Offset: 0x0003324C
		public Version ProtocolVersion
		{
			get
			{
				return new Version((int)this.m_NativeResponse.Version.MajorVersion, (int)this.m_NativeResponse.Version.MinorVersion);
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Major != 1 || (value.Minor != 0 && value.Minor != 1))
				{
					throw new ArgumentException(SR.GetString("net_wrongversion"), "value");
				}
				this.m_NativeResponse.Version.MajorVersion = (ushort)value.Major;
				this.m_NativeResponse.Version.MinorVersion = (ushort)value.Minor;
			}
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x000350D0 File Offset: 0x000332D0
		public void Abort()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "abort", "");
			}
			try
			{
				if (this.m_ResponseState < HttpListenerResponse.ResponseState.Closed)
				{
					this.m_ResponseState = HttpListenerResponse.ResponseState.Closed;
					this.HttpListenerContext.Abort();
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "abort", "");
				}
			}
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00035148 File Offset: 0x00033348
		public void Close(byte[] responseEntity, bool willBlock)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close", " responseEntity=" + ValidationHelper.HashString(responseEntity) + " willBlock=" + willBlock.ToString());
			}
			try
			{
				this.CheckDisposed();
				if (responseEntity == null)
				{
					throw new ArgumentNullException("responseEntity");
				}
				if (this.m_ResponseState < HttpListenerResponse.ResponseState.SentHeaders && this.m_BoundaryType != BoundaryType.Chunked)
				{
					this.ContentLength64 = (long)responseEntity.Length;
				}
				this.EnsureResponseStream();
				if (willBlock)
				{
					try
					{
						this.m_ResponseStream.Write(responseEntity, 0, responseEntity.Length);
						return;
					}
					catch (Win32Exception)
					{
						return;
					}
					finally
					{
						this.m_ResponseStream.Close();
						this.m_ResponseState = HttpListenerResponse.ResponseState.Closed;
						this.HttpListenerContext.Close();
					}
				}
				this.m_ResponseStream.BeginWrite(responseEntity, 0, responseEntity.Length, new AsyncCallback(this.NonBlockingCloseCallback), null);
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Close", "");
				}
			}
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0003525C File Offset: 0x0003345C
		public void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close", "");
			}
			try
			{
				((IDisposable)this).Dispose();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Close", "");
				}
			}
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000352BC File Offset: 0x000334BC
		private void Dispose(bool disposing)
		{
			if (this.m_ResponseState >= HttpListenerResponse.ResponseState.Closed)
			{
				return;
			}
			this.EnsureResponseStream();
			this.m_ResponseStream.Close();
			this.m_ResponseState = HttpListenerResponse.ResponseState.Closed;
			this.HttpListenerContext.Close();
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x000352EB File Offset: 0x000334EB
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x000352FA File Offset: 0x000334FA
		internal BoundaryType BoundaryType
		{
			get
			{
				return this.m_BoundaryType;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x00035302 File Offset: 0x00033502
		internal bool SentHeaders
		{
			get
			{
				return this.m_ResponseState >= HttpListenerResponse.ResponseState.SentHeaders;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x00035310 File Offset: 0x00033510
		internal bool ComputedHeaders
		{
			get
			{
				return this.m_ResponseState >= HttpListenerResponse.ResponseState.ComputedHeaders;
			}
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0003531E File Offset: 0x0003351E
		private void EnsureResponseStream()
		{
			if (this.m_ResponseStream == null)
			{
				this.m_ResponseStream = new HttpResponseStream(this.HttpListenerContext);
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0003533C File Offset: 0x0003353C
		private void NonBlockingCloseCallback(IAsyncResult asyncResult)
		{
			try
			{
				this.m_ResponseStream.EndWrite(asyncResult);
			}
			catch (Win32Exception)
			{
			}
			finally
			{
				this.m_ResponseStream.Close();
				this.HttpListenerContext.Close();
				this.m_ResponseState = HttpListenerResponse.ResponseState.Closed;
			}
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00035394 File Offset: 0x00033594
		internal unsafe uint SendHeaders(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK* pDataChunk, HttpResponseStreamAsyncResult asyncResult, UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS flags, bool isWebSocketHandshake)
		{
			if (this.StatusCode == 401)
			{
				this.HttpListenerContext.SetAuthenticationHeaders();
			}
			if (Logging.On)
			{
				StringBuilder stringBuilder = new StringBuilder("HttpListenerResponse Headers:\n");
				for (int i = 0; i < this.Headers.Count; i++)
				{
					stringBuilder.Append("\t");
					stringBuilder.Append(this.Headers.GetKey(i));
					stringBuilder.Append(" : ");
					stringBuilder.Append(this.Headers.Get(i));
					stringBuilder.Append("\n");
				}
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", stringBuilder.ToString());
			}
			this.m_ResponseState = HttpListenerResponse.ResponseState.SentHeaders;
			List<GCHandle> list = this.SerializeHeaders(ref this.m_NativeResponse.Headers, isWebSocketHandshake);
			uint num;
			try
			{
				if (pDataChunk != null)
				{
					this.m_NativeResponse.EntityChunkCount = 1;
					this.m_NativeResponse.pEntityChunks = pDataChunk;
				}
				else if (asyncResult != null && asyncResult.pDataChunks != null)
				{
					this.m_NativeResponse.EntityChunkCount = asyncResult.dataChunkCount;
					this.m_NativeResponse.pEntityChunks = asyncResult.pDataChunks;
				}
				else
				{
					this.m_NativeResponse.EntityChunkCount = 0;
					this.m_NativeResponse.pEntityChunks = null;
				}
				uint num2;
				if (this.StatusDescription.Length > 0)
				{
					byte[] array = new byte[WebHeaderCollection.HeaderEncoding.GetByteCount(this.StatusDescription)];
					try
					{
						byte[] array2;
						byte* ptr;
						if ((array2 = array) == null || array2.Length == 0)
						{
							ptr = null;
						}
						else
						{
							ptr = &array2[0];
						}
						this.m_NativeResponse.ReasonLength = (ushort)array.Length;
						WebHeaderCollection.HeaderEncoding.GetBytes(this.StatusDescription, 0, array.Length, array, 0);
						this.m_NativeResponse.pReason = (sbyte*)ptr;
						try
						{
							fixed (UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE* ptr2 = &this.m_NativeResponse)
							{
								UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE* ptr3 = ptr2;
								if (asyncResult != null)
								{
									this.HttpListenerContext.EnsureBoundHandle();
								}
								num = UnsafeNclNativeMethods.HttpApi.HttpSendHttpResponse(this.HttpListenerContext.RequestQueueHandle, this.HttpListenerRequest.RequestId, (uint)flags, ptr3, null, &num2, SafeLocalFree.Zero, 0U, (asyncResult == null) ? null : asyncResult.m_pOverlapped, null);
								if (asyncResult != null && num == 0U && HttpListener.SkipIOCPCallbackOnSuccess)
								{
									asyncResult.IOCompleted(num, num2);
								}
								return num;
							}
						}
						finally
						{
							UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE* ptr2 = null;
						}
					}
					finally
					{
						byte[] array2 = null;
					}
				}
				try
				{
					fixed (UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE* ptr4 = &this.m_NativeResponse)
					{
						UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE* ptr5 = ptr4;
						if (asyncResult != null)
						{
							this.HttpListenerContext.EnsureBoundHandle();
						}
						num = UnsafeNclNativeMethods.HttpApi.HttpSendHttpResponse(this.HttpListenerContext.RequestQueueHandle, this.HttpListenerRequest.RequestId, (uint)flags, ptr5, null, &num2, SafeLocalFree.Zero, 0U, (asyncResult == null) ? null : asyncResult.m_pOverlapped, null);
						if (asyncResult != null && num == 0U && HttpListener.SkipIOCPCallbackOnSuccess)
						{
							asyncResult.IOCompleted(num, num2);
						}
					}
				}
				finally
				{
					UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE* ptr4 = null;
				}
			}
			finally
			{
				this.FreePinnedHeaders(list);
			}
			return num;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00035694 File Offset: 0x00033894
		internal void ComputeCookies()
		{
			if (this.m_Cookies != null)
			{
				string text = null;
				string text2 = null;
				for (int i = 0; i < this.m_Cookies.Count; i++)
				{
					Cookie cookie = this.m_Cookies[i];
					string text3 = cookie.ToServerString();
					if (text3 != null && text3.Length != 0)
					{
						if (cookie.Variant == CookieVariant.Rfc2965 || (this.HttpListenerContext.PromoteCookiesToRfc2965 && cookie.Variant == CookieVariant.Rfc2109))
						{
							text = ((text == null) ? text3 : (text + ", " + text3));
						}
						else
						{
							text2 = ((text2 == null) ? text3 : (text2 + ", " + text3));
						}
					}
				}
				if (!string.IsNullOrEmpty(text2))
				{
					this.Headers.Set(HttpResponseHeader.SetCookie, text2);
					if (string.IsNullOrEmpty(text))
					{
						this.Headers.Remove("Set-Cookie2");
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.Headers.Set("Set-Cookie2", text);
					if (string.IsNullOrEmpty(text2))
					{
						this.Headers.Remove("Set-Cookie");
					}
				}
			}
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00035794 File Offset: 0x00033994
		internal UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS ComputeHeaders()
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE;
			this.m_ResponseState = HttpListenerResponse.ResponseState.ComputedHeaders;
			this.ComputeCoreHeaders();
			if (this.m_BoundaryType == BoundaryType.None)
			{
				if (this.HttpListenerRequest.ProtocolVersion.Minor == 0)
				{
					this.m_KeepAlive = false;
				}
				else
				{
					this.m_BoundaryType = BoundaryType.Chunked;
				}
				if (this.CanSendResponseBody(this.m_HttpContext.Response.StatusCode))
				{
					this.m_ContentLength = -1L;
				}
				else
				{
					this.ContentLength64 = 0L;
				}
			}
			if (this.m_BoundaryType == BoundaryType.ContentLength)
			{
				this.Headers.SetInternal(HttpResponseHeader.ContentLength, this.m_ContentLength.ToString("D", NumberFormatInfo.InvariantInfo));
				if (this.m_ContentLength == 0L)
				{
					http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE;
				}
			}
			else if (this.m_BoundaryType == BoundaryType.Chunked)
			{
				this.Headers.SetInternal(HttpResponseHeader.TransferEncoding, "chunked");
			}
			else if (this.m_BoundaryType == BoundaryType.None)
			{
				http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE;
			}
			else
			{
				this.m_KeepAlive = false;
			}
			if (!this.m_KeepAlive)
			{
				this.Headers.Add(HttpResponseHeader.Connection, "close");
				if (http_FLAGS == UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE)
				{
					http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_RECEIVE_REQUEST_FLAG_COPY_BODY;
				}
			}
			else if (this.HttpListenerRequest.ProtocolVersion.Minor == 0)
			{
				this.Headers.SetInternal(HttpResponseHeader.KeepAlive, "true");
			}
			return http_FLAGS;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x000358AF File Offset: 0x00033AAF
		internal void ComputeCoreHeaders()
		{
			if (this.HttpListenerContext.MutualAuthentication != null && this.HttpListenerContext.MutualAuthentication.Length > 0)
			{
				this.Headers.SetInternal(HttpResponseHeader.WwwAuthenticate, this.HttpListenerContext.MutualAuthentication);
			}
			this.ComputeCookies();
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x000358F0 File Offset: 0x00033AF0
		private unsafe List<GCHandle> SerializeHeaders(ref UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADERS headers, bool isWebSocketHandshake)
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER[] array = null;
			if (this.Headers.Count == 0)
			{
				return null;
			}
			List<GCHandle> list = new List<GCHandle>();
			int num = 0;
			for (int i = 0; i < this.Headers.Count; i++)
			{
				string text = this.Headers.GetKey(i);
				int num2 = UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.IndexOfKnownHeader(text);
				if (num2 == 27 || (isWebSocketHandshake && num2 == 1))
				{
					num2 = -1;
				}
				if (num2 == -1)
				{
					string[] values = this.Headers.GetValues(i);
					num += values.Length;
				}
			}
			try
			{
				try
				{
					fixed (UnsafeNclNativeMethods.HttpApi.HTTP_KNOWN_HEADER* ptr = &headers.KnownHeaders)
					{
						UnsafeNclNativeMethods.HttpApi.HTTP_KNOWN_HEADER* ptr2 = ptr;
						for (int j = 0; j < this.Headers.Count; j++)
						{
							string text = this.Headers.GetKey(j);
							string text2 = this.Headers.Get(j);
							int num2 = UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.IndexOfKnownHeader(text);
							if (num2 == 27 || (isWebSocketHandshake && num2 == 1))
							{
								num2 = -1;
							}
							if (num2 == -1)
							{
								if (array == null)
								{
									array = new UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER[num];
									GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
									list.Add(gchandle);
									headers.pUnknownHeaders = (UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER*)(void*)gchandle.AddrOfPinnedObject();
								}
								string[] values2 = this.Headers.GetValues(j);
								for (int k = 0; k < values2.Length; k++)
								{
									byte[] array2 = new byte[WebHeaderCollection.HeaderEncoding.GetByteCount(text)];
									array[(int)headers.UnknownHeaderCount].NameLength = (ushort)array2.Length;
									WebHeaderCollection.HeaderEncoding.GetBytes(text, 0, array2.Length, array2, 0);
									GCHandle gchandle = GCHandle.Alloc(array2, GCHandleType.Pinned);
									list.Add(gchandle);
									array[(int)headers.UnknownHeaderCount].pName = (sbyte*)(void*)gchandle.AddrOfPinnedObject();
									text2 = values2[k];
									array2 = new byte[WebHeaderCollection.HeaderEncoding.GetByteCount(text2)];
									array[(int)headers.UnknownHeaderCount].RawValueLength = (ushort)array2.Length;
									WebHeaderCollection.HeaderEncoding.GetBytes(text2, 0, array2.Length, array2, 0);
									gchandle = GCHandle.Alloc(array2, GCHandleType.Pinned);
									list.Add(gchandle);
									array[(int)headers.UnknownHeaderCount].pRawValue = (sbyte*)(void*)gchandle.AddrOfPinnedObject();
									headers.UnknownHeaderCount += 1;
								}
							}
							else if (text2 != null)
							{
								byte[] array2 = new byte[WebHeaderCollection.HeaderEncoding.GetByteCount(text2)];
								ptr2[num2].RawValueLength = (ushort)array2.Length;
								WebHeaderCollection.HeaderEncoding.GetBytes(text2, 0, array2.Length, array2, 0);
								GCHandle gchandle = GCHandle.Alloc(array2, GCHandleType.Pinned);
								list.Add(gchandle);
								ptr2[num2].pRawValue = (sbyte*)(void*)gchandle.AddrOfPinnedObject();
							}
						}
					}
				}
				finally
				{
					UnsafeNclNativeMethods.HttpApi.HTTP_KNOWN_HEADER* ptr = null;
				}
			}
			catch
			{
				this.FreePinnedHeaders(list);
				throw;
			}
			return list;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00035BBC File Offset: 0x00033DBC
		private void FreePinnedHeaders(List<GCHandle> pinnedHeaders)
		{
			if (pinnedHeaders != null)
			{
				foreach (GCHandle gchandle in pinnedHeaders)
				{
					if (gchandle.IsAllocated)
					{
						gchandle.Free();
					}
				}
			}
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00035C18 File Offset: 0x00033E18
		private void CheckDisposed()
		{
			if (this.m_ResponseState >= HttpListenerResponse.ResponseState.Closed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00035C34 File Offset: 0x00033E34
		internal void CancelLastWrite(CriticalHandle requestQueueHandle)
		{
			if (this.m_ResponseStream != null)
			{
				this.m_ResponseStream.CancelLastWrite(requestQueueHandle);
			}
		}

		// Token: 0x04000E3E RID: 3646
		private Encoding m_ContentEncoding;

		// Token: 0x04000E3F RID: 3647
		private CookieCollection m_Cookies;

		// Token: 0x04000E40 RID: 3648
		private string m_StatusDescription;

		// Token: 0x04000E41 RID: 3649
		private bool m_KeepAlive;

		// Token: 0x04000E42 RID: 3650
		private HttpListenerResponse.ResponseState m_ResponseState;

		// Token: 0x04000E43 RID: 3651
		private WebHeaderCollection m_WebHeaders;

		// Token: 0x04000E44 RID: 3652
		private HttpResponseStream m_ResponseStream;

		// Token: 0x04000E45 RID: 3653
		private long m_ContentLength;

		// Token: 0x04000E46 RID: 3654
		private BoundaryType m_BoundaryType;

		// Token: 0x04000E47 RID: 3655
		private UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE m_NativeResponse;

		// Token: 0x04000E48 RID: 3656
		private HttpListenerContext m_HttpContext;

		// Token: 0x04000E49 RID: 3657
		private static readonly int[] s_NoResponseBody = new int[] { 100, 101, 204, 205, 304 };

		// Token: 0x02000706 RID: 1798
		private enum ResponseState
		{
			// Token: 0x040030D4 RID: 12500
			Created,
			// Token: 0x040030D5 RID: 12501
			ComputedHeaders,
			// Token: 0x040030D6 RID: 12502
			SentHeaders,
			// Token: 0x040030D7 RID: 12503
			Closed
		}
	}
}
