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
	// Token: 0x0200006A RID: 106
	public class StringSettingTag : ModalKeyboardTag
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000B65F File Offset: 0x0000985F
		public override string[] Aliases
		{
			get
			{
				return new string[] { "string-setting" };
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000B670 File Offset: 0x00009870
		public override GameObject CreateObject(Transform parent)
		{
			BoolSettingsController boolSettingsController = Object.Instantiate<BoolSettingsController>(Resources.FindObjectsOfTypeAll<BoolSettingsController>().First((BoolSettingsController x) => x.name == "Fullscreen"), parent, false);
			boolSettingsController.name = "BSMLStringSetting";
			GameObject gameObject = boolSettingsController.gameObject;
			gameObject.SetActive(false);
			Object.Destroy(boolSettingsController);
			StringSetting stringSetting = gameObject.AddComponent<StringSetting>();
			Transform transform = gameObject.transform.Find("ValuePicker");
			Button button = transform.GetComponentsInChildren<Button>().First<Button>();
			button.enabled = false;
			button.interactable = true;
			Object.Destroy(button.transform.Find("Arrow").gameObject);
			stringSetting.text = transform.GetComponentsInChildren<TextMeshProUGUI>().First<TextMeshProUGUI>();
			stringSetting.editButton = transform.GetComponentsInChildren<Button>().Last<Button>();
			stringSetting.boundingBox = transform as RectTransform;
			TextMeshProUGUI componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>();
			componentInChildren.text = "Default Text";
			gameObject.AddComponent<ExternalComponents>().components.Add(componentInChildren);
			Object.Destroy(componentInChildren.GetComponent<LocalizedTextMeshProUGUI>());
			gameObject.GetComponent<LayoutElement>().preferredWidth = 90f;
			stringSetting.text.alignment = 4100;
			stringSetting.text.enableWordWrapping = false;
			Image component = stringSetting.editButton.transform.Find("Arrow").GetComponent<Image>();
			component.name = "EditIcon";
			component.sprite = Utilities.EditIcon;
			component.rectTransform.sizeDelta = new Vector2(4f, 4f);
			stringSetting.editButton.interactable = true;
			(stringSetting.editButton.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
			stringSetting.modalKeyboard = base.CreateObject(gameObject.transform).GetComponent<ModalKeyboard>();
			gameObject.SetActive(true);
			return gameObject;
		}
	}
}
