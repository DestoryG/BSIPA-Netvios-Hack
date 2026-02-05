using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001F3 RID: 499
	[ComVisible(false)]
	internal sealed class SafeOverlappedFree : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001305 RID: 4869 RVA: 0x000642D9 File Offset: 0x000624D9
		private SafeOverlappedFree()
			: base(true)
		{
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x000642E2 File Offset: 0x000624E2
		private SafeOverlappedFree(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x000642EC File Offset: 0x000624EC
		public static SafeOverlappedFree Alloc()
		{
			SafeOverlappedFree safeOverlappedFree = UnsafeNclNativeMethods.SafeNetHandlesSafeOverlappedFree.LocalAlloc(64, (UIntPtr)((ulong)((long)Win32.OverlappedSize)));
			if (safeOverlappedFree.IsInvalid)
			{
				safeOverlappedFree.SetHandleAsInvalid();
				throw new OutOfMemoryException();
			}
			return safeOverlappedFree;
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00064324 File Offset: 0x00062524
		public static SafeOverlappedFree Alloc(SafeCloseSocket socketHandle)
		{
			SafeOverlappedFree safeOverlappedFree = SafeOverlappedFree.Alloc();
			safeOverlappedFree._socketHandle = socketHandle;
			return safeOverlappedFree;
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00064340 File Offset: 0x00062540
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Close(bool resetOwner)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (resetOwner)
				{
					this._socketHandle = null;
				}
				base.Close();
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x00064378 File Offset: 0x00062578
		protected override bool ReleaseHandle()
		{
			SafeCloseSocket socketHandle = this._socketHandle;
			if (socketHandle != null && !socketHandle.IsInvalid)
			{
				socketHandle.Dispose();
			}
			return UnsafeNclNativeMethods.SafeNetHandles.LocalFree(this.handle) == IntPtr.Zero;
		}

		// Token: 0x04001535 RID: 5429
		private const int LPTR = 64;

		// Token: 0x04001536 RID: 5430
		internal static readonly SafeOverlappedFree Zero = new SafeOverlappedFree(false);

		// Token: 0x04001537 RID: 5431
		private SafeCloseSocket _socketHandle;
	}
}
