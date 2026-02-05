using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000006 RID: 6
	public struct IVRChaperone
	{
		// Token: 0x0400005E RID: 94
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._GetCalibrationState GetCalibrationState;

		// Token: 0x0400005F RID: 95
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._GetPlayAreaSize GetPlayAreaSize;

		// Token: 0x04000060 RID: 96
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._GetPlayAreaRect GetPlayAreaRect;

		// Token: 0x04000061 RID: 97
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._ReloadInfo ReloadInfo;

		// Token: 0x04000062 RID: 98
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._SetSceneColor SetSceneColor;

		// Token: 0x04000063 RID: 99
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._GetBoundsColor GetBoundsColor;

		// Token: 0x04000064 RID: 100
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._AreBoundsVisible AreBoundsVisible;

		// Token: 0x04000065 RID: 101
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperone._ForceBoundsVisible ForceBoundsVisible;

		// Token: 0x02000146 RID: 326
		// (Invoke) Token: 0x060003CC RID: 972
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ChaperoneCalibrationState _GetCalibrationState();

		// Token: 0x02000147 RID: 327
		// (Invoke) Token: 0x060003D0 RID: 976
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetPlayAreaSize(ref float pSizeX, ref float pSizeZ);

		// Token: 0x02000148 RID: 328
		// (Invoke) Token: 0x060003D4 RID: 980
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetPlayAreaRect(ref HmdQuad_t rect);

		// Token: 0x02000149 RID: 329
		// (Invoke) Token: 0x060003D8 RID: 984
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ReloadInfo();

		// Token: 0x0200014A RID: 330
		// (Invoke) Token: 0x060003DC RID: 988
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetSceneColor(HmdColor_t color);

		// Token: 0x0200014B RID: 331
		// (Invoke) Token: 0x060003E0 RID: 992
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetBoundsColor(ref HmdColor_t pOutputColorArray, int nNumOutputColors, float flCollisionBoundsFadeDistance, ref HmdColor_t pOutputCameraColor);

		// Token: 0x0200014C RID: 332
		// (Invoke) Token: 0x060003E4 RID: 996
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _AreBoundsVisible();

		// Token: 0x0200014D RID: 333
		// (Invoke) Token: 0x060003E8 RID: 1000
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ForceBoundsVisible(bool bForce);
	}
}
