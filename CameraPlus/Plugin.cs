using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using HarmonyLib;
using IPA;
using IPA.Logging;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CameraPlus
{
	// Token: 0x0200000A RID: 10
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00005A33 File Offset: 0x00003C33
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00005A3A File Offset: 0x00003C3A
		public static Plugin Instance { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00005A42 File Offset: 0x00003C42
		public static string Name
		{
			get
			{
				return "CameraPlus";
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00005A49 File Offset: 0x00003C49
		public static string MainCamera
		{
			get
			{
				return "cameraplus";
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00005A50 File Offset: 0x00003C50
		[Init]
		public void Init(Logger logger)
		{
			Logger.log = logger;
			Logger.Log("Logger prepared", Logger.Level.Debug);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00005A64 File Offset: 0x00003C64
		[OnStart]
		public void OnApplicationStart()
		{
			if (this._init)
			{
				return;
			}
			this._init = true;
			Plugin.Instance = this;
			this._harmony = new Harmony("com.brian91292.beatsaber.cameraplus");
			try
			{
				this._harmony.PatchAll(Assembly.GetExecutingAssembly());
			}
			catch (Exception ex)
			{
				Logger.Log(string.Format("Failed to apply harmony patches! {0}", ex), Logger.Level.Error);
			}
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			CameraUtilities.AddNewCamera(Plugin.MainCamera, null, false);
			CameraProfiles.CreateMainDirectory();
			string text = Path.Combine(UnityGame.UserDataPath, Plugin.Name + ".ini");
			this._rootConfig = new RootConfig(text);
			this._profileChanger = new ProfileChanger();
			Logger.Log(Plugin.Name + " has started", Logger.Level.Notice);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00005B38 File Offset: 0x00003D38
		public void OnActiveSceneChanged(Scene from, Scene to)
		{
			PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this.DelayedActiveSceneChanged(from, to));
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00005B4D File Offset: 0x00003D4D
		private IEnumerator DelayedActiveSceneChanged(Scene from, Scene to)
		{
			yield return new WaitForSeconds(0.5f);
			CameraUtilities.ReloadCameras();
			if (this.ActiveSceneChanged != null)
			{
				if (this._rootConfig.ProfileSceneChange)
				{
					if (to.name == "GameCore" && this._rootConfig.GameProfile != "")
					{
						this._profileChanger.ProfileChange(this._rootConfig.GameProfile);
					}
					else if (to.name == "MenuCore" && this._rootConfig.MenuProfile != "")
					{
						this._profileChanger.ProfileChange(this._rootConfig.MenuProfile);
					}
				}
				yield return new WaitForSeconds(1f);
				Action<Scene, Scene> activeSceneChanged = this.ActiveSceneChanged;
				foreach (Delegate @delegate in (activeSceneChanged != null) ? activeSceneChanged.GetInvocationList() : null)
				{
					try
					{
						if (@delegate != null)
						{
							@delegate.DynamicInvoke(new object[] { from, to });
						}
					}
					catch (Exception ex)
					{
						Logger.Log("Exception while invoking ActiveSceneChanged: " + ex.Message + "\n" + ex.StackTrace, Logger.Level.Error);
					}
				}
			}
			if (to.name == "GameCore")
			{
				CameraUtilities.SetAllCameraCulling();
			}
			yield break;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00005B6A File Offset: 0x00003D6A
		[OnExit]
		public void OnApplicationQuit()
		{
			this._harmony.UnpatchAll("com.brian91292.beatsaber.cameraplus");
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003561 File Offset: 0x00001761
		public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003561 File Offset: 0x00001761
		public void OnSceneUnloaded(Scene scene)
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003561 File Offset: 0x00001761
		public void OnUpdate()
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00005B7C File Offset: 0x00003D7C
		public void OnFixedUpdate()
		{
			if (CameraPlusBehaviour.currentCursor != CameraPlusBehaviour.CursorType.None && !CameraPlusBehaviour.anyInstanceBusy && CameraPlusBehaviour.wasWithinBorder && CameraPlusBehaviour.GetTopmostInstanceAtCursorPos() == null)
			{
				CameraPlusBehaviour.SetCursor(CameraPlusBehaviour.CursorType.None);
				CameraPlusBehaviour.wasWithinBorder = false;
			}
		}

		// Token: 0x04000044 RID: 68
		private bool _init;

		// Token: 0x04000045 RID: 69
		private Harmony _harmony;

		// Token: 0x04000046 RID: 70
		public Action<Scene, Scene> ActiveSceneChanged;

		// Token: 0x04000047 RID: 71
		public ConcurrentDictionary<string, CameraPlusInstance> Cameras = new ConcurrentDictionary<string, CameraPlusInstance>();

		// Token: 0x04000049 RID: 73
		private RootConfig _rootConfig;

		// Token: 0x0400004A RID: 74
		private ProfileChanger _profileChanger;
	}
}
