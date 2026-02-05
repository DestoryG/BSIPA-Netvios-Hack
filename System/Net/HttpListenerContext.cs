using System;
using System.ComponentModel;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020000F8 RID: 248
	public sealed class HttpListenerContext
	{
		// Token: 0x060008E0 RID: 2272 RVA: 0x00032718 File Offset: 0x00030918
		internal unsafe HttpListenerContext(HttpListener httpListener, RequestContextBase memoryBlob)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", "httpListener#" + ValidationHelper.HashString(httpListener) + " requestBlob=" + ValidationHelper.HashString((IntPtr)((void*)memoryBlob.RequestBlob)));
			}
			this.m_Listener = httpListener;
			this.m_Request = new HttpListenerRequest(this, memoryBlob);
			this.m_AuthenticationSchemes = httpListener.AuthenticationSchemes;
			this.m_ExtendedProtectionPolicy = httpListener.ExtendedProtectionPolicy;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00032798 File Offset: 0x00030998
		internal void SetIdentity(IPrincipal principal, string mutualAuthentication)
		{
			this.m_MutualAuthentication = mutualAuthentication;
			this.m_User = principal;
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x000327A8 File Offset: 0x000309A8
		public HttpListenerRequest Request
		{
			get
			{
				return this.m_Request;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x000327B0 File Offset: 0x000309B0
		public HttpListenerResponse Response
		{
			get
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.HttpListener, this, "Response", "");
				}
				if (this.m_Response == null)
				{
					this.m_Response = new HttpListenerResponse(this);
				}
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Response", "");
				}
				return this.m_Response;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0003280F File Offset: 0x00030A0F
		public IPrincipal User
		{
			get
			{
				if (!(this.m_User is WindowsPrincipal))
				{
					return this.m_User;
				}
				new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Demand();
				return this.m_User;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0003283A File Offset: 0x00030A3A
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x00032842 File Offset: 0x00030A42
		internal AuthenticationSchemes AuthenticationSchemes
		{
			get
			{
				return this.m_AuthenticationSchemes;
			}
			set
			{
				this.m_AuthenticationSchemes = value;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0003284B File Offset: 0x00030A4B
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x00032853 File Offset: 0x00030A53
		internal ExtendedProtectionPolicy ExtendedProtectionPolicy
		{
			get
			{
				return this.m_ExtendedProtectionPolicy;
			}
			set
			{
				this.m_ExtendedProtectionPolicy = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0003285C File Offset: 0x00030A5C
		internal bool PromoteCookiesToRfc2965
		{
			get
			{
				return this.m_PromoteCookiesToRfc2965;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x00032864 File Offset: 0x00030A64
		internal string MutualAuthentication
		{
			get
			{
				return this.m_MutualAuthentication;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0003286C File Offset: 0x00030A6C
		internal HttpListener Listener
		{
			get
			{
				return this.m_Listener;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x00032874 File Offset: 0x00030A74
		internal CriticalHandle RequestQueueHandle
		{
			get
			{
				return this.m_Listener.RequestQueueHandle;
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00032881 File Offset: 0x00030A81
		internal void EnsureBoundHandle()
		{
			this.m_Listener.EnsureBoundHandle();
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x0003288E File Offset: 0x00030A8E
		internal ulong RequestId
		{
			get
			{
				return this.Request.RequestId;
			}
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0003289B File Offset: 0x00030A9B
		public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(string subProtocol)
		{
			return this.AcceptWebSocketAsync(subProtocol, 16384, WebSocket.DefaultKeepAliveInterval);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x000328AE File Offset: 0x00030AAE
		public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(string subProtocol, TimeSpan keepAliveInterval)
		{
			return this.AcceptWebSocketAsync(subProtocol, 16384, keepAliveInterval);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000328C0 File Offset: 0x00030AC0
		public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(string subProtocol, int receiveBufferSize, TimeSpan keepAliveInterval)
		{
			WebSocketHelpers.ValidateOptions(subProtocol, receiveBufferSize, 16, keepAliveInterval);
			ArraySegment<byte> arraySegment = WebSocketBuffer.CreateInternalBufferArraySegment(receiveBufferSize, 16, true);
			return this.AcceptWebSocketAsync(subProtocol, receiveBufferSize, keepAliveInterval, arraySegment);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x000328EB File Offset: 0x00030AEB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(string subProtocol, int receiveBufferSize, TimeSpan keepAliveInterval, ArraySegment<byte> internalBuffer)
		{
			return WebSocketHelpers.AcceptWebSocketAsync(this, subProtocol, receiveBufferSize, keepAliveInterval, internalBuffer);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x000328F8 File Offset: 0x00030AF8
		internal void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close()", "");
			}
			try
			{
				if (this.m_Response != null)
				{
					this.m_Response.Close();
				}
			}
			finally
			{
				try
				{
					this.m_Request.Close();
				}
				finally
				{
					IDisposable disposable = ((this.m_User == null) ? null : (this.m_User.Identity as IDisposable));
					if (disposable != null && this.m_User.Identity.AuthenticationType != "NTLM" && !this.m_Listener.UnsafeConnectionNtlmAuthentication)
					{
						disposable.Dispose();
					}
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Close", "");
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x000329D0 File Offset: 0x00030BD0
		internal void Abort()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Abort", "");
			}
			this.ForceCancelRequest(this.RequestQueueHandle, this.m_Request.RequestId);
			try
			{
				this.m_Request.Close();
			}
			finally
			{
				IDisposable disposable = ((this.m_User == null) ? null : (this.m_User.Identity as IDisposable));
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Abort", "");
			}
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00032A70 File Offset: 0x00030C70
		internal UnsafeNclNativeMethods.HttpApi.HTTP_VERB GetKnownMethod()
		{
			return UnsafeNclNativeMethods.HttpApi.GetKnownVerb(this.Request.RequestBuffer, this.Request.OriginalBlobAddress);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00032A90 File Offset: 0x00030C90
		internal static void CancelRequest(CriticalHandle requestQueueHandle, ulong requestId)
		{
			uint num = UnsafeNclNativeMethods.HttpApi.HttpCancelHttpRequest(requestQueueHandle, requestId, IntPtr.Zero);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00032AAC File Offset: 0x00030CAC
		internal void ForceCancelRequest(CriticalHandle requestQueueHandle, ulong requestId)
		{
			uint num = UnsafeNclNativeMethods.HttpApi.HttpCancelHttpRequest(requestQueueHandle, requestId, IntPtr.Zero);
			if (num == 1229U)
			{
				this.m_Response.CancelLastWrite(requestQueueHandle);
			}
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00032ADA File Offset: 0x00030CDA
		internal void SetAuthenticationHeaders()
		{
			this.Listener.SetAuthenticationHeaders(this);
		}

		// Token: 0x04000E01 RID: 3585
		private HttpListener m_Listener;

		// Token: 0x04000E02 RID: 3586
		private HttpListenerRequest m_Request;

		// Token: 0x04000E03 RID: 3587
		private HttpListenerResponse m_Response;

		// Token: 0x04000E04 RID: 3588
		private IPrincipal m_User;

		// Token: 0x04000E05 RID: 3589
		private string m_MutualAuthentication;

		// Token: 0x04000E06 RID: 3590
		private AuthenticationSchemes m_AuthenticationSchemes;

		// Token: 0x04000E07 RID: 3591
		private ExtendedProtectionPolicy m_ExtendedProtectionPolicy;

		// Token: 0x04000E08 RID: 3592
		private bool m_PromoteCookiesToRfc2965;

		// Token: 0x04000E09 RID: 3593
		internal const string NTLM = "NTLM";
	}
}
