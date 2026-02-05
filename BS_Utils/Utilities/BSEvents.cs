using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

namespace BS_Utils.Utilities
{
	// Token: 0x02000003 RID: 3
	public class BSEvents : MonoBehaviour
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600000F RID: 15 RVA: 0x000022BC File Offset: 0x000004BC
		// (remove) Token: 0x06000010 RID: 16 RVA: 0x000022F0 File Offset: 0x000004F0
		public static event Action menuSceneActive;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000011 RID: 17 RVA: 0x00002324 File Offset: 0x00000524
		// (remove) Token: 0x06000012 RID: 18 RVA: 0x00002358 File Offset: 0x00000558
		public static event Action menuSceneLoaded;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000013 RID: 19 RVA: 0x0000238C File Offset: 0x0000058C
		// (remove) Token: 0x06000014 RID: 20 RVA: 0x000023C0 File Offset: 0x000005C0
		public static event Action<ScenesTransitionSetupDataSO> earlyMenuSceneLoadedFresh;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000015 RID: 21 RVA: 0x000023F4 File Offset: 0x000005F4
		// (remove) Token: 0x06000016 RID: 22 RVA: 0x00002428 File Offset: 0x00000628
		[Obsolete("Use earlyMenuSceneLoadedFresh or lateMenuSceneLoadedFresh.")]
		public static event Action menuSceneLoadedFresh;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000017 RID: 23 RVA: 0x0000245C File Offset: 0x0000065C
		// (remove) Token: 0x06000018 RID: 24 RVA: 0x00002490 File Offset: 0x00000690
		public static event Action<ScenesTransitionSetupDataSO> lateMenuSceneLoadedFresh;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000019 RID: 25 RVA: 0x000024C4 File Offset: 0x000006C4
		// (remove) Token: 0x0600001A RID: 26 RVA: 0x000024F8 File Offset: 0x000006F8
		public static event Action gameSceneActive;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600001B RID: 27 RVA: 0x0000252C File Offset: 0x0000072C
		// (remove) Token: 0x0600001C RID: 28 RVA: 0x00002560 File Offset: 0x00000760
		public static event Action gameSceneLoaded;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600001D RID: 29 RVA: 0x00002594 File Offset: 0x00000794
		// (remove) Token: 0x0600001E RID: 30 RVA: 0x000025C8 File Offset: 0x000007C8
		public static event Action<StandardLevelDetailViewController, IDifficultyBeatmap> difficultySelected;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600001F RID: 31 RVA: 0x000025FC File Offset: 0x000007FC
		// (remove) Token: 0x06000020 RID: 32 RVA: 0x00002630 File Offset: 0x00000830
		public static event Action<BeatmapCharacteristicSegmentedControlController, BeatmapCharacteristicSO> characteristicSelected;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000021 RID: 33 RVA: 0x00002664 File Offset: 0x00000864
		// (remove) Token: 0x06000022 RID: 34 RVA: 0x00002698 File Offset: 0x00000898
		public static event Action<LevelSelectionNavigationController, IBeatmapLevelPack> levelPackSelected;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000023 RID: 35 RVA: 0x000026CC File Offset: 0x000008CC
		// (remove) Token: 0x06000024 RID: 36 RVA: 0x00002700 File Offset: 0x00000900
		public static event Action<LevelCollectionViewController, IPreviewBeatmapLevel> levelSelected;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000025 RID: 37 RVA: 0x00002734 File Offset: 0x00000934
		// (remove) Token: 0x06000026 RID: 38 RVA: 0x00002768 File Offset: 0x00000968
		public static event Action songPaused;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000027 RID: 39 RVA: 0x0000279C File Offset: 0x0000099C
		// (remove) Token: 0x06000028 RID: 40 RVA: 0x000027D0 File Offset: 0x000009D0
		public static event Action songUnpaused;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000029 RID: 41 RVA: 0x00002804 File Offset: 0x00000A04
		// (remove) Token: 0x0600002A RID: 42 RVA: 0x00002838 File Offset: 0x00000A38
		public static event Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelCleared;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600002B RID: 43 RVA: 0x0000286C File Offset: 0x00000A6C
		// (remove) Token: 0x0600002C RID: 44 RVA: 0x000028A0 File Offset: 0x00000AA0
		public static event Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelQuit;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600002D RID: 45 RVA: 0x000028D4 File Offset: 0x00000AD4
		// (remove) Token: 0x0600002E RID: 46 RVA: 0x00002908 File Offset: 0x00000B08
		public static event Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelFailed;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600002F RID: 47 RVA: 0x0000293C File Offset: 0x00000B3C
		// (remove) Token: 0x06000030 RID: 48 RVA: 0x00002970 File Offset: 0x00000B70
		public static event Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelRestarted;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000031 RID: 49 RVA: 0x000029A4 File Offset: 0x00000BA4
		// (remove) Token: 0x06000032 RID: 50 RVA: 0x000029D8 File Offset: 0x00000BD8
		public static event Action<NoteData, NoteCutInfo, int> noteWasCut;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000033 RID: 51 RVA: 0x00002A0C File Offset: 0x00000C0C
		// (remove) Token: 0x06000034 RID: 52 RVA: 0x00002A40 File Offset: 0x00000C40
		public static event Action<NoteData, int> noteWasMissed;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000035 RID: 53 RVA: 0x00002A74 File Offset: 0x00000C74
		// (remove) Token: 0x06000036 RID: 54 RVA: 0x00002AA8 File Offset: 0x00000CA8
		public static event Action<int, float> multiplierDidChange;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000037 RID: 55 RVA: 0x00002ADC File Offset: 0x00000CDC
		// (remove) Token: 0x06000038 RID: 56 RVA: 0x00002B10 File Offset: 0x00000D10
		public static event Action<int> multiplierDidIncrease;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000039 RID: 57 RVA: 0x00002B44 File Offset: 0x00000D44
		// (remove) Token: 0x0600003A RID: 58 RVA: 0x00002B78 File Offset: 0x00000D78
		public static event Action<int> comboDidChange;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600003B RID: 59 RVA: 0x00002BAC File Offset: 0x00000DAC
		// (remove) Token: 0x0600003C RID: 60 RVA: 0x00002BE0 File Offset: 0x00000DE0
		public static event Action comboDidBreak;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600003D RID: 61 RVA: 0x00002C14 File Offset: 0x00000E14
		// (remove) Token: 0x0600003E RID: 62 RVA: 0x00002C48 File Offset: 0x00000E48
		public static event Action<int> scoreDidChange;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x0600003F RID: 63 RVA: 0x00002C7C File Offset: 0x00000E7C
		// (remove) Token: 0x06000040 RID: 64 RVA: 0x00002CB0 File Offset: 0x00000EB0
		public static event Action<float> energyDidChange;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000041 RID: 65 RVA: 0x00002CE4 File Offset: 0x00000EE4
		// (remove) Token: 0x06000042 RID: 66 RVA: 0x00002D18 File Offset: 0x00000F18
		public static event Action energyReachedZero;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000043 RID: 67 RVA: 0x00002D4C File Offset: 0x00000F4C
		// (remove) Token: 0x06000044 RID: 68 RVA: 0x00002D80 File Offset: 0x00000F80
		public static event Action<BeatmapEventData> beatmapEvent;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000045 RID: 69 RVA: 0x00002DB4 File Offset: 0x00000FB4
		// (remove) Token: 0x06000046 RID: 70 RVA: 0x00002DE8 File Offset: 0x00000FE8
		public static event Action<SaberType> sabersStartCollide;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000047 RID: 71 RVA: 0x00002E1C File Offset: 0x0000101C
		// (remove) Token: 0x06000048 RID: 72 RVA: 0x00002E50 File Offset: 0x00001050
		public static event Action<SaberType> sabersEndCollide;

