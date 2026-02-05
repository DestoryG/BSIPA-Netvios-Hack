using System;
using BS_Utils.Utilities;
using SongCore.Utilities;
using UnityEngine;

namespace SongCore.UI
{
	// Token: 0x0200001B RID: 27
	internal static class BasicUI
	{
		// Token: 0x0600016C RID: 364 RVA: 0x00006F20 File Offset: 0x00005120
		internal static void GetIcons()
		{
			if (!BasicUI.MissingCharIcon)
			{
				BasicUI.MissingCharIcon = Utils.LoadSpriteFromResources("SongCore.Icons.MissingChar.png", 100f);
			}
			if (!BasicUI.LightshowIcon)
			{
				BasicUI.LightshowIcon = Utils.LoadSpriteFromResources("SongCore.Icons.Lightshow.png", 100f);
			}
			if (!BasicUI.ExtraDiffsIcon)
			{
				BasicUI.ExtraDiffsIcon = Utils.LoadSpriteFromResources("SongCore.Icons.ExtraDiffsIcon.png", 100f);
			}
			if (!BasicUI.WIPIcon)
			{
				BasicUI.WIPIcon = Utils.LoadSpriteFromResources("SongCore.Icons.squek.png", 100f);
			}
			if (!BasicUI.FolderIcon)
			{
				BasicUI.FolderIcon = Utils.LoadSpriteFromResources("SongCore.Icons.FolderIcon.png", 100f);
			}
			if (!BasicUI.DownloadIcon)
			{
				BasicUI.DownloadIcon = Utils.LoadSpriteFromResources("SongCore.Icons.Download.png", 100f);
			}
		}

		// Token: 0x04000079 RID: 121
		internal static Config ModPrefs = new Config("SongCore/SongCore");

		// Token: 0x0400007A RID: 122
		internal static Sprite MissingCharIcon;

		// Token: 0x0400007B RID: 123
		internal static Sprite LightshowIcon;

		// Token: 0x0400007C RID: 124
		internal static Sprite ExtraDiffsIcon;

		// Token: 0x0400007D RID: 125
		internal static Sprite WIPIcon;

		// Token: 0x0400007E RID: 126
		internal static Sprite FolderIcon;

		// Token: 0x0400007F RID: 127
		internal static Sprite DownloadIcon;
	}
}
