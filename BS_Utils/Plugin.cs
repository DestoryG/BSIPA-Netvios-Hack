using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BS_Utils.Gameplay;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA;
using IPA.Loader;
using IPA.Logging;
using IPA.Utilities.Async;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BS_Utils
{
	// Token: 0x02000002 RID: 2
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (remove) Token: 0x06000002 RID: 2 RVA: 0x00002084 File Offset: 0x00000284
		public static event Plugin.LevelDidFinish LevelDidFinishEvent;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000003 RID: 3 RVA: 0x000020B8 File Offset: 0x000002B8
		// (remove) Token: 0x06000004 RID: 4 RVA: 0x000020EC File Offset: 0x000002EC
		public static event Plugin.MissionDidFinish MissionDidFinishEvent;

		// Token: 0x06000005 RID: 5 RVA: 0x0000211F File Offset: 0x0000031F
		[OnStart]
		public void OnApplicationStart()
		{
			Plugin.harmony = new Harmony("com.kyle1413.BeatSaber.BS-Utils");
			BSEvents.OnLoad();
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			PluginManager.OnPluginsStateChanged += this.PluginManager_OnPluginsStateChanged;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002158 File Offset: 0x00000358
		private void PluginManager_OnPluginsStateChanged(Task task)
		{
			MenuTransitionsHelper transitionHelper = Resources.FindObjectsOfTypeAll<MenuTransitionsHelper>().FirstOrDefault<MenuTransitionsHelper>();
			FadeInOutController fadeInOutController = Resources.FindObjectsOfTypeAll<FadeInOutController>().FirstOrDefault<FadeInOutController>();
			if (fadeInOutController != null)
			{
				fadeInOutController.FadeOut();
			}
			task.ContinueWith(delegate(Task t)
			{
				MenuTransitionsHelper transitionHelper2 = transitionHelper;
				if (transitionHelper2 == null)
				{
					return;
				}
				transitionHelper2.RestartGame();
			}, UnityMainThreadTaskScheduler.Default);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021A7 File Offset: 0x000003A7
		[Init]
		public void Init(global::IPA.Logging.Logger logger)
		{
			BS_Utils.Utilities.Logger.log = logger;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021AF File Offset: 0x000003AF
		[OnExit]
		public void Exit()
		{
			SceneManager.activeSceneChanged -= new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			PluginManager.OnPluginsStateChanged -= this.PluginManager_OnPluginsStateChanged;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021D3 File Offset: 0x000003D3
		public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
		{
			if (nextScene.name == "MenuCore")
			{
				if (Gamemode.IsIsolatedLevel)
				{
					BS_Utils.Utilities.Logger.Log("Removing Isolated Level");
				}
				Gamemode.IsIsolatedLevel = false;
				Gamemode.IsolatingMod = "";
				Plugin.LevelData.Clear();
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002213 File Offset: 0x00000413
		internal static void TriggerLevelFinishEvent(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
		{
			Plugin.LevelDidFinish levelDidFinishEvent = Plugin.LevelDidFinishEvent;
			if (levelDidFinishEvent == null)
			{
				return;
			}
			levelDidFinishEvent(levelScenesTransitionSetupDataSO, levelCompletionResults);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002226 File Offset: 0x00000426
		internal static void TriggerMissionFinishEvent(MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupDataSO, MissionCompletionResults missionCompletionResults)
		{
			Plugin.MissionDidFinish missionDidFinishEvent = Plugin.MissionDidFinishEvent;
			if (missionDidFinishEvent == null)
			{
				return;
			}
			missionDidFinishEvent(missionLevelScenesTransitionSetupDataSO, missionCompletionResults);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000223C File Offset: 0x0000043C
		internal static void ApplyHarmonyPatches()
		{
			if (Plugin.patched)
			{
				return;
			}
			try
			{
				BS_Utils.Utilities.Logger.Log("Applying Harmony Patches", global::IPA.Logging.Logger.Level.Debug);
				Plugin.harmony.PatchAll(Assembly.GetExecutingAssembly());
				Plugin.patched = true;
			}
			catch (Exception ex)
			{
				BS_Utils.Utilities.Logger.Log("Exception Trying to Apply Harmony Patches", global::IPA.Logging.Logger.Level.Error);
				BS_Utils.Utilities.Logger.Log(ex.ToString(), global::IPA.Logging.Logger.Level.Error);
			}
		}

		// Token: 0x04000001 RID: 1
		internal static bool patched = false;

		// Token: 0x04000002 RID: 2
		internal static Harmony harmony;

		// Token: 0x04000003 RID: 3
		public static LevelData LevelData = new LevelData();

		// Token: 0x02000014 RID: 20
		// (Invoke) Token: 0x060000F0 RID: 240
		public delegate void LevelDidFinish(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults);

		// Token: 0x02000015 RID: 21
		// (Invoke) Token: 0x060000F4 RID: 244
		public delegate void MissionDidFinish(MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupDataSO, MissionCompletionResults missionCompletionResults);
	}
}
