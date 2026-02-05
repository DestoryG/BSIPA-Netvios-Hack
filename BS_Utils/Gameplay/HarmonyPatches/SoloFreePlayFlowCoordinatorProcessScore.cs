using System;
using HarmonyLib;

namespace BS_Utils.Gameplay.HarmonyPatches
{
	// Token: 0x02000011 RID: 17
	[HarmonyPatch(typeof(SoloFreePlayFlowCoordinator))]
	[HarmonyPatch("ProcessScore", MethodType.Normal)]
	internal class SoloFreePlayFlowCoordinatorProcessScore
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x00004CF0 File Offset: 0x00002EF0
		private static bool Prefix()
		{
			return !ScoreSubmission.WasDisabled && !ScoreSubmission.disabled && !ScoreSubmission.prolongedDisable;
		}
	}
}
