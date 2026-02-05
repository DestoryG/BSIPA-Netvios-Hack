using System;

namespace HarmonyLib
{
	// Token: 0x0200004D RID: 77
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyAfter : HarmonyAttribute
	{
		// Token: 0x0600016A RID: 362 RVA: 0x00009812 File Offset: 0x00007A12
		public HarmonyAfter(params string[] after)
		{
			this.info.after = after;
		}
	}
}
