using System;

namespace System.Net
{
	// Token: 0x020001B0 RID: 432
	internal enum FtpLoginState : byte
	{
		// Token: 0x040013EB RID: 5099
		NotLoggedIn,
		// Token: 0x040013EC RID: 5100
		LoggedIn,
		// Token: 0x040013ED RID: 5101
		LoggedInButNeedsRelogin,
		// Token: 0x040013EE RID: 5102
		ReloginFailed
	}
}
