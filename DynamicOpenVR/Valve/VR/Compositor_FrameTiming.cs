using System;

namespace Valve.VR
{
	// Token: 0x020000AE RID: 174
	public struct Compositor_FrameTiming
	{
		// Token: 0x04000668 RID: 1640
		public uint m_nSize;

		// Token: 0x04000669 RID: 1641
		public uint m_nFrameIndex;

		// Token: 0x0400066A RID: 1642
		public uint m_nNumFramePresents;

		// Token: 0x0400066B RID: 1643
		public uint m_nNumMisPresented;

		// Token: 0x0400066C RID: 1644
		public uint m_nNumDroppedFrames;

		// Token: 0x0400066D RID: 1645
		public uint m_nReprojectionFlags;

		// Token: 0x0400066E RID: 1646
		public double m_flSystemTimeInSeconds;

		// Token: 0x0400066F RID: 1647
		public float m_flPreSubmitGpuMs;

		// Token: 0x04000670 RID: 1648
		public float m_flPostSubmitGpuMs;

		// Token: 0x04000671 RID: 1649
		public float m_flTotalRenderGpuMs;

		// Token: 0x04000672 RID: 1650
		public float m_flCompositorRenderGpuMs;

		// Token: 0x04000673 RID: 1651
		public float m_flCompositorRenderCpuMs;

		// Token: 0x04000674 RID: 1652
		public float m_flCompositorIdleCpuMs;

		// Token: 0x04000675 RID: 1653
		public float m_flClientFrameIntervalMs;

		// Token: 0x04000676 RID: 1654
		public float m_flPresentCallCpuMs;

		// Token: 0x04000677 RID: 1655
		public float m_flWaitForPresentCpuMs;

		// Token: 0x04000678 RID: 1656
		public float m_flSubmitFrameMs;

		// Token: 0x04000679 RID: 1657
		public float m_flWaitGetPosesCalledMs;

		// Token: 0x0400067A RID: 1658
		public float m_flNewPosesReadyMs;

		// Token: 0x0400067B RID: 1659
		public float m_flNewFrameReadyMs;

		// Token: 0x0400067C RID: 1660
		public float m_flCompositorUpdateStartMs;

		// Token: 0x0400067D RID: 1661
		public float m_flCompositorUpdateEndMs;

		// Token: 0x0400067E RID: 1662
		public float m_flCompositorRenderStartMs;

		// Token: 0x0400067F RID: 1663
		public TrackedDevicePose_t m_HmdPose;

		// Token: 0x04000680 RID: 1664
		public uint m_nNumVSyncsReadyForUse;

		// Token: 0x04000681 RID: 1665
		public uint m_nNumVSyncsToFirstView;
	}
}
