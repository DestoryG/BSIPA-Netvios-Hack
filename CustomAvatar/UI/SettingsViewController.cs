using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomAvatar.Avatar;
using CustomAvatar.Tracking;
using CustomAvatar.Utilities;
using HMUI;
using TMPro;
using UnityEngine;

namespace CustomAvatar.UI
{
	// Token: 0x02000034 RID: 52
	internal class SettingsViewController : BSMLResourceViewController
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00007460 File Offset: 0x00005660
		public override string ResourceName
		{
			get
			{
				return "CustomAvatar.Views.SettingsViewController.bsml";
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00007468 File Offset: 0x00005668
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			this.visibleInFirstPerson = SettingsManager.settings.isAvatarVisibleInFirstPerson;
			this.resizeMode = SettingsManager.settings.resizeMode;
			this.floorHeightAdjust = SettingsManager.settings.enableFloorAdjust;
			this.calibrateFullBodyTrackingOnStart = SettingsManager.settings.calibrateFullBodyTrackingOnStart;
			base.DidActivate(firstActivation, type);
			this.armSpanLabel.SetText(string.Format("{0:0.00} m", SettingsManager.settings.playerArmSpan));
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000074E4 File Offset: 0x000056E4
		[UIAction("visible-first-person-change")]
		private void OnVisibleInFirstPersonChanged(bool value)
		{
			SettingsManager.settings.isAvatarVisibleInFirstPerson = value;
			SpawnedAvatar currentlySpawnedAvatar = AvatarManager.instance.currentlySpawnedAvatar;
			if (currentlySpawnedAvatar != null)
			{
				currentlySpawnedAvatar.OnFirstPersonEnabledChanged();
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00007508 File Offset: 0x00005708
		[UIAction("resize-change")]
		private void OnResizeModeChanged(AvatarResizeMode value)
		{
			SettingsManager.settings.resizeMode = value;
			AvatarManager.instance.ResizeCurrentAvatar();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007521 File Offset: 0x00005721
		[UIAction("floor-adjust-change")]
		private void OnFloorHeightAdjustChanged(bool value)
		{
			SettingsManager.settings.enableFloorAdjust = value;
			AvatarManager.instance.ResizeCurrentAvatar();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000753C File Offset: 0x0000573C
		[UIAction("resize-mode-formatter")]
		private string ResizeModeFormatter(object value)
		{
			bool flag = !(value is AvatarResizeMode);
			string text;
			if (flag)
			{
				text = null;
			}
			else
			{
				switch ((AvatarResizeMode)value)
				{
				case AvatarResizeMode.ArmSpan:
					text = "臂展";
					break;
				case AvatarResizeMode.Height:
					text = "身高";
					break;
				case AvatarResizeMode.None:
					text = "不调整";
					break;
				default:
					text = null;
					break;
				}
			}
			return text;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007598 File Offset: 0x00005798
		[UIAction("measure-arm-span-click")]
		private void OnMeasureArmSpanButtonClicked()
		{
			this.MeasureArmSpan();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000075A2 File Offset: 0x000057A2
		[UIAction("calibrate-fbt-click")]
		private void OnCalibrateFullBodyTrackingClicked()
		{
			AvatarManager.instance.avatarTailor.CalibrateFullBodyTracking();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000075B5 File Offset: 0x000057B5
		[UIAction("calibrate-fbt-on-start-change")]
		private void OnCalibrateFullBodyTrackingOnStartChanged(bool value)
		{
			SettingsManager.settings.calibrateFullBodyTrackingOnStart = value;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000075C3 File Offset: 0x000057C3
		[UIAction("clear-fbt-calibration-data-click")]
		private void OnClearFullBodyTrackingCalibrationDataClicked()
		{
			AvatarManager.instance.avatarTailor.ClearFullBodyTrackingData();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000075D8 File Offset: 0x000057D8
		private void MeasureArmSpan()
		{
			bool flag = this.isMeasuring;
			if (!flag)
			{
				this.isMeasuring = true;
				this.maxMeasuredArmSpan = 0.5f;
				this.lastUpdateTime = Time.timeSinceLevelLoad;
				base.InvokeRepeating("ScanArmSpan", 0f, 0.1f);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00007628 File Offset: 0x00005828
		private void ScanArmSpan()
		{
			float num = Vector3.Distance(this.playerInput.leftHand.position, this.playerInput.rightHand.position);
			bool flag = num > this.maxMeasuredArmSpan;
			if (flag)
			{
				this.maxMeasuredArmSpan = num;
				this.lastUpdateTime = Time.timeSinceLevelLoad;
			}
			bool flag2 = Time.timeSinceLevelLoad - this.lastUpdateTime < 2f;
			if (flag2)
			{
				this.armSpanLabel.SetText(string.Format("Measuring... {0:0.00} m", this.maxMeasuredArmSpan));
			}
			else
			{
				base.CancelInvoke("ScanArmSpan");
				this.armSpanLabel.SetText(string.Format("{0:0.00} m", this.maxMeasuredArmSpan));
				SettingsManager.settings.playerArmSpan = this.maxMeasuredArmSpan;
				this.isMeasuring = false;
				bool flag3 = SettingsManager.settings.resizeMode == AvatarResizeMode.ArmSpan;
				if (flag3)
				{
					AvatarManager.instance.ResizeCurrentAvatar();
				}
			}
		}

		// Token: 0x04000183 RID: 387
		[UIComponent("arm-span")]
		private TextMeshProUGUI armSpanLabel;

		// Token: 0x04000184 RID: 388
		[UIValue("resize-options")]
		private readonly List<object> resizeModeOptions = new List<object>
		{
			AvatarResizeMode.None,
			AvatarResizeMode.Height,
			AvatarResizeMode.ArmSpan
		};

		// Token: 0x04000185 RID: 389
		[UIValue("visible-first-person-value")]
		private bool visibleInFirstPerson;

		// Token: 0x04000186 RID: 390
		[UIValue("resize-value")]
		private AvatarResizeMode resizeMode;

		// Token: 0x04000187 RID: 391
		[UIValue("floor-adjust-value")]
		private bool floorHeightAdjust;

		// Token: 0x04000188 RID: 392
		[UIValue("calibrate-fbt-on-start")]
		private bool calibrateFullBodyTrackingOnStart;

		// Token: 0x04000189 RID: 393
		private const float KMinArmSpan = 0.5f;

		// Token: 0x0400018A RID: 394
		private TrackedDeviceManager playerInput = PersistentSingleton<TrackedDeviceManager>.instance;

		// Token: 0x0400018B RID: 395
		private bool isMeasuring;

		// Token: 0x0400018C RID: 396
		private float maxMeasuredArmSpan;

		// Token: 0x0400018D RID: 397
		private float lastUpdateTime;
	}
}
