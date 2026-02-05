using System;

namespace System.Net
{
	// Token: 0x02000113 RID: 275
	internal static class IntPtrHelper
	{
		// Token: 0x06000B03 RID: 2819 RVA: 0x0003CC50 File Offset: 0x0003AE50
		internal static IntPtr Add(IntPtr a, int b)
		{
			return (IntPtr)((long)a + (long)b);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0003CC60 File Offset: 0x0003AE60
		internal static long Subtract(IntPtr a, IntPtr b)
		{
			return (long)a - (long)b;
		}
	}
}
