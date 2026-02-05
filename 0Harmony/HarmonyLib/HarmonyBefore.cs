using System;

namespace HarmonyLib
{
	// Token: 0x0200004C RID: 76
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyBefore : HarmonyAttribute
	{
		// Token: 0x06000169 RID: 361 RVA: 0x000097FE File Offset: 0x000079FE
		public HarmonyBefore(params string[] before)
		{
			this.info.before = before;
		}
	}
}
