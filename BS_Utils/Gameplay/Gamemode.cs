using System;
using System.Linq;
using BS_Utils.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BS_Utils.Gameplay
{
	// Token: 0x0200000A RID: 10
	public class Gamemode
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000042C4 File Offset: 0x000024C4
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x000042CB File Offset: 0x000024CB
		public static BeatmapCharacteristicSO SelectedCharacteristic { get; internal set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000042D3 File Offset: 0x000024D3
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x000042DA File Offset: 0x000024DA
		public static bool IsPartyActive { get; private set; } = false;

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000042E2 File Offset: 0x000024E2
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x000042E9 File Offset: 0x000024E9
		public static string GameMode { get; private set; } = "Standard";

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000042F1 File Offset: 0x000024F1
		// (set) Token: 0x060000AA RID: 170 RVA: 0x000042F8 File Offset: 0x000024F8
		public static bool IsIsolatedLevel { get; internal set; } = false;

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004300 File Offset: 0x00002500
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00004307 File Offset: 0x00002507
		public static string IsolatingMod { get; internal set; } = "";

		// Token: 0x060000AD RID: 173 RVA: 0x00004310 File Offset: 0x00002510
		public static void Init()
		{
			Plugin.ApplyHarmonyPatches();
			if (Gamemode.MainMenuViewController == null)
			{
				Gamemode.SoloFreePlayFlowCoordinator = Resources.FindObjectsOfTypeAll<SoloFreePlayFlowCoordinator>().FirstOrDefault<SoloFreePlayFlowCoordinator>();
				Gamemode.PartyFreePlayFlowCoordinator = Resources.FindObjectsOfTypeAll<PartyFreePlayFlowCoordinator>().FirstOrDefault<PartyFreePlayFlowCoordinator>();
				Gamemode.MainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().FirstOrDefault<MainMenuViewController>();
				if (Gamemode.MainMenuViewController == null)
				{
					return;
				}
				Gamemode.MainMenuViewController.didFinishEvent += Gamemode.MainMenuViewController_didFinishEvent;
				if (Gamemode.CharacteristicSelectionViewController == null)
				{
					Gamemode.CharacteristicSelectionViewController = Resources.FindObjectsOfTypeAll<BeatmapCharacteristicSegmentedControlController>().FirstOrDefault<BeatmapCharacteristicSegmentedControlController>();
					if (Gamemode.CharacteristicSelectionViewController != null)
					{
						Gamemode.CharacteristicSelectionViewController.didSelectBeatmapCharacteristicEvent += Gamemode.CharacteristicSelectionViewController_didSelectBeatmapCharacteristicEvent;
					}
				}
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000043C2 File Offset: 0x000025C2
		private static void MainMenuViewController_didFinishEvent(MainMenuViewController arg1, MainMenuViewController.MenuButton arg2)
		{
			if (arg2 == 1)
			{
				Gamemode.IsPartyActive = true;
				return;
			}
			Gamemode.IsPartyActive = false;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000043D5 File Offset: 0x000025D5
		internal static void CharacteristicSelectionViewController_didSelectBeatmapCharacteristicEvent(BeatmapCharacteristicSegmentedControlController arg1, BeatmapCharacteristicSO arg2)
		{
			Gamemode.GameMode = arg2.serializedName;
			Gamemode.SelectedCharacteristic = arg2;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000043E8 File Offset: 0x000025E8
		internal static void ResetGameMode()
		{
			Gamemode.GameMode = "Standard";
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000043F4 File Offset: 0x000025F4
		public static void NextLevelIsIsolated(string modName)
		{
			Plugin.ApplyHarmonyPatches();
			Gamemode.IsIsolatedLevel = true;
			Logger.Log("Isolated level being started by " + modName);
			Gamemode.IsolatingMod = modName;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004417 File Offset: 0x00002617
		private static void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
		{
		}

		// Token: 0x04000034 RID: 52
		internal static BeatmapCharacteristicSegmentedControlController CharacteristicSelectionViewController;

		// Token: 0x04000036 RID: 54
		internal static SoloFreePlayFlowCoordinator SoloFreePlayFlowCoordinator;

		// Token: 0x04000037 RID: 55
		internal static PartyFreePlayFlowCoordinator PartyFreePlayFlowCoordinator;

		// Token: 0x04000038 RID: 56
		internal static MainMenuViewController MainMenuViewController;
	}
}
