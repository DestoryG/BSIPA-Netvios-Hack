using System;
using System.Reflection;
using BeatSaberMultiplayer.Configuration;
using BeatSaberMultiplayer.Helper;
using BeatSaberMultiplayer.UI;
using BeatSaberMultiplayer.UI.ViewControllers.InGame;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Loader;
using IPA.Logging;
using IPA.Netvios;
using UnityEngine;

namespace BeatSaberMultiplayer
{
	// Token: 0x0200004E RID: 78
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0001BB31 File Offset: 0x00019D31
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x0001BB38 File Offset: 0x00019D38
		internal static Plugin instance { get; private set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x0001BB40 File Offset: 0x00019D40
		internal static string Name
		{
			get
			{
				return "BeatSaberMultiplayer";
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0001BB47 File Offset: 0x00019D47
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x0001BB4E File Offset: 0x00019D4E
		public static bool DownloaderExists { get; private set; }

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001BB58 File Offset: 0x00019D58
		[Init]
		public void Init(global::IPA.Logging.Logger logger)
		{
			Plugin.instance = this;
			BeatSaberMultiplayer.Logger.log = logger;
			BeatSaberMultiplayer.Logger.log.Debug("Logger initialized.");
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
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001BBAC File Offset: 0x00019DAC
		[Init]
		public void InitWithConfig(global::IPA.Config.Config conf)
		{
			PluginConfig.Instance = conf.Generated(true);
			BeatSaberMultiplayer.Logger.log.Debug("Config loaded");
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001BBCC File Offset: 0x00019DCC
		[OnStart]
		public void OnApplicationStart()
		{
			BeatSaberMultiplayer.Logger.log.Debug("OnApplicationStart");
			this._harmony = new Harmony("com.netvios.beatsaber.multiplayer");
			try
			{
				this._harmony.PatchAll(Assembly.GetExecutingAssembly());
			}
			catch (Exception ex)
			{
				BeatSaberMultiplayer.Logger.log.Error("Unable to patch assembly! Exception: " + ex.Message);
			}
			if (PluginManager.GetPluginFromId("BeatSaverDownloader") != null)
			{
				Plugin.DownloaderExists = true;
			}
			BSEvents.OnLoad();
			BSEvents.lateMenuSceneLoadedFresh += this.MenuSceneLoadedFresh;
			BSEvents.menuSceneLoaded += this.MenuSceneLoaded;
			BSEvents.gameSceneLoaded += this.GameSceneLoaded;
			Sprites.ConvertSprites();
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001BC88 File Offset: 0x00019E88
		private void MenuSceneLoadedFresh(ScenesTransitionSetupDataSO scenes)
		{
			PluginUI.OnLoad();
			InGameController.OnLoad();
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001BC94 File Offset: 0x00019E94
		private void MenuSceneLoaded()
		{
			InGameController instance = InGameController.instance;
			if (instance == null)
			{
				return;
			}
			instance.MenuSceneLoaded();
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001BCA5 File Offset: 0x00019EA5
		private void GameSceneLoaded()
		{
			InGameController instance = InGameController.instance;
			if (instance == null)
			{
				return;
			}
			instance.GameSceneLoaded();
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001BCB8 File Offset: 0x00019EB8
		[OnExit]
		public void OnApplicationQuit()
		{
			BSEvents.lateMenuSceneLoadedFresh -= this.MenuSceneLoadedFresh;
			BSEvents.menuSceneLoaded -= this.MenuSceneLoaded;
			BSEvents.gameSceneLoaded -= this.GameSceneLoaded;
			BeatSaberMultiplayer.Logger.log.Debug("OnApplicationQuit");
		}

		// Token: 0x0400032B RID: 811
		private Harmony _harmony;
	}
}
