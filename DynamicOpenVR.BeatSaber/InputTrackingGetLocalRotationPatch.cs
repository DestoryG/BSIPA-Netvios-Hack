using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x02000010 RID: 16
	[HarmonyPatch(typeof(InputTracking))]
	[HarmonyPatch("GetLocalRotation", MethodType.Normal)]
	internal class InputTrackingGetLocalRotationPatch
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00002AA1 File Offset: 0x00000CA1
		[HarmonyPriority(800)]
		public static bool Prefix(XRNode node, ref Quaternion __result)
		{
			if (node == 4)
			{
				__result = Plugin.leftHandPose.pose.rotation;
				return false;
			}
			if (node == 5)
			{
				__result = Plugin.rightHandPose.pose.rotation;
				return false;
			}
			return true;
		}
	}
}
