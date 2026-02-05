using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net.WebSockets
{
	// Token: 0x0200022B RID: 555
	public sealed class ClientWebSocketOptions
	{
		// Token: 0x0600147E RID: 5246 RVA: 0x0006C2F0 File Offset: 0x0006A4F0
		internal ClientWebSocketOptions()
		{
			this.requestedSubProtocols = new List<string>();
			this.requestHeaders = new WebHeaderCollection(WebHeaderCollectionType.HttpWebRequest);
			this.Proxy = WebRequest.DefaultWebProxy;
			this.receiveBufferSize = 16384;
			this.sendBufferSize = 16384;
			this.keepAliveInterval = WebSocket.DefaultKeepAliveInterval;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0006C346 File Offset: 0x0006A546
		public void SetRequestHeader(string headerName, string headerValue)
		{
			this.ThrowIfReadOnly();
			this.requestHeaders.Set(headerName, headerValue);
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001480 RID: 5248 RVA: 0x0006C35B File Offset: 0x0006A55B
		internal WebHeaderCollection RequestHeaders
		{
			get
			{
				return this.requestHeaders;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x0006C363 File Offset: 0x0006A563
		// (set) Token: 0x06001482 RID: 5250 RVA: 0x0006C36B File Offset: 0x0006A56B
		public bool UseDefaultCredentials
		{
			get
			{
				return this.useDefaultCredentials;
			}
			set
			{
				this.ThrowIfReadOnly();
				this.useDefaultCredentials = value;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x0006C37A File Offset: 0x0006A57A
		// (set) Token: 0x06001484 RID: 5252 RVA: 0x0006C382 File Offset: 0x0006A582
		public ICredentials Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.ThrowIfReadOnly();
				this.credentials = value;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x0006C391 File Offset: 0x0006A591
		// (set) Token: 0x06001486 RID: 5254 RVA: 0x0006C399 File Offset: 0x0006A599
		public IWebProxy Proxy
		{
			get
			{
				return this.proxy;
			}
			set
			{
				this.ThrowIfReadOnly();
				this.proxy = value;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0006C3A8 File Offset: 0x0006A5A8
		// (set) Token: 0x06001488 RID: 5256 RVA: 0x0006C3C3 File Offset: 0x0006A5C3
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.clientCertificates == null)
				{
					this.clientCertificates = new X509CertificateCollection();
				}
				return this.clientCertificates;
			}
			set
			{
				this.ThrowIfReadOnly();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.clientCertificates = value;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0006C3E0 File Offset: 0x0006A5E0
		internal X509CertificateCollection InternalClientCertificates
		{
			get
			{
				return this.clientCertificates;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x0006C3E8 File Offset: 0x0006A5E8
		// (set) Token: 0x0600148B RID: 5259 RVA: 0x0006C3F0 File Offset: 0x0006A5F0
		public CookieContainer Cookies
		{
			get
			{
				return this.cookies;
			}
			set
			{
				this.ThrowIfReadOnly();
				this.cookies = value;
			}
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0006C3FF File Offset: 0x0006A5FF
		public void SetBuffer(int receiveBufferSize, int sendBufferSize)
		{
			this.ThrowIfReadOnly();
			WebSocketHelpers.ValidateBufferSizes(receiveBufferSize, sendBufferSize);
			this.buffer = null;
			this.receiveBufferSize = receiveBufferSize;
			this.sendBufferSize = sendBufferSize;
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0006C428 File Offset: 0x0006A628
		public void SetBuffer(int receiveBufferSize, int sendBufferSize, ArraySegment<byte> buffer)
		{
			this.ThrowIfReadOnly();
			WebSocketHelpers.ValidateBufferSizes(receiveBufferSize, sendBufferSize);
			WebSocketHelpers.ValidateArraySegment<byte>(buffer, "buffer");
			WebSocketBuffer.Validate(buffer.Count, receiveBufferSize, sendBufferSize, false);
			this.receiveBufferSize = receiveBufferSize;
			this.sendBufferSize = sendBufferSize;
			if (AppDomain.CurrentDomain.IsFullyTrusted)
			{
				this.buffer = new ArraySegment<byte>?(buffer);
				return;
			}
			this.buffer = null;
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x0006C48F File Offset: 0x0006A68F
		internal int ReceiveBufferSize
		{
			get
			{
				return this.receiveBufferSize;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0006C497 File Offset: 0x0006A697
		internal int SendBufferSize
		{
			get
			{
				return this.sendBufferSize;
			}
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0006C49F File Offset: 0x0006A69F
		internal ArraySegment<byte> GetOrCreateBuffer()
		{
			if (this.buffer == null)
			{
				this.buffer = new ArraySegment<byte>?(WebSocket.CreateClientBuffer(this.receiveBufferSize, this.sendBufferSize));
			}
			return this.buffer.Value;
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0006C4D8 File Offset: 0x0006A6D8
		public void AddSubProtocol(string subProtocol)
		{
			this.ThrowIfReadOnly();
			WebSocketHelpers.ValidateSubprotocol(subProtocol);
			foreach (string text in this.requestedSubProtocols)
			{
				if (string.Equals(text, subProtocol, StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(global::System.SR.GetString("net_WebSockets_NoDuplicateProtocol", new object[] { subProtocol }), "subProtocol");
				}
			}
			this.requestedSubProtocols.Add(subProtocol);
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x0006C560 File Offset: 0x0006A760
		internal IList<string> RequestedSubProtocols
		{
			get
			{
				return this.requestedSubProtocols;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x0006C568 File Offset: 0x0006A768
		// (set) Token: 0x06001494 RID: 5268 RVA: 0x0006C570 File Offset: 0x0006A770
		public TimeSpan KeepAliveInterval
		{
			get
			{
				return this.keepAliveInterval;
			}
			set
			{
				this.ThrowIfReadOnly();
				if (value < Timeout.InfiniteTimeSpan)
				{
					throw new ArgumentOutOfRangeException("value", value, global::System.SR.GetString("net_WebSockets_ArgumentOutOfRange_TooSmall", new object[] { Timeout.InfiniteTimeSpan.ToString() }));
				}
				this.keepAliveInterval = value;
			}
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0006C5CE File Offset: 0x0006A7CE
		internal void SetToReadOnly()
		{
			this.isReadOnly = true;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0006C5D7 File Offset: 0x0006A7D7
		private void ThrowIfReadOnly()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(global::System.SR.GetString("net_WebSockets_AlreadyStarted"));
			}
		}

		// Token: 0x0400162F RID: 5679
		private bool isReadOnly;

		// Token: 0x04001630 RID: 5680
		private readonly IList<string> requestedSubProtocols;

		// Token: 0x04001631 RID: 5681
		private readonly WebHeaderCollection requestHeaders;

		// Token: 0x04001632 RID: 5682
		private TimeSpan keepAliveInterval;

		// Token: 0x04001633 RID: 5683
		private int receiveBufferSize;

		// Token: 0x04001634 RID: 5684
		private int sendBufferSize;

		// Token: 0x04001635 RID: 5685
		private ArraySegment<byte>? buffer;

		// Token: 0x04001636 RID: 5686
		private bool useDefaultCredentials;

		// Token: 0x04001637 RID: 5687
		private ICredentials credentials;

		// Token: 0x04001638 RID: 5688
		private IWebProxy proxy;

		// Token: 0x04001639 RID: 5689
		private X509CertificateCollection clientCertificates;

		// Token: 0x0400163A RID: 5690
		private CookieContainer cookies;
	}
}
