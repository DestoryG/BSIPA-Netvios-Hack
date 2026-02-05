using System;
using HarmonyLib;
using SongCore.Data;
using SongCore.Utilities;
using UnityEngine;

namespace SongCore.HarmonyPatches
{
	// Token: 0x02000027 RID: 39
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
	[HarmonyPatch("Init", MethodType.Normal)]
	internal class SceneTransitionPatch
	{
		// Token: 0x06000195 RID: 405 RVA: 0x00007C4C File Offset: 0x00005E4C
		private static void Prefix(IDifficultyBeatmap difficultyBeatmap, ref ColorScheme overrideColorScheme)
		{
			EnvironmentInfoSO environmentInfo = BeatmapEnvironmentHelper.GetEnvironmentInfo(difficultyBeatmap);
			ExtraSongData.DifficultyData difficultyData = Collections.RetrieveDifficultyData(difficultyBeatmap);
			ColorScheme colorScheme = overrideColorScheme ?? new ColorScheme(environmentInfo.colorScheme);
			if (difficultyData == null)
			{
				return;
			}
			if ((difficultyData._colorLeft != null || difficultyData._colorRight != null || difficultyData._envColorLeft != null || difficultyData._envColorRight != null || difficultyData._obstacleColor != null) && Plugin.customSongColors)
			{
				Logging.logger.Info("Custom Song Colors On");
				Color color = ((difficultyData._colorLeft == null) ? colorScheme.saberAColor : Utils.ColorFromMapColor(difficultyData._colorLeft));
				Color color2 = ((difficultyData._colorRight == null) ? colorScheme.saberBColor : Utils.ColorFromMapColor(difficultyData._colorRight));
				Color color3 = ((difficultyData._envColorLeft == null) ? ((difficultyData._colorLeft == null) ? colorScheme.environmentColor0 : Utils.ColorFromMapColor(difficultyData._colorLeft)) : Utils.ColorFromMapColor(difficultyData._envColorLeft));
				Color color4 = ((difficultyData._envColorRight == null) ? ((difficultyData._colorRight == null) ? colorScheme.environmentColor1 : Utils.ColorFromMapColor(difficultyData._colorRight)) : Utils.ColorFromMapColor(difficultyData._envColorRight));
				Color color5 = ((difficultyData._obstacleColor == null) ? colorScheme.obstaclesColor : Utils.ColorFromMapColor(difficultyData._obstacleColor));
				ColorScheme colorScheme2 = new ColorScheme("SongCoreMapColorScheme", "SongCore Map Color Scheme", false, color, color2, color3, color4, color5);
				overrideColorScheme = colorScheme2;
			}
		}
	}
}
