using System;
using HarmonyLib;
using SongCore.OverrideClasses;
using SongCore.Utilities;

namespace SongCore.HarmonyPatches
{
	// Token: 0x0200002A RID: 42
	[HarmonyPatch(typeof(LevelFilteringNavigationController))]
	[HarmonyPatch("InitializeIfNeeded", MethodType.Normal)]
	internal class StopVanillaLoadingPatch2
	{
		// Token: 0x0600019B RID: 411 RVA: 0x00007DEC File Offset: 0x00005FEC
		private static void Postfix(ref LevelFilteringNavigationController __instance, ref TabBarViewController ____tabBarViewController)
		{
			object field = __instance.GetField("_customLevelsTabBarData");
			if (field == null)
			{
				return;
			}
			string text = "annotatedBeatmapLevelCollections";
			SongCoreBeatmapLevelPackCollectionSO customBeatmapLevelPackCollectionSO = Loader.CustomBeatmapLevelPackCollectionSO;
			field.SetField(text, (customBeatmapLevelPackCollectionSO != null) ? customBeatmapLevelPackCollectionSO.beatmapLevelPacks : null);
		}
	}
}
