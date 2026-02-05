using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000002 RID: 2
	public struct IVRSystem
	{
		// Token: 0x04000001 RID: 1
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetRecommendedRenderTargetSize GetRecommendedRenderTargetSize;

		// Token: 0x04000002 RID: 2
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetProjectionMatrix GetProjectionMatrix;

		// Token: 0x04000003 RID: 3
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetProjectionRaw GetProjectionRaw;

		// Token: 0x04000004 RID: 4
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._ComputeDistortion ComputeDistortion;

		// Token: 0x04000005 RID: 5
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetEyeToHeadTransform GetEyeToHeadTransform;

		// Token: 0x04000006 RID: 6
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetTimeSinceLastVsync GetTimeSinceLastVsync;

		// Token: 0x04000007 RID: 7
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetD3D9AdapterIndex GetD3D9AdapterIndex;

		// Token: 0x04000008 RID: 8
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetDXGIOutputInfo GetDXGIOutputInfo;

		// Token: 0x04000009 RID: 9
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetOutputDevice GetOutputDevice;

		// Token: 0x0400000A RID: 10
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._IsDisplayOnDesktop IsDisplayOnDesktop;

		// Token: 0x0400000B RID: 11
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._SetDisplayVisibility SetDisplayVisibility;

		// Token: 0x0400000C RID: 12
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetDeviceToAbsoluteTrackingPose GetDeviceToAbsoluteTrackingPose;

		// Token: 0x0400000D RID: 13
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._ResetSeatedZeroPose ResetSeatedZeroPose;

		// Token: 0x0400000E RID: 14
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetSeatedZeroPoseToStandingAbsoluteTrackingPose GetSeatedZeroPoseToStandingAbsoluteTrackingPose;

		// Token: 0x0400000F RID: 15
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetRawZeroPoseToStandingAbsoluteTrackingPose GetRawZeroPoseToStandingAbsoluteTrackingPose;

		// Token: 0x04000010 RID: 16
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetSortedTrackedDeviceIndicesOfClass GetSortedTrackedDeviceIndicesOfClass;

		// Token: 0x04000011 RID: 17
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetTrackedDeviceActivityLevel GetTrackedDeviceActivityLevel;

		// Token: 0x04000012 RID: 18
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._ApplyTransform ApplyTransform;

		// Token: 0x04000013 RID: 19
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetTrackedDeviceIndexForControllerRole GetTrackedDeviceIndexForControllerRole;

		// Token: 0x04000014 RID: 20
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetControllerRoleForTrackedDeviceIndex GetControllerRoleForTrackedDeviceIndex;

		// Token: 0x04000015 RID: 21
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetTrackedDeviceClass GetTrackedDeviceClass;

		// Token: 0x04000016 RID: 22
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._IsTrackedDeviceConnected IsTrackedDeviceConnected;

		// Token: 0x04000017 RID: 23
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetBoolTrackedDeviceProperty GetBoolTrackedDeviceProperty;

		// Token: 0x04000018 RID: 24
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetFloatTrackedDeviceProperty GetFloatTrackedDeviceProperty;

		// Token: 0x04000019 RID: 25
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetInt32TrackedDeviceProperty GetInt32TrackedDeviceProperty;

		// Token: 0x0400001A RID: 26
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetUint64TrackedDeviceProperty GetUint64TrackedDeviceProperty;

		// Token: 0x0400001B RID: 27
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetMatrix34TrackedDeviceProperty GetMatrix34TrackedDeviceProperty;

		// Token: 0x0400001C RID: 28
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetArrayTrackedDeviceProperty GetArrayTrackedDeviceProperty;

		// Token: 0x0400001D RID: 29
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetStringTrackedDeviceProperty GetStringTrackedDeviceProperty;

		// Token: 0x0400001E RID: 30
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetPropErrorNameFromEnum GetPropErrorNameFromEnum;

		// Token: 0x0400001F RID: 31
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._PollNextEvent PollNextEvent;

		// Token: 0x04000020 RID: 32
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._PollNextEventWithPose PollNextEventWithPose;

		// Token: 0x04000021 RID: 33
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetEventTypeNameFromEnum GetEventTypeNameFromEnum;

		// Token: 0x04000022 RID: 34
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetHiddenAreaMesh GetHiddenAreaMesh;

		// Token: 0x04000023 RID: 35
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetControllerState GetControllerState;

		// Token: 0x04000024 RID: 36
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetControllerStateWithPose GetControllerStateWithPose;

		// Token: 0x04000025 RID: 37
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._TriggerHapticPulse TriggerHapticPulse;

		// Token: 0x04000026 RID: 38
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetButtonIdNameFromEnum GetButtonIdNameFromEnum;

		// Token: 0x04000027 RID: 39
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetControllerAxisTypeNameFromEnum GetControllerAxisTypeNameFromEnum;

		// Token: 0x04000028 RID: 40
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._IsInputAvailable IsInputAvailable;

		// Token: 0x04000029 RID: 41
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._IsSteamVRDrawingControllers IsSteamVRDrawingControllers;

		// Token: 0x0400002A RID: 42
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._ShouldApplicationPause ShouldApplicationPause;

		// Token: 0x0400002B RID: 43
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._ShouldApplicationReduceRenderingWork ShouldApplicationReduceRenderingWork;

		// Token: 0x0400002C RID: 44
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._DriverDebugRequest DriverDebugRequest;

		// Token: 0x0400002D RID: 45
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._PerformFirmwareUpdate PerformFirmwareUpdate;

		// Token: 0x0400002E RID: 46
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._AcknowledgeQuit_Exiting AcknowledgeQuit_Exiting;

		// Token: 0x0400002F RID: 47
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._AcknowledgeQuit_UserPrompt AcknowledgeQuit_UserPrompt;

		// Token: 0x020000E9 RID: 233
		// (Invoke) Token: 0x06000258 RID: 600
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetRecommendedRenderTargetSize(ref uint pnWidth, ref uint pnHeight);

		// Token: 0x020000EA RID: 234
		// (Invoke) Token: 0x0600025C RID: 604
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdMatrix44_t _GetProjectionMatrix(EVREye eEye, float fNearZ, float fFarZ);

		// Token: 0x020000EB RID: 235
		// (Invoke) Token: 0x06000260 RID: 608
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetProjectionRaw(EVREye eEye, ref float pfLeft, ref float pfRight, ref float pfTop, ref float pfBottom);

		// Token: 0x020000EC RID: 236
		// (Invoke) Token: 0x06000264 RID: 612
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ComputeDistortion(EVREye eEye, float fU, float fV, ref DistortionCoordinates_t pDistortionCoordinates);

		// Token: 0x020000ED RID: 237
		// (Invoke) Token: 0x06000268 RID: 616
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdMatrix34_t _GetEyeToHeadTransform(EVREye eEye);

		// Token: 0x020000EE RID: 238
		// (Invoke) Token: 0x0600026C RID: 620
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetTimeSinceLastVsync(ref float pfSecondsSinceLastVsync, ref ulong pulFrameCounter);

		// Token: 0x020000EF RID: 239
		// (Invoke) Token: 0x06000270 RID: 624
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate int _GetD3D9AdapterIndex();

		// Token: 0x020000F0 RID: 240
		// (Invoke) Token: 0x06000274 RID: 628
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetDXGIOutputInfo(ref int pnAdapterIndex);

		// Token: 0x020000F1 RID: 241
		// (Invoke) Token: 0x06000278 RID: 632
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetOutputDevice(ref ulong pnDevice, ETextureType textureType, IntPtr pInstance);

		// Token: 0x020000F2 RID: 242
		// (Invoke) Token: 0x0600027C RID: 636
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsDisplayOnDesktop();

		// Token: 0x020000F3 RID: 243
		// (Invoke) Token: 0x06000280 RID: 640
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _SetDisplayVisibility(bool bIsVisibleOnDesktop);

		// Token: 0x020000F4 RID: 244
		// (Invoke) Token: 0x06000284 RID: 644
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetDeviceToAbsoluteTrackingPose(ETrackingUniverseOrigin eOrigin, float fPredictedSecondsToPhotonsFromNow, [In] [Out] TrackedDevicePose_t[] pTrackedDevicePoseArray, uint unTrackedDevicePoseArrayCount);

		// Token: 0x020000F5 RID: 245
		// (Invoke) Token: 0x06000288 RID: 648
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ResetSeatedZeroPose();

		// Token: 0x020000F6 RID: 246
		// (Invoke) Token: 0x0600028C RID: 652
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdMatrix34_t _GetSeatedZeroPoseToStandingAbsoluteTrackingPose();

		// Token: 0x020000F7 RID: 247
		// (Invoke) Token: 0x06000290 RID: 656
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdMatrix34_t _GetRawZeroPoseToStandingAbsoluteTrackingPose();

		// Token: 0x020000F8 RID: 248
		// (Invoke) Token: 0x06000294 RID: 660
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetSortedTrackedDeviceIndicesOfClass(ETrackedDeviceClass eTrackedDeviceClass, [In] [Out] uint[] punTrackedDeviceIndexArray, uint unTrackedDeviceIndexArrayCount, uint unRelativeToTrackedDeviceIndex);

		// Token: 0x020000F9 RID: 249
		// (Invoke) Token: 0x06000298 RID: 664
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EDeviceActivityLevel _GetTrackedDeviceActivityLevel(uint unDeviceId);

		// Token: 0x020000FA RID: 250
		// (Invoke) Token: 0x0600029C RID: 668
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ApplyTransform(ref TrackedDevicePose_t pOutputPose, ref TrackedDevicePose_t pTrackedDevicePose, ref HmdMatrix34_t pTransform);

		// Token: 0x020000FB RID: 251
		// (Invoke) Token: 0x060002A0 RID: 672
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole unDeviceType);

		// Token: 0x020000FC RID: 252
		// (Invoke) Token: 0x060002A4 RID: 676
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ETrackedControllerRole _GetControllerRoleForTrackedDeviceIndex(uint unDeviceIndex);

		// Token: 0x020000FD RID: 253
		// (Invoke) Token: 0x060002A8 RID: 680
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ETrackedDeviceClass _GetTrackedDeviceClass(uint unDeviceIndex);

		// Token: 0x020000FE RID: 254
		// (Invoke) Token: 0x060002AC RID: 684
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsTrackedDeviceConnected(uint unDeviceIndex);

		// Token: 0x020000FF RID: 255
		// (Invoke) Token: 0x060002B0 RID: 688
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetBoolTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError);

		// Token: 0x02000100 RID: 256
		// (Invoke) Token: 0x060002B4 RID: 692
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate float _GetFloatTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError);

		// Token: 0x02000101 RID: 257
		// (Invoke) Token: 0x060002B8 RID: 696
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate int _GetInt32TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError);

		// Token: 0x02000102 RID: 258
		// (Invoke) Token: 0x060002BC RID: 700
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetUint64TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError);

		// Token: 0x02000103 RID: 259
		// (Invoke) Token: 0x060002C0 RID: 704
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdMatrix34_t _GetMatrix34TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError);

		// Token: 0x02000104 RID: 260
		// (Invoke) Token: 0x060002C4 RID: 708
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetArrayTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, uint propType, IntPtr pBuffer, uint unBufferSize, ref ETrackedPropertyError pError);

		// Token: 0x02000105 RID: 261
		// (Invoke) Token: 0x060002C8 RID: 712
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetStringTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, StringBuilder pchValue, uint unBufferSize, ref ETrackedPropertyError pError);

		// Token: 0x02000106 RID: 262
		// (Invoke) Token: 0x060002CC RID: 716
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetPropErrorNameFromEnum(ETrackedPropertyError error);

		// Token: 0x02000107 RID: 263
		// (Invoke) Token: 0x060002D0 RID: 720
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _PollNextEvent(ref VREvent_t pEvent, uint uncbVREvent);

		// Token: 0x02000108 RID: 264
		// (Invoke) Token: 0x060002D4 RID: 724
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _PollNextEventWithPose(ETrackingUniverseOrigin eOrigin, ref VREvent_t pEvent, uint uncbVREvent, ref TrackedDevicePose_t pTrackedDevicePose);

		// Token: 0x02000109 RID: 265
		// (Invoke) Token: 0x060002D8 RID: 728
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetEventTypeNameFromEnum(EVREventType eType);

		// Token: 0x0200010A RID: 266
		// (Invoke) Token: 0x060002DC RID: 732
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HiddenAreaMesh_t _GetHiddenAreaMesh(EVREye eEye, EHiddenAreaMeshType type);

		// Token: 0x0200010B RID: 267
		// (Invoke) Token: 0x060002E0 RID: 736
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetControllerState(uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, uint unControllerStateSize);

		// Token: 0x0200010C RID: 268
		// (Invoke) Token: 0x060002E4 RID: 740
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetControllerStateWithPose(ETrackingUniverseOrigin eOrigin, uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, uint unControllerStateSize, ref TrackedDevicePose_t pTrackedDevicePose);

		// Token: 0x0200010D RID: 269
		// (Invoke) Token: 0x060002E8 RID: 744
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _TriggerHapticPulse(uint unControllerDeviceIndex, uint unAxisId, ushort usDurationMicroSec);

		// Token: 0x0200010E RID: 270
		// (Invoke) Token: 0x060002EC RID: 748
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetButtonIdNameFromEnum(EVRButtonId eButtonId);

		// Token: 0x0200010F RID: 271
		// (Invoke) Token: 0x060002F0 RID: 752
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetControllerAxisTypeNameFromEnum(EVRControllerAxisType eAxisType);

		// Token: 0x02000110 RID: 272
		// (Invoke) Token: 0x060002F4 RID: 756
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsInputAvailable();

		// Token: 0x02000111 RID: 273
		// (Invoke) Token: 0x060002F8 RID: 760
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsSteamVRDrawingControllers();

		// Token: 0x02000112 RID: 274
		// (Invoke) Token: 0x060002FC RID: 764
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ShouldApplicationPause();

		// Token: 0x02000113 RID: 275
		// (Invoke) Token: 0x06000300 RID: 768
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ShouldApplicationReduceRenderingWork();

		// Token: 0x02000114 RID: 276
		// (Invoke) Token: 0x06000304 RID: 772
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _DriverDebugRequest(uint unDeviceIndex, string pchRequest, StringBuilder pchResponseBuffer, uint unResponseBufferSize);

		// Token: 0x02000115 RID: 277
		// (Invoke) Token: 0x06000308 RID: 776
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRFirmwareError _PerformFirmwareUpdate(uint unDeviceIndex);

		// Token: 0x02000116 RID: 278
		// (Invoke) Token: 0x0600030C RID: 780
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _AcknowledgeQuit_Exiting();

		// Token: 0x02000117 RID: 279
		// (Invoke) Token: 0x06000310 RID: 784
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _AcknowledgeQuit_UserPrompt();
	}
}
