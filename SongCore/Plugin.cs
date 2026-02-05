using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BS_Utils;
using BS_Utils.Gameplay;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA;
using IPA.Logging;
using IPA.Netvios;
using Microsoft.Win32;
using Newtonsoft.Json;
using SongCore.Data;
using SongCore.UI;
using SongCore.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace SongCore
{
	// Token: 0x02000013 RID: 19
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x000048A0 File Offset: 0x00002AA0
		[OnStart]
		public void OnApplicationStart()
		{
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
			try
			{
				if (File.Exists(Environment.CurrentDirectory + "/UserData/SongCore.ini"))
				{
					File.Delete(Environment.CurrentDirectory + "/UserData/SongCore.ini");
				}
			}
			catch
			{
				Logging.logger.Warn("Failed to delete old config file!");
			}
			Plugin.PlatformsInstalled = SongCore.Utilities.Utils.IsModInstalled("Custom Platforms");
			Plugin.harmony = new Harmony("com.kyle1413.BeatSaber.SongCore");
			Plugin.harmony.PatchAll(Assembly.GetExecutingAssembly());
			BasicUI.GetIcons();
			BSEvents.levelSelected += this.BSEvents_levelSelected;
			BSEvents.gameSceneLoaded += this.BSEvents_gameSceneLoaded;
			BSEvents.menuSceneLoadedFresh += this.BSEvents_menuSceneLoadedFresh;
			if (!File.Exists(Collections.dataPath))
			{
				File.Create(Collections.dataPath);
			}
			else
			{
				Collections.LoadExtraSongData();
			}
			Collections.RegisterCustomCharacteristic(BasicUI.MissingCharIcon, "Missing Characteristic", "Missing Characteristic", "MissingCharacteristic", "MissingCharacteristic", false, false, 1000);
			Collections.RegisterCustomCharacteristic(BasicUI.LightshowIcon, "Lightshow", "Lightshow", "Lightshow", "Lightshow", false, false, 100);
			Collections.RegisterCustomCharacteristic(BasicUI.ExtraDiffsIcon, "Lawless", "Lawless - Anything Goes", "Lawless", "Lawless", false, false, 101);
			if (!File.Exists(Environment.CurrentDirectory + "/UserData/SongCore/folders.xml"))
			{
				File.WriteAllBytes(Environment.CurrentDirectory + "/UserData/SongCore/folders.xml", SongCore.Utilities.Utils.GetResource(Assembly.GetExecutingAssembly(), "SongCore.Data.folders.xml"));
			}
			Loader.SeperateSongFolders.InsertRange(0, SeperateSongFolder.ReadSeperateFoldersFromFile(Environment.CurrentDirectory + "/UserData/SongCore/folders.xml"));
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004A64 File Offset: 0x00002C64
		private void BSEvents_menuSceneLoadedFresh()
		{
			Loader.OnLoad();
			PersistentSingleton<RequirementsUI>.instance.Setup();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004A75 File Offset: 0x00002C75
		private void BSEvents_gameSceneLoaded()
		{
			PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this.DelayedNoteJumpMovementSpeedFix());
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004A88 File Offset: 0x00002C88
		private void BSEvents_levelSelected(LevelCollectionViewController arg1, IPreviewBeatmapLevel level)
		{
			if (level is CustomPreviewBeatmapLevel)
			{
				CustomPreviewBeatmapLevel customPreviewBeatmapLevel = level as CustomPreviewBeatmapLevel;
				ExtraSongData extraSongData = Collections.RetrieveExtraSongData(Hashing.GetCustomLevelHash(customPreviewBeatmapLevel), customPreviewBeatmapLevel.customLevelPath);
				Collections.SaveExtraSongData();
				if (extraSongData == null)
				{
					return;
				}
				if (Plugin.PlatformsInstalled && Plugin.customSongPlatforms && !string.IsNullOrWhiteSpace(extraSongData._customEnvironmentName) && Plugin.findCustomEnvironment(extraSongData._customEnvironmentName) == -1)
				{
					Console.WriteLine("CustomPlatform not found: " + extraSongData._customEnvironmentName);
					if (!string.IsNullOrWhiteSpace(extraSongData._customEnvironmentHash))
					{
						Console.WriteLine("Downloading with hash: " + extraSongData._customEnvironmentHash);
						PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this.downloadCustomPlatform(extraSongData._customEnvironmentHash, extraSongData._customEnvironmentName));
					}
				}
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004B40 File Offset: 0x00002D40
		[Init]
		public void Init(object thisIsNull, global::IPA.Logging.Logger pluginLogger)
		{
			Logging.logger = pluginLogger;
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
			this.registerOneClickDownCmd();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000020D3 File Offset: 0x000002D3
		public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
		{
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000020D3 File Offset: 0x000002D3
		public void OnSceneUnloaded(Scene scene)
		{
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004B84 File Offset: 0x00002D84
		public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
		{
			Plugin.customSongColors = BasicUI.ModPrefs.GetBool("SongCore", "customSongColors", true, true);
			Plugin.customSongPlatforms = BasicUI.ModPrefs.GetBool("SongCore", "customSongPlatforms", true, true);
			Object.Destroy(GameObject.Find("SongCore Color Setter"));
			if (nextScene.name == "MenuViewControllers")
			{
				Gamemode.Init();
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004BEE File Offset: 0x00002DEE
		private IEnumerator DelayedNoteJumpMovementSpeedFix()
		{
			yield return new WaitForSeconds(0.1f);
			if (Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpMovementSpeed < 0f)
			{
				Plugin.SetNJS(Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().FirstOrDefault<BeatmapObjectSpawnController>());
			}
			yield break;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004BF8 File Offset: 0x00002DF8
		public static void SetNJS(BeatmapObjectSpawnController _spawnController)
		{
			BeatmapObjectSpawnMovementData privateField = _spawnController.GetPrivateField("_beatmapObjectSpawnMovementData");
			float currentBPM = _spawnController.GetPrivateField("_variableBPMProcessor").currentBPM;
			privateField.SetPrivateField("_startNoteJumpMovementSpeed", Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpMovementSpeed);
			privateField.SetPrivateField("_noteJumpStartBeatOffset", Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.noteJumpStartBeatOffset);
			privateField.Update(currentBPM, _spawnController.GetPrivateField("_jumpOffsetY"));
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000020D3 File Offset: 0x000002D3
		[OnExit]
		public void OnApplicationQuit()
		{
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000020D3 File Offset: 0x000002D3
		public void OnLevelWasLoaded(int level)
		{
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000020D3 File Offset: 0x000002D3
		public void OnLevelWasInitialized(int level)
		{
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000020D3 File Offset: 0x000002D3
		public void OnUpdate()
		{
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000020D3 File Offset: 0x000002D3
		public void OnFixedUpdate()
		{
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004C7C File Offset: 0x00002E7C
		internal static void CheckCustomSongEnvironment(IDifficultyBeatmap song)
		{
			ExtraSongData extraSongData = Collections.RetrieveExtraSongData(Hashing.GetCustomLevelHash(song.level as CustomPreviewBeatmapLevel), "");
			if (extraSongData == null)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(extraSongData._customEnvironmentName))
			{
				Plugin._currentPlatform = -1;
				return;
			}
			try
			{
				int num = Plugin.customEnvironment(extraSongData._customEnvironmentName);
				if (Plugin.customSongPlatforms)
				{
					int currentPlatform = Plugin._currentPlatform;
				}
			}
			catch (Exception ex)
			{
				Logging.logger.Error(string.Format("Failed to Change to Platform {0}\n {1}", extraSongData._customEnvironmentName, ex));
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004D08 File Offset: 0x00002F08
		internal static int customEnvironment(string platform)
		{
			if (!Plugin.PlatformsInstalled)
			{
				return -1;
			}
			return Plugin.findCustomEnvironment(platform);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004D19 File Offset: 0x00002F19
		private static int findCustomEnvironment(string name)
		{
			return -1;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004D1C File Offset: 0x00002F1C
		private IEnumerator downloadCustomPlatform(string hash, string name)
		{
			using (UnityWebRequest www = UnityWebRequest.Get("https://modelsaber.com/api/v1/platform/get.php?filter=hash:" + hash))
			{
				yield return www.SendWebRequest();
				if (www.isNetworkError || www.isHttpError)
				{
					Console.WriteLine(www.error);
				}
				else
				{
					Plugin.platformDownloadData value = JsonConvert.DeserializeObject<Dictionary<string, Plugin.platformDownloadData>>(www.downloadHandler.text).FirstOrDefault<KeyValuePair<string, Plugin.platformDownloadData>>().Value;
					if (value != null && value.name == name)
					{
						PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this._downloadCustomPlatform(value));
					}
				}
			}
			UnityWebRequest www = null;
			yield break;
			yield break;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004D39 File Offset: 0x00002F39
		private IEnumerator _downloadCustomPlatform(Plugin.platformDownloadData downloadData)
		{
			using (UnityWebRequest www = UnityWebRequest.Get(downloadData.download))
			{
				yield return www.SendWebRequest();
				if (www.isNetworkError || www.isHttpError)
				{
					Console.WriteLine(www.error);
				}
				else
				{
					File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "CustomPlatforms", downloadData.name) + ".plat", www.downloadHandler.data);
				}
			}
			UnityWebRequest www = null;
			yield break;
			yield break;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004D48 File Offset: 0x00002F48
		private void registerOneClickDownCmd()
		{
			string text = "beatsaberbbs";
			foreach (string text2 in Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes").GetSubKeyNames())
			{
				if (text == text2)
				{
					Logging.logger.Info("beatsaberbbs of registerkey exist~~~");
					return;
				}
			}
			string text3 = "shell";
			string text4 = "open";
			string text5 = "command";
			string text6 = "OneClickDown.bat";
			string text7 = Path.Combine(Environment.CurrentDirectory, text6);
			string text8 = "\"" + text7 + "\" \"%1\"";
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + text);
			registryKey.SetValue("", "URL: beatsaberbbs");
			registryKey.SetValue("URL Protocol", "");
			registryKey.CreateSubKey(text3).CreateSubKey(text4).CreateSubKey(text5)
				.SetValue("", text8);
		}

		// Token: 0x04000049 RID: 73
		public static string costomLevelsDirName = "NetviosBBSDownloadLevels";

		// Token: 0x0400004A RID: 74
		public static string standardCharacteristicName = "Standard";

		// Token: 0x0400004B RID: 75
		public static string oneSaberCharacteristicName = "OneSaber";

		// Token: 0x0400004C RID: 76
		public static string noArrowsCharacteristicName = "NoArrows";

		// Token: 0x0400004D RID: 77
		internal static Harmony harmony;

		// Token: 0x0400004E RID: 78
		internal static bool PlatformsInstalled = false;

		// Token: 0x0400004F RID: 79
		internal static bool customSongColors;

		// Token: 0x04000050 RID: 80
		internal static bool customSongPlatforms;

		// Token: 0x04000051 RID: 81
		internal static int _currentPlatform = -1;

		// Token: 0x0200004A RID: 74
		[Serializable]
		public class platformDownloadData
		{
			// Token: 0x040000F8 RID: 248
			public string name;

			// Token: 0x040000F9 RID: 249
			public string author;

			// Token: 0x040000FA RID: 250
			public string image;

			// Token: 0x040000FB RID: 251
			public string hash;

			// Token: 0x040000FC RID: 252
			public string download;

			// Token: 0x040000FD RID: 253
			public string date;
		}
	}
}
