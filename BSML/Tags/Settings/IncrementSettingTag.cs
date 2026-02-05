using System;
using BeatSaberMarkupLanguage.Components.Settings;

namespace BeatSaberMarkupLanguage.Tags.Settings
{
	// Token: 0x02000066 RID: 102
	public class IncrementSettingTag : IncDecSettingTag<IncrementSetting>
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000B5FF File Offset: 0x000097FF
		public override string[] Aliases
		{
			get
			{
				return new string[] { "increment-setting" };
			}
		}
	}
}
