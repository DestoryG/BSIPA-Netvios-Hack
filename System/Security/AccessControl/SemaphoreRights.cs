using System;
using System.Runtime.InteropServices;

namespace System.Security.AccessControl
{
	// Token: 0x0200048C RID: 1164
	[Flags]
	[ComVisible(false)]
	public enum SemaphoreRights
	{
		// Token: 0x0400266C RID: 9836
		Modify = 2,
		// Token: 0x0400266D RID: 9837
		Delete = 65536,
		// Token: 0x0400266E RID: 9838
		ReadPermissions = 131072,
		// Token: 0x0400266F RID: 9839
		ChangePermissions = 262144,
		// Token: 0x04002670 RID: 9840
		TakeOwnership = 524288,
		// Token: 0x04002671 RID: 9841
		Synchronize = 1048576,
		// Token: 0x04002672 RID: 9842
		FullControl = 2031619
	}
}
