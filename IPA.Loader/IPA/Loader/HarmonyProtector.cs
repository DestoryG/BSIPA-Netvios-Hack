using System;
using System.Reflection;
using HarmonyLib;

namespace IPA.Loader
{
	// Token: 0x02000040 RID: 64
	internal static class HarmonyProtector
	{
		// Token: 0x0600018F RID: 399 RVA: 0x000067C4 File Offset: 0x000049C4
		public static void Protect(Harmony inst = null)
		{
			HarmonyProtector.selfAssem = Assembly.GetExecutingAssembly();
			HarmonyProtector.harmonyAssem = typeof(Harmony).Assembly;
			if (inst == null)
			{
				if (HarmonyProtector.instance == null)
				{
					HarmonyProtector.instance = new Harmony("BSIPA Safeguard");
				}
				inst = HarmonyProtector.instance;
			}
			MethodInfo target = typeof(PatchProcessor).GetMethod("Patch");
			MethodInfo patch = typeof(HarmonyProtector).GetMethod("PatchProcessor_Patch_Prefix", BindingFlags.Static | BindingFlags.NonPublic);
			inst.Patch(target, new HarmonyMethod(patch), null, null, null);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000684C File Offset: 0x00004A4C
		private static bool PatchProcessor_Patch_Prefix(MethodBase ___original, out MethodInfo __result)
		{
			Assembly asm = ___original.DeclaringType.Assembly;
			__result = ___original as MethodInfo;
			return !asm.Equals(HarmonyProtector.selfAssem) && !asm.Equals(HarmonyProtector.harmonyAssem);
		}

		// Token: 0x04000094 RID: 148
		private static Harmony instance;

		// Token: 0x04000095 RID: 149
		private static Assembly selfAssem;

		// Token: 0x04000096 RID: 150
		private static Assembly harmonyAssem;
	}
}
