using System;
using HarmonyLib;

namespace BS_Utils.Gameplay.HarmonyPatches
{
	// Token: 0x0200000E RID: 14
	[HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), "Init", new Type[]
	{
		typeof(IDifficultyBeatmap),
		typeof(OverrideEnvironmentSettings),
		typeof(ColorScheme),
		typeof(GameplayModifiers),
		typeof(PlayerSpecificSettings),
		typeof(PracticeSettings),
		typeof(string),
		typeof(bool)
	})]
	internal class BlahBlahGrabTheLevelData
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x00004B48 File Offset: 0x00002D48
		private static void Prefix(StandardLevelScenesTransitionSetupDataSO __instance, IDifficultyBeatmap difficultyBeatmap, GameplayModifiers gameplayModifiers, PlayerSpecificSettings playerSpecificSettings, PracticeSettings practiceSettings, string backButtonText, bool useTestNoteCutSoundEffects)
		{
			ScoreSubmission._wasDisabled = false;
			ScoreSubmission.LastDisablers = Array.Empty<string>();
			Plugin.LevelData.GameplayCoreSceneSetupData = new GameplayCoreSceneSetupData(difficultyBeatmap, gameplayModifiers, playerSpecificSettings, practiceSettings, useTestNoteCutSoundEffects);
			Plugin.LevelData.IsSet = true;
			__instance.didFinishEvent -= BlahBlahGrabTheLevelData.__instance_didFinishEvent;
			__instance.didFinishEvent += BlahBlahGrabTheLevelData.__instance_didFinishEvent;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004BAA File Offset: 0x00002DAA
		private static void __instance_didFinishEvent(StandardLevelScenesTransitionSetupDataSO levelScenesTransitionSetupDataSO, LevelCompletionResults levelCompletionResults)
		{
			Plugin.TriggerLevelFinishEvent(levelScenesTransitionSetupDataSO, levelCompletionResults);
		}
	}
}
