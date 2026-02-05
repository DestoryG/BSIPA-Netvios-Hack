using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002E0 RID: 736
	[Flags]
	public enum NetworkInformationAccess
	{
		// Token: 0x04001A48 RID: 6728
		None = 0,
		// Token: 0x04001A49 RID: 6729
		Read = 1,
		// Token: 0x04001A4A RID: 6730
		Ping = 4
	}
}
