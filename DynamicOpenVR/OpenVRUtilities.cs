using System;
using DynamicOpenVR.Exceptions;
using UnityEngine.XR;
using Valve.VR;

namespace DynamicOpenVR
{
	// Token: 0x020000CA RID: 202
	public static class OpenVRUtilities
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600019E RID: 414 RVA: 0x000058C9 File Offset: 0x00003AC9
		// (set) Token: 0x0600019F RID: 415 RVA: 0x000058D0 File Offset: 0x00003AD0
		public static bool isInitialized { get; private set; }

		// Token: 0x060001A0 RID: 416 RVA: 0x000058D8 File Offset: 0x00003AD8
		public static void Init()
		{
			if (string.Compare(XRSettings.loadedDeviceName, "OpenVR", StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				throw new OpenVRInitException("OpenVR is not the selected VR SDK (" + XRSettings.loadedDeviceName + ")");
			}
			if (!OpenVRWrapper.isRuntimeInstalled)
			{
				throw new OpenVRInitException("OpenVR runtime is not installed");
			}
			EVRInitError evrinitError = EVRInitError.None;
			bool flag = OpenVR.Init(ref evrinitError, EVRApplicationType.VRApplication_Scene, "") != null;
			if (evrinitError != EVRInitError.None)
			{
				throw new OpenVRInitException(evrinitError);
			}
			if (!flag)
			{
				throw new OpenVRInitException("OpenVR.Init returned null");
			}
			OpenVRUtilities.isInitialized = true;
		}
	}
}
