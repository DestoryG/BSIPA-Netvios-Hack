using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001F1 RID: 497
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeLocalFree : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060012FD RID: 4861 RVA: 0x00064248 File Offset: 0x00062448
		private SafeLocalFree()
			: base(true)
		{
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00064251 File Offset: 0x00062451
		private SafeLocalFree(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0006425C File Offset: 0x0006245C
		public static SafeLocalFree LocalAlloc(int cb)
		{
			SafeLocalFree safeLocalFree = UnsafeNclNativeMethods.SafeNetHandles.LocalAlloc(0, (UIntPtr)((ulong)((long)cb)));
			if (safeLocalFree.IsInvalid)
			{
				safeLocalFree.SetHandleAsInvalid();
				throw new OutOfMemoryException();
			}
			return safeLocalFree;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x0006428C File Offset: 0x0006248C
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.LocalFree(this.handle) == IntPtr.Zero;
		}

		// Token: 0x04001532 RID: 5426
		private const int LMEM_FIXED = 0;

		// Token: 0x04001533 RID: 5427
		private const int NULL = 0;

		// Token: 0x04001534 RID: 5428
		public static SafeLocalFree Zero = new SafeLocalFree(false);
	}
}
