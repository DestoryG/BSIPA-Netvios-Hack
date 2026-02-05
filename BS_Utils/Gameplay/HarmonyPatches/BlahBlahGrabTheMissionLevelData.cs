using System;
using HarmonyLib;

namespace BS_Utils.Gameplay.HarmonyPatches
{
	// Token: 0x0200000F RID: 15
	[HarmonyPatch(typeof(MissionLevelScenesTransitionSetupDataSO), "Init", new Type[]
	{
		typeof(IDifficultyBeatmap),
		typeof(MissionObjective[]),
		typeof(OverrideEnvironmentSettings),
		typeof(ColorScheme),
		typeof(GameplayModifiers),
		typeof(PlayerSpecificSettings),
		typeof(string)
	})]
	internal class BlahBlahGrabTheMissionLevelData
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x00004BB4 File Offset: 0x00002DB4
		private static void Prefix(MissionLevelScenesTransitionSetupDataSO __instance, IDifficultyBeatmap difficultyBeatmap, OverrideEnvironmentSettings overrideEnvironmentSettings, MissionObjective[] missionObjectives, GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings)
		{
			ScoreSubmission._wasDisabled = false;
			ScoreSubmission.LastDisablers = Array.Empty<string>();
			Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, gameplayModifiers, playerSpecificSettings, PracticeSettings.defaultPracticeSettings, false);
			Plugin.LevelData.IsSet = true;
			__instance.didFinishEvent -= BlahBlahGrabTheMissionLevelData.__instance_didFinishEvent;
			__instance.didFinishEvent += BlahBlahGrabTheMissionLevelData.__instance_didFinishEvent;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004C1A File Offset: 0x00002E1A
		private static void __instance_didFinishEvent(MissionLevelScenesTransitionSetupDataSO missionLevelScenesTransitionSetupDataSO, MissionCompletionResults missionCompletionResults)
		{
			Plugin.TriggerMissionFinishEvent(missionLevelScenesTransitionSetupDataSO, missionCompletionResults);
		}
	}
}
