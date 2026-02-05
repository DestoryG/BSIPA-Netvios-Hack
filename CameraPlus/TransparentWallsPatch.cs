using System;
using HarmonyLib;
using UnityEngine;

namespace CameraPlus
{
	// Token: 0x02000011 RID: 17
	[HarmonyPatch(typeof(ObstacleController))]
	[HarmonyPatch("Init", MethodType.Normal)]
	public class TransparentWallsPatch
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00007DC0 File Offset: 0x00005FC0
		private static void Postfix(ref ObstacleController __instance)
		{
			Camera.main.cullingMask |= 1 << TransparentWallsPatch.WallLayerMask;
			GameObject gameObject = __instance.gameObject;
			Renderer renderer = ((gameObject != null) ? gameObject.GetComponentInChildren<Renderer>(false) : null);
			if ((renderer != null) ? renderer.gameObject : null)
			{
				renderer.gameObject.layer = TransparentWallsPatch.WallLayerMask;
			}
		}

		// Token: 0x0400009A RID: 154
		public static int WallLayerMask = 25;
	}
}
