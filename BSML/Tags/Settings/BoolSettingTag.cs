using System;
using BeatSaberMarkupLanguage.Components.Settings;

namespace BeatSaberMarkupLanguage.Tags.Settings
{
	// Token: 0x02000060 RID: 96
	public class BoolSettingTag : IncDecSettingTag<BoolSetting>
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000AD9C File Offset: 0x00008F9C
		public override string[] Aliases
		{
			get
			{
				return new string[] { "bool-setting" };
			}
		}
	}
}
