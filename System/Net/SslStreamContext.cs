using System;
using System.Net.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000159 RID: 345
	internal class SslStreamContext : TransportContext
	{
		// Token: 0x06000C12 RID: 3090 RVA: 0x0004106F File Offset: 0x0003F26F
		internal SslStreamContext(SslStream sslStream)
		{
			this.sslStream = sslStream;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0004107E File Offset: 0x0003F27E
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			return this.sslStream.GetChannelBinding(kind);
		}

		// Token: 0x0400113C RID: 4412
		private SslStream sslStream;
	}
}
