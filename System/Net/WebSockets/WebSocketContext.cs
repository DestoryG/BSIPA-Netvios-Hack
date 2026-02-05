using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;

namespace System.Net.WebSockets
{
	// Token: 0x02000234 RID: 564
	public abstract class WebSocketContext
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001524 RID: 5412
		public abstract Uri RequestUri { get; }

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001525 RID: 5413
		public abstract NameValueCollection Headers { get; }

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001526 RID: 5414
		public abstract string Origin { get; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001527 RID: 5415
		public abstract IEnumerable<string> SecWebSocketProtocols { get; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001528 RID: 5416
		public abstract string SecWebSocketVersion { get; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001529 RID: 5417
		public abstract string SecWebSocketKey { get; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600152A RID: 5418
		public abstract CookieCollection CookieCollection { get; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600152B RID: 5419
		public abstract IPrincipal User { get; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x0600152C RID: 5420
		public abstract bool IsAuthenticated { get; }

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600152D RID: 5421
		public abstract bool IsLocal { get; }

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600152E RID: 5422
		public abstract bool IsSecureConnection { get; }

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600152F RID: 5423
		public abstract WebSocket WebSocket { get; }
	}
}
