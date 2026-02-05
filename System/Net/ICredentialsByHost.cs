using System;

namespace System.Net
{
	// Token: 0x02000112 RID: 274
	[global::__DynamicallyInvokable]
	public interface ICredentialsByHost
	{
		// Token: 0x06000B02 RID: 2818
		[global::__DynamicallyInvokable]
		NetworkCredential GetCredential(string host, int port, string authenticationType);
	}
}
