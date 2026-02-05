using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Settings;
using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BeatSaberMarkupLanguage.Tags.Settings
{
	// Token: 0x0200006B RID: 107
	public class SubmenuTag : BSMLTag
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000B83B File Offset: 0x00009A3B
		public override string[] Aliases
		{
			get
			{
				return new string[] { "settings-submenu" };
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000B84C File Offset: 0x00009A4C
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject("BSMLSubmenu");
			gameObject.SetActive(false);
			ClickableText clickableText = gameObject.AddComponent<ClickableText>();
			clickableText.font = Object.Instantiate<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First((TMP_FontAsset t) => t.name == "Teko-Medium SDF No Glow"));
			clickableText.rectTransform.SetParent(parent, false);
			clickableText.text = "Default Text";
			clickableText.fontSize = 5f;
			clickableText.color = Color.white;
			clickableText.rectTransform.sizeDelta = new Vector2(90f, 8f);
			ViewController submenuController = BeatSaberUI.CreateViewController<ViewController>();
			SettingsMenu.SetupViewControllerTransform(submenuController);
			ClickableText clickableText2 = clickableText;
			clickableText2.OnClickEvent = (Action<PointerEventData>)Delegate.Combine(clickableText2.OnClickEvent, new Action<PointerEventData>(delegate
			{
				ModSettingsFlowCoordinator modSettingsFlowCoordinator = Resources.FindObjectsOfTypeAll<ModSettingsFlowCoordinator>().FirstOrDefault<ModSettingsFlowCoordinator>();
				if (modSettingsFlowCoordinator)
				{
					modSettingsFlowCoordinator.OpenMenu(submenuController, true, false);
				}
			}));
			ExternalComponents externalComponents = submenuController.gameObject.AddComponent<ExternalComponents>();
			externalComponents.components.Add(clickableText);
			externalComponents.components.Add(clickableText.rectTransform);
			gameObject.SetActive(true);
			return submenuController.gameObject;
		}
	}
}
