using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001F2 RID: 498
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeGlobalFree : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001302 RID: 4866 RVA: 0x000642B0 File Offset: 0x000624B0
		private SafeGlobalFree()
			: base(true)
		{
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x000642B9 File Offset: 0x000624B9
		private SafeGlobalFree(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000642C2 File Offset: 0x000624C2
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.GlobalFree(this.handle) == IntPtr.Zero;
		}
	}
}
