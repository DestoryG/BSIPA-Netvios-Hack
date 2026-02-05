using System;
using System.Linq;
using UnityEngine;

namespace CustomAvatar.Utilities
{
	// Token: 0x0200001C RID: 28
	internal static class BeatSaberUtil
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003F68 File Offset: 0x00002168
		private static MainSettingsModelSO mainSettingsModel
		{
			get
			{
				bool flag = !BeatSaberUtil._mainSettingsModel;
				if (flag)
				{
					BeatSaberUtil._mainSettingsModel = Resources.FindObjectsOfTypeAll<MainSettingsModelSO>().FirstOrDefault<MainSettingsModelSO>();
				}
				return BeatSaberUtil._mainSettingsModel;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003FA4 File Offset: 0x000021A4
		private static PlayerDataModel playerDataModel
		{
			get
			{
				bool flag = !BeatSaberUtil._playerDataModel;
				if (flag)
				{
					BeatSaberUtil._playerDataModel = Resources.FindObjectsOfTypeAll<PlayerDataModel>().FirstOrDefault((PlayerDataModel m) => m.playerData != null);
				}
				return BeatSaberUtil._playerDataModel;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003FFC File Offset: 0x000021FC
		public static float GetPlayerHeight()
		{
			return (!BeatSaberUtil.playerDataModel) ? 1.8f : BeatSaberUtil.playerDataModel.playerData.playerSpecificSettings.playerHeight;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004038 File Offset: 0x00002238
		public static float GetPlayerEyeHeight()
		{
			return BeatSaberUtil.GetPlayerHeight() - 0.1f;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004058 File Offset: 0x00002258
		public static Vector3 GetRoomCenter()
		{
			return (!BeatSaberUtil.mainSettingsModel) ? Vector3.zero : BeatSaberUtil.mainSettingsModel.roomCenter;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000408C File Offset: 0x0000228C
		public static Quaternion GetRoomRotation()
		{
			return (!BeatSaberUtil.mainSettingsModel) ? Quaternion.identity : Quaternion.Euler(0f, BeatSaberUtil.mainSettingsModel.roomRotation, 0f);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000040D0 File Offset: 0x000022D0
		public static Vector3 GetControllerPositionOffset()
		{
			return (!BeatSaberUtil.mainSettingsModel) ? Vector3.zero : BeatSaberUtil.mainSettingsModel.controllerPosition;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004104 File Offset: 0x00002304
		public static Vector3 GetControllerRotationOffset()
		{
			return (!BeatSaberUtil.mainSettingsModel) ? Vector3.zero : BeatSaberUtil.mainSettingsModel.controllerRotation;
		}

		// Token: 0x0400011B RID: 283
		private static MainSettingsModelSO _mainSettingsModel;

		// Token: 0x0400011C RID: 284
		private static PlayerDataModel _playerDataModel;

		// Token: 0x0400011D RID: 285
		public const float kDefaultPlayerEyeHeight = 1.6999999f;
	}
}
