using System;

namespace System.Net
{
	// Token: 0x02000111 RID: 273
	[global::__DynamicallyInvokable]
	public interface ICredentials
	{
		// Token: 0x06000B01 RID: 2817
		[global::__DynamicallyInvokable]
		NetworkCredential GetCredential(Uri uri, string authType);
	}
}
