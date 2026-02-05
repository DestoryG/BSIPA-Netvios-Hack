using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000021 RID: 33
	public class CVRInput
	{
		// Token: 0x06000132 RID: 306 RVA: 0x00003D91 File Offset: 0x00001F91
		internal CVRInput(IntPtr pInterface)
		{
			this.FnTable = (IVRInput)Marshal.PtrToStructure(pInterface, typeof(IVRInput));
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public EVRInputError SetActionManifestPath(string pchActionManifestPath)
		{
			return this.FnTable.SetActionManifestPath(pchActionManifestPath);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00003DC7 File Offset: 0x00001FC7
		public EVRInputError GetActionSetHandle(string pchActionSetName, ref ulong pHandle)
		{
			pHandle = 0UL;
			return this.FnTable.GetActionSetHandle(pchActionSetName, ref pHandle);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00003DDF File Offset: 0x00001FDF
		public EVRInputError GetActionHandle(string pchActionName, ref ulong pHandle)
		{
			pHandle = 0UL;
			return this.FnTable.GetActionHandle(pchActionName, ref pHandle);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00003DF7 File Offset: 0x00001FF7
		public EVRInputError GetInputSourceHandle(string pchInputSourcePath, ref ulong pHandle)
		{
			pHandle = 0UL;
			return this.FnTable.GetInputSourceHandle(pchInputSourcePath, ref pHandle);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00003E0F File Offset: 0x0000200F
		public EVRInputError UpdateActionState(VRActiveActionSet_t[] pSets, uint unSizeOfVRSelectedActionSet_t)
		{
			return this.FnTable.UpdateActionState(pSets, unSizeOfVRSelectedActionSet_t, (uint)pSets.Length);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00003E26 File Offset: 0x00002026
		public EVRInputError GetDigitalActionData(ulong action, ref InputDigitalActionData_t pActionData, uint unActionDataSize, ulong ulRestrictToDevice)
		{
			return this.FnTable.GetDigitalActionData(action, ref pActionData, unActionDataSize, ulRestrictToDevice);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00003E3D File Offset: 0x0000203D
		public EVRInputError GetAnalogActionData(ulong action, ref InputAnalogActionData_t pActionData, uint unActionDataSize, ulong ulRestrictToDevice)
		{
			return this.FnTable.GetAnalogActionData(action, ref pActionData, unActionDataSize, ulRestrictToDevice);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00003E54 File Offset: 0x00002054
		public EVRInputError GetPoseActionData(ulong action, ETrackingUniverseOrigin eOrigin, float fPredictedSecondsFromNow, ref InputPoseActionData_t pActionData, uint unActionDataSize, ulong ulRestrictToDevice)
		{
			return this.FnTable.GetPoseActionData(action, eOrigin, fPredictedSecondsFromNow, ref pActionData, unActionDataSize, ulRestrictToDevice);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00003E6F File Offset: 0x0000206F
		public EVRInputError GetSkeletalActionData(ulong action, ref InputSkeletalActionData_t pActionData, uint unActionDataSize)
		{
			return this.FnTable.GetSkeletalActionData(action, ref pActionData, unActionDataSize);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00003E84 File Offset: 0x00002084
		public EVRInputError GetBoneCount(ulong action, ref uint pBoneCount)
		{
			pBoneCount = 0U;
			return this.FnTable.GetBoneCount(action, ref pBoneCount);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00003E9B File Offset: 0x0000209B
		public EVRInputError GetBoneHierarchy(ulong action, int[] pParentIndices)
		{
			return this.FnTable.GetBoneHierarchy(action, pParentIndices, (uint)pParentIndices.Length);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00003EB2 File Offset: 0x000020B2
		public EVRInputError GetBoneName(ulong action, int nBoneIndex, StringBuilder pchBoneName, uint unNameBufferSize)
		{
			return this.FnTable.GetBoneName(action, nBoneIndex, pchBoneName, unNameBufferSize);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00003EC9 File Offset: 0x000020C9
		public EVRInputError GetSkeletalReferenceTransforms(ulong action, EVRSkeletalTransformSpace eTransformSpace, EVRSkeletalReferencePose eReferencePose, VRBoneTransform_t[] pTransformArray)
		{
			return this.FnTable.GetSkeletalReferenceTransforms(action, eTransformSpace, eReferencePose, pTransformArray, (uint)pTransformArray.Length);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00003EE4 File Offset: 0x000020E4
		public EVRInputError GetSkeletalTrackingLevel(ulong action, ref EVRSkeletalTrackingLevel pSkeletalTrackingLevel)
		{
			return this.FnTable.GetSkeletalTrackingLevel(action, ref pSkeletalTrackingLevel);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00003EF8 File Offset: 0x000020F8
		public EVRInputError GetSkeletalBoneData(ulong action, EVRSkeletalTransformSpace eTransformSpace, EVRSkeletalMotionRange eMotionRange, VRBoneTransform_t[] pTransformArray)
		{
			return this.FnTable.GetSkeletalBoneData(action, eTransformSpace, eMotionRange, pTransformArray, (uint)pTransformArray.Length);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00003F13 File Offset: 0x00002113
		public EVRInputError GetSkeletalSummaryData(ulong action, ref VRSkeletalSummaryData_t pSkeletalSummaryData)
		{
			return this.FnTable.GetSkeletalSummaryData(action, ref pSkeletalSummaryData);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00003F27 File Offset: 0x00002127
		public EVRInputError GetSkeletalBoneDataCompressed(ulong action, EVRSkeletalMotionRange eMotionRange, IntPtr pvCompressedData, uint unCompressedSize, ref uint punRequiredCompressedSize)
		{
			punRequiredCompressedSize = 0U;
			return this.FnTable.GetSkeletalBoneDataCompressed(action, eMotionRange, pvCompressedData, unCompressedSize, ref punRequiredCompressedSize);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00003F44 File Offset: 0x00002144
		public EVRInputError DecompressSkeletalBoneData(IntPtr pvCompressedBuffer, uint unCompressedBufferSize, EVRSkeletalTransformSpace eTransformSpace, VRBoneTransform_t[] pTransformArray)
		{
			return this.FnTable.DecompressSkeletalBoneData(pvCompressedBuffer, unCompressedBufferSize, eTransformSpace, pTransformArray, (uint)pTransformArray.Length);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00003F5F File Offset: 0x0000215F
		public EVRInputError TriggerHapticVibrationAction(ulong action, float fStartSecondsFromNow, float fDurationSeconds, float fFrequency, float fAmplitude, ulong ulRestrictToDevice)
		{
			return this.FnTable.TriggerHapticVibrationAction(action, fStartSecondsFromNow, fDurationSeconds, fFrequency, fAmplitude, ulRestrictToDevice);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00003F7A File Offset: 0x0000217A
		public EVRInputError GetActionOrigins(ulong actionSetHandle, ulong digitalActionHandle, ulong[] originsOut)
		{
			return this.FnTable.GetActionOrigins(actionSetHandle, digitalActionHandle, originsOut, (uint)originsOut.Length);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00003F92 File Offset: 0x00002192
		public EVRInputError GetOriginLocalizedName(ulong origin, StringBuilder pchNameArray, uint unNameArraySize, int unStringSectionsToInclude)
		{
			return this.FnTable.GetOriginLocalizedName(origin, pchNameArray, unNameArraySize, unStringSectionsToInclude);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00003FA9 File Offset: 0x000021A9
		public EVRInputError GetOriginTrackedDeviceInfo(ulong origin, ref InputOriginInfo_t pOriginInfo, uint unOriginInfoSize)
		{
			return this.FnTable.GetOriginTrackedDeviceInfo(origin, ref pOriginInfo, unOriginInfoSize);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00003FBE File Offset: 0x000021BE
		public EVRInputError ShowActionOrigins(ulong actionSetHandle, ulong ulActionHandle)
		{
			return this.FnTable.ShowActionOrigins(actionSetHandle, ulActionHandle);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00003FD2 File Offset: 0x000021D2
		public EVRInputError ShowBindingsForActionSet(VRActiveActionSet_t[] pSets, uint unSizeOfVRSelectedActionSet_t, ulong originToHighlight)
		{
			return this.FnTable.ShowBindingsForActionSet(pSets, unSizeOfVRSelectedActionSet_t, (uint)pSets.Length, originToHighlight);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00003FEA File Offset: 0x000021EA
		public bool IsUsingLegacyInput()
		{
			return this.FnTable.IsUsingLegacyInput();
		}

		// Token: 0x04000155 RID: 341
		private IVRInput FnTable;
	}
}
