using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000013 RID: 19
	public class CVRSystem
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		internal CVRSystem(IntPtr pInterface)
		{
			this.FnTable = (IVRSystem)Marshal.PtrToStructure(pInterface, typeof(IVRSystem));
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002073 File Offset: 0x00000273
		public void GetRecommendedRenderTargetSize(ref uint pnWidth, ref uint pnHeight)
		{
			pnWidth = 0U;
			pnHeight = 0U;
			this.FnTable.GetRecommendedRenderTargetSize(ref pnWidth, ref pnHeight);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000208D File Offset: 0x0000028D
		public HmdMatrix44_t GetProjectionMatrix(EVREye eEye, float fNearZ, float fFarZ)
		{
			return this.FnTable.GetProjectionMatrix(eEye, fNearZ, fFarZ);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020A2 File Offset: 0x000002A2
		public void GetProjectionRaw(EVREye eEye, ref float pfLeft, ref float pfRight, ref float pfTop, ref float pfBottom)
		{
			pfLeft = 0f;
			pfRight = 0f;
			pfTop = 0f;
			pfBottom = 0f;
			this.FnTable.GetProjectionRaw(eEye, ref pfLeft, ref pfRight, ref pfTop, ref pfBottom);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020D9 File Offset: 0x000002D9
		public bool ComputeDistortion(EVREye eEye, float fU, float fV, ref DistortionCoordinates_t pDistortionCoordinates)
		{
			return this.FnTable.ComputeDistortion(eEye, fU, fV, ref pDistortionCoordinates);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F0 File Offset: 0x000002F0
		public HmdMatrix34_t GetEyeToHeadTransform(EVREye eEye)
		{
			return this.FnTable.GetEyeToHeadTransform(eEye);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002103 File Offset: 0x00000303
		public bool GetTimeSinceLastVsync(ref float pfSecondsSinceLastVsync, ref ulong pulFrameCounter)
		{
			pfSecondsSinceLastVsync = 0f;
			pulFrameCounter = 0UL;
			return this.FnTable.GetTimeSinceLastVsync(ref pfSecondsSinceLastVsync, ref pulFrameCounter);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002122 File Offset: 0x00000322
		public int GetD3D9AdapterIndex()
		{
			return this.FnTable.GetD3D9AdapterIndex();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002134 File Offset: 0x00000334
		public void GetDXGIOutputInfo(ref int pnAdapterIndex)
		{
			pnAdapterIndex = 0;
			this.FnTable.GetDXGIOutputInfo(ref pnAdapterIndex);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000214A File Offset: 0x0000034A
		public void GetOutputDevice(ref ulong pnDevice, ETextureType textureType, IntPtr pInstance)
		{
			pnDevice = 0UL;
			this.FnTable.GetOutputDevice(ref pnDevice, textureType, pInstance);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002163 File Offset: 0x00000363
		public bool IsDisplayOnDesktop()
		{
			return this.FnTable.IsDisplayOnDesktop();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002175 File Offset: 0x00000375
		public bool SetDisplayVisibility(bool bIsVisibleOnDesktop)
		{
			return this.FnTable.SetDisplayVisibility(bIsVisibleOnDesktop);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002188 File Offset: 0x00000388
		public void GetDeviceToAbsoluteTrackingPose(ETrackingUniverseOrigin eOrigin, float fPredictedSecondsToPhotonsFromNow, TrackedDevicePose_t[] pTrackedDevicePoseArray)
		{
			this.FnTable.GetDeviceToAbsoluteTrackingPose(eOrigin, fPredictedSecondsToPhotonsFromNow, pTrackedDevicePoseArray, (uint)pTrackedDevicePoseArray.Length);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021A0 File Offset: 0x000003A0
		public void ResetSeatedZeroPose()
		{
			this.FnTable.ResetSeatedZeroPose();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021B2 File Offset: 0x000003B2
		public HmdMatrix34_t GetSeatedZeroPoseToStandingAbsoluteTrackingPose()
		{
			return this.FnTable.GetSeatedZeroPoseToStandingAbsoluteTrackingPose();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021C4 File Offset: 0x000003C4
		public HmdMatrix34_t GetRawZeroPoseToStandingAbsoluteTrackingPose()
		{
			return this.FnTable.GetRawZeroPoseToStandingAbsoluteTrackingPose();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021D6 File Offset: 0x000003D6
		public uint GetSortedTrackedDeviceIndicesOfClass(ETrackedDeviceClass eTrackedDeviceClass, uint[] punTrackedDeviceIndexArray, uint unRelativeToTrackedDeviceIndex)
		{
			return this.FnTable.GetSortedTrackedDeviceIndicesOfClass(eTrackedDeviceClass, punTrackedDeviceIndexArray, (uint)punTrackedDeviceIndexArray.Length, unRelativeToTrackedDeviceIndex);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000021EE File Offset: 0x000003EE
		public EDeviceActivityLevel GetTrackedDeviceActivityLevel(uint unDeviceId)
		{
			return this.FnTable.GetTrackedDeviceActivityLevel(unDeviceId);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002201 File Offset: 0x00000401
		public void ApplyTransform(ref TrackedDevicePose_t pOutputPose, ref TrackedDevicePose_t pTrackedDevicePose, ref HmdMatrix34_t pTransform)
		{
			this.FnTable.ApplyTransform(ref pOutputPose, ref pTrackedDevicePose, ref pTransform);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002216 File Offset: 0x00000416
		public uint GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole unDeviceType)
		{
			return this.FnTable.GetTrackedDeviceIndexForControllerRole(unDeviceType);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002229 File Offset: 0x00000429
		public ETrackedControllerRole GetControllerRoleForTrackedDeviceIndex(uint unDeviceIndex)
		{
			return this.FnTable.GetControllerRoleForTrackedDeviceIndex(unDeviceIndex);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000223C File Offset: 0x0000043C
		public ETrackedDeviceClass GetTrackedDeviceClass(uint unDeviceIndex)
		{
			return this.FnTable.GetTrackedDeviceClass(unDeviceIndex);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000224F File Offset: 0x0000044F
		public bool IsTrackedDeviceConnected(uint unDeviceIndex)
		{
			return this.FnTable.IsTrackedDeviceConnected(unDeviceIndex);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002262 File Offset: 0x00000462
		public bool GetBoolTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetBoolTrackedDeviceProperty(unDeviceIndex, prop, ref pError);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002277 File Offset: 0x00000477
		public float GetFloatTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetFloatTrackedDeviceProperty(unDeviceIndex, prop, ref pError);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000228C File Offset: 0x0000048C
		public int GetInt32TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetInt32TrackedDeviceProperty(unDeviceIndex, prop, ref pError);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000022A1 File Offset: 0x000004A1
		public ulong GetUint64TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetUint64TrackedDeviceProperty(unDeviceIndex, prop, ref pError);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000022B6 File Offset: 0x000004B6
		public HmdMatrix34_t GetMatrix34TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetMatrix34TrackedDeviceProperty(unDeviceIndex, prop, ref pError);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000022CB File Offset: 0x000004CB
		public uint GetArrayTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, uint propType, IntPtr pBuffer, uint unBufferSize, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetArrayTrackedDeviceProperty(unDeviceIndex, prop, propType, pBuffer, unBufferSize, ref pError);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000022E6 File Offset: 0x000004E6
		public uint GetStringTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, StringBuilder pchValue, uint unBufferSize, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetStringTrackedDeviceProperty(unDeviceIndex, prop, pchValue, unBufferSize, ref pError);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000022FF File Offset: 0x000004FF
		public string GetPropErrorNameFromEnum(ETrackedPropertyError error)
		{
			return Marshal.PtrToStringAnsi(this.FnTable.GetPropErrorNameFromEnum(error));
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002318 File Offset: 0x00000518
		public bool PollNextEvent(ref VREvent_t pEvent, uint uncbVREvent)
		{
			if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
			{
				VREvent_t_Packed vrevent_t_Packed = default(VREvent_t_Packed);
				CVRSystem.PollNextEventUnion pollNextEventUnion;
				pollNextEventUnion.pPollNextEventPacked = null;
				pollNextEventUnion.pPollNextEvent = this.FnTable.PollNextEvent;
				bool flag = pollNextEventUnion.pPollNextEventPacked(ref vrevent_t_Packed, (uint)Marshal.SizeOf(typeof(VREvent_t_Packed)));
				vrevent_t_Packed.Unpack(ref pEvent);
				return flag;
			}
			return this.FnTable.PollNextEvent(ref pEvent, uncbVREvent);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002398 File Offset: 0x00000598
		public bool PollNextEventWithPose(ETrackingUniverseOrigin eOrigin, ref VREvent_t pEvent, uint uncbVREvent, ref TrackedDevicePose_t pTrackedDevicePose)
		{
			return this.FnTable.PollNextEventWithPose(eOrigin, ref pEvent, uncbVREvent, ref pTrackedDevicePose);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023AF File Offset: 0x000005AF
		public string GetEventTypeNameFromEnum(EVREventType eType)
		{
			return Marshal.PtrToStringAnsi(this.FnTable.GetEventTypeNameFromEnum(eType));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023C7 File Offset: 0x000005C7
		public HiddenAreaMesh_t GetHiddenAreaMesh(EVREye eEye, EHiddenAreaMeshType type)
		{
			return this.FnTable.GetHiddenAreaMesh(eEye, type);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000023DC File Offset: 0x000005DC
		public bool GetControllerState(uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, uint unControllerStateSize)
		{
			if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
			{
				VRControllerState_t_Packed vrcontrollerState_t_Packed = new VRControllerState_t_Packed(pControllerState);
				CVRSystem.GetControllerStateUnion getControllerStateUnion;
				getControllerStateUnion.pGetControllerStatePacked = null;
				getControllerStateUnion.pGetControllerState = this.FnTable.GetControllerState;
				bool flag = getControllerStateUnion.pGetControllerStatePacked(unControllerDeviceIndex, ref vrcontrollerState_t_Packed, (uint)Marshal.SizeOf(typeof(VRControllerState_t_Packed)));
				vrcontrollerState_t_Packed.Unpack(ref pControllerState);
				return flag;
			}
			return this.FnTable.GetControllerState(unControllerDeviceIndex, ref pControllerState, unControllerStateSize);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002464 File Offset: 0x00000664
		public bool GetControllerStateWithPose(ETrackingUniverseOrigin eOrigin, uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, uint unControllerStateSize, ref TrackedDevicePose_t pTrackedDevicePose)
		{
			if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
			{
				VRControllerState_t_Packed vrcontrollerState_t_Packed = new VRControllerState_t_Packed(pControllerState);
				CVRSystem.GetControllerStateWithPoseUnion getControllerStateWithPoseUnion;
				getControllerStateWithPoseUnion.pGetControllerStateWithPosePacked = null;
				getControllerStateWithPoseUnion.pGetControllerStateWithPose = this.FnTable.GetControllerStateWithPose;
				bool flag = getControllerStateWithPoseUnion.pGetControllerStateWithPosePacked(eOrigin, unControllerDeviceIndex, ref vrcontrollerState_t_Packed, (uint)Marshal.SizeOf(typeof(VRControllerState_t_Packed)), ref pTrackedDevicePose);
				vrcontrollerState_t_Packed.Unpack(ref pControllerState);
				return flag;
			}
			return this.FnTable.GetControllerStateWithPose(eOrigin, unControllerDeviceIndex, ref pControllerState, unControllerStateSize, ref pTrackedDevicePose);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000024F2 File Offset: 0x000006F2
		public void TriggerHapticPulse(uint unControllerDeviceIndex, uint unAxisId, ushort usDurationMicroSec)
		{
			this.FnTable.TriggerHapticPulse(unControllerDeviceIndex, unAxisId, usDurationMicroSec);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002507 File Offset: 0x00000707
		public string GetButtonIdNameFromEnum(EVRButtonId eButtonId)
		{
			return Marshal.PtrToStringAnsi(this.FnTable.GetButtonIdNameFromEnum(eButtonId));
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000251F File Offset: 0x0000071F
		public string GetControllerAxisTypeNameFromEnum(EVRControllerAxisType eAxisType)
		{
			return Marshal.PtrToStringAnsi(this.FnTable.GetControllerAxisTypeNameFromEnum(eAxisType));
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002537 File Offset: 0x00000737
		public bool IsInputAvailable()
		{
			return this.FnTable.IsInputAvailable();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002549 File Offset: 0x00000749
		public bool IsSteamVRDrawingControllers()
		{
			return this.FnTable.IsSteamVRDrawingControllers();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000255B File Offset: 0x0000075B
		public bool ShouldApplicationPause()
		{
			return this.FnTable.ShouldApplicationPause();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000256D File Offset: 0x0000076D
		public bool ShouldApplicationReduceRenderingWork()
		{
			return this.FnTable.ShouldApplicationReduceRenderingWork();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000257F File Offset: 0x0000077F
		public uint DriverDebugRequest(uint unDeviceIndex, string pchRequest, StringBuilder pchResponseBuffer, uint unResponseBufferSize)
		{
			return this.FnTable.DriverDebugRequest(unDeviceIndex, pchRequest, pchResponseBuffer, unResponseBufferSize);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002596 File Offset: 0x00000796
		public EVRFirmwareError PerformFirmwareUpdate(uint unDeviceIndex)
		{
			return this.FnTable.PerformFirmwareUpdate(unDeviceIndex);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000025A9 File Offset: 0x000007A9
		public void AcknowledgeQuit_Exiting()
		{
			this.FnTable.AcknowledgeQuit_Exiting();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025BB File Offset: 0x000007BB
		public void AcknowledgeQuit_UserPrompt()
		{
			this.FnTable.AcknowledgeQuit_UserPrompt();
		}

		// Token: 0x04000147 RID: 327
		private IVRSystem FnTable;

		// Token: 0x0200022F RID: 559
		// (Invoke) Token: 0x06000770 RID: 1904
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _PollNextEventPacked(ref VREvent_t_Packed pEvent, uint uncbVREvent);

		// Token: 0x02000230 RID: 560
		[StructLayout(LayoutKind.Explicit)]
		private struct PollNextEventUnion
		{
			// Token: 0x040008A8 RID: 2216
			[FieldOffset(0)]
			public IVRSystem._PollNextEvent pPollNextEvent;

			// Token: 0x040008A9 RID: 2217
			[FieldOffset(0)]
			public CVRSystem._PollNextEventPacked pPollNextEventPacked;
		}

		// Token: 0x02000231 RID: 561
		// (Invoke) Token: 0x06000774 RID: 1908
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetControllerStatePacked(uint unControllerDeviceIndex, ref VRControllerState_t_Packed pControllerState, uint unControllerStateSize);

		// Token: 0x02000232 RID: 562
		[StructLayout(LayoutKind.Explicit)]
		private struct GetControllerStateUnion
		{
			// Token: 0x040008AA RID: 2218
			[FieldOffset(0)]
			public IVRSystem._GetControllerState pGetControllerState;

			// Token: 0x040008AB RID: 2219
			[FieldOffset(0)]
			public CVRSystem._GetControllerStatePacked pGetControllerStatePacked;
		}

		// Token: 0x02000233 RID: 563
		// (Invoke) Token: 0x06000778 RID: 1912
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetControllerStateWithPosePacked(ETrackingUniverseOrigin eOrigin, uint unControllerDeviceIndex, ref VRControllerState_t_Packed pControllerState, uint unControllerStateSize, ref TrackedDevicePose_t pTrackedDevicePose);

		// Token: 0x02000234 RID: 564
		[StructLayout(LayoutKind.Explicit)]
		private struct GetControllerStateWithPoseUnion
		{
			// Token: 0x040008AC RID: 2220
			[FieldOffset(0)]
			public IVRSystem._GetControllerStateWithPose pGetControllerStateWithPose;

			// Token: 0x040008AD RID: 2221
			[FieldOffset(0)]
			public CVRSystem._GetControllerStateWithPosePacked pGetControllerStateWithPosePacked;
		}
	}
}