		// Token: 0x06000049 RID: 73 RVA: 0x00002E84 File Offset: 0x00001084
		public static void OnLoad()
		{
			if (BSEvents.Instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("BSEvents");
			gameObject.AddComponent<BSEvents>();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002EB1 File Offset: 0x000010B1
		private void Awake()
		{
			if (BSEvents.Instance != null)
			{
				return;
			}
			BSEvents.Instance = this;
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.SceneManagerOnActiveSceneChanged);
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002EE4 File Offset: 0x000010E4
		private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
		{
			try
			{
				if (arg1.name == "GameCore")
				{
					this.InvokeAll(BSEvents.gameSceneActive, Array.Empty<object>());
					this.gameScenesManager = Resources.FindObjectsOfTypeAll<GameScenesManager>().FirstOrDefault<GameScenesManager>();
					if (this.gameScenesManager != null)
					{
						this.gameScenesManager.transitionDidFinishEvent -= this.GameSceneSceneWasLoaded;
						this.gameScenesManager.transitionDidFinishEvent += this.GameSceneSceneWasLoaded;
					}
				}
				else if (arg1.name == "MenuViewControllers")
				{
					this.gameScenesManager = Resources.FindObjectsOfTypeAll<GameScenesManager>().FirstOrDefault<GameScenesManager>();
					this.InvokeAll(BSEvents.menuSceneActive, Array.Empty<object>());
					if (this.gameScenesManager != null)
					{
						if (arg0.name == "EmptyTransition")
						{
							this.gameScenesManager.transitionDidFinishEvent -= this.OnMenuSceneWasLoadedFresh;
							this.gameScenesManager.transitionDidFinishEvent += this.OnMenuSceneWasLoadedFresh;
						}
						else
						{
							this.gameScenesManager.transitionDidFinishEvent -= this.OnMenuSceneWasLoaded;
							this.gameScenesManager.transitionDidFinishEvent += this.OnMenuSceneWasLoaded;
						}
					}
				}
			}
			catch (Exception ex)
			{
				string text = "[BSEvents] ";
				Exception ex2 = ex;
				Console.WriteLine(text + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000305C File Offset: 0x0000125C
		private void OnMenuSceneWasLoaded(ScenesTransitionSetupDataSO transitionSetupData, DiContainer diContainer)
		{
			this.gameScenesManager.transitionDidFinishEvent -= this.OnMenuSceneWasLoaded;
			this.InvokeAll(BSEvents.menuSceneLoaded, Array.Empty<object>());
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003088 File Offset: 0x00001288
		private void OnMenuSceneWasLoadedFresh(ScenesTransitionSetupDataSO transitionSetupData, DiContainer diContainer)
		{
			this.gameScenesManager.transitionDidFinishEvent -= this.OnMenuSceneWasLoadedFresh;
			StandardLevelDetailViewController standardLevelDetailViewController = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().FirstOrDefault<StandardLevelDetailViewController>();
			standardLevelDetailViewController.didChangeDifficultyBeatmapEvent += delegate(StandardLevelDetailViewController vc, IDifficultyBeatmap beatmap)
			{
				this.InvokeAll<StandardLevelDetailViewController, IDifficultyBeatmap>(BSEvents.difficultySelected, new object[] { vc, beatmap });
			};
			BeatmapCharacteristicSegmentedControlController beatmapCharacteristicSegmentedControlController = Resources.FindObjectsOfTypeAll<BeatmapCharacteristicSegmentedControlController>().FirstOrDefault<BeatmapCharacteristicSegmentedControlController>();
			beatmapCharacteristicSegmentedControlController.didSelectBeatmapCharacteristicEvent += delegate(BeatmapCharacteristicSegmentedControlController controller, BeatmapCharacteristicSO characteristic)
			{
				this.InvokeAll<BeatmapCharacteristicSegmentedControlController, BeatmapCharacteristicSO>(BSEvents.characteristicSelected, new object[] { controller, characteristic });
			};
			LevelSelectionNavigationController levelSelectionNavigationController = Resources.FindObjectsOfTypeAll<LevelSelectionNavigationController>().FirstOrDefault<LevelSelectionNavigationController>();
			levelSelectionNavigationController.didSelectLevelPackEvent += delegate(LevelSelectionNavigationController controller, IBeatmapLevelPack pack)
			{
				this.InvokeAll<LevelSelectionNavigationController, IBeatmapLevelPack>(BSEvents.levelPackSelected, new object[] { controller, pack });
			};
			LevelCollectionViewController levelCollectionViewController = Resources.FindObjectsOfTypeAll<LevelCollectionViewController>().FirstOrDefault<LevelCollectionViewController>();
			levelCollectionViewController.didSelectLevelEvent += delegate(LevelCollectionViewController controller, IPreviewBeatmapLevel level)
			{
				this.InvokeAll<LevelCollectionViewController, IPreviewBeatmapLevel>(BSEvents.levelSelected, new object[] { controller, level });
			};
			this.InvokeAll<ScenesTransitionSetupDataSO>(BSEvents.earlyMenuSceneLoadedFresh, new object[] { transitionSetupData });
			this.InvokeAll(BSEvents.menuSceneLoadedFresh, Array.Empty<object>());
			this.InvokeAll<ScenesTransitionSetupDataSO>(BSEvents.lateMenuSceneLoadedFresh, new object[] { transitionSetupData });
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000315C File Offset: 0x0000135C
		private void GameSceneSceneWasLoaded(ScenesTransitionSetupDataSO transitionSetupData, DiContainer diContainer)
		{
			Resources.FindObjectsOfTypeAll<GameScenesManager>().FirstOrDefault<GameScenesManager>().transitionDidFinishEvent -= this.GameSceneSceneWasLoaded;
			GamePause gamePause = Resources.FindObjectsOfTypeAll<GamePause>().FirstOrDefault<GamePause>();
			gamePause.didResumeEvent += delegate
			{
				this.InvokeAll(BSEvents.songUnpaused, Array.Empty<object>());
			};
			gamePause.didPauseEvent += delegate
			{
				this.InvokeAll(BSEvents.songPaused, Array.Empty<object>());
			};
			ScoreController scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault<ScoreController>();
			scoreController.noteWasCutEvent += delegate(NoteData noteData, NoteCutInfo noteCutInfo, int multiplier)
			{
				this.InvokeAll<NoteData, NoteCutInfo, int>(BSEvents.noteWasCut, new object[] { noteData, noteCutInfo, multiplier });
			};
			scoreController.noteWasMissedEvent += delegate(NoteData noteData, int multiplier)
			{
				this.InvokeAll<NoteData, int>(BSEvents.noteWasMissed, new object[] { noteData, multiplier });
			};
			scoreController.multiplierDidChangeEvent += delegate(int multiplier, float progress)
			{
				this.InvokeAll<int, float>(BSEvents.multiplierDidChange, new object[] { multiplier, progress });
				if (multiplier > 1 && progress < 0.1f)
				{
					this.InvokeAll<int>(BSEvents.multiplierDidIncrease, new object[] { multiplier });
				}
			};
			scoreController.comboDidChangeEvent += delegate(int combo)
			{
				this.InvokeAll<int>(BSEvents.comboDidChange, new object[] { combo });
			};
			scoreController.comboBreakingEventHappenedEvent += delegate
			{
				this.InvokeAll(BSEvents.comboDidBreak, Array.Empty<object>());
			};
			scoreController.scoreDidChangeEvent += delegate(int score, int scoreAfterModifier)
			{
				this.InvokeAll<int>(BSEvents.scoreDidChange, Array.Empty<object>());
			};
			ObstacleSaberSparkleEffectManager obstacleSaberSparkleEffectManager = Resources.FindObjectsOfTypeAll<ObstacleSaberSparkleEffectManager>().FirstOrDefault<ObstacleSaberSparkleEffectManager>();
			obstacleSaberSparkleEffectManager.sparkleEffectDidStartEvent += delegate(SaberType saber)
			{
				this.InvokeAll<SaberType>(BSEvents.sabersStartCollide, new object[] { saber });
			};
			obstacleSaberSparkleEffectManager.sparkleEffectDidEndEvent += delegate(SaberType saber)
			{
				this.InvokeAll<SaberType>(BSEvents.sabersEndCollide, new object[] { saber });
			};
			GameEnergyCounter gameEnergyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().FirstOrDefault<GameEnergyCounter>();
			gameEnergyCounter.gameEnergyDidReach0Event += delegate
			{
				this.InvokeAll(BSEvents.energyReachedZero, Array.Empty<object>());
			};
			gameEnergyCounter.gameEnergyDidChangeEvent += delegate(float energy)
			{
				this.InvokeAll<float>(BSEvents.energyDidChange, new object[] { energy });
			};
			BeatmapObjectCallbackController beatmapObjectCallbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().FirstOrDefault<BeatmapObjectCallbackController>();
			beatmapObjectCallbackController.beatmapEventDidTriggerEvent += delegate(BeatmapEventData songEvent)
			{
				this.InvokeAll<BeatmapEventData>(BSEvents.beatmapEvent, new object[] { songEvent });
			};
			StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSO = Resources.FindObjectsOfTypeAll<StandardLevelScenesTransitionSetupDataSO>().FirstOrDefault<StandardLevelScenesTransitionSetupDataSO>();
			if (standardLevelScenesTransitionSetupDataSO)
			{
				standardLevelScenesTransitionSetupDataSO.didFinishEvent -= this.OnTransitionSetupOnDidFinishEvent;
				standardLevelScenesTransitionSetupDataSO.didFinishEvent += this.OnTransitionSetupOnDidFinishEvent;
			}
			this.InvokeAll(BSEvents.gameSceneLoaded, Array.Empty<object>());
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000032F4 File Offset: 0x000014F4
		private void OnTransitionSetupOnDidFinishEvent(StandardLevelScenesTransitionSetupDataSO data, LevelCompletionResults results)
		{
			LevelCompletionResults.LevelEndStateType levelEndStateType = results.levelEndStateType;
			if (levelEndStateType != 1)
			{
				if (levelEndStateType == 2)
				{
					if (results.levelEndAction == 2)
					{
						this.InvokeAll<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(BSEvents.levelRestarted, new object[] { data, results });
					}
					else
					{
						this.InvokeAll<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(BSEvents.levelFailed, new object[] { data, results });
					}
				}
			}
			else
			{
				this.InvokeAll<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(BSEvents.levelCleared, new object[] { data, results });
			}
			LevelCompletionResults.LevelEndAction levelEndAction = results.levelEndAction;
			if (levelEndAction == 1)
			{
				this.InvokeAll<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(BSEvents.levelQuit, new object[] { data, results });
				return;
			}
			if (levelEndAction != 2)
			{
				return;
			}
			this.InvokeAll<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(BSEvents.levelRestarted, new object[] { data, results });
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000033B0 File Offset: 0x000015B0
		public void InvokeAll<T1, T2, T3>(Action<T1, T2, T3> action, params object[] args)
		{
			if (action == null)
			{
				return;
			}
			foreach (Delegate @delegate in action.GetInvocationList())
			{
				try
				{
					if (@delegate != null)
					{
						@delegate.DynamicInvoke(args);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Caught Exception when executing event");
					Console.WriteLine(ex);
				}
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000340C File Offset: 0x0000160C
		public void InvokeAll<T1, T2>(Action<T1, T2> action, params object[] args)
		{
			if (action == null)
			{
				return;
			}
			foreach (Delegate @delegate in action.GetInvocationList())
			{
				try
				{
					if (@delegate != null)
					{
						@delegate.DynamicInvoke(args);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Caught Exception when executing event");
					Console.WriteLine(ex);
				}
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003468 File Offset: 0x00001668
		public void InvokeAll<T>(Action<T> action, params object[] args)
		{
			Delegate[] array = ((action != null) ? action.GetInvocationList() : null);
			if (array == null)
			{
				return;
			}
			foreach (Delegate @delegate in array)
			{
				try
				{
					if (@delegate != null)
					{
						@delegate.DynamicInvoke(args);
					}
				}
				catch (Exception ex)
				{
					Logger.log.Error("Caught Exception when executing event: " + ex.Message);
					Logger.log.Debug(ex);
				}
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000034E4 File Offset: 0x000016E4
		public void InvokeAll(Action action, params object[] args)
		{
			Delegate[] array = ((action != null) ? action.GetInvocationList() : null);
			if (array == null)
			{
				return;
			}
			foreach (Delegate @delegate in array)
			{
				try
				{
					if (@delegate != null)
					{
						@delegate.DynamicInvoke(args);
					}
				}
				catch (Exception ex)
				{
					Logger.log.Error("Caught Exception when executing event: " + ex.Message);
					Logger.log.Debug(ex);
				}
			}
		}

		// Token: 0x04000006 RID: 6
		private static BSEvents Instance;

		// Token: 0x04000024 RID: 36
		private const string Menu = "MenuViewControllers";

		// Token: 0x04000025 RID: 37
		private const string Game = "GameCore";

		// Token: 0x04000026 RID: 38
		private const string EmptyTransition = "EmptyTransition";

		// Token: 0x04000027 RID: 39
		private GameScenesManager gameScenesManager;
	}
}
