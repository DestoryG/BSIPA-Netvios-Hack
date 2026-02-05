using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002A3 RID: 675
	[global::__DynamicallyInvokable]
	public enum ScopeLevel
	{
		// Token: 0x040018C4 RID: 6340
		[global::__DynamicallyInvokable]
		None,
		// Token: 0x040018C5 RID: 6341
		[global::__DynamicallyInvokable]
		Interface,
		// Token: 0x040018C6 RID: 6342
		[global::__DynamicallyInvokable]
		Link,
		// Token: 0x040018C7 RID: 6343
		[global::__DynamicallyInvokable]
		Subnet,
		// Token: 0x040018C8 RID: 6344
		[global::__DynamicallyInvokable]
		Admin,
		// Token: 0x040018C9 RID: 6345
		[global::__DynamicallyInvokable]
		Site,
		// Token: 0x040018CA RID: 6346
		[global::__DynamicallyInvokable]
		Organization = 8,
		// Token: 0x040018CB RID: 6347
		[global::__DynamicallyInvokable]
		Global = 14
	}
}
