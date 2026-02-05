using System;
using System.Linq;
using HarmonyLib;

namespace SongCore.HarmonyPatches
{
	// Token: 0x02000026 RID: 38
	[HarmonyPatch(typeof(BeatmapCharacteristicCollectionSO))]
	[HarmonyPatch("GetBeatmapCharacteristicBySerializedName", MethodType.Normal)]
	internal class CustomCharacteristicsPatch
	{
		// Token: 0x06000193 RID: 403 RVA: 0x00007BCC File Offset: 0x00005DCC
		private static void Postfix(string serializedName, ref BeatmapCharacteristicSO __result)
		{
			if (__result == null)
			{
				if (Collections.customCharacteristics.Any((BeatmapCharacteristicSO x) => x.serializedName == serializedName))
				{
					__result = Collections.customCharacteristics.FirstOrDefault((BeatmapCharacteristicSO x) => x.serializedName == serializedName);
					return;
				}
				__result = Collections.customCharacteristics.FirstOrDefault((BeatmapCharacteristicSO x) => x.serializedName == "MissingCharacteristic");
			}
		}
	}
}
