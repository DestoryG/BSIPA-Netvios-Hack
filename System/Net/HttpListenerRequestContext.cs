using System;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200015A RID: 346
	internal class HttpListenerRequestContext : TransportContext
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x0004108C File Offset: 0x0003F28C
		internal HttpListenerRequestContext(HttpListenerRequest request)
		{
			this.request = request;
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0004109B File Offset: 0x0003F29B
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			if (kind != ChannelBindingKind.Endpoint)
			{
				throw new NotSupportedException(SR.GetString("net_listener_invalid_cbt_type", new object[] { kind.ToString() }));
			}
			return this.request.GetChannelBinding();
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x000410D3 File Offset: 0x0003F2D3
		public override IEnumerable<TokenBinding> GetTlsTokenBindings()
		{
			return this.request.GetTlsTokenBindings();
		}

		// Token: 0x0400113D RID: 4413
		private HttpListenerRequest request;
	}
}
