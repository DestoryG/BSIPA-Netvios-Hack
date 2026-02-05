using System;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000E0 RID: 224
	public class VectorInput : AnalogInput
	{
		// Token: 0x06000220 RID: 544 RVA: 0x0000669F File Offset: 0x0000489F
		public VectorInput(string name)
			: base(name)
		{
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000221 RID: 545 RVA: 0x000066A8 File Offset: 0x000048A8
		public float value
		{
			get
			{
				return this._actionData.x;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000222 RID: 546 RVA: 0x000066B5 File Offset: 0x000048B5
		public float delta
		{
			get
			{
				return this._actionData.deltaX;
			}
		}
	}
}
