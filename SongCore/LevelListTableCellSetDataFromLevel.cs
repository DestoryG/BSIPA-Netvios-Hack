using System;
using HarmonyLib;
using TMPro;

// Token: 0x02000002 RID: 2
[HarmonyPatch(typeof(LevelListTableCell))]
[HarmonyPatch("SetDataFromLevelAsync", MethodType.Normal)]
public class LevelListTableCellSetDataFromLevel
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private static void Postfix(IPreviewBeatmapLevel level, bool isFavorite, ref TextMeshProUGUI ____authorText)
	{
		if (!(level is CustomPreviewBeatmapLevel))
		{
			return;
		}
		CustomPreviewBeatmapLevel customPreviewBeatmapLevel = level as CustomPreviewBeatmapLevel;
		____authorText.richText = true;
		if (!string.IsNullOrWhiteSpace(customPreviewBeatmapLevel.levelAuthorName))
		{
			____authorText.text = customPreviewBeatmapLevel.songAuthorName + " <size=80%>[" + customPreviewBeatmapLevel.levelAuthorName.Replace("<", "<\u200b").Replace(">", ">\u200b") + "]</size>";
		}
	}
}
