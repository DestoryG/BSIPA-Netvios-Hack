using System;
using BeatSaberMarkupLanguage.Components.Settings;

namespace BeatSaberMarkupLanguage.Tags.Settings
{
	// Token: 0x02000069 RID: 105
	public class SliderSettingTag : GenericSliderSettingTag<SliderSetting>
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000B647 File Offset: 0x00009847
		public override string[] Aliases
		{
			get
			{
				return new string[] { "slider-setting" };
			}
		}
	}
}
