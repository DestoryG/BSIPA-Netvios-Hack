using System;

namespace System.Net
{
	// Token: 0x0200021E RID: 542
	internal static class Win32
	{
		// Token: 0x040015F8 RID: 5624
		internal const int OverlappedInternalOffset = 0;

		// Token: 0x040015F9 RID: 5625
		internal static int OverlappedInternalHighOffset = IntPtr.Size;

		// Token: 0x040015FA RID: 5626
		internal static int OverlappedOffsetOffset = IntPtr.Size * 2;

		// Token: 0x040015FB RID: 5627
		internal static int OverlappedOffsetHighOffset = IntPtr.Size * 2 + 4;

		// Token: 0x040015FC RID: 5628
		internal static int OverlappedhEventOffset = IntPtr.Size * 2 + 8;

		// Token: 0x040015FD RID: 5629
		internal static int OverlappedSize = IntPtr.Size * 3 + 8;
	}
}
