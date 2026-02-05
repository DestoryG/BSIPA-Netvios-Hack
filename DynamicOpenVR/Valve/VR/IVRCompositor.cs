using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000008 RID: 8
	public struct IVRCompositor
	{
		// Token: 0x04000079 RID: 121
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._SetTrackingSpace SetTrackingSpace;

		// Token: 0x0400007A RID: 122
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetTrackingSpace GetTrackingSpace;

		// Token: 0x0400007B RID: 123
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._WaitGetPoses WaitGetPoses;

		// Token: 0x0400007C RID: 124
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetLastPoses GetLastPoses;

		// Token: 0x0400007D RID: 125
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetLastPoseForTrackedDeviceIndex GetLastPoseForTrackedDeviceIndex;

		// Token: 0x0400007E RID: 126
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._Submit Submit;

		// Token: 0x0400007F RID: 127
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ClearLastSubmittedFrame ClearLastSubmittedFrame;

		// Token: 0x04000080 RID: 128
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._PostPresentHandoff PostPresentHandoff;

		// Token: 0x04000081 RID: 129
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetFrameTiming GetFrameTiming;

		// Token: 0x04000082 RID: 130
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetFrameTimings GetFrameTimings;

		// Token: 0x04000083 RID: 131
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetFrameTimeRemaining GetFrameTimeRemaining;

		// Token: 0x04000084 RID: 132
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetCumulativeStats GetCumulativeStats;

		// Token: 0x04000085 RID: 133
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._FadeToColor FadeToColor;

		// Token: 0x04000086 RID: 134
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetCurrentFadeColor GetCurrentFadeColor;

		// Token: 0x04000087 RID: 135
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._FadeGrid FadeGrid;

		// Token: 0x04000088 RID: 136
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetCurrentGridAlpha GetCurrentGridAlpha;

		// Token: 0x04000089 RID: 137
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._SetSkyboxOverride SetSkyboxOverride;

		// Token: 0x0400008A RID: 138
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ClearSkyboxOverride ClearSkyboxOverride;

		// Token: 0x0400008B RID: 139
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._CompositorBringToFront CompositorBringToFront;

		// Token: 0x0400008C RID: 140
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._CompositorGoToBack CompositorGoToBack;

		// Token: 0x0400008D RID: 141
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._CompositorQuit CompositorQuit;

		// Token: 0x0400008E RID: 142
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._IsFullscreen IsFullscreen;

		// Token: 0x0400008F RID: 143
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetCurrentSceneFocusProcess GetCurrentSceneFocusProcess;

		// Token: 0x04000090 RID: 144
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetLastFrameRenderer GetLastFrameRenderer;

		// Token: 0x04000091 RID: 145
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._CanRenderScene CanRenderScene;

		// Token: 0x04000092 RID: 146
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ShowMirrorWindow ShowMirrorWindow;

		// Token: 0x04000093 RID: 147
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._HideMirrorWindow HideMirrorWindow;

		// Token: 0x04000094 RID: 148
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._IsMirrorWindowVisible IsMirrorWindowVisible;

		// Token: 0x04000095 RID: 149
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._CompositorDumpImages CompositorDumpImages;

		// Token: 0x04000096 RID: 150
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ShouldAppRenderWithLowResources ShouldAppRenderWithLowResources;

		// Token: 0x04000097 RID: 151
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ForceInterleavedReprojectionOn ForceInterleavedReprojectionOn;

		// Token: 0x04000098 RID: 152
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ForceReconnectProcess ForceReconnectProcess;

		// Token: 0x04000099 RID: 153
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._SuspendRendering SuspendRendering;

		// Token: 0x0400009A RID: 154
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetMirrorTextureD3D11 GetMirrorTextureD3D11;

		// Token: 0x0400009B RID: 155
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ReleaseMirrorTextureD3D11 ReleaseMirrorTextureD3D11;

		// Token: 0x0400009C RID: 156
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetMirrorTextureGL GetMirrorTextureGL;

		// Token: 0x0400009D RID: 157
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._ReleaseSharedGLTexture ReleaseSharedGLTexture;

		// Token: 0x0400009E RID: 158
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._LockGLSharedTextureForAccess LockGLSharedTextureForAccess;

		// Token: 0x0400009F RID: 159
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._UnlockGLSharedTextureForAccess UnlockGLSharedTextureForAccess;

		// Token: 0x040000A0 RID: 160
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetVulkanInstanceExtensionsRequired GetVulkanInstanceExtensionsRequired;

		// Token: 0x040000A1 RID: 161
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._GetVulkanDeviceExtensionsRequired GetVulkanDeviceExtensionsRequired;

		// Token: 0x040000A2 RID: 162
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._SetExplicitTimingMode SetExplicitTimingMode;

		// Token: 0x040000A3 RID: 163
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._SubmitExplicitTimingData SubmitExplicitTimingData;

		// Token: 0x040000A4 RID: 164
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRCompositor._IsMotionSmoothingEnabled IsMotionSmoothingEnabled;

		// Token: 0x02000161 RID: 353
		// (Invoke) Token: 0x06000438 RID: 1080
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetTrackingSpace(ETrackingUniverseOrigin eOrigin);

		// Token: 0x02000162 RID: 354
		// (Invoke) Token: 0x0600043C RID: 1084
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ETrackingUniverseOrigin _GetTrackingSpace();

		// Token: 0x02000163 RID: 355
		// (Invoke) Token: 0x06000440 RID: 1088
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _WaitGetPoses([In] [Out] TrackedDevicePose_t[] pRenderPoseArray, uint unRenderPoseArrayCount, [In] [Out] TrackedDevicePose_t[] pGamePoseArray, uint unGamePoseArrayCount);

		// Token: 0x02000164 RID: 356
		// (Invoke) Token: 0x06000444 RID: 1092
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _GetLastPoses([In] [Out] TrackedDevicePose_t[] pRenderPoseArray, uint unRenderPoseArrayCount, [In] [Out] TrackedDevicePose_t[] pGamePoseArray, uint unGamePoseArrayCount);

		// Token: 0x02000165 RID: 357
		// (Invoke) Token: 0x06000448 RID: 1096
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _GetLastPoseForTrackedDeviceIndex(uint unDeviceIndex, ref TrackedDevicePose_t pOutputPose, ref TrackedDevicePose_t pOutputGamePose);

		// Token: 0x02000166 RID: 358
		// (Invoke) Token: 0x0600044C RID: 1100
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _Submit(EVREye eEye, ref Texture_t pTexture, ref VRTextureBounds_t pBounds, EVRSubmitFlags nSubmitFlags);

		// Token: 0x02000167 RID: 359
		// (Invoke) Token: 0x06000450 RID: 1104
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ClearLastSubmittedFrame();

		// Token: 0x02000168 RID: 360
		// (Invoke) Token: 0x06000454 RID: 1108
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _PostPresentHandoff();

		// Token: 0x02000169 RID: 361
		// (Invoke) Token: 0x06000458 RID: 1112
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetFrameTiming(ref Compositor_FrameTiming pTiming, uint unFramesAgo);

		// Token: 0x0200016A RID: 362
		// (Invoke) Token: 0x0600045C RID: 1116
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetFrameTimings([In] [Out] Compositor_FrameTiming[] pTiming, uint nFrames);

		// Token: 0x0200016B RID: 363
		// (Invoke) Token: 0x06000460 RID: 1120
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate float _GetFrameTimeRemaining();

		// Token: 0x0200016C RID: 364
		// (Invoke) Token: 0x06000464 RID: 1124
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetCumulativeStats(ref Compositor_CumulativeStats pStats, uint nStatsSizeInBytes);

		// Token: 0x0200016D RID: 365
		// (Invoke) Token: 0x06000468 RID: 1128
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _FadeToColor(float fSeconds, float fRed, float fGreen, float fBlue, float fAlpha, bool bBackground);

		// Token: 0x0200016E RID: 366
		// (Invoke) Token: 0x0600046C RID: 1132
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdColor_t _GetCurrentFadeColor(bool bBackground);

		// Token: 0x0200016F RID: 367
		// (Invoke) Token: 0x06000470 RID: 1136
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _FadeGrid(float fSeconds, bool bFadeIn);

		// Token: 0x02000170 RID: 368
		// (Invoke) Token: 0x06000474 RID: 1140
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate float _GetCurrentGridAlpha();

		// Token: 0x02000171 RID: 369
		// (Invoke) Token: 0x06000478 RID: 1144
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _SetSkyboxOverride([In] [Out] Texture_t[] pTextures, uint unTextureCount);

		// Token: 0x02000172 RID: 370
		// (Invoke) Token: 0x0600047C RID: 1148
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ClearSkyboxOverride();

		// Token: 0x02000173 RID: 371
		// (Invoke) Token: 0x06000480 RID: 1152
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _CompositorBringToFront();

		// Token: 0x02000174 RID: 372
		// (Invoke) Token: 0x06000484 RID: 1156
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _CompositorGoToBack();

		// Token: 0x02000175 RID: 373
		// (Invoke) Token: 0x06000488 RID: 1160
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _CompositorQuit();

		// Token: 0x02000176 RID: 374
		// (Invoke) Token: 0x0600048C RID: 1164
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsFullscreen();

		// Token: 0x02000177 RID: 375
		// (Invoke) Token: 0x06000490 RID: 1168
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetCurrentSceneFocusProcess();

		// Token: 0x02000178 RID: 376
		// (Invoke) Token: 0x06000494 RID: 1172
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetLastFrameRenderer();

		// Token: 0x02000179 RID: 377
		// (Invoke) Token: 0x06000498 RID: 1176
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _CanRenderScene();

		// Token: 0x0200017A RID: 378
		// (Invoke) Token: 0x0600049C RID: 1180
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ShowMirrorWindow();

		// Token: 0x0200017B RID: 379
		// (Invoke) Token: 0x060004A0 RID: 1184
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _HideMirrorWindow();

		// Token: 0x0200017C RID: 380
		// (Invoke) Token: 0x060004A4 RID: 1188
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsMirrorWindowVisible();

		// Token: 0x0200017D RID: 381
		// (Invoke) Token: 0x060004A8 RID: 1192
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _CompositorDumpImages();

		// Token: 0x0200017E RID: 382
		// (Invoke) Token: 0x060004AC RID: 1196
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ShouldAppRenderWithLowResources();

		// Token: 0x0200017F RID: 383
		// (Invoke) Token: 0x060004B0 RID: 1200
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ForceInterleavedReprojectionOn(bool bOverride);

		// Token: 0x02000180 RID: 384
		// (Invoke) Token: 0x060004B4 RID: 1204
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ForceReconnectProcess();

		// Token: 0x02000181 RID: 385
		// (Invoke) Token: 0x060004B8 RID: 1208
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SuspendRendering(bool bSuspend);

		// Token: 0x02000182 RID: 386
		// (Invoke) Token: 0x060004BC RID: 1212
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _GetMirrorTextureD3D11(EVREye eEye, IntPtr pD3D11DeviceOrResource, ref IntPtr ppD3D11ShaderResourceView);

		// Token: 0x02000183 RID: 387
		// (Invoke) Token: 0x060004C0 RID: 1216
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ReleaseMirrorTextureD3D11(IntPtr pD3D11ShaderResourceView);

		// Token: 0x02000184 RID: 388
		// (Invoke) Token: 0x060004C4 RID: 1220
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _GetMirrorTextureGL(EVREye eEye, ref uint pglTextureId, IntPtr pglSharedTextureHandle);

		// Token: 0x02000185 RID: 389
		// (Invoke) Token: 0x060004C8 RID: 1224
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ReleaseSharedGLTexture(uint glTextureId, IntPtr glSharedTextureHandle);

		// Token: 0x02000186 RID: 390
		// (Invoke) Token: 0x060004CC RID: 1228
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _LockGLSharedTextureForAccess(IntPtr glSharedTextureHandle);

		// Token: 0x02000187 RID: 391
		// (Invoke) Token: 0x060004D0 RID: 1232
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _UnlockGLSharedTextureForAccess(IntPtr glSharedTextureHandle);

		// Token: 0x02000188 RID: 392
		// (Invoke) Token: 0x060004D4 RID: 1236
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetVulkanInstanceExtensionsRequired(StringBuilder pchValue, uint unBufferSize);

		// Token: 0x02000189 RID: 393
		// (Invoke) Token: 0x060004D8 RID: 1240
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetVulkanDeviceExtensionsRequired(IntPtr pPhysicalDevice, StringBuilder pchValue, uint unBufferSize);

		// Token: 0x0200018A RID: 394
		// (Invoke) Token: 0x060004DC RID: 1244
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetExplicitTimingMode(EVRCompositorTimingMode eTimingMode);

		// Token: 0x0200018B RID: 395
		// (Invoke) Token: 0x060004E0 RID: 1248
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRCompositorError _SubmitExplicitTimingData();

		// Token: 0x0200018C RID: 396
		// (Invoke) Token: 0x060004E4 RID: 1252
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsMotionSmoothingEnabled();
	}
}
