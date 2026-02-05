using System;
using BeatSaberMarkupLanguage.Components.Settings;

namespace BeatSaberMarkupLanguage.Tags.Settings
{
	// Token: 0x02000067 RID: 103
	public class ListSettingTag : IncDecSettingTag<ListSetting>
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000B617 File Offset: 0x00009817
		public override string[] Aliases
		{
			get
			{
				return new string[] { "list-setting" };
			}
		}
	}
}
