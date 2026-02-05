using System;

namespace System.Net
{
	// Token: 0x020000BF RID: 191
	public interface ICredentialPolicy
	{
		// Token: 0x06000657 RID: 1623
		bool ShouldSendCredential(Uri challengeUri, WebRequest request, NetworkCredential credential, IAuthenticationModule authenticationModule);
	}
}
