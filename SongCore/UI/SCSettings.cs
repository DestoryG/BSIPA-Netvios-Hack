using System;
using BeatSaberMarkupLanguage.Attributes;

namespace SongCore.UI
{
	// Token: 0x0200001D RID: 29
	public class SCSettings : PersistentSingleton<SCSettings>
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00007587 File Offset: 0x00005787
		// (set) Token: 0x06000178 RID: 376 RVA: 0x0000759F File Offset: 0x0000579F
		[UIValue("colors")]
		public bool Colors
		{
			get
			{
				return BasicUI.ModPrefs.GetBool("SongCore", "customSongColors", true, true);
			}
			set
			{
				Plugin.customSongColors = value;
				BasicUI.ModPrefs.SetBool("SongCore", "customSongColors", value);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000075BC File Offset: 0x000057BC
		// (set) Token: 0x0600017A RID: 378 RVA: 0x000075D4 File Offset: 0x000057D4
		[UIValue("platforms")]
		public bool Platforms
		{
			get
			{
				return BasicUI.ModPrefs.GetBool("SongCore", "customSongPlatforms", true, true);
			}
			set
			{
				Plugin.customSongPlatforms = value;
				BasicUI.ModPrefs.SetBool("SongCore", "customSongPlatforms", value);
			}
		}
	}
}
