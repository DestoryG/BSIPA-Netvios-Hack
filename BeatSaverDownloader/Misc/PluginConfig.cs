using System;
using System.IO;
using BS_Utils.Utilities;

namespace BeatSaverDownloader.Misc
{
	// Token: 0x02000023 RID: 35
	public class PluginConfig
	{
		// Token: 0x06000167 RID: 359 RVA: 0x0000670B File Offset: 0x0000490B
		public static void LoadConfig()
		{
			if (!Directory.Exists("UserData"))
			{
				Directory.CreateDirectory("UserData");
			}
			PluginConfig.Load();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006729 File Offset: 0x00004929
		public static void Load()
		{
			PluginConfig.maxSimultaneousDownloads = PluginConfig.config.GetInt("BeatSaverDownloader", "maxSimultaneousDownloads", 3, true);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006746 File Offset: 0x00004946
		public static void SaveConfig()
		{
			PluginConfig.config.SetInt("BeatSaverDownloader", "maxSimultaneousDownloads", PluginConfig.maxSimultaneousDownloads);
		}

		// Token: 0x0400009A RID: 154
		private static Config config = new Config("BeatSaverDownloader");

		// Token: 0x0400009B RID: 155
		public static int maxSimultaneousDownloads = 3;
	}
}
