using System;

namespace Microsoft.CSharp.RuntimeBinder.Errors
{
	// Token: 0x020000C2 RID: 194
	[Flags]
	internal enum ErrArgFlags
	{
		// Token: 0x04000612 RID: 1554
		None = 0,
		// Token: 0x04000613 RID: 1555
		NoStr = 2,
		// Token: 0x04000614 RID: 1556
		Unique = 4,
		// Token: 0x04000615 RID: 1557
		UseGetErrorInfo = 8
	}
}
