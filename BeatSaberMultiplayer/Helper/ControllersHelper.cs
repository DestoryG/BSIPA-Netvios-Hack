using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valve.VR;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x02000075 RID: 117
	internal static class ControllersHelper
	{
		// Token: 0x06000867 RID: 2151 RVA: 0x00023EF8 File Offset: 0x000220F8
		public static void Init()
		{
			if (ControllersHelper.platformHelper == null)
			{
				ControllersHelper.platformHelper = Resources.FindObjectsOfTypeAll<VRPlatformHelper>().FirstOrDefault<VRPlatformHelper>();
			}
			if (ControllersHelper.platformHelper.currentXRDeviceModel == 1)
			{
				ControllersHelper.platform = 1;
			}
			else if (ControllersHelper.platformHelper.vrPlatformSDK == 1)
			{
				ControllersHelper.platform = 1;
			}
			else if (ControllersHelper.platformHelper.vrPlatformSDK == null)
			{
				ControllersHelper.platform = 0;
			}
			else if (Environment.CommandLine.Contains("fpfc") && ControllersHelper.platformHelper.vrPlatformSDK == 2)
			{
				ControllersHelper.platform = 2;
			}
			else
			{
				ControllersHelper.platform = -1;
			}
			ControllersHelper.initialized = true;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00023F94 File Offset: 0x00022194
		public static bool GetRightGrip()
		{
			if (!ControllersHelper.initialized)
			{
				ControllersHelper.Init();
			}
			switch (ControllersHelper.platform)
			{
			case 0:
				return ControllersHelper.OpenVRRightTrigger();
			case 1:
				return ControllersHelper.OculusRightTrigger();
			case 2:
				return Input.GetKey(104);
			default:
				return false;
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00023FDC File Offset: 0x000221DC
		public static bool GetLeftGrip()
		{
			if (!ControllersHelper.initialized)
			{
				ControllersHelper.Init();
			}
			switch (ControllersHelper.platform)
			{
			case 0:
				return ControllersHelper.OpenVRLeftTrigger();
			case 1:
				return ControllersHelper.OculusLeftTrigger();
			case 2:
				return Input.GetKey(104);
			default:
				return false;
			}
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00024024 File Offset: 0x00022224
		private static bool OculusRightTrigger()
		{
			Debug.Log("===OculusRightTrigger===");
			return OVRInput.Get(4, 2) > 0.85f;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0002403E File Offset: 0x0002223E
		private static bool OculusLeftTrigger()
		{
			Debug.Log("===OculusLeftTrigger===");
			return OVRInput.Get(4, 1) > 0.85f;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00024058 File Offset: 0x00022258
		private static bool OpenVRLeftTrigger()
		{
			Debug.Log("===OpenVRLeftTrigger===");
			if (ControllersHelper._leftControllerIndex == -1)
			{
				ControllersHelper._leftControllerIndex = SteamVR_Controller.GetDeviceIndex(1, 2, 0);
			}
			Debug.Log(string.Format("===_leftControllerIndex: {0}", ControllersHelper._leftControllerIndex));
			ControllersHelper.ControllerLog(ControllersHelper._leftControllerIndex);
			return SteamVR_Controller.Input(ControllersHelper._leftControllerIndex).GetPress(33);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x000240B8 File Offset: 0x000222B8
		private static bool OpenVRRightTrigger()
		{
			Debug.Log("===OpenVRRightTrigger===");
			if (ControllersHelper._rightControllerIndex == -1)
			{
				ControllersHelper._rightControllerIndex = SteamVR_Controller.GetDeviceIndex(2, 2, 0);
			}
			Debug.Log(string.Format("===_rightControllerIndex: {0}", ControllersHelper._rightControllerIndex));
			ControllersHelper.ControllerLog(ControllersHelper._rightControllerIndex);
			return SteamVR_Controller.Input(ControllersHelper._rightControllerIndex).GetPress(33);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00024118 File Offset: 0x00022318
		private static void ControllerLog(int idx)
		{
			Debug.Log("===ControllerLog EVRButtonId ===");
			EVRButtonId[] array = new EVRButtonId[22];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.271560FEECDAC0402928C339ECF5FEEFCD06A0EE81144504B4DDCD9182B02682).FieldHandle);
			foreach (EVRButtonId evrbuttonId in array)
			{
				if (SteamVR_Controller.Input(idx).GetPressDown(evrbuttonId))
				{
					Debug.Log(evrbuttonId.ToString() + " press down");
				}
			}
			Debug.Log("===ControllerLog buttonMasks ===");
			foreach (ulong num in new ulong[] { 1UL, 2UL, 4UL, 4294967296UL, 8589934592UL, 17179869184UL, 34359738368UL, 68719476736UL, 4294967296UL, 8589934592UL })
			{
				if (SteamVR_Controller.Input(idx).GetPress(num))
				{
					Debug.Log(num.ToString() + " press");
				}
			}
		}

		// Token: 0x04000445 RID: 1093
		private static bool initialized = false;

		// Token: 0x04000446 RID: 1094
		private static int platform = -1;

		// Token: 0x04000447 RID: 1095
		private static VRPlatformHelper platformHelper;

		// Token: 0x04000448 RID: 1096
		private static int _leftControllerIndex = -1;

		// Token: 0x04000449 RID: 1097
		private static int _rightControllerIndex = -1;
	}
}
