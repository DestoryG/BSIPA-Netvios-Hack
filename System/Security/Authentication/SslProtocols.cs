using System;

namespace System.Security.Authentication
{
	// Token: 0x0200043C RID: 1084
	[Flags]
	[global::__DynamicallyInvokable]
	public enum SslProtocols
	{
		// Token: 0x04002231 RID: 8753
		[global::__DynamicallyInvokable]
		None = 0,
		// Token: 0x04002232 RID: 8754
		[global::__DynamicallyInvokable]
		Ssl2 = 12,
		// Token: 0x04002233 RID: 8755
		[global::__DynamicallyInvokable]
		Ssl3 = 48,
		// Token: 0x04002234 RID: 8756
		[global::__DynamicallyInvokable]
		Tls = 192,
		// Token: 0x04002235 RID: 8757
		[global::__DynamicallyInvokable]
		Tls11 = 768,
		// Token: 0x04002236 RID: 8758
		[global::__DynamicallyInvokable]
		Tls12 = 3072,
		// Token: 0x04002237 RID: 8759
		Tls13 = 12288,
		// Token: 0x04002238 RID: 8760
		Default = 240
	}
}
