using System;
using System.Security;

namespace System.Net
{
	// Token: 0x020001FE RID: 510
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeDeleteContext_SECURITY : SafeDeleteContext
	{
		// Token: 0x0600133B RID: 4923 RVA: 0x00065ACD File Offset: 0x00063CCD
		internal SafeDeleteContext_SECURITY()
		{
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00065AD5 File Offset: 0x00063CD5
		protected override bool ReleaseHandle()
		{
			if (this._EffectiveCredential != null)
			{
				this._EffectiveCredential.DangerousRelease();
			}
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.DeleteSecurityContext(ref this._handle) == 0;
		}
	}
}
