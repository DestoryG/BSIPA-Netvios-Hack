using System;

namespace CustomAvatar.Tracking
{
	// Token: 0x0200002A RID: 42
	internal class TrackedDeviceTypeAttribute : Attribute
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000059A5 File Offset: 0x00003BA5
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x000059AD File Offset: 0x00003BAD
		public string Name { get; set; }

		// Token: 0x060000B8 RID: 184 RVA: 0x000059B6 File Offset: 0x00003BB6
		public TrackedDeviceTypeAttribute(string name)
		{
			this.Name = name;
		}
	}
}
