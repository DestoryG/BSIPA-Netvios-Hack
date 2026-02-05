using System;
using System.Diagnostics;
using System.Linq;
using BeatSaberMarkupLanguage.MenuButtons;
using CustomAvatar.StereoRendering;
using CustomAvatar.UI;
using CustomAvatar.Utilities;
using IPA;
using IPA.Logging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

namespace CustomAvatar
{
	// Token: 0x0200001A RID: 26
	[Plugin(RuntimeOptions.SingleStartInit)]
	internal class Plugin
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600004D RID: 77 RVA: 0x00003BD8 File Offset: 0x00001DD8
		// (remove) Token: 0x0600004E RID: 78 RVA: 0x00003C10 File Offset: 0x00001E10
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<ScenesTransitionSetupDataSO, DiContainer> sceneTransitionDidFinish;

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003C45 File Offset: 0x00001E45
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00003C4C File Offset: 0x00001E4C
		public static Plugin instance { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003C54 File Offset: 0x00001E54
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00003C5B File Offset: 0x00001E5B
		public static Logger logger { get; private set; }

		// Token: 0x06000053 RID: 83 RVA: 0x00003C63 File Offset: 0x00001E63
		[Init]
		public Plugin(Logger logger)
		{
			Plugin.logger = logger;
			Plugin.instance = this;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003C7C File Offset: 0x00001E7C
		[OnStart]
		public void OnStart()
		{
			SettingsManager.LoadSettings();
			AvatarManager.instance.LoadAvatarFromSettingsAsync();
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
			KeyboardInputHandler keyboardInputHandler = new GameObject("KeyboardInputHandler").AddComponent<KeyboardInputHandler>();
			Object.DontDestroyOnLoad(keyboardInputHandler.gameObject);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003CCC File Offset: 0x00001ECC
		[OnExit]
		public void OnExit()
		{
			bool flag = this._scenesManager != null;
			if (flag)
			{
				this._scenesManager.transitionDidFinishEvent -= this.sceneTransitionDidFinish;
				this._scenesManager.transitionDidFinishEvent -= this.SceneTransitionDidFinish;
			}
			SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
			SettingsManager.SaveSettings();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003D30 File Offset: 0x00001F30
		public void OnSceneLoaded(Scene newScene, LoadSceneMode mode)
		{
			bool flag = this._scenesManager == null;
			if (flag)
			{
				this._scenesManager = Resources.FindObjectsOfTypeAll<GameScenesManager>().FirstOrDefault<GameScenesManager>();
				bool flag2 = this._scenesManager != null;
				if (flag2)
				{
					this._scenesManager.transitionDidFinishEvent += this.sceneTransitionDidFinish;
					this._scenesManager.transitionDidFinishEvent += this.SceneTransitionDidFinish;
				}
			}
			bool flag3 = newScene.name == "MenuCore";
			if (flag3)
			{
				try
				{
					PersistentSingleton<MenuButtons>.instance.RegisterButton(new MenuButton("虚拟角色", delegate
					{
						MainFlowCoordinator mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First<MainFlowCoordinator>();
						AvatarListFlowCoordinator avatarListFlowCoordinator = new GameObject("AvatarListFlowCoordinator").AddComponent<AvatarListFlowCoordinator>();
						mainFlowCoordinator.InvokePrivateMethod("PresentFlowCoordinator", new object[] { avatarListFlowCoordinator, null, true, false });
					}));
				}
				catch (Exception)
				{
					Plugin.logger.Warn("Failed to add menu button, spawning mirror instead");
					this._mirrorContainer = new GameObject();
					Object.DontDestroyOnLoad(this._mirrorContainer);
					PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(MirrorHelper.SpawnMirror(new Vector3(0f, 0f, -1.5f), Quaternion.Euler(-90f, 180f, 0f), new Vector3(0.5f, 1f, 0.25f), this._mirrorContainer.transform));
				}
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003E84 File Offset: 0x00002084
		private void SceneTransitionDidFinish(ScenesTransitionSetupDataSO setupData, DiContainer container)
		{
			foreach (Camera camera in Camera.allCameras)
			{
				bool flag = camera.gameObject.GetComponent<VRRenderEventDetector>() == null;
				if (flag)
				{
					camera.gameObject.AddComponent<VRRenderEventDetector>();
					Plugin.logger.Info(string.Format("Added {0} to {1}", "VRRenderEventDetector", camera));
				}
			}
			Camera main = Camera.main;
			bool flag2 = main;
			if (flag2)
			{
				this.SetCameraCullingMask(main);
				main.nearClipPlane = SettingsManager.settings.cameraNearClipPlane;
			}
			else
			{
				Plugin.logger.Error("Could not find main camera!");
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003F2E File Offset: 0x0000212E
		private void SetCameraCullingMask(Camera camera)
		{
			Plugin.logger.Debug("Adding third person culling mask to " + camera.name);
			camera.cullingMask &= -9;
		}

		// Token: 0x04000116 RID: 278
		private GameScenesManager _scenesManager;

		// Token: 0x04000117 RID: 279
		private GameObject _mirrorContainer;
	}
}
