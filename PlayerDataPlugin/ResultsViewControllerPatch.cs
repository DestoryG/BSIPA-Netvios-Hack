using System;
using HarmonyLib;
using PlayerDataPlugin.BSHandler;

namespace PlayerDataPlugin
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(ResultsViewController))]
	[HarmonyPatch("Init", MethodType.Normal)]
	internal class ResultsViewControllerPatch
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002239 File Offset: 0x00000439
		[HarmonyPriority(800)]
		public static bool Prefix(LevelCompletionResults levelCompletionResults, IDifficultyBeatmap difficultyBeatmap, bool practice, bool newHighScore)
		{
			Singleton<GameEventController>.Instance.SetLevelCompletionResultsAndDifficultyBeatmap(levelCompletionResults, difficultyBeatmap);
			return true;
		}
	}
}
