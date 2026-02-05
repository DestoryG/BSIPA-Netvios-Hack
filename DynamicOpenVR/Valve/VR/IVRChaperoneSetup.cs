using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000007 RID: 7
	public struct IVRChaperoneSetup
	{
		// Token: 0x04000066 RID: 102
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._CommitWorkingCopy CommitWorkingCopy;

		// Token: 0x04000067 RID: 103
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._RevertWorkingCopy RevertWorkingCopy;

		// Token: 0x04000068 RID: 104
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetWorkingPlayAreaSize GetWorkingPlayAreaSize;

		// Token: 0x04000069 RID: 105
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetWorkingPlayAreaRect GetWorkingPlayAreaRect;

		// Token: 0x0400006A RID: 106
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetWorkingCollisionBoundsInfo GetWorkingCollisionBoundsInfo;

		// Token: 0x0400006B RID: 107
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetLiveCollisionBoundsInfo GetLiveCollisionBoundsInfo;

		// Token: 0x0400006C RID: 108
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetWorkingSeatedZeroPoseToRawTrackingPose GetWorkingSeatedZeroPoseToRawTrackingPose;

		// Token: 0x0400006D RID: 109
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetWorkingStandingZeroPoseToRawTrackingPose GetWorkingStandingZeroPoseToRawTrackingPose;

		// Token: 0x0400006E RID: 110
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingPlayAreaSize SetWorkingPlayAreaSize;

		// Token: 0x0400006F RID: 111
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingCollisionBoundsInfo SetWorkingCollisionBoundsInfo;

		// Token: 0x04000070 RID: 112
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingPerimeter SetWorkingPerimeter;

		// Token: 0x04000071 RID: 113
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingSeatedZeroPoseToRawTrackingPose SetWorkingSeatedZeroPoseToRawTrackingPose;

		// Token: 0x04000072 RID: 114
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._SetWorkingStandingZeroPoseToRawTrackingPose SetWorkingStandingZeroPoseToRawTrackingPose;

		// Token: 0x04000073 RID: 115
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._ReloadFromDisk ReloadFromDisk;

		// Token: 0x04000074 RID: 116
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._GetLiveSeatedZeroPoseToRawTrackingPose GetLiveSeatedZeroPoseToRawTrackingPose;

		// Token: 0x04000075 RID: 117
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._ExportLiveToBuffer ExportLiveToBuffer;

		// Token: 0x04000076 RID: 118
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._ImportFromBufferToWorking ImportFromBufferToWorking;

		// Token: 0x04000077 RID: 119
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._ShowWorkingSetPreview ShowWorkingSetPreview;

		// Token: 0x04000078 RID: 120
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRChaperoneSetup._HideWorkingSetPreview HideWorkingSetPreview;

		// Token: 0x0200014E RID: 334
		// (Invoke) Token: 0x060003EC RID: 1004
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _CommitWorkingCopy(EChaperoneConfigFile configFile);

		// Token: 0x0200014F RID: 335
		// (Invoke) Token: 0x060003F0 RID: 1008
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _RevertWorkingCopy();

		// Token: 0x02000150 RID: 336
		// (Invoke) Token: 0x060003F4 RID: 1012
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetWorkingPlayAreaSize(ref float pSizeX, ref float pSizeZ);

		// Token: 0x02000151 RID: 337
		// (Invoke) Token: 0x060003F8 RID: 1016
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetWorkingPlayAreaRect(ref HmdQuad_t rect);

		// Token: 0x02000152 RID: 338
		// (Invoke) Token: 0x060003FC RID: 1020
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetWorkingCollisionBoundsInfo([In] [Out] HmdQuad_t[] pQuadsBuffer, ref uint punQuadsCount);

		// Token: 0x02000153 RID: 339
		// (Invoke) Token: 0x06000400 RID: 1024
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetLiveCollisionBoundsInfo([In] [Out] HmdQuad_t[] pQuadsBuffer, ref uint punQuadsCount);

		// Token: 0x02000154 RID: 340
		// (Invoke) Token: 0x06000404 RID: 1028
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetWorkingSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatSeatedZeroPoseToRawTrackingPose);

		// Token: 0x02000155 RID: 341
		// (Invoke) Token: 0x06000408 RID: 1032
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetWorkingStandingZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatStandingZeroPoseToRawTrackingPose);

		// Token: 0x02000156 RID: 342
		// (Invoke) Token: 0x0600040C RID: 1036
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetWorkingPlayAreaSize(float sizeX, float sizeZ);

		// Token: 0x02000157 RID: 343
		// (Invoke) Token: 0x06000410 RID: 1040
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetWorkingCollisionBoundsInfo([In] [Out] HmdQuad_t[] pQuadsBuffer, uint unQuadsCount);

		// Token: 0x02000158 RID: 344
		// (Invoke) Token: 0x06000414 RID: 1044
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetWorkingPerimeter([In] [Out] HmdVector2_t[] pPointBuffer, uint unPointCount);

		// Token: 0x02000159 RID: 345
		// (Invoke) Token: 0x06000418 RID: 1048
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetWorkingSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pMatSeatedZeroPoseToRawTrackingPose);

		// Token: 0x0200015A RID: 346
		// (Invoke) Token: 0x0600041C RID: 1052
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetWorkingStandingZeroPoseToRawTrackingPose(ref HmdMatrix34_t pMatStandingZeroPoseToRawTrackingPose);

		// Token: 0x0200015B RID: 347
		// (Invoke) Token: 0x06000420 RID: 1056
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ReloadFromDisk(EChaperoneConfigFile configFile);

		// Token: 0x0200015C RID: 348
		// (Invoke) Token: 0x06000424 RID: 1060
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetLiveSeatedZeroPoseToRawTrackingPose(ref HmdMatrix34_t pmatSeatedZeroPoseToRawTrackingPose);

		// Token: 0x0200015D RID: 349
		// (Invoke) Token: 0x06000428 RID: 1064
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ExportLiveToBuffer(StringBuilder pBuffer, ref uint pnBufferLength);

		// Token: 0x0200015E RID: 350
		// (Invoke) Token: 0x0600042C RID: 1068
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ImportFromBufferToWorking(string pBuffer, uint nImportFlags);

		// Token: 0x0200015F RID: 351
		// (Invoke) Token: 0x06000430 RID: 1072
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ShowWorkingSetPreview();

		// Token: 0x02000160 RID: 352
		// (Invoke) Token: 0x06000434 RID: 1076
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _HideWorkingSetPreview();
	}
}
