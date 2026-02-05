using System;
using CustomAvatar.Tracking;
using CustomAvatar.Utilities;
using UnityEngine;

namespace CustomAvatar.Avatar
{
	// Token: 0x02000036 RID: 54
	internal class AvatarTracking : BodyAwareBehaviour
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00007FA8 File Offset: 0x000061A8
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00007FBA File Offset: 0x000061BA
		public float verticalPosition
		{
			get
			{
				return base.transform.position.y;
			}
			set
			{
				base.transform.position = new Vector3(0f, value, 0f);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00007FD8 File Offset: 0x000061D8
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00007FF6 File Offset: 0x000061F6
		public float scale
		{
			get
			{
				return base.transform.localScale.y / this._initialScale.y;
			}
			set
			{
				base.transform.localScale = this._initialScale * value;
				Plugin.logger.Info("Avatar resized with scale: " + value.ToString());
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000802D File Offset: 0x0000622D
		private void Awake()
		{
			this._initialScale = base.transform.localScale;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00008044 File Offset: 0x00006244
		protected override void Start()
		{
			base.Start();
			bool flag = this.input == null;
			if (flag)
			{
				Object.Destroy(this);
				throw new ArgumentNullException("input");
			}
			bool flag2 = this.customAvatar == null;
			if (flag2)
			{
				Object.Destroy(this);
				throw new ArgumentNullException("customAvatar");
			}
			this._vrPlatformHelper = PersistentSingleton<VRPlatformHelper>.instance;
			bool flag3 = this._pelvis;
			if (flag3)
			{
				this._initialPelvisPose = new Pose(this._pelvis.position, this._pelvis.rotation);
			}
			bool flag4 = this._leftLeg;
			if (flag4)
			{
				this._initialLeftFootPose = new Pose(this._leftLeg.position, this._leftLeg.rotation);
			}
			bool flag5 = this._rightLeg;
			if (flag5)
			{
				this._initialRightFootPose = new Pose(this._rightLeg.position, this._rightLeg.rotation);
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00008138 File Offset: 0x00006338
		private void LateUpdate()
		{
			try
			{
				Pose pose;
				bool flag = this._head && this.input.TryGetHeadPose(out pose);
				if (flag)
				{
					this._head.position = pose.position;
					this._head.rotation = pose.rotation;
				}
				Vector3 controllerPositionOffset = BeatSaberUtil.GetControllerPositionOffset();
				Vector3 controllerRotationOffset = BeatSaberUtil.GetControllerRotationOffset();
				Pose pose2;
				bool flag2 = this._rightHand && this.input.TryGetRightHandPose(out pose2);
				if (flag2)
				{
					this._rightHand.position = pose2.position;
					this._rightHand.rotation = pose2.rotation;
					this._vrPlatformHelper.AdjustPlatformSpecificControllerTransform(5, this._rightHand, controllerPositionOffset, controllerRotationOffset);
				}
				controllerPositionOffset..ctor(-controllerPositionOffset.x, controllerPositionOffset.y, controllerPositionOffset.z);
				controllerRotationOffset..ctor(controllerRotationOffset.x, -controllerRotationOffset.y, controllerRotationOffset.z);
				Pose pose3;
				bool flag3 = this._leftHand && this.input.TryGetLeftHandPose(out pose3);
				if (flag3)
				{
					this._leftHand.position = pose3.position;
					this._leftHand.rotation = pose3.rotation;
					this._vrPlatformHelper.AdjustPlatformSpecificControllerTransform(4, this._leftHand, controllerPositionOffset, controllerRotationOffset);
				}
				Pose pose4;
				bool flag4 = this._leftLeg && this.input.TryGetLeftFootPose(out pose4);
				if (flag4)
				{
					Pose leftLeg = SettingsManager.settings.fullBodyCalibration.leftLeg;
					this._prevLeftLegPose.position = Vector3.Lerp(this._prevLeftLegPose.position, this.AdjustTransformPosition(pose4.position, leftLeg.position, this._initialLeftFootPose.position), SettingsManager.settings.fullBodyMotionSmoothing.feet.position * Time.deltaTime);
					this._prevLeftLegPose.rotation = Quaternion.Slerp(this._prevLeftLegPose.rotation, pose4.rotation * leftLeg.rotation, SettingsManager.settings.fullBodyMotionSmoothing.feet.rotation * Time.deltaTime);
					this._leftLeg.position = this._prevLeftLegPose.position;
					this._leftLeg.rotation = this._prevLeftLegPose.rotation;
				}
				Pose pose5;
				bool flag5 = this._rightLeg && this.input.TryGetRightFootPose(out pose5);
				if (flag5)
				{
					Pose rightLeg = SettingsManager.settings.fullBodyCalibration.rightLeg;
					this._prevRightLegPose.position = Vector3.Lerp(this._prevRightLegPose.position, this.AdjustTransformPosition(pose5.position, rightLeg.position, this._initialRightFootPose.position), SettingsManager.settings.fullBodyMotionSmoothing.feet.position * Time.deltaTime);
					this._prevRightLegPose.rotation = Quaternion.Slerp(this._prevRightLegPose.rotation, pose5.rotation * rightLeg.rotation, SettingsManager.settings.fullBodyMotionSmoothing.feet.rotation * Time.deltaTime);
					this._rightLeg.position = this._prevRightLegPose.position;
					this._rightLeg.rotation = this._prevRightLegPose.rotation;
				}
				Pose pose6;
				bool flag6 = this._pelvis && this.input.TryGetWaistPose(out pose6);
				if (flag6)
				{
					Pose pelvis = SettingsManager.settings.fullBodyCalibration.pelvis;
					this._prevPelvisPose.position = Vector3.Lerp(this._prevPelvisPose.position, this.AdjustTransformPosition(pose6.position, pelvis.position, this._initialPelvisPose.position), SettingsManager.settings.fullBodyMotionSmoothing.waist.position * Time.deltaTime);
					this._prevPelvisPose.rotation = Quaternion.Slerp(this._prevPelvisPose.rotation, pose6.rotation * pelvis.rotation, SettingsManager.settings.fullBodyMotionSmoothing.waist.rotation * Time.deltaTime);
					this._pelvis.position = this._prevPelvisPose.position;
					this._pelvis.rotation = this._prevPelvisPose.rotation;
				}
				bool flag7 = this._body;
				if (flag7)
				{
					this._body.position = this._head.position - this._head.up * 0.1f;
					Vector3 vector;
					vector..ctor(this._body.localPosition.x - this._prevBodyLocalPosition.x, 0f, this._body.localPosition.z - this._prevBodyLocalPosition.z);
					Quaternion quaternion = Quaternion.Euler(0f, this._head.localEulerAngles.y, 0f);
					Vector3 vector2 = Vector3.Cross(base.transform.up, vector);
					this._body.localRotation = Quaternion.Lerp(this._body.localRotation, Quaternion.AngleAxis(vector.magnitude * 1250f, vector2) * quaternion, Time.deltaTime * 10f);
					this._prevBodyLocalPosition = this._body.localPosition;
				}
			}
			catch (Exception ex)
			{
				Plugin.logger.Error(ex.Message + "\n" + ex.StackTrace);
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000086F4 File Offset: 0x000068F4
		private Vector3 AdjustTransformPosition(Vector3 original, Vector3 correction, Vector3 originalPosition)
		{
			Vector3 vector = original + correction;
			float num = this.verticalPosition;
			bool moveFloorWithRoomAdjust = SettingsManager.settings.moveFloorWithRoomAdjust;
			if (moveFloorWithRoomAdjust)
			{
				num -= BeatSaberUtil.GetRoomCenter().y;
			}
			return new Vector3(vector.x, vector.y + (1f - originalPosition.y / this.customAvatar.eyeHeight) * num, vector.z);
		}

		// Token: 0x04000196 RID: 406
		public AvatarInput input;

		// Token: 0x04000197 RID: 407
		public LoadedAvatar customAvatar;

		// Token: 0x04000198 RID: 408
		private Vector3 _initialScale;

		// Token: 0x04000199 RID: 409
		private Pose _initialPelvisPose;

		// Token: 0x0400019A RID: 410
		private Pose _initialLeftFootPose;

		// Token: 0x0400019B RID: 411
		private Pose _initialRightFootPose;

		// Token: 0x0400019C RID: 412
		private Vector3 _prevBodyLocalPosition = Vector3.zero;

		// Token: 0x0400019D RID: 413
		private Pose _prevPelvisPose = Pose.identity;

		// Token: 0x0400019E RID: 414
		private Pose _prevLeftLegPose = Pose.identity;

		// Token: 0x0400019F RID: 415
		private Pose _prevRightLegPose = Pose.identity;

		// Token: 0x040001A0 RID: 416
		private VRPlatformHelper _vrPlatformHelper;
	}
}
