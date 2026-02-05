using System;
using Valve.VR;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000DD RID: 221
	public class SkeletalSummaryData
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00006539 File Offset: 0x00004739
		public float thumbCurl { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00006541 File Offset: 0x00004741
		public float indexCurl { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00006549 File Offset: 0x00004749
		public float middleCurl { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00006551 File Offset: 0x00004751
		public float ringCurl { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00006559 File Offset: 0x00004759
		public float littleCurl { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00006561 File Offset: 0x00004761
		public float thumbIndexSplay { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00006569 File Offset: 0x00004769
		public float indexMiddleSplay { get; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00006571 File Offset: 0x00004771
		public float middleRingSplay { get; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00006579 File Offset: 0x00004779
		public float ringLittleSplay { get; }

		// Token: 0x06000219 RID: 537 RVA: 0x00006584 File Offset: 0x00004784
		internal SkeletalSummaryData(VRSkeletalSummaryData_t summaryDataStruct)
		{
			this.thumbCurl = summaryDataStruct.flFingerCurl0;
			this.indexCurl = summaryDataStruct.flFingerCurl1;
			this.middleCurl = summaryDataStruct.flFingerCurl2;
			this.ringCurl = summaryDataStruct.flFingerCurl3;
			this.littleCurl = summaryDataStruct.flFingerCurl4;
			this.thumbIndexSplay = summaryDataStruct.flFingerSplay0;
			this.indexMiddleSplay = summaryDataStruct.flFingerSplay1;
			this.middleRingSplay = summaryDataStruct.flFingerSplay2;
			this.ringLittleSplay = summaryDataStruct.flFingerSplay3;
		}
	}
}
