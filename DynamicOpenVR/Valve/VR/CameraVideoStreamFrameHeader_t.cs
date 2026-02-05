using System;

namespace Valve.VR
{
	// Token: 0x020000AA RID: 170
	public struct CameraVideoStreamFrameHeader_t
	{
		// Token: 0x04000656 RID: 1622
		public EVRTrackedCameraFrameType eFrameType;

		// Token: 0x04000657 RID: 1623
		public uint nWidth;

		// Token: 0x04000658 RID: 1624
		public uint nHeight;

		// Token: 0x04000659 RID: 1625
		public uint nBytesPerPixel;

		// Token: 0x0400065A RID: 1626
		public uint nFrameSequence;

		// Token: 0x0400065B RID: 1627
		public TrackedDevicePose_t standingTrackedDevicePose;

		// Token: 0x0400065C RID: 1628
		public ulong ulFrameExposureTime;
	}
}
