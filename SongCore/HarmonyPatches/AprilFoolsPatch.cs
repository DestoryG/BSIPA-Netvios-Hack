using System;
using BS_Utils.Gameplay;
using HarmonyLib;
using IPA.Utilities;
using SongCore.Utilities;

namespace SongCore.HarmonyPatches
{
	// Token: 0x02000024 RID: 36
	[HarmonyPatch(typeof(GamePause))]
	[HarmonyPatch("Pause")]
	internal class AprilFoolsPatch
	{
		// Token: 0x0600018F RID: 399 RVA: 0x00007B20 File Offset: 0x00005D20
		private static void Prefix()
		{
			try
			{
				GetUserInfo.GetUserID();
			}
			catch (Exception)
			{
				Logging.logger.Error("Error in Bunbundai Contingency");
			}
			DateTime dateTime;
			if (IPA.Utilities.Utils.CanUseDateTimeNowSafely)
			{
				dateTime = DateTime.Now;
			}
			else
			{
				dateTime = DateTime.UtcNow;
			}
			if (dateTime.Month == 4 && dateTime.Day == 1)
			{
				ScoreSubmission.DisableSubmission("Nice Pause Idjot");
			}
		}
	}
}
