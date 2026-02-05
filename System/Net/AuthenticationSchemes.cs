using System;

namespace System.Net
{
	// Token: 0x020000C4 RID: 196
	[Flags]
	[global::__DynamicallyInvokable]
	public enum AuthenticationSchemes
	{
		// Token: 0x04000C80 RID: 3200
		[global::__DynamicallyInvokable]
		None = 0,
		// Token: 0x04000C81 RID: 3201
		[global::__DynamicallyInvokable]
		Digest = 1,
		// Token: 0x04000C82 RID: 3202
		[global::__DynamicallyInvokable]
		Negotiate = 2,
		// Token: 0x04000C83 RID: 3203
		[global::__DynamicallyInvokable]
		Ntlm = 4,
		// Token: 0x04000C84 RID: 3204
		[global::__DynamicallyInvokable]
		Basic = 8,
		// Token: 0x04000C85 RID: 3205
		[global::__DynamicallyInvokable]
		Anonymous = 32768,
		// Token: 0x04000C86 RID: 3206
		[global::__DynamicallyInvokable]
		IntegratedWindowsAuthentication = 6
	}
}
