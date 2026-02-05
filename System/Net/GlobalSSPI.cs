using System;

namespace System.Net
{
	// Token: 0x020001C4 RID: 452
	internal static class GlobalSSPI
	{
		// Token: 0x04001472 RID: 5234
		internal static SSPIInterface SSPIAuth = new SSPIAuthType();

		// Token: 0x04001473 RID: 5235
		internal static SSPIInterface SSPISecureChannel = new SSPISecureChannelType();
	}
}
