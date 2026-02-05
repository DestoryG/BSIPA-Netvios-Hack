using System;
using Valve.VR;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000DC RID: 220
	public class SkeletalInput : OVRInput
	{
		// Token: 0x0600020C RID: 524 RVA: 0x000064F2 File Offset: 0x000046F2
		public SkeletalInput(string name)
			: base(name)
		{
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600020D RID: 525 RVA: 0x000064FB File Offset: 0x000046FB
		public override bool isActive
		{
			get
			{
				return this._actionData.bActive;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00006508 File Offset: 0x00004708
		public SkeletalSummaryData summaryData
		{
			get
			{
				return new SkeletalSummaryData(this._summaryData);
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00006515 File Offset: 0x00004715
		internal override void UpdateData()
		{
			this._actionData = OpenVRWrapper.GetSkeletalActionData(base.handle);
			this._summaryData = OpenVRWrapper.GetSkeletalSummaryData(base.handle);
		}

		// Token: 0x04000886 RID: 2182
		private InputSkeletalActionData_t _actionData;

		// Token: 0x04000887 RID: 2183
		private VRSkeletalSummaryData_t _summaryData;
	}
}
