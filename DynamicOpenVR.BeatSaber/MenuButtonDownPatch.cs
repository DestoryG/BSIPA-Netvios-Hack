using System;
using HarmonyLib;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x0200000C RID: 12
	[HarmonyPatch(typeof(VRControllersInputManager))]
	[HarmonyPatch("MenuButtonDown", MethodType.Normal)]
	internal class MenuButtonDownPatch
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00002988 File Offset: 0x00000B88
		[HarmonyPriority(800)]
		public static bool Prefix(ref bool __result)
		{
			try
			{
				__result = Plugin.menu.activeChange;
			}
			catch (Exception)
			{
				__result = false;
			}
			return false;
		}
	}
}
