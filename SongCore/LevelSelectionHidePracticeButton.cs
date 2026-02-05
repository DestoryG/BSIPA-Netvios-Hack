using System;
using HarmonyLib;

// Token: 0x02000003 RID: 3
[HarmonyPatch(typeof(LevelSelectionFlowCoordinator))]
[HarmonyPatch("hidePracticeButton", MethodType.Getter)]
public class LevelSelectionHidePracticeButton
{
	// Token: 0x06000003 RID: 3 RVA: 0x000020CA File Offset: 0x000002CA
	[HarmonyPriority(800)]
	public static bool Prefix(ref bool __result)
	{
		__result = true;
		return false;
	}
}
