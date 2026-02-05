using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000010 RID: 16
	public struct IVRInput
	{
		// Token: 0x04000124 RID: 292
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._SetActionManifestPath SetActionManifestPath;

		// Token: 0x04000125 RID: 293
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetActionSetHandle GetActionSetHandle;

		// Token: 0x04000126 RID: 294
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetActionHandle GetActionHandle;

		// Token: 0x04000127 RID: 295
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetInputSourceHandle GetInputSourceHandle;

		// Token: 0x04000128 RID: 296
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._UpdateActionState UpdateActionState;

		// Token: 0x04000129 RID: 297
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetDigitalActionData GetDigitalActionData;

		// Token: 0x0400012A RID: 298
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetAnalogActionData GetAnalogActionData;

		// Token: 0x0400012B RID: 299
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetPoseActionData GetPoseActionData;

		// Token: 0x0400012C RID: 300
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetSkeletalActionData GetSkeletalActionData;

		// Token: 0x0400012D RID: 301
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetBoneCount GetBoneCount;

		// Token: 0x0400012E RID: 302
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetBoneHierarchy GetBoneHierarchy;

		// Token: 0x0400012F RID: 303
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetBoneName GetBoneName;

		// Token: 0x04000130 RID: 304
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetSkeletalReferenceTransforms GetSkeletalReferenceTransforms;

		// Token: 0x04000131 RID: 305
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetSkeletalTrackingLevel GetSkeletalTrackingLevel;

		// Token: 0x04000132 RID: 306
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetSkeletalBoneData GetSkeletalBoneData;

		// Token: 0x04000133 RID: 307
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetSkeletalSummaryData GetSkeletalSummaryData;

		// Token: 0x04000134 RID: 308
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetSkeletalBoneDataCompressed GetSkeletalBoneDataCompressed;

		// Token: 0x04000135 RID: 309
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._DecompressSkeletalBoneData DecompressSkeletalBoneData;

		// Token: 0x04000136 RID: 310
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._TriggerHapticVibrationAction TriggerHapticVibrationAction;

		// Token: 0x04000137 RID: 311
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetActionOrigins GetActionOrigins;

		// Token: 0x04000138 RID: 312
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetOriginLocalizedName GetOriginLocalizedName;

		// Token: 0x04000139 RID: 313
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._GetOriginTrackedDeviceInfo GetOriginTrackedDeviceInfo;

		// Token: 0x0400013A RID: 314
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._ShowActionOrigins ShowActionOrigins;

		// Token: 0x0400013B RID: 315
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._ShowBindingsForActionSet ShowBindingsForActionSet;

		// Token: 0x0400013C RID: 316
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRInput._IsUsingLegacyInput IsUsingLegacyInput;

		// Token: 0x0200020C RID: 524
		// (Invoke) Token: 0x060006E4 RID: 1764
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _SetActionManifestPath(string pchActionManifestPath);

		// Token: 0x0200020D RID: 525
		// (Invoke) Token: 0x060006E8 RID: 1768
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetActionSetHandle(string pchActionSetName, ref ulong pHandle);

		// Token: 0x0200020E RID: 526
		// (Invoke) Token: 0x060006EC RID: 1772
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetActionHandle(string pchActionName, ref ulong pHandle);

		// Token: 0x0200020F RID: 527
		// (Invoke) Token: 0x060006F0 RID: 1776
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetInputSourceHandle(string pchInputSourcePath, ref ulong pHandle);

		// Token: 0x02000210 RID: 528
		// (Invoke) Token: 0x060006F4 RID: 1780
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _UpdateActionState([In] [Out] VRActiveActionSet_t[] pSets, uint unSizeOfVRSelectedActionSet_t, uint unSetCount);

		// Token: 0x02000211 RID: 529
		// (Invoke) Token: 0x060006F8 RID: 1784
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetDigitalActionData(ulong action, ref InputDigitalActionData_t pActionData, uint unActionDataSize, ulong ulRestrictToDevice);

		// Token: 0x02000212 RID: 530
		// (Invoke) Token: 0x060006FC RID: 1788
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetAnalogActionData(ulong action, ref InputAnalogActionData_t pActionData, uint unActionDataSize, ulong ulRestrictToDevice);

		// Token: 0x02000213 RID: 531
		// (Invoke) Token: 0x06000700 RID: 1792
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetPoseActionData(ulong action, ETrackingUniverseOrigin eOrigin, float fPredictedSecondsFromNow, ref InputPoseActionData_t pActionData, uint unActionDataSize, ulong ulRestrictToDevice);

		// Token: 0x02000214 RID: 532
		// (Invoke) Token: 0x06000704 RID: 1796
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetSkeletalActionData(ulong action, ref InputSkeletalActionData_t pActionData, uint unActionDataSize);

		// Token: 0x02000215 RID: 533
		// (Invoke) Token: 0x06000708 RID: 1800
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetBoneCount(ulong action, ref uint pBoneCount);

		// Token: 0x02000216 RID: 534
		// (Invoke) Token: 0x0600070C RID: 1804
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetBoneHierarchy(ulong action, [In] [Out] int[] pParentIndices, uint unIndexArayCount);

		// Token: 0x02000217 RID: 535
		// (Invoke) Token: 0x06000710 RID: 1808
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetBoneName(ulong action, int nBoneIndex, StringBuilder pchBoneName, uint unNameBufferSize);

		// Token: 0x02000218 RID: 536
		// (Invoke) Token: 0x06000714 RID: 1812
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetSkeletalReferenceTransforms(ulong action, EVRSkeletalTransformSpace eTransformSpace, EVRSkeletalReferencePose eReferencePose, [In] [Out] VRBoneTransform_t[] pTransformArray, uint unTransformArrayCount);

		// Token: 0x02000219 RID: 537
		// (Invoke) Token: 0x06000718 RID: 1816
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetSkeletalTrackingLevel(ulong action, ref EVRSkeletalTrackingLevel pSkeletalTrackingLevel);

		// Token: 0x0200021A RID: 538
		// (Invoke) Token: 0x0600071C RID: 1820
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetSkeletalBoneData(ulong action, EVRSkeletalTransformSpace eTransformSpace, EVRSkeletalMotionRange eMotionRange, [In] [Out] VRBoneTransform_t[] pTransformArray, uint unTransformArrayCount);

		// Token: 0x0200021B RID: 539
		// (Invoke) Token: 0x06000720 RID: 1824
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetSkeletalSummaryData(ulong action, ref VRSkeletalSummaryData_t pSkeletalSummaryData);

		// Token: 0x0200021C RID: 540
		// (Invoke) Token: 0x06000724 RID: 1828
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetSkeletalBoneDataCompressed(ulong action, EVRSkeletalMotionRange eMotionRange, IntPtr pvCompressedData, uint unCompressedSize, ref uint punRequiredCompressedSize);

		// Token: 0x0200021D RID: 541
		// (Invoke) Token: 0x06000728 RID: 1832
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _DecompressSkeletalBoneData(IntPtr pvCompressedBuffer, uint unCompressedBufferSize, EVRSkeletalTransformSpace eTransformSpace, [In] [Out] VRBoneTransform_t[] pTransformArray, uint unTransformArrayCount);

		// Token: 0x0200021E RID: 542
		// (Invoke) Token: 0x0600072C RID: 1836
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _TriggerHapticVibrationAction(ulong action, float fStartSecondsFromNow, float fDurationSeconds, float fFrequency, float fAmplitude, ulong ulRestrictToDevice);

		// Token: 0x0200021F RID: 543
		// (Invoke) Token: 0x06000730 RID: 1840
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetActionOrigins(ulong actionSetHandle, ulong digitalActionHandle, [In] [Out] ulong[] originsOut, uint originOutCount);

		// Token: 0x02000220 RID: 544
		// (Invoke) Token: 0x06000734 RID: 1844
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetOriginLocalizedName(ulong origin, StringBuilder pchNameArray, uint unNameArraySize, int unStringSectionsToInclude);

		// Token: 0x02000221 RID: 545
		// (Invoke) Token: 0x06000738 RID: 1848
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _GetOriginTrackedDeviceInfo(ulong origin, ref InputOriginInfo_t pOriginInfo, uint unOriginInfoSize);

		// Token: 0x02000222 RID: 546
		// (Invoke) Token: 0x0600073C RID: 1852
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _ShowActionOrigins(ulong actionSetHandle, ulong ulActionHandle);

		// Token: 0x02000223 RID: 547
		// (Invoke) Token: 0x06000740 RID: 1856
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRInputError _ShowBindingsForActionSet([In] [Out] VRActiveActionSet_t[] pSets, uint unSizeOfVRSelectedActionSet_t, uint unSetCount, ulong originToHighlight);

		// Token: 0x02000224 RID: 548
		// (Invoke) Token: 0x06000744 RID: 1860
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsUsingLegacyInput();
	}
}
