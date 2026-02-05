using System;
using DynamicOpenVR.IO;
using UnityEngine;

namespace CustomAvatar.Avatar
{
	// Token: 0x02000037 RID: 55
	internal class AvatarFingerTracking : MonoBehaviour
	{
		// Token: 0x06000129 RID: 297 RVA: 0x00008799 File Offset: 0x00006999
		private void Start()
		{
			this._animator = base.GetComponentInChildren<Animator>();
			this._poseManager = base.GetComponentInChildren<PoseManager>();
			this._leftHandAnimAction = new SkeletalInput("/actions/customavatars/in/lefthandanim");
			this._rightHandAnimAction = new SkeletalInput("/actions/customavatars/in/righthandanim");
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000087D4 File Offset: 0x000069D4
		private void Update()
		{
			this.ApplyFingerTracking();
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000087DE File Offset: 0x000069DE
		private void OnDestroy()
		{
			SkeletalInput leftHandAnimAction = this._leftHandAnimAction;
			if (leftHandAnimAction != null)
			{
				leftHandAnimAction.Dispose();
			}
			SkeletalInput rightHandAnimAction = this._rightHandAnimAction;
			if (rightHandAnimAction != null)
			{
				rightHandAnimAction.Dispose();
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008808 File Offset: 0x00006A08
		public void ApplyFingerTracking()
		{
			SkeletalSummaryData summaryData = this._leftHandAnimAction.summaryData;
			SkeletalSummaryData summaryData2 = this._rightHandAnimAction.summaryData;
			bool flag = this._leftHandAnimAction.isActive && summaryData != null;
			if (flag)
			{
				this.ApplyLeftHandFingerPoses(summaryData.thumbCurl, summaryData.indexCurl, summaryData.middleCurl, summaryData.ringCurl, summaryData.littleCurl);
			}
			else
			{
				this.ApplyLeftHandFingerPoses(1f, 1f, 1f, 1f, 1f);
			}
			bool flag2 = this._rightHandAnimAction.isActive && summaryData2 != null;
			if (flag2)
			{
				this.ApplyRightHandFingerPoses(summaryData2.thumbCurl, summaryData2.indexCurl, summaryData2.middleCurl, summaryData2.ringCurl, summaryData2.littleCurl);
			}
			else
			{
				this.ApplyRightHandFingerPoses(1f, 1f, 1f, 1f, 1f);
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000088F4 File Offset: 0x00006AF4
		private void ApplyLeftHandFingerPoses(float thumbCurl, float indexCurl, float middleCurl, float ringCurl, float littleCurl)
		{
			this.ApplyBodyBonePose(24, this._poseManager.openHand_LeftThumbProximal, this._poseManager.closedHand_LeftThumbProximal, thumbCurl);
			this.ApplyBodyBonePose(25, this._poseManager.openHand_LeftThumbIntermediate, this._poseManager.closedHand_LeftThumbIntermediate, thumbCurl);
			this.ApplyBodyBonePose(26, this._poseManager.openHand_LeftThumbDistal, this._poseManager.closedHand_LeftThumbDistal, thumbCurl);
			this.ApplyBodyBonePose(27, this._poseManager.openHand_LeftIndexProximal, this._poseManager.closedHand_LeftIndexProximal, indexCurl);
			this.ApplyBodyBonePose(28, this._poseManager.openHand_LeftIndexIntermediate, this._poseManager.closedHand_LeftIndexIntermediate, indexCurl);
			this.ApplyBodyBonePose(29, this._poseManager.openHand_LeftIndexDistal, this._poseManager.closedHand_LeftIndexDistal, indexCurl);
			this.ApplyBodyBonePose(30, this._poseManager.openHand_LeftMiddleProximal, this._poseManager.closedHand_LeftMiddleProximal, middleCurl);
			this.ApplyBodyBonePose(31, this._poseManager.openHand_LeftMiddleIntermediate, this._poseManager.closedHand_LeftMiddleIntermediate, middleCurl);
			this.ApplyBodyBonePose(32, this._poseManager.openHand_LeftMiddleDistal, this._poseManager.closedHand_LeftMiddleDistal, middleCurl);
			this.ApplyBodyBonePose(33, this._poseManager.openHand_LeftRingProximal, this._poseManager.closedHand_LeftRingProximal, ringCurl);
			this.ApplyBodyBonePose(34, this._poseManager.openHand_LeftRingIntermediate, this._poseManager.closedHand_LeftRingIntermediate, ringCurl);
			this.ApplyBodyBonePose(35, this._poseManager.openHand_LeftRingDistal, this._poseManager.closedHand_LeftRingDistal, ringCurl);
			this.ApplyBodyBonePose(36, this._poseManager.openHand_LeftLittleProximal, this._poseManager.closedHand_LeftLittleProximal, littleCurl);
			this.ApplyBodyBonePose(37, this._poseManager.openHand_LeftLittleIntermediate, this._poseManager.closedHand_LeftLittleIntermediate, littleCurl);
			this.ApplyBodyBonePose(38, this._poseManager.openHand_LeftLittleDistal, this._poseManager.closedHand_LeftLittleDistal, littleCurl);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00008AE8 File Offset: 0x00006CE8
		private void ApplyRightHandFingerPoses(float thumbCurl, float indexCurl, float middleCurl, float ringCurl, float littleCurl)
		{
			this.ApplyBodyBonePose(39, this._poseManager.openHand_RightThumbProximal, this._poseManager.closedHand_RightThumbProximal, thumbCurl);
			this.ApplyBodyBonePose(40, this._poseManager.openHand_RightThumbIntermediate, this._poseManager.closedHand_RightThumbIntermediate, thumbCurl);
			this.ApplyBodyBonePose(41, this._poseManager.openHand_RightThumbDistal, this._poseManager.closedHand_RightThumbDistal, thumbCurl);
			this.ApplyBodyBonePose(42, this._poseManager.openHand_RightIndexProximal, this._poseManager.closedHand_RightIndexProximal, indexCurl);
			this.ApplyBodyBonePose(43, this._poseManager.openHand_RightIndexIntermediate, this._poseManager.closedHand_RightIndexIntermediate, indexCurl);
			this.ApplyBodyBonePose(44, this._poseManager.openHand_RightIndexDistal, this._poseManager.closedHand_RightIndexDistal, indexCurl);
			this.ApplyBodyBonePose(45, this._poseManager.openHand_RightMiddleProximal, this._poseManager.closedHand_RightMiddleProximal, middleCurl);
			this.ApplyBodyBonePose(46, this._poseManager.openHand_RightMiddleIntermediate, this._poseManager.closedHand_RightMiddleIntermediate, middleCurl);
			this.ApplyBodyBonePose(47, this._poseManager.openHand_RightMiddleDistal, this._poseManager.closedHand_RightMiddleDistal, middleCurl);
			this.ApplyBodyBonePose(48, this._poseManager.openHand_RightRingProximal, this._poseManager.closedHand_RightRingProximal, ringCurl);
			this.ApplyBodyBonePose(49, this._poseManager.openHand_RightRingIntermediate, this._poseManager.closedHand_RightRingIntermediate, ringCurl);
			this.ApplyBodyBonePose(50, this._poseManager.openHand_RightRingDistal, this._poseManager.closedHand_RightRingDistal, ringCurl);
			this.ApplyBodyBonePose(51, this._poseManager.openHand_RightLittleProximal, this._poseManager.closedHand_RightLittleProximal, littleCurl);
			this.ApplyBodyBonePose(52, this._poseManager.openHand_RightLittleIntermediate, this._poseManager.closedHand_RightLittleIntermediate, littleCurl);
			this.ApplyBodyBonePose(53, this._poseManager.openHand_RightLittleDistal, this._poseManager.closedHand_RightLittleDistal, littleCurl);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00008CDC File Offset: 0x00006EDC
		private void ApplyBodyBonePose(HumanBodyBones bodyBone, Pose open, Pose closed, float fade)
		{
			Transform boneTransform = this._animator.GetBoneTransform(bodyBone);
			bool flag = !boneTransform;
			if (!flag)
			{
				boneTransform.localPosition = Vector3.Lerp(open.position, closed.position, fade);
				boneTransform.localRotation = Quaternion.Slerp(open.rotation, closed.rotation, fade);
			}
		}

		// Token: 0x040001A1 RID: 417
		private SkeletalInput _leftHandAnimAction;

		// Token: 0x040001A2 RID: 418
		private SkeletalInput _rightHandAnimAction;

		// Token: 0x040001A3 RID: 419
		private Animator _animator;

		// Token: 0x040001A4 RID: 420
		private PoseManager _poseManager;
	}
}
