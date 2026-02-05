using System;
using System.IO;
using BS_Utils.Utilities;
using CustomSaber.Settings;
using CustomSaber.Settings.UI;
using CustomSaber.Utilities;
using IPA;
using IPA.Config;
using IPA.Loader;
using IPA.Logging;
using IPA.Utilities;
using SemVer;

namespace CustomSaber
{
	// Token: 0x0200000B RID: 11
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000025D9 File Offset: 0x000007D9
		public static string PluginName
		{
			get
			{
				return "Custom Sabers";
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000025E0 File Offset: 0x000007E0
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000025E7 File Offset: 0x000007E7
		public static global::SemVer.Version PluginVersion { get; private set; } = new global::SemVer.Version("0.0.0", false);

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000025EF File Offset: 0x000007EF
		public static string PluginAssetPath
		{
			get
			{
				return Path.Combine(UnityGame.InstallPath, "CustomSabers");
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002600 File Offset: 0x00000800
		[Init]
		public void Init(global::IPA.Logging.Logger logger, global::IPA.Config.Config config, PluginMetadata metadata)
		{
			CustomSaber.Logger.log = logger;
			Configuration.Init(config);
			bool flag = metadata != null;
			if (flag)
			{
				Plugin.PluginVersion = metadata.Version;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002632 File Offset: 0x00000832
		[OnStart]
		public void OnApplicationStart()
		{
			this.Load();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000263B File Offset: 0x0000083B
		[OnExit]
		public void OnApplicationQuit()
		{
			this.Unload();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002644 File Offset: 0x00000844
		private void OnGameSceneLoaded()
		{
			SaberScript.Load();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000264D File Offset: 0x0000084D
		private void Load()
		{
			Configuration.Load();
			SaberAssetLoader.Load();
			SettingsUI.CreateMenu();
			this.AddEvents();
			CustomSaber.Logger.log.Info(string.Format("{0} v.{1} has started.", Plugin.PluginName, Plugin.PluginVersion));
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002688 File Offset: 0x00000888
		private void Unload()
		{
			Configuration.Save();
			SaberAssetLoader.Clear();
			this.RemoveEvents();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000269E File Offset: 0x0000089E
		private void AddEvents()
		{
			this.RemoveEvents();
			BSEvents.gameSceneLoaded += this.OnGameSceneLoaded;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000026BA File Offset: 0x000008BA
		private void RemoveEvents()
		{
			BSEvents.gameSceneLoaded -= this.OnGameSceneLoaded;
		}
	}
}
