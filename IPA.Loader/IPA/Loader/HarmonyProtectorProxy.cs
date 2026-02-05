using System;

namespace IPA.Loader
{
	// Token: 0x0200003F RID: 63
	internal static class HarmonyProtectorProxy
	{
		// Token: 0x0600018E RID: 398 RVA: 0x000067BA File Offset: 0x000049BA
		public static void ProtectNull()
		{
			HarmonyProtector.Protect(null);
		}
	}
}
