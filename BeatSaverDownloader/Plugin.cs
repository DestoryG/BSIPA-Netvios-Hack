using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BeatSaverDownloader.Misc;
using BeatSaverDownloader.UI;
using BeatSaverSharp;
using BS_Utils.Gameplay;
using BS_Utils.Utilities;
using IPA;
using IPA.Logging;
using IPA.Netvios;
using IPA.Utilities;
using SongCore;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BeatSaverDownloader
{
	// Token: 0x0200000E RID: 14
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x000032FC File Offset: 0x000014FC
		[Init]
		public void Init(object nullObject, global::IPA.Logging.Logger logger)
		{
			try
			{
				if (!global::IPA.Netvios.Utils.CheckIPA())
				{
					Application.Quit();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Application.Quit();
			}
			Plugin.log = logger;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000333C File Offset: 0x0000153C
		[OnExit]
		public void OnApplicationQuit()
		{
			PluginConfig.SaveConfig();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003344 File Offset: 0x00001544
		[OnStart]
		public void OnApplicationStart()
		{
			string text = (File.Exists(Path.Combine(UnityGame.InstallPath, "Beat Saber_Data", "Plugins", "steam_api64.dll")) ? "steam" : "oculus");
			string text2 = UnityGame.GameVersion.ToString() + "-" + text;
			Plugin.BeatSaver = new BeatSaver(new HttpOptions
			{
				ApplicationName = "BeatSaverDownloader",
				Version = Assembly.GetExecutingAssembly().GetName().Version,
				Agents = new ApplicationAgent[]
				{
					new ApplicationAgent("BeatSaber", text2)
				}
			});
			Plugin.instance = this;
			PluginConfig.LoadConfig();
			Sprites.ConvertToSprites();
			PersistentSingleton<PluginUI>.instance.Setup();
			BSEvents.menuSceneLoadedFresh += this.OnMenuSceneLoadedFresh;
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003438 File Offset: 0x00001638
		private void OnMenuSceneLoadedFresh()
		{
			try
			{
				PluginUI.SetupLevelDetailClone();
				Settings.SetupSettings();
				Loader.SongsLoadedEvent += this.Loader_SongsLoadedEvent;
				GetUserInfo.GetUserName();
			}
			catch (Exception ex)
			{
				global::IPA.Logging.Logger logger = Plugin.log;
				string text = "Exception on fresh menu scene change: ";
				Exception ex2 = ex;
				logger.Critical(text + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000349C File Offset: 0x0000169C
		private void Loader_SongsLoadedEvent(Loader arg1, Dictionary<string, CustomPreviewBeatmapLevel> arg2)
		{
			if (!PersistentSingleton<PluginUI>.instance.moreSongsButton.Interactable)
			{
				PersistentSingleton<PluginUI>.instance.moreSongsButton.Interactable = true;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00002053 File Offset: 0x00000253
		public void OnUpdate()
		{
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00002053 File Offset: 0x00000253
		public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
		{
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00002053 File Offset: 0x00000253
		public void OnSceneUnloaded(Scene scene)
		{
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00002053 File Offset: 0x00000253
		public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
		{
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00002053 File Offset: 0x00000253
		public void OnFixedUpdate()
		{
		}

		// Token: 0x0400001F RID: 31
		public static Plugin instance;

		// Token: 0x04000020 RID: 32
		public static global::IPA.Logging.Logger log;

		// Token: 0x04000021 RID: 33
		public static BeatSaver BeatSaver;
	}
}
