using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net.Mail
{
	// Token: 0x02000267 RID: 615
	internal interface ISmtpAuthenticationModule
	{
		// Token: 0x0600171B RID: 5915
		Authorization Authenticate(string challenge, NetworkCredential credentials, object sessionCookie, string spn, ChannelBinding channelBindingToken);

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x0600171C RID: 5916
		string AuthenticationType { get; }

		// Token: 0x0600171D RID: 5917
		void CloseContext(object sessionCookie);
	}
}
