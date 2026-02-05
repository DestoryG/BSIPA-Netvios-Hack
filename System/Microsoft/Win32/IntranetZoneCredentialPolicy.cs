using System;
using System.Net;

namespace Microsoft.Win32
{
	// Token: 0x02000029 RID: 41
	public class IntranetZoneCredentialPolicy : ICredentialPolicy
	{
		// Token: 0x06000280 RID: 640 RVA: 0x0000F436 File Offset: 0x0000D636
		public IntranetZoneCredentialPolicy()
		{
			ExceptionHelper.ControlPolicyPermission.Demand();
			this._ManagerRef = (IInternetSecurityManager)new InternetSecurityManager();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000F458 File Offset: 0x0000D658
		public virtual bool ShouldSendCredential(Uri challengeUri, WebRequest request, NetworkCredential credential, IAuthenticationModule authModule)
		{
			int num;
			this._ManagerRef.MapUrlToZone(challengeUri.AbsoluteUri, out num, 0);
			return num == 1;
		}

		// Token: 0x0400038E RID: 910
		private const int URLZONE_INTRANET = 1;

		// Token: 0x0400038F RID: 911
		private IInternetSecurityManager _ManagerRef;
	}
}
