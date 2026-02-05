using System;

namespace HarmonyLib
{
	// Token: 0x0200004E RID: 78
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HarmonyDebug : HarmonyAttribute
	{
		// Token: 0x0600016B RID: 363 RVA: 0x00009826 File Offset: 0x00007A26
		public HarmonyDebug()
		{
			this.info.debug = new bool?(true);
		}
	}
}
