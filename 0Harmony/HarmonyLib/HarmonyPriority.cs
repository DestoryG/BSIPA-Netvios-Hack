using System;

namespace HarmonyLib
{
	// Token: 0x0200004B RID: 75
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyPriority : HarmonyAttribute
	{
		// Token: 0x06000168 RID: 360 RVA: 0x000097EA File Offset: 0x000079EA
		public HarmonyPriority(int priority)
		{
			this.info.priority = priority;
		}
	}
}
