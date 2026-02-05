using System;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA.Logging;

namespace BeatSaberMultiplayer.OverriddenClasses
{
	// Token: 0x0200006F RID: 111
	internal class HarmonyPatcher
	{
		// Token: 0x02000101 RID: 257
		[HarmonyPatch(typeof(PauseController))]
		[HarmonyPatch("Pause")]
		private class GameplayManagerPausePatch
		{
			// Token: 0x06000B01 RID: 2817 RVA: 0x00029008 File Offset: 0x00027208
			public static void Postfix(PauseController __instance)
			{
				try
				{
					if (Client.Instance.isConnected)
					{
						__instance.GetField("_gamePause").Resume();
					}
				}
				catch (Exception ex)
				{
					IPA.Logging.Logger log = BeatSaberMultiplayer.Logger.log;
					string text = "Exception in Harmony patch PauseController.Pause: ";
					Exception ex2 = ex;
					log.Error(text + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}
	}
}
