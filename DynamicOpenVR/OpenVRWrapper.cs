using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DynamicOpenVR.Exceptions;
using DynamicOpenVR.Logging;
using Valve.VR;

namespace DynamicOpenVR
{
	// Token: 0x020000CB RID: 203
	internal static class OpenVRWrapper
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000594F File Offset: 0x00003B4F
		internal static bool isRuntimeInstalled
		{
			get
			{
				return OpenVR.IsRuntimeInstalled();
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00005958 File Offset: 0x00003B58
		internal static void SetActionManifestPath(string manifestPath)
		{
			Logger.Info("Setting action manifest path to '" + manifestPath + "'");
			EVRInputError evrinputError = OpenVR.Input.SetActionManifestPath(manifestPath);
			if (evrinputError != EVRInputError.None)
			{
				throw new OpenVRInputException(string.Format("Could not set action manifest path: {0}", evrinputError), evrinputError);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000059A0 File Offset: 0x00003BA0
		internal static ulong GetActionSetHandle(string actionSetName)
		{
			ulong num = 0UL;
			EVRInputError actionSetHandle = OpenVR.Input.GetActionSetHandle(actionSetName, ref num);
			if (actionSetHandle != EVRInputError.None)
			{
				throw new OpenVRInputException(string.Format("Could not get handle for action set '{0}': {1}", actionSetName, actionSetHandle), actionSetHandle);
			}
			return num;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000059DC File Offset: 0x00003BDC
		internal static ulong GetActionHandle(string actionName)
		{
			ulong num = 0UL;
			EVRInputError actionHandle = OpenVR.Input.GetActionHandle(actionName, ref num);
			if (actionHandle != EVRInputError.None)
			{
				throw new OpenVRInputException(string.Format("Could not get handle for action '{0}': {1}", actionName, actionHandle), actionHandle);
			}
			return num;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00005A18 File Offset: 0x00003C18
		internal static void UpdateActionState(List<ulong> handles)
		{
			VRActiveActionSet_t[] array = new VRActiveActionSet_t[handles.Count];
			for (int i = 0; i < handles.Count; i++)
			{
				array[i] = new VRActiveActionSet_t
				{
					ulActionSet = handles[i],
					ulRestrictedToDevice = 0UL
				};
			}
			EVRInputError evrinputError = OpenVR.Input.UpdateActionState(array, (uint)Marshal.SizeOf(typeof(VRActiveActionSet_t)));
			if (evrinputError != EVRInputError.None && evrinputError != EVRInputError.NoData)
			{
				throw new OpenVRInputException(string.Format("Could not update action states: {0}", evrinputError), evrinputError);
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00005AA4 File Offset: 0x00003CA4
		internal static InputAnalogActionData_t GetAnalogActionData(ulong actionHandle)
		{
			InputAnalogActionData_t inputAnalogActionData_t = default(InputAnalogActionData_t);
			EVRInputError analogActionData = OpenVR.Input.GetAnalogActionData(actionHandle, ref inputAnalogActionData_t, (uint)Marshal.SizeOf(typeof(InputAnalogActionData_t)), 0UL);
			if (analogActionData != EVRInputError.None && analogActionData != EVRInputError.NoData)
			{
				throw new OpenVRInputException(string.Format("Could not get analog data for action with handle {0}: {1}", actionHandle, analogActionData), analogActionData);
			}
			return inputAnalogActionData_t;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00005B00 File Offset: 0x00003D00
		internal static InputDigitalActionData_t GetDigitalActionData(ulong actionHandle)
		{
			InputDigitalActionData_t inputDigitalActionData_t = default(InputDigitalActionData_t);
			EVRInputError digitalActionData = OpenVR.Input.GetDigitalActionData(actionHandle, ref inputDigitalActionData_t, (uint)Marshal.SizeOf(typeof(InputDigitalActionData_t)), 0UL);
			if (digitalActionData != EVRInputError.None && digitalActionData != EVRInputError.NoData)
			{
				throw new OpenVRInputException(string.Format("Could not get digital data for action with handle {0}: {1}", actionHandle, digitalActionData), digitalActionData);
			}
			return inputDigitalActionData_t;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00005B5C File Offset: 0x00003D5C
		internal static InputSkeletalActionData_t GetSkeletalActionData(ulong actionHandle)
		{
			InputSkeletalActionData_t inputSkeletalActionData_t = default(InputSkeletalActionData_t);
			EVRInputError skeletalActionData = OpenVR.Input.GetSkeletalActionData(actionHandle, ref inputSkeletalActionData_t, (uint)Marshal.SizeOf(typeof(InputSkeletalActionData_t)));
			if (skeletalActionData != EVRInputError.None && skeletalActionData != EVRInputError.NoData)
			{
				throw new OpenVRInputException(string.Format("Could not get skeletal data for action with handle {0}: {1}", actionHandle, skeletalActionData), skeletalActionData);
			}
			return inputSkeletalActionData_t;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00005BB4 File Offset: 0x00003DB4
		internal static InputPoseActionData_t GetPoseActionData(ulong actionHandle, ETrackingUniverseOrigin origin = ETrackingUniverseOrigin.TrackingUniverseStanding)
		{
			InputPoseActionData_t inputPoseActionData_t = default(InputPoseActionData_t);
			EVRInputError poseActionData = OpenVR.Input.GetPoseActionData(actionHandle, origin, 0f, ref inputPoseActionData_t, (uint)Marshal.SizeOf(typeof(InputPoseActionData_t)), 0UL);
			if (poseActionData != EVRInputError.None && poseActionData != EVRInputError.NoData)
			{
				throw new OpenVRInputException(string.Format("Could not get pose data for action with handle {0}: {1}", actionHandle, poseActionData), poseActionData);
			}
			return inputPoseActionData_t;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00005C14 File Offset: 0x00003E14
		internal static VRSkeletalSummaryData_t GetSkeletalSummaryData(ulong actionHandle)
		{
			VRSkeletalSummaryData_t vrskeletalSummaryData_t = default(VRSkeletalSummaryData_t);
			EVRInputError skeletalSummaryData = OpenVR.Input.GetSkeletalSummaryData(actionHandle, ref vrskeletalSummaryData_t);
			if (skeletalSummaryData != EVRInputError.None && skeletalSummaryData != EVRInputError.NoData)
			{
				throw new OpenVRInputException(string.Format("Could not get skeletal summary data for action with handle {0}: {1}", actionHandle, skeletalSummaryData), skeletalSummaryData);
			}
			return vrskeletalSummaryData_t;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005C60 File Offset: 0x00003E60
		internal static void TriggerHapticVibrationAction(ulong actionHandle, float startSecondsFromNow, float durationSeconds, float frequency, float amplitude)
		{
			EVRInputError evrinputError = OpenVR.Input.TriggerHapticVibrationAction(actionHandle, startSecondsFromNow, durationSeconds, frequency, amplitude, 0UL);
			if (evrinputError != EVRInputError.None && evrinputError != EVRInputError.NoData)
			{
				throw new OpenVRInputException(string.Format("Failed to trigger haptic feedback vibration for action with handle {0}: {1}", actionHandle, evrinputError), evrinputError);
			}
		}
	}
}
