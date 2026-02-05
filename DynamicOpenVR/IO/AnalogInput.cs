using System;
using Valve.VR;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000D6 RID: 214
	public abstract class AnalogInput : OVRInput
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x0000604A File Offset: 0x0000424A
		protected AnalogInput(string name)
			: base(name)
		{
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00006053 File Offset: 0x00004253
		public override bool isActive
		{
			get
			{
				return this._actionData.bActive;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00006060 File Offset: 0x00004260
		internal override void UpdateData()
		{
			this._actionData = OpenVRWrapper.GetAnalogActionData(base.handle);
		}

		// Token: 0x0400087F RID: 2175
		protected InputAnalogActionData_t _actionData;
	}
}
