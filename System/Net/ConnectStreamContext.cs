using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000158 RID: 344
	internal class ConnectStreamContext : TransportContext
	{
		// Token: 0x06000C10 RID: 3088 RVA: 0x00041052 File Offset: 0x0003F252
		internal ConnectStreamContext(ConnectStream connectStream)
		{
			this.connectStream = connectStream;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00041061 File Offset: 0x0003F261
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			return this.connectStream.GetChannelBinding(kind);
		}

		// Token: 0x0400113B RID: 4411
		private ConnectStream connectStream;
	}
}
