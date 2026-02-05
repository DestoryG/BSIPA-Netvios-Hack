using System;
using HarmonyLib;
using SongCore.Utilities;

namespace SongCore.HarmonyPatches
{
	// Token: 0x02000028 RID: 40
	[HarmonyPatch(typeof(LevelCollectionViewController))]
	[HarmonyPatch("HandleLevelCollectionTableViewDidSelectLevel", MethodType.Normal)]
	internal class LevelPackLevelsSelectedPatch
	{
		// Token: 0x06000197 RID: 407 RVA: 0x00007D9C File Offset: 0x00005F9C
		private static void Prefix(LevelCollectionTableView tableView, IPreviewBeatmapLevel level)
		{
			if (level is CustomPreviewBeatmapLevel)
			{
				CustomPreviewBeatmapLevel customPreviewBeatmapLevel = level as CustomPreviewBeatmapLevel;
				if (customPreviewBeatmapLevel != null)
				{
					Collections.AddSong(Hashing.GetCustomLevelHash(customPreviewBeatmapLevel), customPreviewBeatmapLevel.customLevelPath);
					Collections.SaveExtraSongData();
				}
			}
		}
	}
}
