using System;
using HarmonyLib;

namespace SongCore.HarmonyPatches
{
	// Token: 0x02000023 RID: 35
	[HarmonyPatch(typeof(BeatmapDifficultyMethods))]
	[HarmonyPatch("Name", MethodType.Normal)]
	public class BeatmapDifficultyMethodsName
	{
		// Token: 0x0600018D RID: 397 RVA: 0x000079C0 File Offset: 0x00005BC0
		private static void Postfix(BeatmapDifficulty difficulty, ref string __result)
		{
			if (difficulty == null && StandardLevelDetailViewRefreshContent.currentLabels.EasyOverride != "")
			{
				__result = StandardLevelDetailViewRefreshContent.currentLabels.EasyOverride.Replace("<", "<\u200b").Replace(">", ">\u200b");
			}
			if (difficulty == 1 && StandardLevelDetailViewRefreshContent.currentLabels.NormalOverride != "")
			{
				__result = StandardLevelDetailViewRefreshContent.currentLabels.NormalOverride.Replace("<", "<\u200b").Replace(">", ">\u200b");
			}
			if (difficulty == 2 && StandardLevelDetailViewRefreshContent.currentLabels.HardOverride != "")
			{
				__result = StandardLevelDetailViewRefreshContent.currentLabels.HardOverride.Replace("<", "<\u200b").Replace(">", ">\u200b");
			}
			if (difficulty == 3 && StandardLevelDetailViewRefreshContent.currentLabels.ExpertOverride != "")
			{
				__result = StandardLevelDetailViewRefreshContent.currentLabels.ExpertOverride.Replace("<", "<\u200b").Replace(">", ">\u200b");
			}
			if (difficulty == 4 && StandardLevelDetailViewRefreshContent.currentLabels.ExpertPlusOverride != "")
			{
				__result = StandardLevelDetailViewRefreshContent.currentLabels.ExpertPlusOverride.Replace("<", "<\u200b").Replace(">", ">\u200b");
			}
		}
	}
}
