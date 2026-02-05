using System;

namespace HarmonyLib
{
	// Token: 0x02000049 RID: 73
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class HarmonyReversePatch : HarmonyAttribute
	{
		// Token: 0x06000166 RID: 358 RVA: 0x000097D1 File Offset: 0x000079D1
		public HarmonyReversePatch(HarmonyReversePatchType type = HarmonyReversePatchType.Original)
		{
			this.info.reversePatchType = new HarmonyReversePatchType?(type);
		}
	}
}
