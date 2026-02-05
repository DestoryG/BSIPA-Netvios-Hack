using System;

namespace System.Net.Security
{
	// Token: 0x02000358 RID: 856
	[Flags]
	[global::__DynamicallyInvokable]
	public enum SslPolicyErrors
	{
		// Token: 0x04001CFA RID: 7418
		[global::__DynamicallyInvokable]
		None = 0,
		// Token: 0x04001CFB RID: 7419
		[global::__DynamicallyInvokable]
		RemoteCertificateNotAvailable = 1,
		// Token: 0x04001CFC RID: 7420
		[global::__DynamicallyInvokable]
		RemoteCertificateNameMismatch = 2,
		// Token: 0x04001CFD RID: 7421
		[global::__DynamicallyInvokable]
		RemoteCertificateChainErrors = 4
	}
}
