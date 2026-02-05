using System;

namespace System.Net
{
	// Token: 0x0200014E RID: 334
	[global::__DynamicallyInvokable]
	public interface IWebProxy
	{
		// Token: 0x06000BA7 RID: 2983
		[global::__DynamicallyInvokable]
		Uri GetProxy(Uri destination);

		// Token: 0x06000BA8 RID: 2984
		[global::__DynamicallyInvokable]
		bool IsBypassed(Uri host);

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000BA9 RID: 2985
		// (set) Token: 0x06000BAA RID: 2986
		[global::__DynamicallyInvokable]
		ICredentials Credentials
		{
			[global::__DynamicallyInvokable]
			get;
			[global::__DynamicallyInvokable]
			set;
		}
	}
}
