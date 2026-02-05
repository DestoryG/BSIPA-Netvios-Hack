using System;
using System.Security;

namespace System.Net
{
	// Token: 0x020001F0 RID: 496
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBuffer_SECURITY : SafeFreeContextBuffer
	{
		// Token: 0x060012FB RID: 4859 RVA: 0x00064230 File Offset: 0x00062430
		internal SafeFreeContextBuffer_SECURITY()
		{
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00064238 File Offset: 0x00062438
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.FreeContextBuffer(this.handle) == 0;
		}
	}
}
