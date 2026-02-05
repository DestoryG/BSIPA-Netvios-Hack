using System;

namespace System.Net.Mail
{
	// Token: 0x0200025D RID: 605
	internal enum ServerState
	{
		// Token: 0x04001770 RID: 6000
		Starting = 1,
		// Token: 0x04001771 RID: 6001
		Started,
		// Token: 0x04001772 RID: 6002
		Stopping,
		// Token: 0x04001773 RID: 6003
		Stopped,
		// Token: 0x04001774 RID: 6004
		Pausing,
		// Token: 0x04001775 RID: 6005
		Paused,
		// Token: 0x04001776 RID: 6006
		Continuing
	}
}
