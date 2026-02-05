using System;
using System.Collections;
using CustomAvatar.Avatar;
using CustomAvatar.Tracking;
using CustomAvatar.Utilities;
using IPA.Logging;
using UnityEngine;

namespace CustomAvatar
{
	// Token: 0x02000010 RID: 16
	internal class AvatarTailor
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00002CC8 File Offset: 0x00000EC8
		public void ResizeAvatar(SpawnedAvatar avatar)
		{
			bool flag = !avatar.customAvatar.descriptor.allowHeightCalibration || !avatar.customAvatar.isIKAvatar;
			if (!flag)
			{
				AvatarResizeMode resizeMode = SettingsManager.settings.resizeMode;
				AvatarResizeMode avatarResizeMode = resizeMode;
				AvatarResizeMode avatarResizeMode2 = avatarResizeMode;
				float num;
				if (avatarResizeMode2 != AvatarResizeMode.ArmSpan)
				{
					if (avatarResizeMode2 != AvatarResizeMode.Height)
					{
						num = 1f;
					}
					else
					{
						float eyeHeight = avatar.customAvatar.eyeHeight;
						bool flag2 = eyeHeight > 0f;
						if (flag2)
						{
							num = BeatSaberUtil.GetPlayerEyeHeight() / eyeHeight;
						}
						else
						{
							num = 1f;
						}
					}
				}
				else
				{
					float armSpan = avatar.customAvatar.GetArmSpan();
					bool flag3 = armSpan > 0f;
					if (flag3)
					{
						num = SettingsManager.settings.playerArmSpan / armSpan;
					}
					else
					{
						num = 1f;
					}
				}
				bool flag4 = num <= 0f;
				if (flag4)
				{
					Plugin.logger.Warn("Calculated scale is <= 0; reverting to 1");
					num = 1f;
				}
				avatar.tracking.scale = num;
				PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this.FloorMendingWithDelay(avatar));
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002DD9 File Offset: 0x00000FD9
		private IEnumerator FloorMendingWithDelay(SpawnedAvatar avatar)
		{
			yield return new WaitForEndOfFrame();
			float floorOffset = 0f;
			bool flag = SettingsManager.settings.enableFloorAdjust && avatar.customAvatar.isIKAvatar;
			if (flag)
			{
				float playerEyeHeight = BeatSaberUtil.GetPlayerEyeHeight();
				float avatarEyeHeight = avatar.customAvatar.eyeHeight;
				floorOffset = playerEyeHeight - avatarEyeHeight * avatar.tracking.scale;
				bool moveFloorWithRoomAdjust = SettingsManager.settings.moveFloorWithRoomAdjust;
				if (moveFloorWithRoomAdjust)
				{
					floorOffset += BeatSaberUtil.GetRoomCenter().y;
				}
			}
			floorOffset = (float)Math.Round((double)floorOffset, 3);
			avatar.tracking.verticalPosition = floorOffset;
			GameObject menuPlayersPlace = GameObject.Find("MenuPlayersPlace");
			GameObject originalFloor = GameObject.Find("Environment/PlayersPlace");
			GameObject customFloor = GameObject.Find("Platform Loader");
			bool flag2 = menuPlayersPlace;
			if (flag2)
			{
				Plugin.logger.Info(string.Format("Moving MenuPlayersPlace floor {0} m {1}", Math.Abs(floorOffset), (floorOffset >= 0f) ? "up" : "down"));
				menuPlayersPlace.transform.position = new Vector3(0f, floorOffset, 0f);
			}
			bool flag3 = originalFloor;
			if (flag3)
			{
				Plugin.logger.Info(string.Format("Moving PlayersPlace {0} m {1}", Math.Abs(floorOffset), (floorOffset >= 0f) ? "up" : "down"));
				originalFloor.transform.position = new Vector3(0f, floorOffset, 0f);
			}
			bool flag4 = customFloor;
			if (flag4)
			{
				Plugin.logger.Info(string.Format("Moving Custom Platforms floor {0} m {1}", Math.Abs(floorOffset), (floorOffset >= 0f) ? "up" : "down"));
				this._initialPlatformPosition = new Vector3?(this._initialPlatformPosition ?? customFloor.transform.position);
				customFloor.transform.position = (Vector3.up * floorOffset + this._initialPlatformPosition) ?? Vector3.zero;
			}
			yield break;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002DF0 File Offset: 0x00000FF0
		public void CalibrateFullBodyTracking()
		{
			Plugin.logger.Info("Calibrating full body tracking");
			TrackedDeviceManager instance = PersistentSingleton<TrackedDeviceManager>.instance;
			TrackedDeviceState head = instance.head;
			TrackedDeviceState leftFoot = instance.leftFoot;
			TrackedDeviceState rightFoot = instance.rightFoot;
			TrackedDeviceState waist = instance.waist;
			Vector3 up = Vector3.up;
			float num = (SettingsManager.settings.moveFloorWithRoomAdjust ? BeatSaberUtil.GetRoomCenter().y : 0f);
			bool found = leftFoot.found;
			if (found)
			{
				Vector3 vector = leftFoot.rotation * Vector3.up;
				Vector3 vector2 = Vector3.ProjectOnPlane(vector, up);
				Quaternion quaternion = Quaternion.Inverse(leftFoot.rotation) * Quaternion.LookRotation(Vector3.up, vector2);
				SettingsManager.settings.fullBodyCalibration.leftLeg = new Pose((leftFoot.position.y - num) * Vector3.down, quaternion);
				Logger logger = Plugin.logger;
				string text = "Saved left foot pose correction ";
				Pose pose = SettingsManager.settings.fullBodyCalibration.leftLeg;
				logger.Info(text + pose.ToString());
			}
			bool found2 = rightFoot.found;
			if (found2)
			{
				Vector3 vector3 = rightFoot.rotation * Vector3.up;
				Vector3 vector4 = Vector3.ProjectOnPlane(vector3, up);
				Quaternion quaternion2 = Quaternion.Inverse(rightFoot.rotation) * Quaternion.LookRotation(Vector3.up, vector4);
				SettingsManager.settings.fullBodyCalibration.rightLeg = new Pose((rightFoot.position.y - num) * Vector3.down, quaternion2);
				Logger logger2 = Plugin.logger;
				string text2 = "Saved right foot pose correction ";
				Pose pose = SettingsManager.settings.fullBodyCalibration.rightLeg;
				logger2.Info(text2 + pose.ToString());
			}
			bool flag = head.found && waist.found;
			if (flag)
			{
				float num2 = head.position.y - num;
				Vector3 vector5;
				vector5..ctor(0f, num2 / 15f * 10f, 0f);
				Vector3 vector6 = vector5 - Vector3.up * (waist.position.y - num);
				Vector3 vector7 = waist.rotation * Vector3.forward;
				Vector3 vector8 = Vector3.ProjectOnPlane(vector7, up);
				Quaternion quaternion3 = Quaternion.Inverse(waist.rotation) * Quaternion.LookRotation(vector8, Vector3.up);
				SettingsManager.settings.fullBodyCalibration.pelvis = new Pose(vector6, quaternion3);
				Logger logger3 = Plugin.logger;
				string text3 = "Saved pelvis pose correction ";
				Pose pose = SettingsManager.settings.fullBodyCalibration.pelvis;
				logger3.Info(text3 + pose.ToString());
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000030A7 File Offset: 0x000012A7
		public void ClearFullBodyTrackingData()
		{
			SettingsManager.settings.fullBodyCalibration.leftLeg = Pose.identity;
			SettingsManager.settings.fullBodyCalibration.rightLeg = Pose.identity;
			SettingsManager.settings.fullBodyCalibration.pelvis = Pose.identity;
		}

		// Token: 0x0400004E RID: 78
		private Vector3? _initialPlatformPosition;
	}
}
