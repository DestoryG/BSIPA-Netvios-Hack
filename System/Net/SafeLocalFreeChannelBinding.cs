using System;
using System.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000202 RID: 514
	[SuppressUnmanagedCodeSecurity]
	internal class SafeLocalFreeChannelBinding : ChannelBinding
	{
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001354 RID: 4948 RVA: 0x00065E74 File Offset: 0x00064074
		public override int Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00065E7C File Offset: 0x0006407C
		public static SafeLocalFreeChannelBinding LocalAlloc(int cb)
		{
			SafeLocalFreeChannelBinding safeLocalFreeChannelBinding = UnsafeNclNativeMethods.SafeNetHandles.LocalAllocChannelBinding(0, (UIntPtr)((ulong)((long)cb)));
			if (safeLocalFreeChannelBinding.IsInvalid)
			{
				safeLocalFreeChannelBinding.SetHandleAsInvalid();
				throw new OutOfMemoryException();
			}
			safeLocalFreeChannelBinding.size = cb;
			return safeLocalFreeChannelBinding;
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00065EB3 File Offset: 0x000640B3
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.LocalFree(this.handle) == IntPtr.Zero;
		}

		// Token: 0x04001552 RID: 5458
		private const int LMEM_FIXED = 0;

		// Token: 0x04001553 RID: 5459
		private int size;
	}
}
