using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200015B RID: 347
	internal class CachedTransportContext : TransportContext
	{
		// Token: 0x06000C17 RID: 3095 RVA: 0x000410E0 File Offset: 0x0003F2E0
		internal CachedTransportContext(ChannelBinding binding)
		{
			this.binding = binding;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x000410EF File Offset: 0x0003F2EF
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			if (kind != ChannelBindingKind.Endpoint)
			{
				return null;
			}
			return this.binding;
		}

		// Token: 0x0400113E RID: 4414
		private ChannelBinding binding;
	}
}
