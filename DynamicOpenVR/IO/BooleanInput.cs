using System;
using Valve.VR;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000D7 RID: 215
	public class BooleanInput : OVRInput
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x00006073 File Offset: 0x00004273
		public BooleanInput(string name)
			: base(name)
		{
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000607C File Offset: 0x0000427C
		public override bool isActive
		{
			get
			{
				return this._actionData.bActive;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00006089 File Offset: 0x00004289
		public bool state
		{
			get
			{
				return this._actionData.bState;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00006096 File Offset: 0x00004296
		public bool activeChange
		{
			get
			{
				return this._actionData.bState && this._actionData.bChanged;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001EB RID: 491 RVA: 0x000060B2 File Offset: 0x000042B2
		public bool inactiveChange
		{
			get
			{
				return !this._actionData.bState && this._actionData.bChanged;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000060CE File Offset: 0x000042CE
		internal override void UpdateData()
		{
			this._actionData = OpenVRWrapper.GetDigitalActionData(base.handle);
		}

		// Token: 0x04000880 RID: 2176
		private InputDigitalActionData_t _actionData;
	}
}
