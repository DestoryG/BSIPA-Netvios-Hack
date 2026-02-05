using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000204 RID: 516
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBufferChannelBinding_SECURITY : SafeFreeContextBufferChannelBinding
	{
		// Token: 0x0600135E RID: 4958 RVA: 0x00065FD4 File Offset: 0x000641D4
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.FreeContextBuffer(this.handle) == 0;
		}
	}
}
