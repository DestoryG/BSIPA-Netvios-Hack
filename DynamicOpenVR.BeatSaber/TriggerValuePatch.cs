using System;
using HarmonyLib;
using UnityEngine.XR;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x0200000B RID: 11
	[HarmonyPatch(typeof(VRControllersInputManager))]
	[HarmonyPatch("TriggerValue", MethodType.Normal)]
	internal class TriggerValuePatch
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002934 File Offset: 0x00000B34
		[HarmonyPriority(800)]
		public static bool Prefix(XRNode node, ref float __result)
		{
			try
			{
				if (node == 4)
				{
					__result = Plugin.leftTriggerValue.value;
				}
				else if (node == 5)
				{
					__result = Plugin.rightTriggerValue.value;
				}
			}
			catch (Exception)
			{
				__result = 0f;
			}
			return false;
		}
	}
}
