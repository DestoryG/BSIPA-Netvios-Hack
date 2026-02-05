using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;

namespace System.Net.WebSockets
{
	// Token: 0x0200022D RID: 557
	public class HttpListenerWebSocketContext : WebSocketContext
	{
		// Token: 0x0600149A RID: 5274 RVA: 0x0006C678 File Offset: 0x0006A878
		internal HttpListenerWebSocketContext(Uri requestUri, NameValueCollection headers, CookieCollection cookieCollection, IPrincipal user, bool isAuthenticated, bool isLocal, bool isSecureConnection, string origin, IEnumerable<string> secWebSocketProtocols, string secWebSocketVersion, string secWebSocketKey, WebSocket webSocket)
		{
			this.m_CookieCollection = new CookieCollection();
			this.m_CookieCollection.Add(cookieCollection);
			this.m_Headers = new NameValueCollection(headers);
			this.m_User = HttpListenerWebSocketContext.CopyPrincipal(user);
			this.m_RequestUri = requestUri;
			this.m_IsAuthenticated = isAuthenticated;
			this.m_IsLocal = isLocal;
			this.m_IsSecureConnection = isSecureConnection;
			this.m_Origin = origin;
			this.m_SecWebSocketProtocols = secWebSocketProtocols;
			this.m_SecWebSocketVersion = secWebSocketVersion;
			this.m_SecWebSocketKey = secWebSocketKey;
			this.m_WebSocket = webSocket;
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x0006C702 File Offset: 0x0006A902
		public override Uri RequestUri
		{
			get
			{
				return this.m_RequestUri;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x0006C70A File Offset: 0x0006A90A
		public override NameValueCollection Headers
		{
			get
			{
				return this.m_Headers;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0006C712 File Offset: 0x0006A912
		public override string Origin
		{
			get
			{
				return this.m_Origin;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x0006C71A File Offset: 0x0006A91A
		public override IEnumerable<string> SecWebSocketProtocols
		{
			get
			{
				return this.m_SecWebSocketProtocols;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600149F RID: 5279 RVA: 0x0006C722 File Offset: 0x0006A922
		public override string SecWebSocketVersion
		{
			get
			{
				return this.m_SecWebSocketVersion;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060014A0 RID: 5280 RVA: 0x0006C72A File Offset: 0x0006A92A
		public override string SecWebSocketKey
		{
			get
			{
				return this.m_SecWebSocketKey;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0006C732 File Offset: 0x0006A932
		public override CookieCollection CookieCollection
		{
			get
			{
				return this.m_CookieCollection;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060014A2 RID: 5282 RVA: 0x0006C73A File Offset: 0x0006A93A
		public override IPrincipal User
		{
			get
			{
				return this.m_User;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x0006C742 File Offset: 0x0006A942
		public override bool IsAuthenticated
		{
			get
			{
				return this.m_IsAuthenticated;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x0006C74A File Offset: 0x0006A94A
		public override bool IsLocal
		{
			get
			{
				return this.m_IsLocal;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0006C752 File Offset: 0x0006A952
		public override bool IsSecureConnection
		{
			get
			{
				return this.m_IsSecureConnection;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x0006C75A File Offset: 0x0006A95A
		public override WebSocket WebSocket
		{
			get
			{
				return this.m_WebSocket;
			}
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x0006C764 File Offset: 0x0006A964
		private static IPrincipal CopyPrincipal(IPrincipal user)
		{
			IPrincipal principal = null;
			if (user != null)
			{
				if (!(user is WindowsPrincipal))
				{
					HttpListenerBasicIdentity httpListenerBasicIdentity = user.Identity as HttpListenerBasicIdentity;
					if (httpListenerBasicIdentity != null)
					{
						principal = new GenericPrincipal(new HttpListenerBasicIdentity(httpListenerBasicIdentity.Name, httpListenerBasicIdentity.Password), null);
					}
				}
				else
				{
					WindowsIdentity windowsIdentity = (WindowsIdentity)user.Identity;
					principal = new WindowsPrincipal(HttpListener.CreateWindowsIdentity(windowsIdentity.Token, windowsIdentity.AuthenticationType, WindowsAccountType.Normal, true));
				}
			}
			return principal;
		}

		// Token: 0x0400163D RID: 5693
		private Uri m_RequestUri;

		// Token: 0x0400163E RID: 5694
		private NameValueCollection m_Headers;

		// Token: 0x0400163F RID: 5695
		private CookieCollection m_CookieCollection;

		// Token: 0x04001640 RID: 5696
		private IPrincipal m_User;

		// Token: 0x04001641 RID: 5697
		private bool m_IsAuthenticated;

		// Token: 0x04001642 RID: 5698
		private bool m_IsLocal;

		// Token: 0x04001643 RID: 5699
		private bool m_IsSecureConnection;

		// Token: 0x04001644 RID: 5700
		private string m_Origin;

		// Token: 0x04001645 RID: 5701
		private IEnumerable<string> m_SecWebSocketProtocols;

		// Token: 0x04001646 RID: 5702
		private string m_SecWebSocketVersion;

		// Token: 0x04001647 RID: 5703
		private string m_SecWebSocketKey;

		// Token: 0x04001648 RID: 5704
		private WebSocket m_WebSocket;
	}
}
