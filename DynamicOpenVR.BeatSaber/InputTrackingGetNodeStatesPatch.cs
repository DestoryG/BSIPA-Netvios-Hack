using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine.XR;

namespace DynamicOpenVR.BeatSaber
{
	// Token: 0x02000011 RID: 17
	[HarmonyPatch(typeof(InputTracking))]
	[HarmonyPatch("GetNodeStates", MethodType.Normal)]
	internal class InputTrackingGetNodeStatesPatch
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00002AE4 File Offset: 0x00000CE4
		[HarmonyPriority(800)]
		public static void Postfix(List<XRNodeState> nodeStates)
		{
			foreach (XRNodeState xrnodeState in nodeStates.ToList<XRNodeState>())
			{
				XRNode nodeType = xrnodeState.nodeType;
				if (nodeType != 4)
				{
					if (nodeType == 5)
					{
						nodeStates.Remove(xrnodeState);
						XRNodeState xrnodeState2 = default(XRNodeState);
						xrnodeState2.nodeType = 5;
						xrnodeState2.position = Plugin.rightHandPose.pose.position;
						xrnodeState2.rotation = Plugin.rightHandPose.pose.rotation;
						xrnodeState2.tracked = Plugin.rightHandPose.isTracking;
						xrnodeState2.velocity = Plugin.rightHandPose.velocity;
						xrnodeState2.angularVelocity = Plugin.rightHandPose.angularVelocity;
						xrnodeState2.uniqueID = xrnodeState.uniqueID;
						nodeStates.Add(xrnodeState2);
					}
				}
				else
				{
					nodeStates.Remove(xrnodeState);
					XRNodeState xrnodeState2 = default(XRNodeState);
					xrnodeState2.nodeType = 4;
					xrnodeState2.position = Plugin.leftHandPose.pose.position;
					xrnodeState2.rotation = Plugin.leftHandPose.pose.rotation;
					xrnodeState2.tracked = Plugin.leftHandPose.isTracking;
					xrnodeState2.velocity = Plugin.leftHandPose.velocity;
					xrnodeState2.angularVelocity = Plugin.leftHandPose.angularVelocity;
					xrnodeState2.uniqueID = xrnodeState.uniqueID;
					nodeStates.Add(xrnodeState2);
				}
			}
		}
	}
}
