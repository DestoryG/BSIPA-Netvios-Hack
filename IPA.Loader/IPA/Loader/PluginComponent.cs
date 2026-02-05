using System;
using IPA.Config;
using IPA.Loader.Composite;
using IPA.Updating.BeatMods;
using IPA.Utilities;
using IPA.Utilities.Async;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace IPA.Loader
{
	// Token: 0x0200004B RID: 75
	internal class PluginComponent : MonoBehaviour
	{
		// Token: 0x06000215 RID: 533 RVA: 0x0000AE94 File Offset: 0x00009094
		internal static PluginComponent Create()
		{
			return PluginComponent.Instance = new GameObject("IPA_PluginManager").AddComponent<PluginComponent>();
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000AEAC File Offset: 0x000090AC
		internal void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
			if (!PluginComponent.initialized)
			{
				UnityGame.SetMainThread();
				UnityGame.EnsureRuntimeGameVersion();
				PluginManager.Load();
				this.bsPlugins = new CompositeBSPlugin(PluginManager.BSMetas);
				this.ipaPlugins = new CompositeIPAPlugin(PluginManager.Plugins);
				base.gameObject.AddComponent<Updater>();
				this.bsPlugins.OnEnable();
				this.ipaPlugins.OnApplicationStart();
				SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
				SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
				SceneManager.sceneUnloaded += new UnityAction<Scene>(this.OnSceneUnloaded);
				UnityMainThreadTaskScheduler unitySched = UnityMainThreadTaskScheduler.Default as UnityMainThreadTaskScheduler;
				if (!unitySched.IsRunning)
				{
					base.StartCoroutine(unitySched.Coroutine());
				}
				PluginComponent.initialized = true;
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000AF78 File Offset: 0x00009178
		internal void Update()
		{
			this.bsPlugins.OnUpdate();
			this.ipaPlugins.OnUpdate();
			UnityMainThreadTaskScheduler unitySched = UnityMainThreadTaskScheduler.Default as UnityMainThreadTaskScheduler;
			if (!unitySched.IsRunning)
			{
				base.StartCoroutine(unitySched.Coroutine());
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000AFBB File Offset: 0x000091BB
		internal void LateUpdate()
		{
			this.bsPlugins.OnLateUpdate();
			this.ipaPlugins.OnLateUpdate();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000AFD3 File Offset: 0x000091D3
		internal void FixedUpdate()
		{
			this.bsPlugins.OnFixedUpdate();
			this.ipaPlugins.OnFixedUpdate();
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000AFEB File Offset: 0x000091EB
		internal void OnDestroy()
		{
			if (!this.quitting)
			{
				PluginComponent.Create();
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000AFFC File Offset: 0x000091FC
		internal void OnApplicationQuit()
		{
			SceneManager.activeSceneChanged -= new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
			SceneManager.sceneUnloaded -= new UnityAction<Scene>(this.OnSceneUnloaded);
			this.bsPlugins.OnApplicationQuit();
			this.ipaPlugins.OnApplicationQuit();
			ConfigRuntime.ShutdownRuntime();
			this.quitting = true;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000B05E File Offset: 0x0000925E
		internal void OnLevelWasLoaded(int level)
		{
			this.ipaPlugins.OnLevelWasLoaded(level);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000B06C File Offset: 0x0000926C
		internal void OnLevelWasInitialized(int level)
		{
			this.ipaPlugins.OnLevelWasInitialized(level);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000B07A File Offset: 0x0000927A
		private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
		{
			this.bsPlugins.OnSceneLoaded(scene, sceneMode);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000B089 File Offset: 0x00009289
		private void OnSceneUnloaded(Scene scene)
		{
			this.bsPlugins.OnSceneUnloaded(scene);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000B097 File Offset: 0x00009297
		private void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
		{
			this.bsPlugins.OnActiveSceneChanged(prevScene, nextScene);
		}

		// Token: 0x040000D9 RID: 217
		private CompositeBSPlugin bsPlugins;

		// Token: 0x040000DA RID: 218
		private CompositeIPAPlugin ipaPlugins;

		// Token: 0x040000DB RID: 219
		private bool quitting;

		// Token: 0x040000DC RID: 220
		public static PluginComponent Instance;

		// Token: 0x040000DD RID: 221
		private static bool initialized;
	}
}
