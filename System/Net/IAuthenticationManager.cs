using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Net
{
	// Token: 0x020000BE RID: 190
	internal interface IAuthenticationManager
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000649 RID: 1609
		// (set) Token: 0x0600064A RID: 1610
		ICredentialPolicy CredentialPolicy { get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600064B RID: 1611
		StringDictionary CustomTargetNameDictionary { get; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600064C RID: 1612
		SpnDictionary SpnDictionary { get; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600064D RID: 1613
		bool OSSupportsExtendedProtection { get; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600064E RID: 1614
		bool SspSupportsExtendedProtection { get; }

		// Token: 0x0600064F RID: 1615
		void EnsureConfigLoaded();

		// Token: 0x06000650 RID: 1616
		Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials);

		// Token: 0x06000651 RID: 1617
		Authorization PreAuthenticate(WebRequest request, ICredentials credentials);

		// Token: 0x06000652 RID: 1618
		void Register(IAuthenticationModule authenticationModule);

		// Token: 0x06000653 RID: 1619
		void Unregister(IAuthenticationModule authenticationModule);

		// Token: 0x06000654 RID: 1620
		void Unregister(string authenticationScheme);

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000655 RID: 1621
		IEnumerator RegisteredModules { get; }

		// Token: 0x06000656 RID: 1622
		void BindModule(Uri uri, Authorization response, IAuthenticationModule module);
	}
}
