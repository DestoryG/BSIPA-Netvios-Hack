using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000003 RID: 3
	public struct IVRExtendedDisplay
	{
		// Token: 0x04000030 RID: 48
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRExtendedDisplay._GetWindowBounds GetWindowBounds;

		// Token: 0x04000031 RID: 49
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRExtendedDisplay._GetEyeOutputViewport GetEyeOutputViewport;

		// Token: 0x04000032 RID: 50
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRExtendedDisplay._GetDXGIOutputInfo GetDXGIOutputInfo;

		// Token: 0x02000118 RID: 280
		// (Invoke) Token: 0x06000314 RID: 788
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetWindowBounds(ref int pnX, ref int pnY, ref uint pnWidth, ref uint pnHeight);

		// Token: 0x02000119 RID: 281
		// (Invoke) Token: 0x06000318 RID: 792
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetEyeOutputViewport(EVREye eEye, ref uint pnX, ref uint pnY, ref uint pnWidth, ref uint pnHeight);

		// Token: 0x0200011A RID: 282
		// (Invoke) Token: 0x0600031C RID: 796
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetDXGIOutputInfo(ref int pnAdapterIndex, ref int pnAdapterOutputIndex);
	}
}
