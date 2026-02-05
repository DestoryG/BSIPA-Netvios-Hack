using System;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000D9 RID: 217
	public abstract class OVRInput : OVRAction
	{
		// Token: 0x060001EF RID: 495 RVA: 0x000060FF File Offset: 0x000042FF
		protected OVRInput(string name)
			: base(name)
		{
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001F0 RID: 496
		public abstract bool isActive { get; }

		// Token: 0x060001F1 RID: 497
		internal abstract void UpdateData();
	}
}
