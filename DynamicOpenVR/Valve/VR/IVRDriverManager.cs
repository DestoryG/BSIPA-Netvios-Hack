using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200000F RID: 15
	public struct IVRDriverManager
	{
		// Token: 0x04000121 RID: 289
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRDriverManager._GetDriverCount GetDriverCount;

		// Token: 0x04000122 RID: 290
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRDriverManager._GetDriverName GetDriverName;

		// Token: 0x04000123 RID: 291
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRDriverManager._GetDriverHandle GetDriverHandle;

		// Token: 0x02000209 RID: 521
		// (Invoke) Token: 0x060006D8 RID: 1752
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetDriverCount();

		// Token: 0x0200020A RID: 522
		// (Invoke) Token: 0x060006DC RID: 1756
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetDriverName(uint nDriver, StringBuilder pchValue, uint unBufferSize);

		// Token: 0x0200020B RID: 523
		// (Invoke) Token: 0x060006E0 RID: 1760
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetDriverHandle(string pchDriverName);
	}
}
