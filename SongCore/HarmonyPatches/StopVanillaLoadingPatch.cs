using System;
using System.Linq;
using System.Threading;
using HarmonyLib;
using SongCore.Utilities;
using UnityEngine;

namespace SongCore.HarmonyPatches
{
	// Token: 0x02000029 RID: 41
	[HarmonyPatch(typeof(BeatmapLevelsModel))]
	[HarmonyPatch("GetCustomLevelPackCollectionAsync", MethodType.Normal)]
	internal class StopVanillaLoadingPatch
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00007DD1 File Offset: 0x00005FD1
		private static void Prefix()
		{
			Resources.FindObjectsOfTypeAll<LevelFilteringNavigationController>().First<LevelFilteringNavigationController>().GetField("_cancellationTokenSource")
				.Cancel();
		}
	}
}
