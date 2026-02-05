using System;
using HarmonyLib;
using SongCore.OverrideClasses;
using SongCore.Utilities;

namespace SongCore.HarmonyPatches
{
	// Token: 0x0200002B RID: 43
	[HarmonyPatch(typeof(LevelFilteringNavigationController))]
	[HarmonyPatch("ReloadSongListIfNeeded", MethodType.Normal)]
	internal class StopVanillaLoadingPatch3
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00007E1A File Offset: 0x0000601A
		private static bool Prefix(ref LevelFilteringNavigationController __instance, ref TabBarViewController ____tabBarViewController)
		{
			object field = __instance.GetField("_customLevelsTabBarData");
			if (field != null)
			{
				string text = "annotatedBeatmapLevelCollections";
				SongCoreBeatmapLevelPackCollectionSO customBeatmapLevelPackCollectionSO = Loader.CustomBeatmapLevelPackCollectionSO;
				field.SetField(text, (customBeatmapLevelPackCollectionSO != null) ? customBeatmapLevelPackCollectionSO.beatmapLevelPacks : null);
			}
			return false;
		}
	}
}
