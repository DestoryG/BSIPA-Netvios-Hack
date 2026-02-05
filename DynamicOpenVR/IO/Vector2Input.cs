using System;
using UnityEngine;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000DE RID: 222
	public class Vector2Input : AnalogInput
	{
		// Token: 0x0600021A RID: 538 RVA: 0x00006603 File Offset: 0x00004803
		public Vector2Input(string name)
			: base(name)
		{
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000660C File Offset: 0x0000480C
		public Vector2 vector
		{
			get
			{
				return new Vector2(this._actionData.x, this._actionData.y);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00006629 File Offset: 0x00004829
		public Vector2 delta
		{
			get
			{
				return new Vector2(this._actionData.deltaX, this._actionData.deltaY);
			}
		}
	}
}
