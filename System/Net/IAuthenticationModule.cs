using System;

namespace System.Net
{
	// Token: 0x0200010F RID: 271
	public interface IAuthenticationModule
	{
		// Token: 0x06000AFC RID: 2812
		Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials);

		// Token: 0x06000AFD RID: 2813
		Authorization PreAuthenticate(WebRequest request, ICredentials credentials);

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000AFE RID: 2814
		bool CanPreAuthenticate { get; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000AFF RID: 2815
		string AuthenticationType { get; }
	}
}
