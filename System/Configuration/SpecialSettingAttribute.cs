using System;

namespace System.Configuration
{
	// Token: 0x020000A3 RID: 163
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class SpecialSettingAttribute : Attribute
	{
		// Token: 0x060005A9 RID: 1449 RVA: 0x00022AD4 File Offset: 0x00020CD4
		public SpecialSettingAttribute(SpecialSetting specialSetting)
		{
			this._specialSetting = specialSetting;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00022AE3 File Offset: 0x00020CE3
		public SpecialSetting SpecialSetting
		{
			get
			{
				return this._specialSetting;
			}
		}

		// Token: 0x04000C3E RID: 3134
		private readonly SpecialSetting _specialSetting;
	}
}
