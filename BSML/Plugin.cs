using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Animations;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.MenuButtons;
using BeatSaberMarkupLanguage.Settings;
using BeatSaberMarkupLanguage.ViewControllers;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA;
using IPA.Logging;
using IPA.Utilities.Async;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BeatSaberMarkupLanguage
{
	// Token: 0x0200000A RID: 10
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00003A20 File Offset: 0x00001C20
		[Init]
		public void Init(global::IPA.Logging.Logger logger)
		{
			BeatSaberMarkupLanguage.Logger.log = logger;
			try
			{
				new Harmony("com.monkeymanboy.BeatSaberMarkupLanguage").PatchAll(Assembly.GetExecutingAssembly());
			}
			catch (Exception ex)
			{
				BeatSaberMarkupLanguage.Logger.log.Error(ex.Message);
			}
			PersistentSingleton<AnimationController>.instance.InitializeLoadingAnimation();
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
			BSEvents.menuSceneLoadedFresh += this.MenuLoadFresh;
			Plugin.config = new Config("BSML");
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003AA8 File Offset: 0x00001CA8
		[OnStart]
		public void OnStart()
		{
			FontManager.AsyncLoadSystemFonts().ContinueWith(delegate(Task _)
			{
				Plugin.<>c__DisplayClass2_0 CS$<>8__locals1 = new Plugin.<>c__DisplayClass2_0();
				if (!FontManager.TryGetTMPFontByFullName("Segoe UI", out CS$<>8__locals1.fallback, true) && !FontManager.TryGetTMPFontByFamily("Arial", out CS$<>8__locals1.fallback, null, false, true))
				{
					BeatSaberMarkupLanguage.Logger.log.Warn("Could not find fonts for either Segoe UI or Arial to set up fallbacks");
					return;
				}
				if (CS$<>8__locals1.fallback != null)
				{
					PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(CS$<>8__locals1.<OnStart>g__SetupFont|2());
				}
			}, UnityMainThreadTaskScheduler.Default).ContinueWith(delegate(Task t)
			{
				BeatSaberMarkupLanguage.Logger.log.Error("Errored while setting up fallback fonts:");
				BeatSaberMarkupLanguage.Logger.log.Error(t.Exception);
			}, TaskContinuationOptions.NotOnRanToCompletion);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003B0D File Offset: 0x00001D0D
		public void MenuLoadFresh()
		{
			BSMLSettings.instance.Setup();
			PersistentSingleton<MenuButtons>.instance.Setup();
			PersistentSingleton<GameplaySetup>.instance.Setup();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003B2D File Offset: 0x00001D2D
		public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
		{
			if (nextScene.name.Contains("Menu") && prevScene.name == "EmptyTransition")
			{
				PersistentSingleton<BSMLParser>.instance.MenuSceneLoaded();
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003B5F File Offset: 0x00001D5F
		private IEnumerator PresentTest()
		{
			yield return new WaitForSeconds(1f);
			TestViewController testViewController = BeatSaberUI.CreateViewController<TestViewController>();
			Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First<MainFlowCoordinator>().InvokeMethod("PresentViewController", new object[] { testViewController, null, false });
			yield break;
		}

		// Token: 0x04000014 RID: 20
		public static Config config;
	}
}
