using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using Polyglot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags.Settings
{
	// Token: 0x02000062 RID: 98
	public class ColorSettingTag : ModalColorPickerTag
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000AEEB File Offset: 0x000090EB
		public override string[] Aliases
		{
			get
			{
				return new string[] { "color-setting" };
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000AEFC File Offset: 0x000090FC
		public override GameObject CreateObject(Transform parent)
		{
			BoolSettingsController boolSettingsController = Object.Instantiate<BoolSettingsController>(Resources.FindObjectsOfTypeAll<BoolSettingsController>().First((BoolSettingsController x) => x.name == "Fullscreen"), parent, false);
			boolSettingsController.name = "BSMLColorSetting";
			GameObject gameObject = boolSettingsController.gameObject;
			gameObject.SetActive(false);
			Object.Destroy(boolSettingsController);
			ColorSetting colorSetting = gameObject.AddComponent<ColorSetting>();
			Transform transform = gameObject.transform.Find("ValuePicker");
			(transform.transform as RectTransform).sizeDelta = new Vector2(13f, 0f);
			Button button = transform.GetComponentsInChildren<Button>().First<Button>();
			button.enabled = false;
			button.interactable = true;
			Object.Destroy(button.transform.Find("Arrow").gameObject);
			Object.Destroy(transform.GetComponentsInChildren<TextMeshProUGUI>().First<TextMeshProUGUI>().gameObject);
			colorSetting.editButton = transform.GetComponentsInChildren<Button>().Last<Button>();
			TextMeshProUGUI componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>();
			componentInChildren.text = "Default Text";
			gameObject.AddComponent<ExternalComponents>().components.Add(componentInChildren);
			Object.Destroy(componentInChildren.GetComponent<LocalizedTextMeshProUGUI>());
			gameObject.GetComponent<LayoutElement>().preferredWidth = 90f;
			Image image = Object.Instantiate<Image>(Resources.FindObjectsOfTypeAll<Image>().First(delegate(Image x)
			{
				if (x.gameObject.name == "ColorImage")
				{
					Sprite sprite = x.sprite;
					return ((sprite != null) ? sprite.name : null) == "NoteCircle";
				}
				return false;
			}), transform, false);
			image.name = "BSMLCurrentColor";
			(image.gameObject.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
			(image.gameObject.transform as RectTransform).sizeDelta = new Vector2(5f, 5f);
			(image.gameObject.transform as RectTransform).anchorMin = new Vector2(0.3f, 0.2f);
			(image.gameObject.transform as RectTransform).anchorMax = new Vector2(0.3f, 0.2f);
			colorSetting.colorImage = image;
			Image component = colorSetting.editButton.transform.Find("Arrow").GetComponent<Image>();
			component.name = "EditIcon";
			component.sprite = Utilities.EditIcon;
			component.rectTransform.sizeDelta = new Vector2(4f, 4f);
			colorSetting.editButton.interactable = true;
			(colorSetting.editButton.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
			colorSetting.modalColorPicker = base.CreateObject(gameObject.transform).GetComponent<ModalColorPicker>();
			gameObject.SetActive(true);
			return gameObject;
		}
	}
}
