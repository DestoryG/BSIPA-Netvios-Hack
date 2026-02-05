using System;
using System.Security;

namespace System.Net
{
	// Token: 0x020001FC RID: 508
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCredential_SECURITY : SafeFreeCredentials
	{
		// Token: 0x06001330 RID: 4912 RVA: 0x00064B69 File Offset: 0x00062D69
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.FreeCredentialsHandle(ref this._handle) == 0;
		}
	}
}
