using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x0200000F RID: 15
	[HarmonyPatch(typeof(InputTracking))]
	[HarmonyPatch("GetLocalPosition", MethodType.Normal)]
	internal class InputTrackingGetLocalPositionPatch
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002A60 File Offset: 0x00000C60
		[HarmonyPriority(800)]
		public static bool Prefix(XRNode node, ref Vector3 __result)
		{
			if (node == 4)
			{
				__result = Plugin.leftHandPose.pose.position;
				return false;
			}
			if (node == 5)
			{
				__result = Plugin.rightHandPose.pose.position;
				return false;
			}
			return true;
		}
	}
}
