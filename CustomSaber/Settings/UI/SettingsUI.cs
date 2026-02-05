using System;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;

namespace CustomSaber.Settings.UI
{
	// Token: 0x02000019 RID: 25
	internal class SettingsUI
	{
		// Token: 0x060000BA RID: 186 RVA: 0x000052B8 File Offset: 0x000034B8
		public static void CreateMenu()
		{
			bool flag = !SettingsUI.created;
			if (flag)
			{
				MenuButton menuButton = new MenuButton("光剑定制", "更换用户光剑!", new Action(SettingsUI.SabersMenuButtonPressed), true);
				PersistentSingleton<MenuButtons>.instance.RegisterButton(menuButton);
				SettingsUI.created = true;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005304 File Offset: 0x00003504
		public static void ShowSaberFlow()
		{
			bool flag = SettingsUI.sabersFlowCoordinator == null;
			if (flag)
			{
				SettingsUI.sabersFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<SabersFlowCoordinator>();
			}
			BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(SettingsUI.sabersFlowCoordinator, null, false, false);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005340 File Offset: 0x00003540
		private static void SabersMenuButtonPressed()
		{
			SettingsUI.ShowSaberFlow();
		}

		// Token: 0x04000074 RID: 116
		public static SabersFlowCoordinator sabersFlowCoordinator;

		// Token: 0x04000075 RID: 117
		public static bool created = false;
	}
}
