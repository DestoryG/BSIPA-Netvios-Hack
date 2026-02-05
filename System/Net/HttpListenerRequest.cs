using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020000FE RID: 254
	public sealed class HttpListenerRequest
	{
		// Token: 0x0600091A RID: 2330 RVA: 0x00033030 File Offset: 0x00031230
		internal unsafe HttpListenerRequest(HttpListenerContext httpContext, RequestContextBase memoryBlob)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", "httpContext#" + ValidationHelper.HashString(httpContext) + " memoryBlob# " + ValidationHelper.HashString((IntPtr)((void*)memoryBlob.RequestBlob)));
			}
			if (Logging.On)
			{
				Logging.Associate(Logging.HttpListener, this, httpContext);
			}
			this.m_HttpContext = httpContext;
			this.m_MemoryBlob = memoryBlob;
			this.m_BoundaryType = BoundaryType.None;
			this.m_RequestId = memoryBlob.RequestBlob->RequestId;
			this.m_ConnectionId = memoryBlob.RequestBlob->ConnectionId;
			this.m_SslStatus = ((memoryBlob.RequestBlob->pSslInfo == null) ? HttpListenerRequest.SslStatus.Insecure : ((memoryBlob.RequestBlob->pSslInfo->SslClientCertNegotiated == 0U) ? HttpListenerRequest.SslStatus.NoClientCert : HttpListenerRequest.SslStatus.ClientCert));
			if (memoryBlob.RequestBlob->pRawUrl != null && memoryBlob.RequestBlob->RawUrlLength > 0)
			{
				this.m_RawUrl = Marshal.PtrToStringAnsi((IntPtr)((void*)memoryBlob.RequestBlob->pRawUrl), (int)memoryBlob.RequestBlob->RawUrlLength);
			}
			UnsafeNclNativeMethods.HttpApi.HTTP_COOKED_URL cookedUrl = memoryBlob.RequestBlob->CookedUrl;
			if (cookedUrl.pHost != null && cookedUrl.HostLength > 0)
			{
				this.m_CookedUrlHost = Marshal.PtrToStringUni((IntPtr)((void*)cookedUrl.pHost), (int)(cookedUrl.HostLength / 2));
			}
			if (cookedUrl.pAbsPath != null && cookedUrl.AbsPathLength > 0)
			{
				this.m_CookedUrlPath = Marshal.PtrToStringUni((IntPtr)((void*)cookedUrl.pAbsPath), (int)(cookedUrl.AbsPathLength / 2));
			}
			if (cookedUrl.pQueryString != null && cookedUrl.QueryStringLength > 0)
			{
				this.m_CookedUrlQuery = Marshal.PtrToStringUni((IntPtr)((void*)cookedUrl.pQueryString), (int)(cookedUrl.QueryStringLength / 2));
			}
			this.m_Version = new Version((int)memoryBlob.RequestBlob->Version.MajorVersion, (int)memoryBlob.RequestBlob->Version.MinorVersion);
			this.m_ClientCertState = ListenerClientCertState.NotInitialized;
			this.m_KeepAlive = TriState.Unspecified;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", string.Concat(new string[]
				{
					"httpContext#",
					ValidationHelper.HashString(httpContext),
					" RequestUri:",
					ValidationHelper.ToString(this.RequestUri),
					" Content-Length:",
					ValidationHelper.ToString(this.ContentLength64),
					" HTTP Method:",
					ValidationHelper.ToString(this.HttpMethod)
				}));
			}
			if (Logging.On)
			{
				StringBuilder stringBuilder = new StringBuilder("HttpListenerRequest Headers:\n");
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
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x00033327 File Offset: 0x00031527
		internal HttpListenerContext HttpListenerContext
		{
			get
			{
				return this.m_HttpContext;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x0003332F File Offset: 0x0003152F
		internal byte[] RequestBuffer
		{
			get
			{
				this.CheckDisposed();
				return this.m_MemoryBlob.RequestBuffer;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x00033342 File Offset: 0x00031542
		internal IntPtr OriginalBlobAddress
		{
			get
			{
				this.CheckDisposed();
				return this.m_MemoryBlob.OriginalBlobAddress;
			}
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00033355 File Offset: 0x00031555
		internal void DetachBlob(RequestContextBase memoryBlob)
		{
			if (memoryBlob != null && memoryBlob == this.m_MemoryBlob)
			{
				this.m_MemoryBlob = null;
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0003336A File Offset: 0x0003156A
		internal void ReleasePins()
		{
			this.m_MemoryBlob.ReleasePins();
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x00033378 File Offset: 0x00031578
		public unsafe Guid RequestTraceIdentifier
		{
			get
			{
				Guid guid = default(Guid);
				1[(long*)(&guid)] = (long)this.RequestId;
				return guid;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x0003339A File Offset: 0x0003159A
		internal ulong RequestId
		{
			get
			{
				return this.m_RequestId;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x000333A2 File Offset: 0x000315A2
		public string[] AcceptTypes
		{
			get
			{
				return HttpListenerRequest.Helpers.ParseMultivalueHeader(this.GetKnownHeader(HttpRequestHeader.Accept));
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x000333B4 File Offset: 0x000315B4
		public Encoding ContentEncoding
		{
			get
			{
				if (this.UserAgent != null && CultureInfo.InvariantCulture.CompareInfo.IsPrefix(this.UserAgent, "UP"))
				{
					string text = this.Headers["x-up-devcap-post-charset"];
					if (text != null && text.Length > 0)
					{
						try
						{
							return Encoding.GetEncoding(text);
						}
						catch (ArgumentException)
						{
						}
					}
				}
				if (this.HasEntityBody && this.ContentType != null)
				{
					string attributeFromHeader = HttpListenerRequest.Helpers.GetAttributeFromHeader(this.ContentType, "charset");
					if (attributeFromHeader != null)
					{
						try
						{
							return Encoding.GetEncoding(attributeFromHeader);
						}
						catch (ArgumentException)
						{
						}
					}
				}
				return Encoding.Default;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x00033464 File Offset: 0x00031664
		public long ContentLength64
		{
			get
			{
				if (this.m_BoundaryType == BoundaryType.None)
				{
					if ("chunked".Equals(this.GetKnownHeader(HttpRequestHeader.TransferEncoding), StringComparison.OrdinalIgnoreCase))
					{
						this.m_BoundaryType = BoundaryType.Chunked;
						this.m_ContentLength = -1L;
					}
					else
					{
						this.m_ContentLength = 0L;
						this.m_BoundaryType = BoundaryType.ContentLength;
						string knownHeader = this.GetKnownHeader(HttpRequestHeader.ContentLength);
						if (knownHeader != null && !long.TryParse(knownHeader, NumberStyles.None, CultureInfo.InvariantCulture.NumberFormat, out this.m_ContentLength))
						{
							this.m_ContentLength = 0L;
							this.m_BoundaryType = BoundaryType.Invalid;
						}
					}
				}
				return this.m_ContentLength;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x000334EA File Offset: 0x000316EA
		public string ContentType
		{
			get
			{
				return this.GetKnownHeader(HttpRequestHeader.ContentType);
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x000334F4 File Offset: 0x000316F4
		public NameValueCollection Headers
		{
			get
			{
				if (this.m_WebHeaders == null)
				{
					this.m_WebHeaders = UnsafeNclNativeMethods.HttpApi.GetHeaders(this.RequestBuffer, this.OriginalBlobAddress);
				}
				return this.m_WebHeaders;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x0003351B File Offset: 0x0003171B
		public string HttpMethod
		{
			get
			{
				if (this.m_HttpMethod == null)
				{
					this.m_HttpMethod = UnsafeNclNativeMethods.HttpApi.GetVerb(this.RequestBuffer, this.OriginalBlobAddress);
				}
				return this.m_HttpMethod;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x00033544 File Offset: 0x00031744
		public Stream InputStream
		{
			get
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.HttpListener, this, "InputStream_get", "");
				}
				if (this.m_RequestStream == null)
				{
					this.m_RequestStream = (this.HasEntityBody ? new HttpRequestStream(this.HttpListenerContext) : Stream.Null);
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "InputStream_get", "");
				}
				return this.m_RequestStream;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x000335B8 File Offset: 0x000317B8
		public bool IsAuthenticated
		{
			get
			{
				IPrincipal user = this.HttpListenerContext.User;
				return user != null && user.Identity != null && user.Identity.IsAuthenticated;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x000335E9 File Offset: 0x000317E9
		public bool IsLocal
		{
			get
			{
				return this.LocalEndPoint.Address.Equals(this.RemoteEndPoint.Address);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x00033606 File Offset: 0x00031806
		public bool IsSecureConnection
		{
			get
			{
				return this.m_SslStatus > HttpListenerRequest.SslStatus.Insecure;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x00033614 File Offset: 0x00031814
		public bool IsWebSocketRequest
		{
			get
			{
				if (!WebSocketProtocolComponent.IsSupported)
				{
					return false;
				}
				bool flag = false;
				if (string.IsNullOrEmpty(this.Headers["Connection"]) || string.IsNullOrEmpty(this.Headers["Upgrade"]))
				{
					return false;
				}
				foreach (string text in this.Headers.GetValues("Connection"))
				{
					if (string.Compare(text, "Upgrade", StringComparison.OrdinalIgnoreCase) == 0)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				foreach (string text2 in this.Headers.GetValues("Upgrade"))
				{
					if (string.Compare(text2, "websocket", StringComparison.OrdinalIgnoreCase) == 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x000336D4 File Offset: 0x000318D4
		public NameValueCollection QueryString
		{
			get
			{
				NameValueCollection nameValueCollection = new NameValueCollection();
				HttpListenerRequest.Helpers.FillFromString(nameValueCollection, this.Url.Query, true, this.ContentEncoding);
				return nameValueCollection;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x00033700 File Offset: 0x00031900
		public string RawUrl
		{
			get
			{
				return this.m_RawUrl;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x00033708 File Offset: 0x00031908
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x00033710 File Offset: 0x00031910
		public string ServiceName
		{
			get
			{
				return this.m_ServiceName;
			}
			internal set
			{
				this.m_ServiceName = value;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x00033719 File Offset: 0x00031919
		public Uri Url
		{
			get
			{
				return this.RequestUri;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x00033724 File Offset: 0x00031924
		public Uri UrlReferrer
		{
			get
			{
				string knownHeader = this.GetKnownHeader(HttpRequestHeader.Referer);
				if (knownHeader == null)
				{
					return null;
				}
				Uri uri;
				if (!Uri.TryCreate(knownHeader, UriKind.RelativeOrAbsolute, out uri))
				{
					return null;
				}
				return uri;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x0003374F File Offset: 0x0003194F
		public string UserAgent
		{
			get
			{
				return this.GetKnownHeader(HttpRequestHeader.UserAgent);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x00033759 File Offset: 0x00031959
		public string UserHostAddress
		{
			get
			{
				return this.LocalEndPoint.ToString();
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x00033766 File Offset: 0x00031966
		public string UserHostName
		{
			get
			{
				return this.GetKnownHeader(HttpRequestHeader.Host);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x00033770 File Offset: 0x00031970
		public string[] UserLanguages
		{
			get
			{
				return HttpListenerRequest.Helpers.ParseMultivalueHeader(this.GetKnownHeader(HttpRequestHeader.AcceptLanguage));
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x00033780 File Offset: 0x00031980
		public int ClientCertificateError
		{
			get
			{
				if (this.m_ClientCertState == ListenerClientCertState.NotInitialized)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_listener_mustcall", new object[] { "GetClientCertificate()/BeginGetClientCertificate()" }));
				}
				if (this.m_ClientCertState == ListenerClientCertState.InProgress)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_listener_mustcompletecall", new object[] { "GetClientCertificate()/BeginGetClientCertificate()" }));
				}
				return this.m_ClientCertificateError;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x000337E0 File Offset: 0x000319E0
		internal X509Certificate2 ClientCertificate
		{
			set
			{
				this.m_ClientCertificate = value;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (set) Token: 0x06000939 RID: 2361 RVA: 0x000337E9 File Offset: 0x000319E9
		internal ListenerClientCertState ClientCertState
		{
			set
			{
				this.m_ClientCertState = value;
			}
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x000337F2 File Offset: 0x000319F2
		internal void SetClientCertificateError(int clientCertificateError)
		{
			this.m_ClientCertificateError = clientCertificateError;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x000337FC File Offset: 0x000319FC
		public X509Certificate2 GetClientCertificate()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "GetClientCertificate", "");
			}
			try
			{
				this.ProcessClientCertificate();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "GetClientCertificate", ValidationHelper.ToString(this.m_ClientCertificate));
				}
			}
			return this.m_ClientCertificate;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00033868 File Offset: 0x00031A68
		public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "BeginGetClientCertificate", "");
			}
			return this.AsyncProcessClientCertificate(requestCallback, state);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00033890 File Offset: 0x00031A90
		public X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "EndGetClientCertificate", "");
			}
			X509Certificate2 x509Certificate = null;
			try
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				ListenerClientCertAsyncResult listenerClientCertAsyncResult = asyncResult as ListenerClientCertAsyncResult;
				if (listenerClientCertAsyncResult == null || listenerClientCertAsyncResult.AsyncObject != this)
				{
					throw new ArgumentException(global::System.SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (listenerClientCertAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(global::System.SR.GetString("net_io_invalidendcall", new object[] { "EndGetClientCertificate" }));
				}
				listenerClientCertAsyncResult.EndCalled = true;
				x509Certificate = listenerClientCertAsyncResult.InternalWaitForCompletion() as X509Certificate2;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "EndGetClientCertificate", ValidationHelper.HashString(x509Certificate));
				}
			}
			return x509Certificate;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0003395C File Offset: 0x00031B5C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<X509Certificate2> GetClientCertificateAsync()
		{
			return Task<X509Certificate2>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginGetClientCertificate), new Func<IAsyncResult, X509Certificate2>(this.EndGetClientCertificate), null);
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00033981 File Offset: 0x00031B81
		public TransportContext TransportContext
		{
			get
			{
				return new HttpListenerRequestContext(this);
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0003398C File Offset: 0x00031B8C
		private CookieCollection ParseCookies(Uri uri, string setCookieHeader)
		{
			CookieCollection cookieCollection = new CookieCollection();
			CookieParser cookieParser = new CookieParser(setCookieHeader);
			for (;;)
			{
				Cookie server = cookieParser.GetServer();
				if (server == null)
				{
					break;
				}
				if (server.Name.Length != 0)
				{
					cookieCollection.InternalAdd(server, true);
				}
			}
			return cookieCollection;
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x000339CC File Offset: 0x00031BCC
		public CookieCollection Cookies
		{
			get
			{
				if (this.m_Cookies == null)
				{
					string knownHeader = this.GetKnownHeader(HttpRequestHeader.Cookie);
					if (knownHeader != null && knownHeader.Length > 0)
					{
						this.m_Cookies = this.ParseCookies(this.RequestUri, knownHeader);
					}
					if (this.m_Cookies == null)
					{
						this.m_Cookies = new CookieCollection();
					}
					if (this.HttpListenerContext.PromoteCookiesToRfc2965)
					{
						for (int i = 0; i < this.m_Cookies.Count; i++)
						{
							if (this.m_Cookies[i].Variant == CookieVariant.Rfc2109)
							{
								this.m_Cookies[i].Variant = CookieVariant.Rfc2965;
							}
						}
					}
				}
				return this.m_Cookies;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x00033A6E File Offset: 0x00031C6E
		public Version ProtocolVersion
		{
			get
			{
				return this.m_Version;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x00033A76 File Offset: 0x00031C76
		public bool HasEntityBody
		{
			get
			{
				return (this.ContentLength64 > 0L && this.m_BoundaryType == BoundaryType.ContentLength) || this.m_BoundaryType == BoundaryType.Chunked || this.m_BoundaryType == BoundaryType.Multipart;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x00033AA0 File Offset: 0x00031CA0
		public bool KeepAlive
		{
			get
			{
				if (this.m_KeepAlive == TriState.Unspecified)
				{
					string text = this.Headers["Proxy-Connection"];
					if (string.IsNullOrEmpty(text))
					{
						text = this.GetKnownHeader(HttpRequestHeader.Connection);
					}
					if (string.IsNullOrEmpty(text))
					{
						if (this.ProtocolVersion >= HttpVersion.Version11)
						{
							this.m_KeepAlive = TriState.True;
						}
						else
						{
							text = this.GetKnownHeader(HttpRequestHeader.KeepAlive);
							this.m_KeepAlive = (string.IsNullOrEmpty(text) ? TriState.False : TriState.True);
						}
					}
					else
					{
						text = text.ToLower(CultureInfo.InvariantCulture);
						this.m_KeepAlive = ((text.IndexOf("close") < 0 || text.IndexOf("keep-alive") >= 0) ? TriState.True : TriState.False);
					}
				}
				return this.m_KeepAlive == TriState.True;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00033B54 File Offset: 0x00031D54
		public IPEndPoint RemoteEndPoint
		{
			get
			{
				if (this.m_RemoteEndPoint == null)
				{
					this.m_RemoteEndPoint = UnsafeNclNativeMethods.HttpApi.GetRemoteEndPoint(this.RequestBuffer, this.OriginalBlobAddress);
				}
				return this.m_RemoteEndPoint;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x00033B7B File Offset: 0x00031D7B
		public IPEndPoint LocalEndPoint
		{
			get
			{
				if (this.m_LocalEndPoint == null)
				{
					this.m_LocalEndPoint = UnsafeNclNativeMethods.HttpApi.GetLocalEndPoint(this.RequestBuffer, this.OriginalBlobAddress);
				}
				return this.m_LocalEndPoint;
			}
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00033BA4 File Offset: 0x00031DA4
		internal void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close", "");
			}
			RequestContextBase memoryBlob = this.m_MemoryBlob;
			if (memoryBlob != null)
			{
				memoryBlob.Close();
				this.m_MemoryBlob = null;
			}
			this.m_IsDisposed = true;
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Close", "");
			}
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00033C08 File Offset: 0x00031E08
		private unsafe ListenerClientCertAsyncResult AsyncProcessClientCertificate(AsyncCallback requestCallback, object state)
		{
			if (this.m_ClientCertState == ListenerClientCertState.InProgress)
			{
				throw new InvalidOperationException(global::System.SR.GetString("net_listener_callinprogress", new object[] { "GetClientCertificate()/BeginGetClientCertificate()" }));
			}
			this.m_ClientCertState = ListenerClientCertState.InProgress;
			this.HttpListenerContext.EnsureBoundHandle();
			ListenerClientCertAsyncResult listenerClientCertAsyncResult = null;
			if (this.m_SslStatus != HttpListenerRequest.SslStatus.Insecure)
			{
				uint num = 1500U;
				listenerClientCertAsyncResult = new ListenerClientCertAsyncResult(this, state, requestCallback, num);
				try
				{
					uint num2;
					uint num3;
					for (;;)
					{
						num2 = 0U;
						num3 = UnsafeNclNativeMethods.HttpApi.HttpReceiveClientCertificate(this.HttpListenerContext.RequestQueueHandle, this.m_ConnectionId, 0U, listenerClientCertAsyncResult.RequestBlob, num, &num2, listenerClientCertAsyncResult.NativeOverlapped);
						if (num3 != 234U)
						{
							break;
						}
						UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO* requestBlob = listenerClientCertAsyncResult.RequestBlob;
						num = num2 + requestBlob->CertEncodedSize;
						listenerClientCertAsyncResult.Reset(num);
					}
					if (num3 != 0U && num3 != 997U)
					{
						throw new HttpListenerException((int)num3);
					}
					if (num3 == 0U && HttpListener.SkipIOCPCallbackOnSuccess)
					{
						listenerClientCertAsyncResult.IOCompleted(num3, num2);
					}
					return listenerClientCertAsyncResult;
				}
				catch
				{
					if (listenerClientCertAsyncResult != null)
					{
						listenerClientCertAsyncResult.InternalCleanup();
					}
					throw;
				}
			}
			listenerClientCertAsyncResult = new ListenerClientCertAsyncResult(this, state, requestCallback, 0U);
			listenerClientCertAsyncResult.InvokeCallback();
			return listenerClientCertAsyncResult;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00033D0C File Offset: 0x00031F0C
		private unsafe void ProcessClientCertificate()
		{
			if (this.m_ClientCertState == ListenerClientCertState.InProgress)
			{
				throw new InvalidOperationException(global::System.SR.GetString("net_listener_callinprogress", new object[] { "GetClientCertificate()/BeginGetClientCertificate()" }));
			}
			this.m_ClientCertState = ListenerClientCertState.InProgress;
			if (this.m_SslStatus != HttpListenerRequest.SslStatus.Insecure)
			{
				uint num = 1500U;
				for (;;)
				{
					byte[] array = new byte[checked((int)num)];
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
						UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO* ptr2 = (UnsafeNclNativeMethods.HttpApi.HTTP_SSL_CLIENT_CERT_INFO*)ptr;
						uint num2 = 0U;
						uint num3 = UnsafeNclNativeMethods.HttpApi.HttpReceiveClientCertificate(this.HttpListenerContext.RequestQueueHandle, this.m_ConnectionId, 0U, ptr2, num, &num2, null);
						if (num3 == 234U)
						{
							num = num2 + ptr2->CertEncodedSize;
							continue;
						}
						if (num3 == 0U && ptr2 != null)
						{
							if (ptr2->pCertEncoded != null)
							{
								try
								{
									byte[] array3 = new byte[ptr2->CertEncodedSize];
									Marshal.Copy((IntPtr)((void*)ptr2->pCertEncoded), array3, 0, array3.Length);
									this.m_ClientCertificate = new X509Certificate2(array3);
								}
								catch (CryptographicException ex)
								{
								}
								catch (SecurityException ex2)
								{
								}
							}
							this.m_ClientCertificateError = (int)ptr2->CertFlags;
						}
					}
					finally
					{
						byte[] array2 = null;
					}
					break;
				}
			}
			this.m_ClientCertState = ListenerClientCertState.Completed;
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x00033E48 File Offset: 0x00032048
		private string RequestScheme
		{
			get
			{
				if (!this.IsSecureConnection)
				{
					return "http";
				}
				return "https";
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00033E5D File Offset: 0x0003205D
		private Uri RequestUri
		{
			get
			{
				if (this.m_RequestUri == null)
				{
					this.m_RequestUri = HttpListenerRequestUriBuilder.GetRequestUri(this.m_RawUrl, this.RequestScheme, this.m_CookedUrlHost, this.m_CookedUrlPath, this.m_CookedUrlQuery);
				}
				return this.m_RequestUri;
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00033E9C File Offset: 0x0003209C
		private string GetKnownHeader(HttpRequestHeader header)
		{
			return UnsafeNclNativeMethods.HttpApi.GetKnownHeader(this.RequestBuffer, this.OriginalBlobAddress, (int)header);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00033EB0 File Offset: 0x000320B0
		internal ChannelBinding GetChannelBinding()
		{
			return this.HttpListenerContext.Listener.GetChannelBindingFromTls(this.m_ConnectionId);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00033EC8 File Offset: 0x000320C8
		internal IEnumerable<TokenBinding> GetTlsTokenBindings()
		{
			if (Volatile.Read<List<TokenBinding>>(ref this.m_TokenBindings) == null)
			{
				object @lock = this.m_Lock;
				lock (@lock)
				{
					if (Volatile.Read<List<TokenBinding>>(ref this.m_TokenBindings) == null)
					{
						if (UnsafeNclNativeMethods.TokenBindingOSHelper.SupportsTokenBinding)
						{
							this.ProcessTlsTokenBindings();
						}
						else
						{
							this.m_TokenBindings = new List<TokenBinding>();
						}
					}
				}
			}
			if (this.m_TokenBindingVerifyMessageStatus != 0)
			{
				throw new HttpListenerException(this.m_TokenBindingVerifyMessageStatus);
			}
			return this.m_TokenBindings.AsReadOnly();
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00033F58 File Offset: 0x00032158
		private unsafe void ProcessTlsTokenBindings()
		{
			if (this.m_TokenBindings != null)
			{
				return;
			}
			this.m_TokenBindings = new List<TokenBinding>();
			UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_TOKEN_BINDING_INFO* tlsTokenBindingRequestInfo = UnsafeNclNativeMethods.HttpApi.GetTlsTokenBindingRequestInfo(this.RequestBuffer, this.OriginalBlobAddress);
			UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_TOKEN_BINDING_INFO_V1* ptr = null;
			bool flag = false;
			if (tlsTokenBindingRequestInfo == null)
			{
				ptr = UnsafeNclNativeMethods.HttpApi.GetTlsTokenBindingRequestInfo_V1(this.RequestBuffer, this.OriginalBlobAddress);
				flag = true;
			}
			if (tlsTokenBindingRequestInfo == null && ptr == null)
			{
				return;
			}
			UnsafeNclNativeMethods.HttpApi.HeapAllocHandle heapAllocHandle = null;
			this.m_TokenBindingVerifyMessageStatus = -1;
			byte[] array;
			byte* ptr2;
			if ((array = this.RequestBuffer) == null || array.Length == 0)
			{
				ptr2 = null;
			}
			else
			{
				ptr2 = &array[0];
			}
			long num = (long)((byte*)ptr2 - (byte*)(void*)this.OriginalBlobAddress);
			if (flag && ptr != null)
			{
				this.m_TokenBindingVerifyMessageStatus = UnsafeNclNativeMethods.HttpApi.TokenBindingVerifyMessage_V1(ptr->TokenBinding + num, ptr->TokenBindingSize, (IntPtr)((void*)((byte*)(void*)ptr->KeyType + num)), ptr->TlsUnique + num, ptr->TlsUniqueSize, out heapAllocHandle);
			}
			else
			{
				this.m_TokenBindingVerifyMessageStatus = UnsafeNclNativeMethods.HttpApi.TokenBindingVerifyMessage(tlsTokenBindingRequestInfo->TokenBinding + num, tlsTokenBindingRequestInfo->TokenBindingSize, tlsTokenBindingRequestInfo->KeyType, tlsTokenBindingRequestInfo->TlsUnique + num, tlsTokenBindingRequestInfo->TlsUniqueSize, out heapAllocHandle);
			}
			array = null;
			if (this.m_TokenBindingVerifyMessageStatus != 0)
			{
				throw new HttpListenerException(this.m_TokenBindingVerifyMessageStatus);
			}
			using (heapAllocHandle)
			{
				if (flag)
				{
					this.GenerateTokenBindings_V1(heapAllocHandle);
				}
				else
				{
					this.GenerateTokenBindings(heapAllocHandle);
				}
			}
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x000340C0 File Offset: 0x000322C0
		private unsafe void GenerateTokenBindings(UnsafeNclNativeMethods.HttpApi.HeapAllocHandle handle)
		{
			UnsafeNclNativeMethods.HttpApi.TOKENBINDING_RESULT_LIST* ptr = (UnsafeNclNativeMethods.HttpApi.TOKENBINDING_RESULT_LIST*)(void*)handle.DangerousGetHandle();
			int num = 0;
			while ((long)num < (long)((ulong)ptr->resultCount))
			{
				UnsafeNclNativeMethods.HttpApi.TOKENBINDING_RESULT_DATA* ptr2 = ptr->resultData + num;
				if (ptr2 != null)
				{
					byte[] array = new byte[ptr2->identifierSize];
					Marshal.Copy((IntPtr)((void*)ptr2->identifierData), array, 0, array.Length);
					if (ptr2->bindingType == UnsafeNclNativeMethods.HttpApi.TOKENBINDING_TYPE.TOKENBINDING_TYPE_PROVIDED)
					{
						this.m_TokenBindings.Add(new TokenBinding(TokenBindingType.Provided, array));
					}
					else if (ptr2->bindingType == UnsafeNclNativeMethods.HttpApi.TOKENBINDING_TYPE.TOKENBINDING_TYPE_REFERRED)
					{
						this.m_TokenBindings.Add(new TokenBinding(TokenBindingType.Referred, array));
					}
				}
				num++;
			}
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0003415C File Offset: 0x0003235C
		private unsafe void GenerateTokenBindings_V1(UnsafeNclNativeMethods.HttpApi.HeapAllocHandle handle)
		{
			UnsafeNclNativeMethods.HttpApi.TOKENBINDING_RESULT_LIST_V1* ptr = (UnsafeNclNativeMethods.HttpApi.TOKENBINDING_RESULT_LIST_V1*)(void*)handle.DangerousGetHandle();
			int num = 0;
			while ((long)num < (long)((ulong)ptr->resultCount))
			{
				UnsafeNclNativeMethods.HttpApi.TOKENBINDING_RESULT_DATA_V1* ptr2 = ptr->resultData + num;
				if (ptr2 != null)
				{
					byte[] array = new byte[ptr2->identifierSize - 1U];
					Marshal.Copy((IntPtr)((void*)(&ptr2->identifierData->hashAlgorithm)), array, 0, array.Length);
					if (ptr2->identifierData->bindingType == UnsafeNclNativeMethods.HttpApi.TOKENBINDING_TYPE.TOKENBINDING_TYPE_PROVIDED)
					{
						this.m_TokenBindings.Add(new TokenBinding(TokenBindingType.Provided, array));
					}
					else if (ptr2->identifierData->bindingType == UnsafeNclNativeMethods.HttpApi.TOKENBINDING_TYPE.TOKENBINDING_TYPE_REFERRED)
					{
						this.m_TokenBindings.Add(new TokenBinding(TokenBindingType.Referred, array));
					}
				}
				num++;
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0003420F File Offset: 0x0003240F
		internal void CheckDisposed()
		{
			if (this.m_IsDisposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x04000E15 RID: 3605
		private Uri m_RequestUri;

		// Token: 0x04000E16 RID: 3606
		private ulong m_RequestId;

		// Token: 0x04000E17 RID: 3607
		internal ulong m_ConnectionId;

		// Token: 0x04000E18 RID: 3608
		private HttpListenerRequest.SslStatus m_SslStatus;

		// Token: 0x04000E19 RID: 3609
		private string m_RawUrl;

		// Token: 0x04000E1A RID: 3610
		private string m_CookedUrlHost;

		// Token: 0x04000E1B RID: 3611
		private string m_CookedUrlPath;

		// Token: 0x04000E1C RID: 3612
		private string m_CookedUrlQuery;

		// Token: 0x04000E1D RID: 3613
		private long m_ContentLength;

		// Token: 0x04000E1E RID: 3614
		private Stream m_RequestStream;

		// Token: 0x04000E1F RID: 3615
		private string m_HttpMethod;

		// Token: 0x04000E20 RID: 3616
		private TriState m_KeepAlive;

		// Token: 0x04000E21 RID: 3617
		private Version m_Version;

		// Token: 0x04000E22 RID: 3618
		private WebHeaderCollection m_WebHeaders;

		// Token: 0x04000E23 RID: 3619
		private IPEndPoint m_LocalEndPoint;

		// Token: 0x04000E24 RID: 3620
		private IPEndPoint m_RemoteEndPoint;

		// Token: 0x04000E25 RID: 3621
		private BoundaryType m_BoundaryType;

		// Token: 0x04000E26 RID: 3622
		private ListenerClientCertState m_ClientCertState;

		// Token: 0x04000E27 RID: 3623
		private X509Certificate2 m_ClientCertificate;

		// Token: 0x04000E28 RID: 3624
		private int m_ClientCertificateError;

		// Token: 0x04000E29 RID: 3625
		private RequestContextBase m_MemoryBlob;

		// Token: 0x04000E2A RID: 3626
		private CookieCollection m_Cookies;

		// Token: 0x04000E2B RID: 3627
		private HttpListenerContext m_HttpContext;

		// Token: 0x04000E2C RID: 3628
		private bool m_IsDisposed;

		// Token: 0x04000E2D RID: 3629
		internal const uint CertBoblSize = 1500U;

		// Token: 0x04000E2E RID: 3630
		private string m_ServiceName;

		// Token: 0x04000E2F RID: 3631
		private object m_Lock = new object();

		// Token: 0x04000E30 RID: 3632
		private List<TokenBinding> m_TokenBindings;

		// Token: 0x04000E31 RID: 3633
		private int m_TokenBindingVerifyMessageStatus;

		// Token: 0x02000702 RID: 1794
		private enum SslStatus : byte
		{
			// Token: 0x040030C9 RID: 12489
			Insecure,
			// Token: 0x040030CA RID: 12490
			NoClientCert,
			// Token: 0x040030CB RID: 12491
			ClientCert
		}

		// Token: 0x02000703 RID: 1795
		private static class Helpers
		{
			// Token: 0x06004095 RID: 16533 RVA: 0x0010E9A0 File Offset: 0x0010CBA0
			internal static string GetAttributeFromHeader(string headerValue, string attrName)
			{
				if (headerValue == null)
				{
					return null;
				}
				int length = headerValue.Length;
				int length2 = attrName.Length;
				int i;
				for (i = 1; i < length; i += length2)
				{
					i = CultureInfo.InvariantCulture.CompareInfo.IndexOf(headerValue, attrName, i, CompareOptions.IgnoreCase);
					if (i < 0 || i + length2 >= length)
					{
						break;
					}
					char c = headerValue[i - 1];
					char c2 = headerValue[i + length2];
					if ((c == ';' || c == ',' || char.IsWhiteSpace(c)) && (c2 == '=' || char.IsWhiteSpace(c2)))
					{
						break;
					}
				}
				if (i < 0 || i >= length)
				{
					return null;
				}
				i += length2;
				while (i < length && char.IsWhiteSpace(headerValue[i]))
				{
					i++;
				}
				if (i >= length || headerValue[i] != '=')
				{
					return null;
				}
				i++;
				while (i < length && char.IsWhiteSpace(headerValue[i]))
				{
					i++;
				}
				if (i >= length)
				{
					return null;
				}
				string text;
				if (i < length && headerValue[i] == '"')
				{
					if (i == length - 1)
					{
						return null;
					}
					int num = headerValue.IndexOf('"', i + 1);
					if (num < 0 || num == i + 1)
					{
						return null;
					}
					text = headerValue.Substring(i + 1, num - i - 1).Trim();
				}
				else
				{
					int num = i;
					while (num < length && headerValue[num] != ' ' && headerValue[num] != ',')
					{
						num++;
					}
					if (num == i)
					{
						return null;
					}
					text = headerValue.Substring(i, num - i).Trim();
				}
				return text;
			}

			// Token: 0x06004096 RID: 16534 RVA: 0x0010EB0C File Offset: 0x0010CD0C
			internal static string[] ParseMultivalueHeader(string s)
			{
				if (s == null)
				{
					return null;
				}
				int length = s.Length;
				ArrayList arrayList = new ArrayList();
				int i = 0;
				while (i < length)
				{
					int num = s.IndexOf(',', i);
					if (num < 0)
					{
						num = length;
					}
					arrayList.Add(s.Substring(i, num - i));
					i = num + 1;
					if (i < length && s[i] == ' ')
					{
						i++;
					}
				}
				int count = arrayList.Count;
				string[] array;
				if (count == 0)
				{
					array = new string[] { string.Empty };
				}
				else
				{
					array = new string[count];
					arrayList.CopyTo(0, array, 0, count);
				}
				return array;
			}

			// Token: 0x06004097 RID: 16535 RVA: 0x0010EBA4 File Offset: 0x0010CDA4
			private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
			{
				int length = s.Length;
				HttpListenerRequest.Helpers.UrlDecoder urlDecoder = new HttpListenerRequest.Helpers.UrlDecoder(length, e);
				int i = 0;
				while (i < length)
				{
					char c = s[i];
					if (c == '+')
					{
						c = ' ';
						goto IL_0106;
					}
					if (c != '%' || i >= length - 2)
					{
						goto IL_0106;
					}
					if (s[i + 1] == 'u' && i < length - 5)
					{
						int num = HttpListenerRequest.Helpers.HexToInt(s[i + 2]);
						int num2 = HttpListenerRequest.Helpers.HexToInt(s[i + 3]);
						int num3 = HttpListenerRequest.Helpers.HexToInt(s[i + 4]);
						int num4 = HttpListenerRequest.Helpers.HexToInt(s[i + 5]);
						if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0)
						{
							goto IL_0106;
						}
						c = (char)((num << 12) | (num2 << 8) | (num3 << 4) | num4);
						i += 5;
						urlDecoder.AddChar(c);
					}
					else
					{
						int num5 = HttpListenerRequest.Helpers.HexToInt(s[i + 1]);
						int num6 = HttpListenerRequest.Helpers.HexToInt(s[i + 2]);
						if (num5 < 0 || num6 < 0)
						{
							goto IL_0106;
						}
						byte b = (byte)((num5 << 4) | num6);
						i += 2;
						urlDecoder.AddByte(b);
					}
					IL_0120:
					i++;
					continue;
					IL_0106:
					if ((c & 'ﾀ') == '\0')
					{
						urlDecoder.AddByte((byte)c);
						goto IL_0120;
					}
					urlDecoder.AddChar(c);
					goto IL_0120;
				}
				return urlDecoder.GetString();
			}

			// Token: 0x06004098 RID: 16536 RVA: 0x0010ECE2 File Offset: 0x0010CEE2
			private static int HexToInt(char h)
			{
				if (h >= '0' && h <= '9')
				{
					return (int)(h - '0');
				}
				if (h >= 'a' && h <= 'f')
				{
					return (int)(h - 'a' + '\n');
				}
				if (h < 'A' || h > 'F')
				{
					return -1;
				}
				return (int)(h - 'A' + '\n');
			}

			// Token: 0x06004099 RID: 16537 RVA: 0x0010ED18 File Offset: 0x0010CF18
			internal static void FillFromString(NameValueCollection nvc, string s, bool urlencoded, Encoding encoding)
			{
				int num = ((s != null) ? s.Length : 0);
				for (int i = ((s.Length > 0 && s[0] == '?') ? 1 : 0); i < num; i++)
				{
					int num2 = i;
					int num3 = -1;
					while (i < num)
					{
						char c = s[i];
						if (c == '=')
						{
							if (num3 < 0)
							{
								num3 = i;
							}
						}
						else if (c == '&')
						{
							break;
						}
						i++;
					}
					string text = null;
					string text2;
					if (num3 >= 0)
					{
						text = s.Substring(num2, num3 - num2);
						text2 = s.Substring(num3 + 1, i - num3 - 1);
					}
					else
					{
						text2 = s.Substring(num2, i - num2);
					}
					if (urlencoded)
					{
						nvc.Add((text == null) ? null : HttpListenerRequest.Helpers.UrlDecodeStringFromStringInternal(text, encoding), HttpListenerRequest.Helpers.UrlDecodeStringFromStringInternal(text2, encoding));
					}
					else
					{
						nvc.Add(text, text2);
					}
					if (i == num - 1 && s[i] == '&')
					{
						nvc.Add(null, "");
					}
				}
			}

			// Token: 0x020008CD RID: 2253
			private class UrlDecoder
			{
				// Token: 0x0600462D RID: 17965 RVA: 0x00125024 File Offset: 0x00123224
				private void FlushBytes()
				{
					if (this._numBytes > 0)
					{
						this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
						this._numBytes = 0;
					}
				}

				// Token: 0x0600462E RID: 17966 RVA: 0x00125072 File Offset: 0x00123272
				internal UrlDecoder(int bufferSize, Encoding encoding)
				{
					this._bufferSize = bufferSize;
					this._encoding = encoding;
					this._charBuffer = new char[bufferSize];
				}

				// Token: 0x0600462F RID: 17967 RVA: 0x00125094 File Offset: 0x00123294
				internal void AddChar(char ch)
				{
					if (this._numBytes > 0)
					{
						this.FlushBytes();
					}
					char[] charBuffer = this._charBuffer;
					int numChars = this._numChars;
					this._numChars = numChars + 1;
					charBuffer[numChars] = ch;
				}

				// Token: 0x06004630 RID: 17968 RVA: 0x001250CC File Offset: 0x001232CC
				internal void AddByte(byte b)
				{
					if (this._byteBuffer == null)
					{
						this._byteBuffer = new byte[this._bufferSize];
					}
					byte[] byteBuffer = this._byteBuffer;
					int numBytes = this._numBytes;
					this._numBytes = numBytes + 1;
					byteBuffer[numBytes] = b;
				}

				// Token: 0x06004631 RID: 17969 RVA: 0x0012510B File Offset: 0x0012330B
				internal string GetString()
				{
					if (this._numBytes > 0)
					{
						this.FlushBytes();
					}
					if (this._numChars > 0)
					{
						return new string(this._charBuffer, 0, this._numChars);
					}
					return string.Empty;
				}

				// Token: 0x04003B60 RID: 15200
				private int _bufferSize;

				// Token: 0x04003B61 RID: 15201
				private int _numChars;

				// Token: 0x04003B62 RID: 15202
				private char[] _charBuffer;

				// Token: 0x04003B63 RID: 15203
				private int _numBytes;

				// Token: 0x04003B64 RID: 15204
				private byte[] _byteBuffer;

				// Token: 0x04003B65 RID: 15205
				private Encoding _encoding;
			}
		}
	}
}
