using System;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets
{
	// Token: 0x0200022A RID: 554
	public sealed class ClientWebSocket : WebSocket
	{
		// Token: 0x0600146A RID: 5226 RVA: 0x0006BC4A File Offset: 0x00069E4A
		static ClientWebSocket()
		{
			WebSocket.RegisterPrefixes();
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0006BC54 File Offset: 0x00069E54
		public ClientWebSocket()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.WebSockets, this, ".ctor", null);
			}
			if (!WebSocketProtocolComponent.IsSupported)
			{
				WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
			}
			this.state = 0;
			this.options = new ClientWebSocketOptions();
			this.cts = new CancellationTokenSource();
			if (Logging.On)
			{
				Logging.Exit(Logging.WebSockets, this, ".ctor", null);
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x0006BCC0 File Offset: 0x00069EC0
		public ClientWebSocketOptions Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x0006BCC8 File Offset: 0x00069EC8
		public override WebSocketCloseStatus? CloseStatus
		{
			get
			{
				if (this.innerWebSocket != null)
				{
					return this.innerWebSocket.CloseStatus;
				}
				return null;
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x0600146E RID: 5230 RVA: 0x0006BCF2 File Offset: 0x00069EF2
		public override string CloseStatusDescription
		{
			get
			{
				if (this.innerWebSocket != null)
				{
					return this.innerWebSocket.CloseStatusDescription;
				}
				return null;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x0006BD09 File Offset: 0x00069F09
		public override string SubProtocol
		{
			get
			{
				if (this.innerWebSocket != null)
				{
					return this.innerWebSocket.SubProtocol;
				}
				return null;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x0006BD20 File Offset: 0x00069F20
		public override WebSocketState State
		{
			get
			{
				if (this.innerWebSocket != null)
				{
					return this.innerWebSocket.State;
				}
				switch (this.state)
				{
				case 0:
					return WebSocketState.None;
				case 1:
					return WebSocketState.Connecting;
				case 3:
					return WebSocketState.Closed;
				}
				return WebSocketState.Closed;
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0006BD68 File Offset: 0x00069F68
		public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (!uri.IsAbsoluteUri)
			{
				throw new ArgumentException(SR.GetString("net_uri_NotAbsolute"), "uri");
			}
			if (uri.Scheme != Uri.UriSchemeWs && uri.Scheme != Uri.UriSchemeWss)
			{
				throw new ArgumentException(SR.GetString("net_WebSockets_Scheme"), "uri");
			}
			int num = Interlocked.CompareExchange(ref this.state, 1, 0);
			if (num == 3)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (num != 0)
			{
				throw new InvalidOperationException(SR.GetString("net_WebSockets_AlreadyStarted"));
			}
			this.options.SetToReadOnly();
			return this.ConnectAsyncCore(uri, cancellationToken);
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0006BE28 File Offset: 0x0006A028
		private async Task ConnectAsyncCore(Uri uri, CancellationToken cancellationToken)
		{
			HttpWebResponse response = null;
			CancellationTokenRegistration connectCancellation = default(CancellationTokenRegistration);
			try
			{
				HttpWebRequest request = this.CreateAndConfigureRequest(uri);
				if (Logging.On)
				{
					Logging.Associate(Logging.WebSockets, this, request);
				}
				connectCancellation = cancellationToken.Register(new Action<object>(this.AbortRequest), request, false);
				WebResponse webResponse = await request.GetResponseAsync().SuppressContextFlow<WebResponse>();
				response = webResponse as HttpWebResponse;
				if (Logging.On)
				{
					Logging.Associate(Logging.WebSockets, this, response);
				}
				string text = this.ValidateResponse(request, response);
				this.innerWebSocket = WebSocket.CreateClientWebSocket(response.GetResponseStream(), text, this.options.ReceiveBufferSize, this.options.SendBufferSize, this.options.KeepAliveInterval, false, this.options.GetOrCreateBuffer());
				if (Logging.On)
				{
					Logging.Associate(Logging.WebSockets, this, this.innerWebSocket);
				}
				if (Interlocked.CompareExchange(ref this.state, 2, 1) != 1)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				request = null;
			}
			catch (WebException ex)
			{
				this.ConnectExceptionCleanup(response);
				WebSocketException ex2 = new WebSocketException(SR.GetString("net_webstatus_ConnectFailure"), ex);
				if (Logging.On)
				{
					Logging.Exception(Logging.WebSockets, this, "ConnectAsync", ex2);
				}
				throw ex2;
			}
			catch (Exception ex3)
			{
				this.ConnectExceptionCleanup(response);
				if (Logging.On)
				{
					Logging.Exception(Logging.WebSockets, this, "ConnectAsync", ex3);
				}
				throw;
			}
			finally
			{
				connectCancellation.Dispose();
			}
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0006BE7B File Offset: 0x0006A07B
		private void ConnectExceptionCleanup(HttpWebResponse response)
		{
			this.Dispose();
			if (response != null)
			{
				response.Dispose();
			}
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0006BE8C File Offset: 0x0006A08C
		private HttpWebRequest CreateAndConfigureRequest(Uri uri)
		{
			HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
			if (httpWebRequest == null)
			{
				throw new InvalidOperationException(SR.GetString("net_WebSockets_InvalidRegistration"));
			}
			foreach (object obj in this.options.RequestHeaders.Keys)
			{
				string text = (string)obj;
				httpWebRequest.Headers.Add(text, this.options.RequestHeaders[text]);
			}
			if (this.options.RequestedSubProtocols.Count > 0)
			{
				httpWebRequest.Headers.Add("Sec-WebSocket-Protocol", string.Join(", ", this.options.RequestedSubProtocols));
			}
			if (this.options.UseDefaultCredentials)
			{
				httpWebRequest.UseDefaultCredentials = true;
			}
			else if (this.options.Credentials != null)
			{
				httpWebRequest.Credentials = this.options.Credentials;
			}
			if (this.options.InternalClientCertificates != null)
			{
				httpWebRequest.ClientCertificates = this.options.InternalClientCertificates;
			}
			httpWebRequest.Proxy = this.options.Proxy;
			httpWebRequest.CookieContainer = this.options.Cookies;
			this.cts.Token.Register(new Action<object>(this.AbortRequest), httpWebRequest, false);
			return httpWebRequest;
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0006BFF4 File Offset: 0x0006A1F4
		private string ValidateResponse(HttpWebRequest request, HttpWebResponse response)
		{
			if (response.StatusCode != HttpStatusCode.SwitchingProtocols)
			{
				throw new WebSocketException(SR.GetString("net_WebSockets_Connect101Expected", new object[] { (int)response.StatusCode }));
			}
			string text = response.Headers["Upgrade"];
			if (!string.Equals(text, "websocket", StringComparison.OrdinalIgnoreCase))
			{
				throw new WebSocketException(SR.GetString("net_WebSockets_InvalidResponseHeader", new object[] { "Upgrade", text }));
			}
			string text2 = response.Headers["Connection"];
			if (!string.Equals(text2, "Upgrade", StringComparison.OrdinalIgnoreCase))
			{
				throw new WebSocketException(SR.GetString("net_WebSockets_InvalidResponseHeader", new object[] { "Connection", text2 }));
			}
			string text3 = response.Headers["Sec-WebSocket-Accept"];
			string secWebSocketAcceptString = WebSocketHelpers.GetSecWebSocketAcceptString(request.Headers["Sec-WebSocket-Key"]);
			if (!string.Equals(text3, secWebSocketAcceptString, StringComparison.OrdinalIgnoreCase))
			{
				throw new WebSocketException(SR.GetString("net_WebSockets_InvalidResponseHeader", new object[] { "Sec-WebSocket-Accept", text3 }));
			}
			string text4 = response.Headers["Sec-WebSocket-Protocol"];
			if (!string.IsNullOrWhiteSpace(text4) && this.options.RequestedSubProtocols.Count > 0)
			{
				bool flag = false;
				foreach (string text5 in this.options.RequestedSubProtocols)
				{
					if (string.Equals(text5, text4, StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					throw new WebSocketException(SR.GetString("net_WebSockets_AcceptUnsupportedProtocol", new object[]
					{
						string.Join(", ", this.options.RequestedSubProtocols),
						text4
					}));
				}
			}
			if (!string.IsNullOrWhiteSpace(text4))
			{
				return text4;
			}
			return null;
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0006C1D4 File Offset: 0x0006A3D4
		public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this.innerWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0006C1EC File Offset: 0x0006A3EC
		public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this.innerWebSocket.ReceiveAsync(buffer, cancellationToken);
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0006C201 File Offset: 0x0006A401
		public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this.innerWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0006C217 File Offset: 0x0006A417
		public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			this.ThrowIfNotConnected();
			return this.innerWebSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0006C22D File Offset: 0x0006A42D
		public override void Abort()
		{
			if (this.state == 3)
			{
				return;
			}
			if (this.innerWebSocket != null)
			{
				this.innerWebSocket.Abort();
			}
			this.Dispose();
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0006C254 File Offset: 0x0006A454
		private void AbortRequest(object obj)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)obj;
			httpWebRequest.Abort();
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0006C270 File Offset: 0x0006A470
		public override void Dispose()
		{
			int num = Interlocked.Exchange(ref this.state, 3);
			if (num == 3)
			{
				return;
			}
			this.cts.Cancel(false);
			this.cts.Dispose();
			if (this.innerWebSocket != null)
			{
				this.innerWebSocket.Dispose();
			}
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0006C2B9 File Offset: 0x0006A4B9
		private void ThrowIfNotConnected()
		{
			if (this.state == 3)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (this.state != 2)
			{
				throw new InvalidOperationException(SR.GetString("net_WebSockets_NotConnected"));
			}
		}

		// Token: 0x04001627 RID: 5671
		private readonly ClientWebSocketOptions options;

		// Token: 0x04001628 RID: 5672
		private WebSocket innerWebSocket;

		// Token: 0x04001629 RID: 5673
		private readonly CancellationTokenSource cts;

		// Token: 0x0400162A RID: 5674
		private int state;

		// Token: 0x0400162B RID: 5675
		private const int created = 0;

		// Token: 0x0400162C RID: 5676
		private const int connecting = 1;

		// Token: 0x0400162D RID: 5677
		private const int connected = 2;

		// Token: 0x0400162E RID: 5678
		private const int disposed = 3;
	}
}
