using System;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000157 RID: 343
	[global::__DynamicallyInvokable]
	public abstract class TransportContext
	{
		// Token: 0x06000C0D RID: 3085
		[global::__DynamicallyInvokable]
		public abstract ChannelBinding GetChannelBinding(ChannelBindingKind kind);

		// Token: 0x06000C0E RID: 3086 RVA: 0x00041043 File Offset: 0x0003F243
		public virtual IEnumerable<TokenBinding> GetTlsTokenBindings()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0004104A File Offset: 0x0003F24A
		[global::__DynamicallyInvokable]
		protected TransportContext()
		{
		}
	}
}
