using System;
using BeatSaberMarkupLanguage.Attributes;

namespace BeatSaverDownloader.UI
{
	// Token: 0x02000012 RID: 18
	public class Settings : PersistentSingleton<Settings>
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00002830 File Offset: 0x00000A30
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00002053 File Offset: 0x00000253
		[UIValue("toggle1")]
		public bool toggle1
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00002053 File Offset: 0x00000253
		public static void SetupSettings()
		{
		}
	}
}
