using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x020001FF RID: 511
	internal class SafeNativeOverlapped : SafeHandle
	{
		// Token: 0x0600133D RID: 4925 RVA: 0x00065AF8 File Offset: 0x00063CF8
		internal SafeNativeOverlapped()
			: this(IntPtr.Zero)
		{
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00065B05 File Offset: 0x00063D05
		internal unsafe SafeNativeOverlapped(NativeOverlapped* handle)
			: this((IntPtr)((void*)handle))
		{
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00065B13 File Offset: 0x00063D13
		internal SafeNativeOverlapped(IntPtr handle)
			: base(IntPtr.Zero, true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x00065B28 File Offset: 0x00063D28
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00065B3C File Offset: 0x00063D3C
		public unsafe void ReinitializeNativeOverlapped()
		{
			IntPtr handle = this.handle;
			if (handle != IntPtr.Zero)
			{
				((NativeOverlapped*)(void*)handle)->InternalHigh = IntPtr.Zero;
				((NativeOverlapped*)(void*)handle)->InternalLow = IntPtr.Zero;
				((NativeOverlapped*)(void*)handle)->EventHandle = IntPtr.Zero;
			}
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00065B90 File Offset: 0x00063D90
		protected unsafe override bool ReleaseHandle()
		{
			IntPtr intPtr = Interlocked.Exchange(ref this.handle, IntPtr.Zero);
			if (intPtr != IntPtr.Zero && !NclUtilities.HasShutdownStarted)
			{
				Overlapped.Free((NativeOverlapped*)(void*)intPtr);
			}
			return true;
		}

		// Token: 0x0400154E RID: 5454
		internal static readonly SafeNativeOverlapped Zero = new SafeNativeOverlapped();
	}
}
