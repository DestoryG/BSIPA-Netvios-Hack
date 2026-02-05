using System;

namespace System.Net
{
	// Token: 0x020001B8 RID: 440
	internal interface ISessionAuthenticationModule : IAuthenticationModule
	{
		// Token: 0x06001142 RID: 4418
		bool Update(string challenge, WebRequest webRequest);

		// Token: 0x06001143 RID: 4419
		void ClearSession(WebRequest webRequest);

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001144 RID: 4420
		bool CanUseDefaultCredentials { get; }
	}
}
