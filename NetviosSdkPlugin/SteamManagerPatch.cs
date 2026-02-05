using System;
using HarmonyLib;

namespace NetviosSdkPlugin
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(SteamManager))]
	[HarmonyPatch("Awake", MethodType.Normal)]
	internal class SteamManagerPatch
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[HarmonyPriority(800)]
		public static bool Prefix()
		{
			return false;
		}
	}
}
