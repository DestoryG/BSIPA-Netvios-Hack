using System;
using HarmonyLib;

namespace BS_Utils.Gameplay.HarmonyPatches
{
	// Token: 0x02000012 RID: 18
	[HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO))]
	[HarmonyPatch("Finish", MethodType.Normal)]
	internal class StandardLevelScenesTransitionSetupDataSOFinishPatch
	{
		// Token: 0x060000EB RID: 235 RVA: 0x00004D0A File Offset: 0x00002F0A
		private static void Prefix(LevelCompletionResults levelCompletionResults)
		{
			if (ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
			{
				ScoreSubmission.DisableScoreSaberScoreSubmission();
			}
		}
	}
}
