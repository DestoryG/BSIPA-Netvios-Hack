using System;
using HarmonyLib;
using SongCore.Utilities;

namespace SongCore.HarmonyPatches
{
	// Token: 0x02000025 RID: 37
	[HarmonyPatch(typeof(LevelSelectionFlowCoordinator))]
	[HarmonyPatch("StartLevel")]
	internal class LevelSelectionFlowCoordinatorStartLevelPatch
	{
		// Token: 0x06000191 RID: 401 RVA: 0x00007B94 File Offset: 0x00005D94
		private static void Prefix(IDifficultyBeatmap difficultyBeatmap)
		{
			if (Collections.RetrieveDifficultyData(difficultyBeatmap) != null)
			{
				if (Plugin.PlatformsInstalled)
				{
					Logging.logger.Info("Checking Custom Environment before song is loaded");
					Plugin.CheckCustomSongEnvironment(difficultyBeatmap);
					return;
				}
			}
			else
			{
				Logging.logger.Info("Null custom song extra data");
			}
		}
	}
}
