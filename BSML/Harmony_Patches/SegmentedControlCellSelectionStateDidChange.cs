using System;
using HarmonyLib;
using HMUI;

namespace BeatSaberMarkupLanguage.Harmony_Patches
{
	// Token: 0x0200008F RID: 143
	[HarmonyPatch(typeof(SegmentedControl), "CellSelectionStateDidChange", new Type[] { typeof(SegmentedControlCell) })]
	internal class SegmentedControlCellSelectionStateDidChange
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x0000DD6C File Offset: 0x0000BF6C
		private static bool Prefix(SegmentedControlCell changedCell, ref int ____selectedCellNumber, Action<SegmentedControl, int> ___didSelectCellEvent, SegmentedControl __instance)
		{
			if (____selectedCellNumber == -1)
			{
				____selectedCellNumber = changedCell.cellNumber;
				if (___didSelectCellEvent != null)
				{
					___didSelectCellEvent(__instance, changedCell.cellNumber);
				}
				return false;
			}
			return true;
		}
	}
}
