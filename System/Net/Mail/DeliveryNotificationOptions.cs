using System;

namespace System.Net.Mail
{
	// Token: 0x0200026F RID: 623
	[Flags]
	public enum DeliveryNotificationOptions
	{
		// Token: 0x040017CD RID: 6093
		None = 0,
		// Token: 0x040017CE RID: 6094
		OnSuccess = 1,
		// Token: 0x040017CF RID: 6095
		OnFailure = 2,
		// Token: 0x040017D0 RID: 6096
		Delay = 4,
		// Token: 0x040017D1 RID: 6097
		Never = 134217728
	}
}
