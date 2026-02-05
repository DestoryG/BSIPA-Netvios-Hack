using System;

namespace Valve.VR
{
	// Token: 0x020000AB RID: 171
	public struct DriverDirectMode_FrameTiming
	{
		// Token: 0x0400065D RID: 1629
		public uint m_nSize;

		// Token: 0x0400065E RID: 1630
		public uint m_nNumFramePresents;

		// Token: 0x0400065F RID: 1631
		public uint m_nNumMisPresented;

		// Token: 0x04000660 RID: 1632
		public uint m_nNumDroppedFrames;

		// Token: 0x04000661 RID: 1633
		public uint m_nReprojectionFlags;
	}
}
