using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HMUI;
using SongCore.Data;
using SongCore.UI;
using SongCore.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SongCore.HarmonyPatches
{
	// Token: 0x0200002C RID: 44
	[HarmonyPatch(typeof(StandardLevelDetailView))]
	[HarmonyPatch("RefreshContent", MethodType.Normal)]
	public class StandardLevelDetailViewRefreshContent
	{
		// Token: 0x0600019F RID: 415 RVA: 0x00007E4C File Offset: 0x0000604C
		internal static void SetCurrentLabels(StandardLevelDetailViewRefreshContent.OverrideLabels labels)
		{
			StandardLevelDetailViewRefreshContent.currentLabels.EasyOverride = labels.EasyOverride;
			StandardLevelDetailViewRefreshContent.currentLabels.NormalOverride = labels.NormalOverride;
			StandardLevelDetailViewRefreshContent.currentLabels.HardOverride = labels.HardOverride;
			StandardLevelDetailViewRefreshContent.currentLabels.ExpertOverride = labels.ExpertOverride;
			StandardLevelDetailViewRefreshContent.currentLabels.ExpertPlusOverride = labels.ExpertPlusOverride;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007EAC File Offset: 0x000060AC
		internal static void clearOverrideLabels()
		{
			StandardLevelDetailViewRefreshContent.currentLabels.EasyOverride = "";
			StandardLevelDetailViewRefreshContent.currentLabels.NormalOverride = "";
			StandardLevelDetailViewRefreshContent.currentLabels.HardOverride = "";
			StandardLevelDetailViewRefreshContent.currentLabels.ExpertOverride = "";
			StandardLevelDetailViewRefreshContent.currentLabels.ExpertPlusOverride = "";
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007F04 File Offset: 0x00006104
		private static void Postfix(ref LevelParamsPanel ____levelParamsPanel, ref IDifficultyBeatmap ____selectedDifficultyBeatmap, ref PlayerData ____playerData, ref TextMeshProUGUI ____songNameText, ref Button ____playButton, ref Button ____practiceButton, ref BeatmapDifficultySegmentedControlController ____beatmapDifficultySegmentedControlController, ref BeatmapCharacteristicSegmentedControlController ____beatmapCharacteristicSegmentedControlController)
		{
			bool flag = false;
			CustomPreviewBeatmapLevel customPreviewBeatmapLevel = ((____selectedDifficultyBeatmap.level is CustomBeatmapLevel) ? (____selectedDifficultyBeatmap.level as CustomPreviewBeatmapLevel) : null);
			if (customPreviewBeatmapLevel != StandardLevelDetailViewRefreshContent.lastLevel)
			{
				flag = true;
				StandardLevelDetailViewRefreshContent.lastLevel = customPreviewBeatmapLevel;
			}
			____playButton.interactable = true;
			____practiceButton.interactable = true;
			____playButton.gameObject.GetComponentInChildren<Image>().color = new Color(0f, 0.706f, 1f, 0.784f);
			____songNameText.text = "<size=78%>" + ____songNameText.text.Replace("<", "<\u200b").Replace(">", ">\u200b");
			____songNameText.richText = true;
			PersistentSingleton<RequirementsUI>.instance.ButtonGlowColor = "none";
			PersistentSingleton<RequirementsUI>.instance.ButtonInteractable = false;
			if (customPreviewBeatmapLevel != null)
			{
				ExtraSongData extraSongData = Collections.RetrieveExtraSongData(Hashing.GetCustomLevelHash(customPreviewBeatmapLevel), customPreviewBeatmapLevel.customLevelPath);
				if (extraSongData == null)
				{
					PersistentSingleton<RequirementsUI>.instance.ButtonGlowColor = "none";
					PersistentSingleton<RequirementsUI>.instance.ButtonInteractable = false;
					return;
				}
				bool flag2 = false;
				IDifficultyBeatmap difficultyBeatmap = ____selectedDifficultyBeatmap;
				ExtraSongData.DifficultyData difficultyData = Collections.RetrieveDifficultyData(difficultyBeatmap);
				if (difficultyData != null)
				{
					if (difficultyData.additionalDifficultyData._requirements.Count<string>() == 0 && difficultyData.additionalDifficultyData._suggestions.Count<string>() == 0 && difficultyData.additionalDifficultyData._warnings.Count<string>() == 0 && difficultyData.additionalDifficultyData._information.Count<string>() == 0 && extraSongData.contributors.Count<ExtraSongData.Contributor>() == 0)
					{
						PersistentSingleton<RequirementsUI>.instance.ButtonGlowColor = "none";
						PersistentSingleton<RequirementsUI>.instance.ButtonInteractable = false;
					}
					else if (difficultyData.additionalDifficultyData._warnings.Count<string>() == 0)
					{
						PersistentSingleton<RequirementsUI>.instance.ButtonGlowColor = "#0000FF";
						PersistentSingleton<RequirementsUI>.instance.ButtonInteractable = true;
					}
					else if (difficultyData.additionalDifficultyData._warnings.Count<string>() > 0)
					{
						PersistentSingleton<RequirementsUI>.instance.ButtonGlowColor = "#FFFF00";
						PersistentSingleton<RequirementsUI>.instance.ButtonInteractable = true;
						if (difficultyData.additionalDifficultyData._warnings.Contains("WIP"))
						{
							____playButton.interactable = false;
							____playButton.gameObject.GetComponentInChildren<Image>().color = Color.yellow;
						}
					}
				}
				if (customPreviewBeatmapLevel.levelID.EndsWith(" WIP"))
				{
					____practiceButton.gameObject.SetActive(true);
					PersistentSingleton<RequirementsUI>.instance.ButtonGlowColor = "#FFFF00";
					PersistentSingleton<RequirementsUI>.instance.ButtonInteractable = true;
					____playButton.interactable = false;
					____playButton.gameObject.GetComponentInChildren<Image>().color = Color.yellow;
					flag2 = true;
				}
				if (difficultyData != null)
				{
					for (int i = 0; i < difficultyData.additionalDifficultyData._requirements.Count<string>(); i++)
					{
						if (!Collections.capabilities.Contains(difficultyData.additionalDifficultyData._requirements[i]))
						{
							____playButton.interactable = false;
							____practiceButton.interactable = false;
							____playButton.gameObject.GetComponentInChildren<Image>().color = Color.red;
							PersistentSingleton<RequirementsUI>.instance.ButtonGlowColor = "#FF0000";
						}
					}
				}
				if (difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName == "MissingCharacteristic")
				{
					____playButton.interactable = false;
					____practiceButton.interactable = false;
					____playButton.gameObject.GetComponentInChildren<Image>().color = Color.red;
					PersistentSingleton<RequirementsUI>.instance.ButtonGlowColor = "#FF0000";
				}
				PersistentSingleton<RequirementsUI>.instance.level = customPreviewBeatmapLevel;
				PersistentSingleton<RequirementsUI>.instance.songData = extraSongData;
				PersistentSingleton<RequirementsUI>.instance.diffData = difficultyData;
				PersistentSingleton<RequirementsUI>.instance.wipFolder = flag2;
				StandardLevelDetailViewRefreshContent.levelLabels.Clear();
				string text = "";
				foreach (ExtraSongData.DifficultyData difficultyData2 in extraSongData._difficulties)
				{
					BeatmapDifficulty difficulty = difficultyData2._difficulty;
					string beatmapCharacteristicName = difficultyData2._beatmapCharacteristicName;
					if (beatmapCharacteristicName == difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName)
					{
						text = beatmapCharacteristicName;
					}
					if (!StandardLevelDetailViewRefreshContent.levelLabels.ContainsKey(beatmapCharacteristicName))
					{
						StandardLevelDetailViewRefreshContent.levelLabels.Add(beatmapCharacteristicName, new StandardLevelDetailViewRefreshContent.OverrideLabels());
					}
					StandardLevelDetailViewRefreshContent.OverrideLabels overrideLabels = StandardLevelDetailViewRefreshContent.levelLabels[beatmapCharacteristicName];
					if (!string.IsNullOrWhiteSpace(difficultyData2._difficultyLabel))
					{
						switch (difficulty)
						{
						case 0:
							overrideLabels.EasyOverride = difficultyData2._difficultyLabel;
							break;
						case 1:
							overrideLabels.NormalOverride = difficultyData2._difficultyLabel;
							break;
						case 2:
							overrideLabels.HardOverride = difficultyData2._difficultyLabel;
							break;
						case 3:
							overrideLabels.ExpertOverride = difficultyData2._difficultyLabel;
							break;
						case 4:
							overrideLabels.ExpertPlusOverride = difficultyData2._difficultyLabel;
							break;
						}
					}
				}
				if (!string.IsNullOrWhiteSpace(text))
				{
					StandardLevelDetailViewRefreshContent.SetCurrentLabels(StandardLevelDetailViewRefreshContent.levelLabels[text]);
				}
				else
				{
					StandardLevelDetailViewRefreshContent.clearOverrideLabels();
				}
				____beatmapDifficultySegmentedControlController.SetData(____selectedDifficultyBeatmap.parentDifficultyBeatmapSet.difficultyBeatmaps, ____beatmapDifficultySegmentedControlController.selectedDifficulty);
				StandardLevelDetailViewRefreshContent.clearOverrideLabels();
				if (extraSongData._defaultCharacteristic != null && flag && ____beatmapCharacteristicSegmentedControlController.selectedBeatmapCharacteristic.serializedName != extraSongData._defaultCharacteristic)
				{
					List<BeatmapCharacteristicSO> field = ____beatmapCharacteristicSegmentedControlController.GetField("_beatmapCharacteristics");
					int num = 0;
					foreach (BeatmapCharacteristicSO beatmapCharacteristicSO in field)
					{
						if (extraSongData._defaultCharacteristic == beatmapCharacteristicSO.serializedName)
						{
							break;
						}
						num++;
					}
					if (num != field.Count)
					{
						____beatmapCharacteristicSegmentedControlController.GetField("_segmentedControl").SelectCellWithNumber(num);
					}
					____beatmapCharacteristicSegmentedControlController.HandleDifficultySegmentedControlDidSelectCell(____beatmapCharacteristicSegmentedControlController.GetField("_segmentedControl"), num);
				}
			}
		}

		// Token: 0x04000094 RID: 148
		public static Dictionary<string, StandardLevelDetailViewRefreshContent.OverrideLabels> levelLabels = new Dictionary<string, StandardLevelDetailViewRefreshContent.OverrideLabels>();

		// Token: 0x04000095 RID: 149
		public static StandardLevelDetailViewRefreshContent.OverrideLabels currentLabels = new StandardLevelDetailViewRefreshContent.OverrideLabels();

		// Token: 0x04000096 RID: 150
		private static IPreviewBeatmapLevel lastLevel;

		// Token: 0x0200005B RID: 91
		public class OverrideLabels
		{
			// Token: 0x04000134 RID: 308
			internal string EasyOverride = "";

			// Token: 0x04000135 RID: 309
			internal string NormalOverride = "";

			// Token: 0x04000136 RID: 310
			internal string HardOverride = "";

			// Token: 0x04000137 RID: 311
			internal string ExpertOverride = "";

			// Token: 0x04000138 RID: 312
			internal string ExpertPlusOverride = "";
		}
	}
}
