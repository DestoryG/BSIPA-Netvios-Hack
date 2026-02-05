using System;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x02000440 RID: 1088
	[global::__DynamicallyInvokable]
	public abstract class ChannelBinding : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600287E RID: 10366 RVA: 0x000B9E20 File Offset: 0x000B8020
		[global::__DynamicallyInvokable]
		protected ChannelBinding()
			: base(true)
		{
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000B9E29 File Offset: 0x000B8029
		[global::__DynamicallyInvokable]
		protected ChannelBinding(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06002880 RID: 10368
		[global::__DynamicallyInvokable]
		public abstract int Size
		{
			[global::__DynamicallyInvokable]
			get;
		}
	}
}
