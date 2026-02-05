using System;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage.Animations;
using HMUI;
using IPA.Utilities;
using Polyglot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage
{
	// Token: 0x02000004 RID: 4
	public static class BeatSaberUI
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002050 File Offset: 0x00000250
		public static MainFlowCoordinator MainFlowCoordinator
		{
			get
			{
				if (BeatSaberUI._mainFlowCoordinator == null)
				{
					BeatSaberUI._mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First<MainFlowCoordinator>();
				}
				return BeatSaberUI._mainFlowCoordinator;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002074 File Offset: 0x00000274
		public static T CreateViewController<T>() where T : ViewController
		{
			T t = new GameObject("BSMLViewController").AddComponent<T>();
			Object.DontDestroyOnLoad(t.gameObject);
			t.rectTransform.anchorMin = new Vector2(0f, 0f);
			t.rectTransform.anchorMax = new Vector2(1f, 1f);
			t.rectTransform.sizeDelta = new Vector2(0f, 0f);
			t.rectTransform.anchoredPosition = new Vector2(0f, 0f);
			return t;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000211C File Offset: 0x0000031C
		public static T CreateFlowCoordinator<T>() where T : FlowCoordinator
		{
			T t = new GameObject("BSMLFlowCoordinator").AddComponent<T>();
			t.SetField("_baseInputModule", BeatSaberUI.MainFlowCoordinator.GetField("_baseInputModule"));
			return t;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000214C File Offset: 0x0000034C
		public static TMP_FontAsset MainTextFont
		{
			get
			{
				TMP_FontAsset tmp_FontAsset;
				if ((tmp_FontAsset = BeatSaberUI.mainTextFont) == null)
				{
					tmp_FontAsset = (BeatSaberUI.mainTextFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault((TMP_FontAsset t) => t.name == "Teko-Medium SDF No Glow"));
				}
				return tmp_FontAsset;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002188 File Offset: 0x00000388
		public static TMP_FontAsset CreateFixedUIFontClone(TMP_FontAsset font)
		{
			Shader shader = BeatSaberUI.MainTextFont.material.shader;
			TMP_FontAsset tmp_FontAsset = Object.Instantiate<TMP_FontAsset>(font);
			tmp_FontAsset.material.shader = shader;
			return tmp_FontAsset;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021B7 File Offset: 0x000003B7
		public static string SetName(this TMP_FontAsset font, string name)
		{
			font.name = name;
			font.hashCode = TMP_TextUtilities.GetSimpleHashCode(font.name);
			return name;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021D2 File Offset: 0x000003D2
		public static TMP_FontAsset CreateTMPFont(Font font, string nameOverride = null)
		{
			TMP_FontAsset tmp_FontAsset = TMP_FontAsset.CreateFontAsset(font);
			tmp_FontAsset.SetName(nameOverride ?? font.name);
			return tmp_FontAsset;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021EC File Offset: 0x000003EC
		public static TextMeshProUGUI CreateText(RectTransform parent, string text, Vector2 anchoredPosition)
		{
			return BeatSaberUI.CreateText(parent, text, anchoredPosition, new Vector2(60f, 10f));
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002208 File Offset: 0x00000408
		public static TextMeshProUGUI CreateText(RectTransform parent, string text, Vector2 anchoredPosition, Vector2 sizeDelta)
		{
			GameObject gameObject = new GameObject("CustomUIText");
			gameObject.SetActive(false);
			TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.font = BeatSaberUI.MainTextFont;
			textMeshProUGUI.rectTransform.SetParent(parent, false);
			textMeshProUGUI.text = text;
			textMeshProUGUI.fontSize = 4f;
			textMeshProUGUI.color = Color.white;
			textMeshProUGUI.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			textMeshProUGUI.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			textMeshProUGUI.rectTransform.sizeDelta = sizeDelta;
			textMeshProUGUI.rectTransform.anchoredPosition = anchoredPosition;
			gameObject.SetActive(true);
			return textMeshProUGUI;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022B8 File Offset: 0x000004B8
		public static void SetButtonText(this Button _button, string _text)
		{
			LocalizedTextMeshProUGUI componentInChildren = _button.GetComponentInChildren<LocalizedTextMeshProUGUI>();
			if (componentInChildren != null)
			{
				Object.Destroy(componentInChildren);
			}
			TextMeshProUGUI componentInChildren2 = _button.GetComponentInChildren<TextMeshProUGUI>();
			if (componentInChildren2 != null)
			{
				componentInChildren2.text = _text;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022F2 File Offset: 0x000004F2
		public static void SetButtonTextSize(this Button _button, float _fontSize)
		{
			if (_button.GetComponentInChildren<TextMeshProUGUI>() != null)
			{
				_button.GetComponentInChildren<TextMeshProUGUI>().fontSize = _fontSize;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000230E File Offset: 0x0000050E
		public static void ToggleWordWrapping(this Button _button, bool enableWordWrapping)
		{
			if (_button.GetComponentInChildren<TextMeshProUGUI>() != null)
			{
				_button.GetComponentInChildren<TextMeshProUGUI>().enableWordWrapping = enableWordWrapping;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000232A File Offset: 0x0000052A
		public static void SetButtonIcon(this Button _button, Sprite _icon)
		{
			if (_button.GetComponentsInChildren<Image>().Count<Image>() > 1)
			{
				_button.GetComponentsInChildren<Image>().First((Image x) => x.name == "Icon").sprite = _icon;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000236A File Offset: 0x0000056A
		public static void SetButtonBackground(this Button _button, Sprite _background)
		{
			if (_button.GetComponentsInChildren<Image>().Count<Image>() > 0)
			{
				_button.GetComponentsInChildren<Image>()[0].sprite = _background;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002388 File Offset: 0x00000588
		public static void SetImage(this Image image, string location)
		{
			AnimationStateUpdater component = image.GetComponent<AnimationStateUpdater>();
			if (component != null)
			{
				Object.DestroyImmediate(component);
			}
			if (location.StartsWith("#"))
			{
				string imgName = location.Substring(1);
				try
				{
					image.sprite = Resources.FindObjectsOfTypeAll<Sprite>().First((Sprite x) => x.name == imgName);
					return;
				}
				catch
				{
					Logger.log.Error("Could not find Texture with image name " + imgName);
					return;
				}
			}
			if (location.EndsWith(".gif") || location.EndsWith(".apng"))
			{
				AnimationStateUpdater stateUpdater2 = image.gameObject.AddComponent<AnimationStateUpdater>();
				stateUpdater2.image = image;
				stateUpdater2.controllerData = PersistentSingleton<AnimationController>.instance.loadingAnimation;
				AnimationControllerData animationControllerData;
				if (PersistentSingleton<AnimationController>.instance.RegisteredAnimations.TryGetValue(location, out animationControllerData))
				{
					stateUpdater2.controllerData = animationControllerData;
					return;
				}
				Action<Texture2D, Rect[], float[], int, int> <>9__2;
				Utilities.GetData(location, delegate(byte[] data)
				{
					AnimationType animationType = (location.EndsWith(".gif") ? AnimationType.GIF : AnimationType.APNG);
					Action<Texture2D, Rect[], float[], int, int> action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate(Texture2D tex, Rect[] uvs, float[] delays, int width, int height)
						{
							AnimationControllerData animationControllerData2 = PersistentSingleton<AnimationController>.instance.Register(location, tex, uvs, delays);
							stateUpdater2.controllerData = animationControllerData2;
						});
					}
					AnimationLoader.Process(animationType, data, action);
				});
				return;
			}
			else
			{
				AnimationStateUpdater stateUpdater = image.gameObject.AddComponent<AnimationStateUpdater>();
				stateUpdater.image = image;
				stateUpdater.controllerData = PersistentSingleton<AnimationController>.instance.loadingAnimation;
				Utilities.GetData(location, delegate(byte[] data)
				{
					if (stateUpdater != null)
					{
						Object.DestroyImmediate(stateUpdater);
					}
					image.sprite = Utilities.LoadSpriteRaw(data, 100f);
					image.sprite.texture.wrapMode = 1;
				});
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002580 File Offset: 0x00000780
		public static void PresentFlowCoordinator(this FlowCoordinator current, FlowCoordinator flowCoordinator, Action finishedCallback = null, bool immediately = false, bool replaceTopViewController = false)
		{
			BeatSaberUI.PresentFlowCoordinatorDelegate(current, flowCoordinator, finishedCallback, immediately, replaceTopViewController);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002592 File Offset: 0x00000792
		public static void DismissFlowCoordinator(this FlowCoordinator current, FlowCoordinator flowCoordinator, Action finishedCallback = null, bool immediately = false)
		{
			BeatSaberUI.DismissFlowCoordinatorDelegate(current, flowCoordinator, finishedCallback, immediately);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000025A4 File Offset: 0x000007A4
		private static PresentFlowCoordinatorDelegate PresentFlowCoordinatorDelegate
		{
			get
			{
				if (BeatSaberUI._presentFlowCoordinatorDelegate == null)
				{
					MethodInfo method = typeof(FlowCoordinator).GetMethod("PresentFlowCoordinator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					BeatSaberUI._presentFlowCoordinatorDelegate = (PresentFlowCoordinatorDelegate)Delegate.CreateDelegate(typeof(PresentFlowCoordinatorDelegate), method);
				}
				return BeatSaberUI._presentFlowCoordinatorDelegate;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000025F0 File Offset: 0x000007F0
		private static DismissFlowCoordinatorDelegate DismissFlowCoordinatorDelegate
		{
			get
			{
				if (BeatSaberUI._dismissFlowCoordinatorDelegate == null)
				{
					MethodInfo method = typeof(FlowCoordinator).GetMethod("DismissFlowCoordinator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					BeatSaberUI._dismissFlowCoordinatorDelegate = (DismissFlowCoordinatorDelegate)Delegate.CreateDelegate(typeof(DismissFlowCoordinatorDelegate), method);
				}
				return BeatSaberUI._dismissFlowCoordinatorDelegate;
			}
		}

		// Token: 0x04000001 RID: 1
		private static MainFlowCoordinator _mainFlowCoordinator;

		// Token: 0x04000002 RID: 2
		private static TMP_FontAsset mainTextFont;

		// Token: 0x04000003 RID: 3
		private static PresentFlowCoordinatorDelegate _presentFlowCoordinatorDelegate;

		// Token: 0x04000004 RID: 4
		private static DismissFlowCoordinatorDelegate _dismissFlowCoordinatorDelegate;
	}
}
