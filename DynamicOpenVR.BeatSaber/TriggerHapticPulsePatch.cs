using System;
using HarmonyLib;
using UnityEngine.XR;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x0200000E RID: 14
	[HarmonyPatch(typeof(VRPlatformHelper))]
	[HarmonyPatch("TriggerHapticPulse", MethodType.Normal)]
	internal class TriggerHapticPulsePatch
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00002A00 File Offset: 0x00000C00
		[HarmonyPriority(0)]
		public static bool Prefix(XRNode node, float strength)
		{
			try
			{
				if (node == 4)
				{
					Plugin.leftSlice.TriggerHapticVibration(0.01f, strength, 25f);
				}
				else if (node == 5)
				{
					Plugin.rightSlice.TriggerHapticVibration(0.01f, strength, 25f);
				}
			}
			catch (Exception)
			{
			}
			return false;
		}
	}
}
