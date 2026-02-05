using System;

namespace System.Net.Mail
{
	// Token: 0x02000296 RID: 662
	internal enum SupportedAuth
	{
		// Token: 0x0400187F RID: 6271
		None,
		// Token: 0x04001880 RID: 6272
		Login,
		// Token: 0x04001881 RID: 6273
		NTLM,
		// Token: 0x04001882 RID: 6274
		GSSAPI = 4,
		// Token: 0x04001883 RID: 6275
		WDigest = 8
	}
}
