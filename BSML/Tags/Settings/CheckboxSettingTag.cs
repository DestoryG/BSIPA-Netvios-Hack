using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags.Settings
{
	// Token: 0x02000061 RID: 97
	public class CheckboxSettingTag : BSMLTag
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000ADB4 File Offset: 0x00008FB4
		public override string[] Aliases
		{
			get
			{
				return new string[] { "checkbox-setting", "checkbox" };
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000ADCC File Offset: 0x00008FCC
		public override GameObject CreateObject(Transform parent)
		{
			GameplayModifierToggle gameplayModifierToggle = Object.Instantiate<GameplayModifierToggle>(Resources.FindObjectsOfTypeAll<GameplayModifierToggle>().First((GameplayModifierToggle x) => x.name == "InstaFail"), parent, false);
			gameplayModifierToggle.name = "BSMLCheckboxSetting";
			GameObject gameObject = gameplayModifierToggle.gameObject;
			gameObject.SetActive(false);
			Object.Destroy(gameplayModifierToggle);
			Object.Destroy(gameObject.transform.GetChild(0).gameObject);
			Object.Destroy(gameObject.GetComponent<SignalOnUIToggleValueChanged>());
			Object.Destroy(gameObject.GetComponent<HoverHint>());
			gameObject.AddComponent<CheckboxSetting>().checkbox = gameObject.GetComponent<Toggle>();
			TextMeshProUGUI componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>();
			componentInChildren.fontSize = 5f;
			componentInChildren.rectTransform.localPosition = Vector2.zero;
			componentInChildren.rectTransform.anchoredPosition = Vector2.zero;
			componentInChildren.rectTransform.sizeDelta = Vector2.zero;
			gameObject.AddComponent<ExternalComponents>().components.Add(componentInChildren);
			LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.preferredWidth = 90f;
			layoutElement.preferredHeight = 8f;
			gameObject.SetActive(true);
			return gameObject;
		}
	}
}
