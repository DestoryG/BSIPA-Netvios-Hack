using System;
using PlayerDataPlugin.BSHandler;
using UnityEngine;

namespace PlayerDataPlugin
{
	// Token: 0x02000003 RID: 3
	public class GameEventController : Singleton<GameEventController>
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public void SetupForMenuViewControllersScene()
		{
			Singleton<Player>.Instance.Init();
			Singleton<DataStore>.Instance.Init();
			Singleton<MainFlowCoordinator_Handler>.Instance.SetupBeforeGameSceneStart();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002078 File Offset: 0x00000278
		public void TearDownForMenuViewControllersScene()
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000207A File Offset: 0x0000027A
		public void SetupForGameCoreScene()
		{
			Singleton<MainFlowCoordinator_Handler>.Instance.UpdateAtGameScene();
			Singleton<StandardLevelGameplayManager_Handler>.Instance.UpdateAtGameScene();
			Singleton<ScoreController_Handler>.Instance.UpdateAtGameScene();
			Singleton<DataStore>.Instance.Restore();
			this.RegisterGameEvents();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020AA File Offset: 0x000002AA
		public void TeardownForGameCoreScene()
		{
			this.RemoveGameEvents();
			Singleton<DataStore>.Instance.UploadZipData();
			Singleton<DataStore>.Instance.DisposeRecord();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020C8 File Offset: 0x000002C8
		private void RegisterGameEvents()
		{
			ScoreController scoreController = Singleton<ScoreController_Handler>.Instance.ScoreController;
			scoreController.scoreDidChangeEvent -= this.HandleScoreChangedEvent;
			scoreController.scoreDidChangeEvent += this.HandleScoreChangedEvent;
			GameSongController songController = Singleton<StandardLevelGameplayManager_Handler>.Instance.SongController;
			songController.songDidFinishEvent -= this.HandleSongDidFinishEvent;
			songController.songDidFinishEvent += this.HandleSongDidFinishEvent;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002130 File Offset: 0x00000330
		private void RemoveGameEvents()
		{
			GameSongController songController = Singleton<StandardLevelGameplayManager_Handler>.Instance.SongController;
			if (songController != null)
			{
				songController.songDidFinishEvent -= this.HandleSongDidFinishEvent;
			}
			ScoreController scoreController = Singleton<ScoreController_Handler>.Instance.ScoreController;
			if (scoreController != null)
			{
				scoreController.scoreDidChangeEvent -= this.HandleScoreChangedEvent;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002189 File Offset: 0x00000389
		public void UpdateInGameCore()
		{
			if (!Singleton<Player>.Instance.IsAnonymous)
			{
				Singleton<Player>.Instance.OnUpdate(Time.deltaTime);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021A6 File Offset: 0x000003A6
		public void SetLevelCompletionResultsAndDifficultyBeatmap(LevelCompletionResults results, IDifficultyBeatmap difficultyBeatmap)
		{
			if (!Singleton<MainFlowCoordinator_Handler>.Instance.IsInSoloMode)
			{
				Logger.log.Warn("Got LevelCompletionResults when not in solo mode, have a check!!!");
			}
			this.UploadResults(results, difficultyBeatmap);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002078 File Offset: 0x00000278
		public void PostProcessAfterExitGameCore()
		{
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021CB File Offset: 0x000003CB
		private void UploadResults(LevelCompletionResults results, IDifficultyBeatmap difficultyBeatmap)
		{
			bool isInSoloMode = Singleton<MainFlowCoordinator_Handler>.Instance.IsInSoloMode;
			if (results != null)
			{
				Singleton<DataStore>.Instance.SetLevelCompletionResults(results);
				Singleton<DataStore>.Instance.SetSongData(difficultyBeatmap);
				Singleton<DataStore>.Instance.UploadDataAsync();
				return;
			}
			Logger.log.Warn("[Result in OnFinishedEvent] results is null !!!");
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000220B File Offset: 0x0000040B
		private void HandleSongDidFinishEvent()
		{
			Singleton<DataStore>.Instance.SetSongDidFinish(true);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002078 File Offset: 0x00000278
		private void HandleGameEnergyDidReach0Event()
		{
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002078 File Offset: 0x00000278
		private void HandleWasMissedEvent(NoteData noteData, int multiplier)
		{
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002078 File Offset: 0x00000278
		private void HandleComboChangedEvent(int combo)
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002078 File Offset: 0x00000278
		private void HandleWasCutEvent(NoteData noteData, NoteCutInfo cutInfo, int multiplier)
		{
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002218 File Offset: 0x00000418
		private void HandleScoreChangedEvent(int rawScore, int modifiedScore)
		{
			Singleton<DataStore>.Instance.SetLastModifiedScoreForActionRecord(modifiedScore);
			Singleton<DataStore>.Instance.SaveScoreDataForActionRecord(rawScore, modifiedScore);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002078 File Offset: 0x00000278
		private void HandleWasCutEvent(NoteController ctrl, NoteCutInfo info)
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002078 File Offset: 0x00000278
		private void HandleStartJump(NoteController noteCtrl)
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002078 File Offset: 0x00000278
		private void HandleFinishJump(NoteController noteCtrl)
		{
		}
	}
}
