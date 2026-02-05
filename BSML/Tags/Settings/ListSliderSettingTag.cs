using System;
using BeatSaberMarkupLanguage.Components.Settings;

namespace BeatSaberMarkupLanguage.Tags.Settings
{
	// Token: 0x02000068 RID: 104
	public class ListSliderSettingTag : GenericSliderSettingTag<ListSliderSetting>
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000B62F File Offset: 0x0000982F
		public override string[] Aliases
		{
			get
			{
				return new string[] { "list-slider-setting" };
			}
		}
	}
}
