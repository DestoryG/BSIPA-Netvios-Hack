using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000015 RID: 21
	public class CVRTrackedCamera
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002655 File Offset: 0x00000855
		internal CVRTrackedCamera(IntPtr pInterface)
		{
			this.FnTable = (IVRTrackedCamera)Marshal.PtrToStructure(pInterface, typeof(IVRTrackedCamera));
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002678 File Offset: 0x00000878
		public string GetCameraErrorNameFromEnum(EVRTrackedCameraError eCameraError)
		{
			return Marshal.PtrToStringAnsi(this.FnTable.GetCameraErrorNameFromEnum(eCameraError));
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002690 File Offset: 0x00000890
		public EVRTrackedCameraError HasCamera(uint nDeviceIndex, ref bool pHasCamera)
		{
			pHasCamera = false;
			return this.FnTable.HasCamera(nDeviceIndex, ref pHasCamera);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000026A7 File Offset: 0x000008A7
		public EVRTrackedCameraError GetCameraFrameSize(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, ref uint pnWidth, ref uint pnHeight, ref uint pnFrameBufferSize)
		{
			pnWidth = 0U;
			pnHeight = 0U;
			pnFrameBufferSize = 0U;
			return this.FnTable.GetCameraFrameSize(nDeviceIndex, eFrameType, ref pnWidth, ref pnHeight, ref pnFrameBufferSize);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000026CB File Offset: 0x000008CB
		public EVRTrackedCameraError GetCameraIntrinsics(uint nDeviceIndex, uint nCameraIndex, EVRTrackedCameraFrameType eFrameType, ref HmdVector2_t pFocalLength, ref HmdVector2_t pCenter)
		{
			return this.FnTable.GetCameraIntrinsics(nDeviceIndex, nCameraIndex, eFrameType, ref pFocalLength, ref pCenter);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000026E4 File Offset: 0x000008E4
		public EVRTrackedCameraError GetCameraProjection(uint nDeviceIndex, uint nCameraIndex, EVRTrackedCameraFrameType eFrameType, float flZNear, float flZFar, ref HmdMatrix44_t pProjection)
		{
			return this.FnTable.GetCameraProjection(nDeviceIndex, nCameraIndex, eFrameType, flZNear, flZFar, ref pProjection);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000026FF File Offset: 0x000008FF
		public EVRTrackedCameraError AcquireVideoStreamingService(uint nDeviceIndex, ref ulong pHandle)
		{
			pHandle = 0UL;
			return this.FnTable.AcquireVideoStreamingService(nDeviceIndex, ref pHandle);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002717 File Offset: 0x00000917
		public EVRTrackedCameraError ReleaseVideoStreamingService(ulong hTrackedCamera)
		{
			return this.FnTable.ReleaseVideoStreamingService(hTrackedCamera);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000272A File Offset: 0x0000092A
		public EVRTrackedCameraError GetVideoStreamFrameBuffer(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, IntPtr pFrameBuffer, uint nFrameBufferSize, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize)
		{
			return this.FnTable.GetVideoStreamFrameBuffer(hTrackedCamera, eFrameType, pFrameBuffer, nFrameBufferSize, ref pFrameHeader, nFrameHeaderSize);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002745 File Offset: 0x00000945
		public EVRTrackedCameraError GetVideoStreamTextureSize(uint nDeviceIndex, EVRTrackedCameraFrameType eFrameType, ref VRTextureBounds_t pTextureBounds, ref uint pnWidth, ref uint pnHeight)
		{
			pnWidth = 0U;
			pnHeight = 0U;
			return this.FnTable.GetVideoStreamTextureSize(nDeviceIndex, eFrameType, ref pTextureBounds, ref pnWidth, ref pnHeight);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002766 File Offset: 0x00000966
		public EVRTrackedCameraError GetVideoStreamTextureD3D11(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, IntPtr pD3D11DeviceOrResource, ref IntPtr ppD3D11ShaderResourceView, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize)
		{
			return this.FnTable.GetVideoStreamTextureD3D11(hTrackedCamera, eFrameType, pD3D11DeviceOrResource, ref ppD3D11ShaderResourceView, ref pFrameHeader, nFrameHeaderSize);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002781 File Offset: 0x00000981
		public EVRTrackedCameraError GetVideoStreamTextureGL(ulong hTrackedCamera, EVRTrackedCameraFrameType eFrameType, ref uint pglTextureId, ref CameraVideoStreamFrameHeader_t pFrameHeader, uint nFrameHeaderSize)
		{
			pglTextureId = 0U;
			return this.FnTable.GetVideoStreamTextureGL(hTrackedCamera, eFrameType, ref pglTextureId, ref pFrameHeader, nFrameHeaderSize);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000279D File Offset: 0x0000099D
		public EVRTrackedCameraError ReleaseVideoStreamTextureGL(ulong hTrackedCamera, uint glTextureId)
		{
			return this.FnTable.ReleaseVideoStreamTextureGL(hTrackedCamera, glTextureId);
		}

		// Token: 0x04000149 RID: 329
		private IVRTrackedCamera FnTable;
	}
}
