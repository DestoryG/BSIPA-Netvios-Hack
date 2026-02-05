using System;
using CustomAvatar.Avatar;
using CustomAvatar.Utilities;
using UnityEngine;

namespace CustomAvatar
{
	// Token: 0x0200000E RID: 14
	internal class KeyboardInputHandler : MonoBehaviour
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000025A8 File Offset: 0x000007A8
		private void Update()
		{
			AvatarManager instance = AvatarManager.instance;
			bool keyDown = Input.GetKeyDown(281);
			if (keyDown)
			{
				instance.SwitchToNextAvatar();
			}
			else
			{
				bool keyDown2 = Input.GetKeyDown(280);
				if (keyDown2)
				{
					instance.SwitchToPreviousAvatar();
				}
				else
				{
					bool keyDown3 = Input.GetKeyDown(278);
					if (keyDown3)
					{
						SettingsManager.settings.isAvatarVisibleInFirstPerson = !SettingsManager.settings.isAvatarVisibleInFirstPerson;
						Plugin.logger.Info((SettingsManager.settings.isAvatarVisibleInFirstPerson ? "Enabled" : "Disabled") + " first person visibility");
						SpawnedAvatar currentlySpawnedAvatar = instance.currentlySpawnedAvatar;
						if (currentlySpawnedAvatar != null)
						{
							currentlySpawnedAvatar.OnFirstPersonEnabledChanged();
						}
					}
					else
					{
						bool keyDown4 = Input.GetKeyDown(279);
						if (keyDown4)
						{
							SettingsManager.settings.resizeMode = (SettingsManager.settings.resizeMode + 1) % (AvatarResizeMode)3;
							Plugin.logger.Info(string.Format("Set resize mode to {0}", SettingsManager.settings.resizeMode));
							instance.ResizeCurrentAvatar();
						}
						else
						{
							bool keyDown5 = Input.GetKeyDown(277);
							if (keyDown5)
							{
								SettingsManager.settings.enableFloorAdjust = !SettingsManager.settings.enableFloorAdjust;
								Plugin.logger.Info((SettingsManager.settings.enableFloorAdjust ? "Enabled" : "Disabled") + " floor adjust");
								instance.ResizeCurrentAvatar();
							}
						}
					}
				}
			}
		}
	}
}
