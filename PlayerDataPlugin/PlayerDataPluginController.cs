using System;
using System.Collections;
using System.Collections.Generic;
using NetViosCommon.Utility;
using PlayerDataPlugin.BSHandler;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

namespace PlayerDataPlugin
{
	// Token: 0x02000009 RID: 9
	public class PlayerDataPluginController : MonoBehaviour
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002659 File Offset: 0x00000859
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002660 File Offset: 0x00000860
		public static PlayerDataPluginController instance { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002671 File Offset: 0x00000871
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002668 File Offset: 0x00000868
		private bool HasHealthWarningEntered { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002682 File Offset: 0x00000882
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002679 File Offset: 0x00000879
		private bool HasLoadedMenuViewControllersScene { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002693 File Offset: 0x00000893
		// (set) Token: 0x0600003B RID: 59 RVA: 0x0000268A File Offset: 0x0000088A
		private bool HasSetupMenuViewControllersScene { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000026A4 File Offset: 0x000008A4
		// (set) Token: 0x0600003D RID: 61 RVA: 0x0000269B File Offset: 0x0000089B
		private bool HasLoadedGameCoreScene { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000026B5 File Offset: 0x000008B5
		// (set) Token: 0x0600003F RID: 63 RVA: 0x000026AC File Offset: 0x000008AC
		private bool HasSetupGameCoreScene { get; set; }

		// Token: 0x06000041 RID: 65 RVA: 0x000026C0 File Offset: 0x000008C0
		private void Awake()
		{
			if (PlayerDataPluginController.instance != null)
			{
				Logger.log.Warn("Instance of " + base.GetType().Name + " already exists, destroying.");
				Object.DestroyImmediate(this);
				return;
			}
			Object.DontDestroyOnLoad(this);
			PlayerDataPluginController.instance = this;
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
			SceneManager.sceneUnloaded += new UnityAction<Scene>(this.OnSceneUnloaded);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002744 File Offset: 0x00000944
		private void SetupInHealthWarningScene()
		{
			GameObject gameObject = GameObject.Find("Init");
			if (gameObject == null)
			{
				Logger.log.Error("Couldn't find Init");
				Application.Quit();
			}
			this.GameScenesManager = gameObject.GetComponent<PCAppInit>().GetPrivateProperty("gameScenesManager");
			if (this.GameScenesManager == null)
			{
				Logger.log.Error("Couldn't find GameScenesManager");
				return;
			}
			this.GameScenesManager.transitionDidStartEvent -= this.HandleTransitionDidStartEvent;
			this.GameScenesManager.transitionDidStartEvent += this.HandleTransitionDidStartEvent;
			this.GameScenesManager.transitionDidFinishEvent -= this.HandleTransitionDidFinishEvent;
			this.GameScenesManager.transitionDidFinishEvent += this.HandleTransitionDidFinishEvent;
			this.GameScenesManager.beforeDismissingScenesEvent -= this.HandleBeforeDismissingScenesEvent;
			this.GameScenesManager.beforeDismissingScenesEvent += this.HandleBeforeDismissingScenesEvent;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002838 File Offset: 0x00000A38
		private void Start()
		{
			GameObject gameObject = GameObject.Find("SdkPluginController");
			if (gameObject == null)
			{
				Logger.log.Error("Couldn't find SdkPluginController");
				Application.Quit();
			}
			SDKDataComponent sdkdataComponent = null;
			List<Component> list = new List<Component>();
			gameObject.GetComponents(typeof(Component), list);
			foreach (Component component in list)
			{
				sdkdataComponent = component as SDKDataComponent;
				if (sdkdataComponent != null)
				{
					break;
				}
			}
			if (sdkdataComponent == null)
			{
				Logger.log.Error("Couldn't find SDKDataComponent");
				Application.Quit();
			}
			sdkdataComponent.OnSDKData += delegate(string sdkData)
			{
				Singleton<Player>.Instance.SdkData = sdkData;
			};
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002910 File Offset: 0x00000B10
		private void Update()
		{
			if (this.HasSetupGameCoreScene)
			{
				Singleton<GameEventController>.Instance.UpdateInGameCore();
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002078 File Offset: 0x00000278
		private void LateUpdate()
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002078 File Offset: 0x00000278
		private void OnEnable()
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002078 File Offset: 0x00000278
		private void OnDisable()
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002924 File Offset: 0x00000B24
		private void OnDestroy()
		{
			PlayerDataPluginController.instance = null;
			SceneManager.activeSceneChanged -= new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
			SceneManager.sceneUnloaded -= new UnityAction<Scene>(this.OnSceneUnloaded);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000295F File Offset: 0x00000B5F
		private void ProcessEnterGameCore()
		{
			Singleton<GameEventController>.Instance.SetupForGameCoreScene();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000296C File Offset: 0x00000B6C
		private void ProcessExitGameCore()
		{
			if (!this.HasLoadedMenuViewControllersScene || !this.HasSetupMenuViewControllersScene)
			{
				Logger.log.Warn("prevScene: GameCore with MenuViewControllersScene status:" + string.Format("loaded: {0}, setup: {1}", this.HasLoadedMenuViewControllersScene, this.HasSetupMenuViewControllersScene));
			}
			if (!this.HasLoadedGameCoreScene || !this.HasSetupGameCoreScene)
			{
				Logger.log.Warn("prevScene: GameCore with itself status:" + string.Format("loaded: {0}, setup: {1}", this.HasLoadedGameCoreScene, this.HasSetupGameCoreScene));
			}
			Singleton<GameEventController>.Instance.TeardownForGameCoreScene();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002A0B File Offset: 0x00000C0B
		private IEnumerator ExecuteSceneLogicDelayFrames(int frames)
		{
			int num;
			for (int i = 0; i < frames; i = num + 1)
			{
				yield return null;
				num = i;
			}
			string name = SceneManager.GetActiveScene().name;
			if (name == "MenuViewControllers")
			{
				if (this.GameCoreEntered)
				{
					this.ProcessExitGameCore();
					Singleton<GameEventController>.Instance.PostProcessAfterExitGameCore();
					this.GameCoreEntered = false;
				}
				if (this.HasLoadedMenuViewControllersScene)
				{
					if (!this.HasSetupMenuViewControllersScene)
					{
						this.HasSetupMenuViewControllersScene = true;
					}
					Singleton<GameEventController>.Instance.SetupForMenuViewControllersScene();
				}
				yield break;
			}
			if (name == "GameCore")
			{
				if (this.HasLoadedGameCoreScene && !this.HasSetupGameCoreScene)
				{
					this.ProcessEnterGameCore();
					this.HasSetupGameCoreScene = true;
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002A21 File Offset: 0x00000C21
		private IEnumerator DetectGameCoreScene(int frames)
		{
			int num;
			for (int i = 0; i < frames; i = num + 1)
			{
				yield return null;
				num = i;
			}
			this.GameCoreEntered = SceneManager.GetActiveScene().name == "GameCore";
			yield break;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002A38 File Offset: 0x00000C38
		public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
		{
			if (nextScene.name == "GameCore")
			{
				base.StartCoroutine(this.DetectGameCoreScene(1));
			}
			if (nextScene.name == "HealthWarning" && !this.HasHealthWarningEntered)
			{
				this.HasHealthWarningEntered = true;
				this.SetupInHealthWarningScene();
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002A90 File Offset: 0x00000C90
		public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
		{
			if (scene.name == "MenuViewControllers")
			{
				if (this.HasLoadedMenuViewControllersScene)
				{
					Logger.log.Warn("Why MenuViewControllers scene loaded again???");
					return;
				}
				this.HasLoadedMenuViewControllersScene = true;
				return;
			}
			else
			{
				if (!(scene.name == "GameCore"))
				{
					return;
				}
				if (!this.HasLoadedMenuViewControllersScene)
				{
					Logger.log.Warn("Why MenuViewControllers haven't been loaded yet???");
					return;
				}
				if (this.HasLoadedGameCoreScene)
				{
					Logger.log.Warn("Why GameCore scene loaded again???");
					return;
				}
				this.HasLoadedGameCoreScene = true;
				return;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002B1C File Offset: 0x00000D1C
		public void OnSceneUnloaded(Scene scene)
		{
			if (scene.name == "GameCore")
			{
				this.HasLoadedGameCoreScene = false;
				this.HasSetupGameCoreScene = false;
				return;
			}
			if (scene.name == "MenuViewControllers")
			{
				this.HasLoadedMenuViewControllersScene = false;
				this.HasSetupMenuViewControllersScene = false;
				return;
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002B6D File Offset: 0x00000D6D
		private void HandleTransitionDidStartEvent(float minDuration)
		{
			SceneManager.GetActiveScene();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002B78 File Offset: 0x00000D78
		private void HandleTransitionDidFinishEvent(ScenesTransitionSetupDataSO scenesTransitionSetupDataSO, DiContainer dic)
		{
			Scene activeScene = SceneManager.GetActiveScene();
			if (activeScene.name == "MenuViewControllers" || activeScene.name == "GameCore")
			{
				base.StartCoroutine(this.ExecuteSceneLogicDelayFrames(0));
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002B6D File Offset: 0x00000D6D
		private void HandleBeforeDismissingScenesEvent()
		{
			SceneManager.GetActiveScene();
		}

		// Token: 0x04000014 RID: 20
		public const string HEALTH_WARNING = "HealthWarning";

		// Token: 0x04000015 RID: 21
		public const string MENU = "MenuViewControllers";

		// Token: 0x04000016 RID: 22
		public const string GAME = "GameCore";

		// Token: 0x04000017 RID: 23
		private bool GameCoreEntered;

		// Token: 0x04000018 RID: 24
		public static List<IHandler> Handlers = new List<IHandler>();

		// Token: 0x0400001E RID: 30
		private GameScenesManager GameScenesManager;
	}
}
