using System;
using UnityEngine;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000DF RID: 223
	public class Vector3Input : AnalogInput
	{
		// Token: 0x0600021D RID: 541 RVA: 0x00006646 File Offset: 0x00004846
		public Vector3Input(string name)
			: base(name)
		{
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000664F File Offset: 0x0000484F
		public Vector3 vector
		{
			get
			{
				return new Vector3(this._actionData.x, this._actionData.y, this._actionData.z);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00006677 File Offset: 0x00004877
		public Vector3 delta
		{
			get
			{
				return new Vector3(this._actionData.deltaX, this._actionData.deltaY, this._actionData.deltaZ);
			}
		}
	}
}
