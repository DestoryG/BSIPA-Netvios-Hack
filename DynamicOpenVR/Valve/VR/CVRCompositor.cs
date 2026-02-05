using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000019 RID: 25
	public class CVRCompositor
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00002D14 File Offset: 0x00000F14
		internal CVRCompositor(IntPtr pInterface)
		{
			this.FnTable = (IVRCompositor)Marshal.PtrToStructure(pInterface, typeof(IVRCompositor));
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002D37 File Offset: 0x00000F37
		public void SetTrackingSpace(ETrackingUniverseOrigin eOrigin)
		{
			this.FnTable.SetTrackingSpace(eOrigin);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002D4A File Offset: 0x00000F4A
		public ETrackingUniverseOrigin GetTrackingSpace()
		{
			return this.FnTable.GetTrackingSpace();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002D5C File Offset: 0x00000F5C
		public EVRCompositorError WaitGetPoses(TrackedDevicePose_t[] pRenderPoseArray, TrackedDevicePose_t[] pGamePoseArray)
		{
			return this.FnTable.WaitGetPoses(pRenderPoseArray, (uint)pRenderPoseArray.Length, pGamePoseArray, (uint)pGamePoseArray.Length);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002D76 File Offset: 0x00000F76
		public EVRCompositorError GetLastPoses(TrackedDevicePose_t[] pRenderPoseArray, TrackedDevicePose_t[] pGamePoseArray)
		{
			return this.FnTable.GetLastPoses(pRenderPoseArray, (uint)pRenderPoseArray.Length, pGamePoseArray, (uint)pGamePoseArray.Length);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002D90 File Offset: 0x00000F90
		public EVRCompositorError GetLastPoseForTrackedDeviceIndex(uint unDeviceIndex, ref TrackedDevicePose_t pOutputPose, ref TrackedDevicePose_t pOutputGamePose)
		{
			return this.FnTable.GetLastPoseForTrackedDeviceIndex(unDeviceIndex, ref pOutputPose, ref pOutputGamePose);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002DA5 File Offset: 0x00000FA5
		public EVRCompositorError Submit(EVREye eEye, ref Texture_t pTexture, ref VRTextureBounds_t pBounds, EVRSubmitFlags nSubmitFlags)
		{
			return this.FnTable.Submit(eEye, ref pTexture, ref pBounds, nSubmitFlags);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002DBC File Offset: 0x00000FBC
		public void ClearLastSubmittedFrame()
		{
			this.FnTable.ClearLastSubmittedFrame();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002DCE File Offset: 0x00000FCE
		public void PostPresentHandoff()
		{
			this.FnTable.PostPresentHandoff();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public bool GetFrameTiming(ref Compositor_FrameTiming pTiming, uint unFramesAgo)
		{
			return this.FnTable.GetFrameTiming(ref pTiming, unFramesAgo);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002DF4 File Offset: 0x00000FF4
		public uint GetFrameTimings(Compositor_FrameTiming[] pTiming)
		{
			return this.FnTable.GetFrameTimings(pTiming, (uint)pTiming.Length);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002E0A File Offset: 0x0000100A
		public float GetFrameTimeRemaining()
		{
			return this.FnTable.GetFrameTimeRemaining();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002E1C File Offset: 0x0000101C
		public void GetCumulativeStats(ref Compositor_CumulativeStats pStats, uint nStatsSizeInBytes)
		{
			this.FnTable.GetCumulativeStats(ref pStats, nStatsSizeInBytes);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002E30 File Offset: 0x00001030
		public void FadeToColor(float fSeconds, float fRed, float fGreen, float fBlue, float fAlpha, bool bBackground)
		{
			this.FnTable.FadeToColor(fSeconds, fRed, fGreen, fBlue, fAlpha, bBackground);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002E4B File Offset: 0x0000104B
		public HmdColor_t GetCurrentFadeColor(bool bBackground)
		{
			return this.FnTable.GetCurrentFadeColor(bBackground);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002E5E File Offset: 0x0000105E
		public void FadeGrid(float fSeconds, bool bFadeIn)
		{
			this.FnTable.FadeGrid(fSeconds, bFadeIn);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002E72 File Offset: 0x00001072
		public float GetCurrentGridAlpha()
		{
			return this.FnTable.GetCurrentGridAlpha();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002E84 File Offset: 0x00001084
		public EVRCompositorError SetSkyboxOverride(Texture_t[] pTextures)
		{
			return this.FnTable.SetSkyboxOverride(pTextures, (uint)pTextures.Length);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00002E9A File Offset: 0x0000109A
		public void ClearSkyboxOverride()
		{
			this.FnTable.ClearSkyboxOverride();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00002EAC File Offset: 0x000010AC
		public void CompositorBringToFront()
		{
			this.FnTable.CompositorBringToFront();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00002EBE File Offset: 0x000010BE
		public void CompositorGoToBack()
		{
			this.FnTable.CompositorGoToBack();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00002ED0 File Offset: 0x000010D0
		public void CompositorQuit()
		{
			this.FnTable.CompositorQuit();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002EE2 File Offset: 0x000010E2
		public bool IsFullscreen()
		{
			return this.FnTable.IsFullscreen();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002EF4 File Offset: 0x000010F4
		public uint GetCurrentSceneFocusProcess()
		{
			return this.FnTable.GetCurrentSceneFocusProcess();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002F06 File Offset: 0x00001106
		public uint GetLastFrameRenderer()
		{
			return this.FnTable.GetLastFrameRenderer();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002F18 File Offset: 0x00001118
		public bool CanRenderScene()
		{
			return this.FnTable.CanRenderScene();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002F2A File Offset: 0x0000112A
		public void ShowMirrorWindow()
		{
			this.FnTable.ShowMirrorWindow();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002F3C File Offset: 0x0000113C
		public void HideMirrorWindow()
		{
			this.FnTable.HideMirrorWindow();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002F4E File Offset: 0x0000114E
		public bool IsMirrorWindowVisible()
		{
			return this.FnTable.IsMirrorWindowVisible();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002F60 File Offset: 0x00001160
		public void CompositorDumpImages()
		{
			this.FnTable.CompositorDumpImages();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00002F72 File Offset: 0x00001172
		public bool ShouldAppRenderWithLowResources()
		{
			return this.FnTable.ShouldAppRenderWithLowResources();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002F84 File Offset: 0x00001184
		public void ForceInterleavedReprojectionOn(bool bOverride)
		{
			this.FnTable.ForceInterleavedReprojectionOn(bOverride);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002F97 File Offset: 0x00001197
		public void ForceReconnectProcess()
		{
			this.FnTable.ForceReconnectProcess();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002FA9 File Offset: 0x000011A9
		public void SuspendRendering(bool bSuspend)
		{
			this.FnTable.SuspendRendering(bSuspend);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00002FBC File Offset: 0x000011BC
		public EVRCompositorError GetMirrorTextureD3D11(EVREye eEye, IntPtr pD3D11DeviceOrResource, ref IntPtr ppD3D11ShaderResourceView)
		{
			return this.FnTable.GetMirrorTextureD3D11(eEye, pD3D11DeviceOrResource, ref ppD3D11ShaderResourceView);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00002FD1 File Offset: 0x000011D1
		public void ReleaseMirrorTextureD3D11(IntPtr pD3D11ShaderResourceView)
		{
			this.FnTable.ReleaseMirrorTextureD3D11(pD3D11ShaderResourceView);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00002FE4 File Offset: 0x000011E4
		public EVRCompositorError GetMirrorTextureGL(EVREye eEye, ref uint pglTextureId, IntPtr pglSharedTextureHandle)
		{
			pglTextureId = 0U;
			return this.FnTable.GetMirrorTextureGL(eEye, ref pglTextureId, pglSharedTextureHandle);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00002FFC File Offset: 0x000011FC
		public bool ReleaseSharedGLTexture(uint glTextureId, IntPtr glSharedTextureHandle)
		{
			return this.FnTable.ReleaseSharedGLTexture(glTextureId, glSharedTextureHandle);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003010 File Offset: 0x00001210
		public void LockGLSharedTextureForAccess(IntPtr glSharedTextureHandle)
		{
			this.FnTable.LockGLSharedTextureForAccess(glSharedTextureHandle);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003023 File Offset: 0x00001223
		public void UnlockGLSharedTextureForAccess(IntPtr glSharedTextureHandle)
		{
			this.FnTable.UnlockGLSharedTextureForAccess(glSharedTextureHandle);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003036 File Offset: 0x00001236
		public uint GetVulkanInstanceExtensionsRequired(StringBuilder pchValue, uint unBufferSize)
		{
			return this.FnTable.GetVulkanInstanceExtensionsRequired(pchValue, unBufferSize);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000304A File Offset: 0x0000124A
		public uint GetVulkanDeviceExtensionsRequired(IntPtr pPhysicalDevice, StringBuilder pchValue, uint unBufferSize)
		{
			return this.FnTable.GetVulkanDeviceExtensionsRequired(pPhysicalDevice, pchValue, unBufferSize);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000305F File Offset: 0x0000125F
		public void SetExplicitTimingMode(EVRCompositorTimingMode eTimingMode)
		{
			this.FnTable.SetExplicitTimingMode(eTimingMode);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003072 File Offset: 0x00001272
		public EVRCompositorError SubmitExplicitTimingData()
		{
			return this.FnTable.SubmitExplicitTimingData();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003084 File Offset: 0x00001284
		public bool IsMotionSmoothingEnabled()
		{
			return this.FnTable.IsMotionSmoothingEnabled();
		}

		// Token: 0x0400014D RID: 333
		private IVRCompositor FnTable;
	}
}
