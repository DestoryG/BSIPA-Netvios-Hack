using System;
using System.Linq;
using BS_Utils.Utilities;
using IPA;
using IPA.Logging;
using IPA.Netvios;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace NetviosHelperPlugin
{
	// Token: 0x02000002 RID: 2
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		internal static Plugin instance { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		internal static string Name
		{
			get
			{
				return "NetviosHelperPlugin";
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002068 File Offset: 0x00000268
		[Init]
		public void Init(global::IPA.Logging.Logger logger)
		{
			try
			{
				bool flag = !Utils.CheckIPA();
				if (flag)
				{
					Application.Quit();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Application.Quit();
			}
			Plugin.instance = this;
			NetviosHelperPlugin.Logger.log = logger;
			NetviosHelperPlugin.Logger.log.Debug("Logger initialized.");
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020CC File Offset: 0x000002CC
		[OnStart]
		public void OnApplicationStart()
		{
			NetviosHelperPlugin.Logger.log.Debug("OnApplicationStart");
			BSEvents.lateMenuSceneLoadedFresh += this.BSEvents_menuSceneLoadedFresh;
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002120 File Offset: 0x00000320
		[OnExit]
		public void OnApplicationQuit()
		{
			NetviosHelperPlugin.Logger.log.Debug("OnApplicationQuit");
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002133 File Offset: 0x00000333
		private void BSEvents_menuSceneLoadedFresh(ScenesTransitionSetupDataSO dataSo)
		{
			this.HiddenButtons();
			this.ChangeModifiers();
			this.ChangeWindowResolution();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000214C File Offset: 0x0000034C
		public void HiddenButtons()
		{
			MainFlowCoordinator mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First<MainFlowCoordinator>();
			bool flag = mainFlowCoordinator == null;
			if (!flag)
			{
				MainMenuViewController privateField = mainFlowCoordinator.GetPrivateField("_mainMenuViewController");
				bool flag2 = privateField == null;
				if (!flag2)
				{
					Button privateField2 = privateField.GetPrivateField("_partyButton");
					if (privateField2 != null)
					{
						privateField2.gameObject.SetActive(false);
					}
					Button privateField3 = privateField.GetPrivateField("_campaignButton");
					if (privateField3 != null)
					{
						privateField3.gameObject.SetActive(false);
					}
					PromoViewController privateField4 = mainFlowCoordinator.GetPrivateField("_promoViewController");
					if (privateField4 != null)
					{
						privateField4.gameObject.SetActive(false);
					}
					for (int i = 0; i < privateField4.transform.childCount; i++)
					{
						privateField4.transform.GetChild(i).gameObject.SetActive(false);
					}
					SoloFreePlayFlowCoordinator privateField5 = mainFlowCoordinator.GetPrivateField("_soloFreePlayFlowCoordinator");
					bool flag3 = privateField5 == null;
					if (!flag3)
					{
						this.HiddenLeaderboardView(privateField5);
						LevelFilteringNavigationController parentPrivateField = privateField5.GetParentPrivateField("_levelFilteringNavigationController");
						bool flag4 = parentPrivateField == null;
						if (!flag4)
						{
							TabBarViewController privateField6 = parentPrivateField.GetPrivateField("_tabBarViewController");
							bool flag5 = privateField6 == null;
							if (!flag5)
							{
								for (int j = 0; j < privateField6.transform.childCount; j++)
								{
									privateField6.transform.GetChild(j).gameObject.SetActive(false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022D4 File Offset: 0x000004D4
		public void HiddenLeaderboardView(SoloFreePlayFlowCoordinator soloFlowCoordinator)
		{
			bool flag = soloFlowCoordinator == null;
			if (!flag)
			{
				PlatformLeaderboardViewController privateField = soloFlowCoordinator.GetPrivateField("_platformLeaderboardViewController");
				bool flag2 = privateField == null;
				if (!flag2)
				{
					for (int i = 0; i < privateField.transform.childCount; i++)
					{
						privateField.transform.GetChild(i).gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002340 File Offset: 0x00000540
		public void ChangeModifiers()
		{
			bool flag = !this.isChangedModifiers;
			if (flag)
			{
				PlayerData playerData = Resources.FindObjectsOfTypeAll<PlayerDataModel>().FirstOrDefault<PlayerDataModel>().playerData;
				playerData.gameplayModifiers.noFail = true;
				this.isChangedModifiers = true;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002384 File Offset: 0x00000584
		public void ChangeWindowResolution()
		{
			bool flag = this.isChangeWindowResolution;
			if (!flag)
			{
				Resolution currentResolution = Screen.currentResolution;
				Debug.Log("=========== screenResolution W: " + currentResolution.width.ToString());
				Debug.Log("=========== screenResolution H: " + currentResolution.height.ToString());
				Debug.Log("=========== screen safeArea: " + Screen.safeArea.ToString());
				bool flag2 = currentResolution.width == 0 || currentResolution.width == 0;
				if (!flag2)
				{
					MainFlowCoordinator mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First<MainFlowCoordinator>();
					bool flag3 = mainFlowCoordinator == null;
					if (!flag3)
					{
						MainSettingsModelSO privateField = mainFlowCoordinator.GetPrivateField("_mainSettingsModel");
						bool flag4 = privateField == null;
						if (!flag4)
						{
							privateField.windowResolution.value = new Vector2Int(currentResolution.width, currentResolution.height);
							privateField.Save();
							Screen.SetResolution(currentResolution.width, currentResolution.height, privateField.fullscreen);
							this.isChangeWindowResolution = true;
						}
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024B0 File Offset: 0x000006B0
		public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
		{
			bool flag = nextScene.name == "HealthWarning";
			if (flag)
			{
				HealthWarningViewController healthWarningViewController = Resources.FindObjectsOfTypeAll<HealthWarningViewController>().First<HealthWarningViewController>();
				bool flag2 = healthWarningViewController == null;
				if (!flag2)
				{
					Button privateField = healthWarningViewController.GetPrivateField("_openDataPrivacyPageButton");
					bool flag3 = privateField == null;
					if (!flag3)
					{
						for (int i = 0; i < privateField.transform.childCount; i++)
						{
							privateField.transform.GetChild(i).gameObject.SetActive(false);
						}
					}
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002544 File Offset: 0x00000744
		public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
		{
			bool flag = this._scenesManager == null;
			if (flag)
			{
				this._scenesManager = Resources.FindObjectsOfTypeAll<GameScenesManager>().FirstOrDefault<GameScenesManager>();
				bool flag2 = this._scenesManager != null;
				if (flag2)
				{
					this._scenesManager.transitionDidStartEvent -= this.HandleTransitionDidStartEvent;
					this._scenesManager.transitionDidStartEvent += this.HandleTransitionDidStartEvent;
					this._scenesManager.transitionDidFinishEvent -= this.HandleTransitionDidFinishEvent;
					this._scenesManager.transitionDidFinishEvent += this.HandleTransitionDidFinishEvent;
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000025EC File Offset: 0x000007EC
		private void HandleTransitionDidStartEvent(float minDuration)
		{
			Scene activeScene = SceneManager.GetActiveScene();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002600 File Offset: 0x00000800
		private void HandleTransitionDidFinishEvent(ScenesTransitionSetupDataSO scenesTransitionSetupDataSO, DiContainer dic)
		{
			Scene activeScene = SceneManager.GetActiveScene();
		}

		// Token: 0x04000002 RID: 2
		internal const string HEALTH_WARNING = "HealthWarning";

		// Token: 0x04000003 RID: 3
		internal const string Menu_Core = "MenuCore";

		// Token: 0x04000004 RID: 4
		internal bool isChangedModifiers = false;

		// Token: 0x04000005 RID: 5
		internal bool isChangeWindowResolution = false;

		// Token: 0x04000006 RID: 6
		private GameScenesManager _scenesManager;
	}
}
