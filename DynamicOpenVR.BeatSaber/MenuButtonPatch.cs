using System;
using HarmonyLib;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x0200000D RID: 13
	[HarmonyPatch(typeof(VRControllersInputManager))]
	[HarmonyPatch("MenuButton", MethodType.Normal)]
	internal class MenuButtonPatch
	{
		// Token: 0x06000030 RID: 48 RVA: 0x000029C4 File Offset: 0x00000BC4
		[HarmonyPriority(800)]
		public static bool Prefix(ref bool __result)
		{
			try
			{
				__result = Plugin.menu.state;
			}
			catch (Exception)
			{
				__result = false;
			}
			return false;
		}
	}
}
