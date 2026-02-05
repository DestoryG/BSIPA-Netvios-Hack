using System;
using HarmonyLib;
using TMPro;

namespace BS_Utils.Gameplay.HarmonyPatches
{
	// Token: 0x02000010 RID: 16
	[HarmonyPatch(typeof(ResultsViewController))]
	[HarmonyPatch("SetDataToUI", MethodType.Normal)]
	internal class ResultsViewControllerSetDataToUI
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x00004C24 File Offset: 0x00002E24
		private static void Postfix(ref TextMeshProUGUI ____clearedDifficultyText, ref TextMeshProUGUI ____failedDifficultyText)
		{
			____clearedDifficultyText.overflowMode = 0;
			____clearedDifficultyText.richText = true;
			____failedDifficultyText.overflowMode = 0;
			____failedDifficultyText.richText = true;
			if (ScoreSubmission.WasDisabled || ScoreSubmission.disabled || ScoreSubmission.prolongedDisable)
			{
				TextMeshProUGUI textMeshProUGUI = ____clearedDifficultyText;
				textMeshProUGUI.text = string.Concat(new string[]
				{
					textMeshProUGUI.text,
					"  \r\n<color=#ff0000ff><size=60%><b>Score Submission Disabled by: ",
					ScoreSubmission.LastDisabledModString,
					" | ",
					ScoreSubmission.ProlongedModString
				});
				textMeshProUGUI = ____failedDifficultyText;
				textMeshProUGUI.text = string.Concat(new string[]
				{
					textMeshProUGUI.text,
					"  \r\n<color=#ff0000ff><size=60%><b>Score Submission Disabled by: ",
					ScoreSubmission.LastDisabledModString,
					" | ",
					ScoreSubmission.ProlongedModString
				});
			}
			ScoreSubmission.ModList.Clear();
			ScoreSubmission.disabled = false;
		}
	}
}
