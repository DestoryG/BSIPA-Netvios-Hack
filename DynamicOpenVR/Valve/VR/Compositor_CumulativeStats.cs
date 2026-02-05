using System;

namespace Valve.VR
{
	// Token: 0x020000AF RID: 175
	public struct Compositor_CumulativeStats
	{
		// Token: 0x04000682 RID: 1666
		public uint m_nPid;

		// Token: 0x04000683 RID: 1667
		public uint m_nNumFramePresents;

		// Token: 0x04000684 RID: 1668
		public uint m_nNumDroppedFrames;

		// Token: 0x04000685 RID: 1669
		public uint m_nNumReprojectedFrames;

		// Token: 0x04000686 RID: 1670
		public uint m_nNumFramePresentsOnStartup;

		// Token: 0x04000687 RID: 1671
		public uint m_nNumDroppedFramesOnStartup;

		// Token: 0x04000688 RID: 1672
		public uint m_nNumReprojectedFramesOnStartup;

		// Token: 0x04000689 RID: 1673
		public uint m_nNumLoading;

		// Token: 0x0400068A RID: 1674
		public uint m_nNumFramePresentsLoading;

		// Token: 0x0400068B RID: 1675
		public uint m_nNumDroppedFramesLoading;

		// Token: 0x0400068C RID: 1676
		public uint m_nNumReprojectedFramesLoading;

		// Token: 0x0400068D RID: 1677
		public uint m_nNumTimedOut;

		// Token: 0x0400068E RID: 1678
		public uint m_nNumFramePresentsTimedOut;

		// Token: 0x0400068F RID: 1679
		public uint m_nNumDroppedFramesTimedOut;

		// Token: 0x04000690 RID: 1680
		public uint m_nNumReprojectedFramesTimedOut;
	}
}
