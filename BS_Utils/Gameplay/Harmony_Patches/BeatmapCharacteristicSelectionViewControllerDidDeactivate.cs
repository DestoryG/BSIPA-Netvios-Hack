using System;
using HarmonyLib;

namespace BS_Utils.Gameplay.Harmony_Patches
{
	// Token: 0x02000013 RID: 19
	[HarmonyPatch(typeof(BeatmapCharacteristicSegmentedControlController))]
	[HarmonyPatch("SetData", MethodType.Normal)]
	internal class BeatmapCharacteristicSelectionViewControllerDidDeactivate
	{
		// Token: 0x060000ED RID: 237 RVA: 0x00004D1F File Offset: 0x00002F1F
		private static void Postfix(BeatmapCharacteristicSegmentedControlController __instance, IDifficultyBeatmapSet[] difficultyBeatmapSets, BeatmapCharacteristicSO selectedBeatmapCharacteristic)
		{
			Gamemode.CharacteristicSelectionViewController_didSelectBeatmapCharacteristicEvent(__instance, __instance.selectedBeatmapCharacteristic);
		}
	}
}
