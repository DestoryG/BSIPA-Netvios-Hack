using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000004 RID: 4
	public struct IVRTrackedCamera
	{
		// Token: 0x04000033 RID: 51
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetCameraErrorNameFromEnum GetCameraErrorNameFromEnum;

		// Token: 0x04000034 RID: 52
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._HasCamera HasCamera;

		// Token: 0x04000035 RID: 53
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetCameraFrameSize GetCameraFrameSize;

		// Token: 0x04000036 RID: 54
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetCameraIntrinsics GetCameraIntrinsics;

		// Token: 0x04000037 RID: 55
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetCameraProjection GetCameraProjection;

		// Token: 0x04000038 RID: 56
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._AcquireVideoStreamingService AcquireVideoStreamingService;

		// Token: 0x04000039 RID: 57
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._ReleaseVideoStreamingService ReleaseVideoStreamingService;

		// Token: 0x0400003A RID: 58
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetVideoStreamFrameBuffer GetVideoStreamFrameBuffer;

		// Token: 0x0400003B RID: 59
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetVideoStreamTextureSize GetVideoStreamTextureSize;

		// Token: 0x0400003C RID: 60
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetVideoStreamTextureD3D11 GetVideoStreamTextureD3D11;

		// Token: 0x0400003D RID: 61
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._GetVideoStreamTextureGL GetVideoStreamTextureGL;

		// Token: 0x0400003E RID: 62
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRTrackedCamera._ReleaseVideoStreamTextureGL ReleaseVideoStreamTextureGL;

		// Token: 0x0200011B RID: 283
		// (Invoke) Token: 0x06000320 RID: 800
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetCameraErrorNameFromEnum(EVRTrackedCameraError eCameraError);

		// Token: 0x0200011C RID: 284
		// (Invoke) Token: 0x06000324 RID: 804
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _HasCamera(uint nDeviceIndex, ref bool pHasCamera);

		// Token: 0x0200011D RID: 285
		// (Invoke) Token: 0x06000328 RID: 808
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetCameraFrameSize(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, ref uint pnWidth, ref uint pnHeight, ref uint pnFrameBufferSize);

		// Token: 0x0200011E RID: 286
		// (Invoke) Token: 0x0600032C RID: 812
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetCameraIntrinsics(uint nDeviceIndex, uint nCameraIndex, EVRTrackedCameraFrameType eFrameType, ref HmdVector2_t pFocalLength, ref HmdVector2_t pCenter);

		// Token: 0x0200011F RID: 287
		// (Invoke) Token: 0x06000330 RID: 816
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetCameraProjection(uint nDeviceIndex, uint nCameraIndex, EVRTrackedCameraFrameType eFrameType, float flZNear, float flZFar, ref HmdMatrix44_t pProjection);

		// Token: 0x02000120 RID: 288
		// (Invoke) Token: 0x06000334 RID: 820
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _AcquireVideoStreamingService(uint nDeviceIndex, ref ulong pHandle);

		// Token: 0x02000121 RID: 289
		// (Invoke) Token: 0x06000338 RID: 824
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _ReleaseVideoStreamingService(ulong hTrackedCamera);

		// Token: 0x02000122 RID: 290
		// (Invoke) Token: 0x0600033C RID: 828
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetVideoStreamFrameBuffer(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, IntPtr pFrameBuffer, uint nFrameBufferSize, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize);

		// Token: 0x02000123 RID: 291
		// (Invoke) Token: 0x06000340 RID: 832
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetVideoStreamTextureSize(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, ref VRTextureBounds_t pTextureBounds, ref uint pnWidth, ref uint pnHeight);

		// Token: 0x02000124 RID: 292
		// (Invoke) Token: 0x06000344 RID: 836
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetVideoStreamTextureD3D11(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, IntPtr pD3D11DeviceOrResource, ref IntPtr ppD3D11ShaderResourceView, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize);

		// Token: 0x02000125 RID: 293
		// (Invoke) Token: 0x06000348 RID: 840
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _GetVideoStreamTextureGL(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, ref uint pglTextureId, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize);

		// Token: 0x02000126 RID: 294
		// (Invoke) Token: 0x0600034C RID: 844
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRTrackedCameraError _ReleaseVideoStreamTextureGL(ulong hTrackedCamera, uint glTextureId);
	}
}
