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
	// Token: 0x02000065 RID: 101
	public abstract class IncDecSettingTag<T> : BSMLTag where T : IncDecSetting
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x0000B4C0 File Offset: 0x000096C0
		public override GameObject CreateObject(Transform parent)
		{
			BoolSettingsController boolSettingsController = Object.Instantiate<BoolSettingsController>(Resources.FindObjectsOfTypeAll<BoolSettingsController>().First((BoolSettingsController x) => x.name == "Fullscreen"), parent, false);
			boolSettingsController.name = "BSMLIncDecSetting";
			GameObject gameObject = boolSettingsController.gameObject;
			Object.Destroy(boolSettingsController);
			gameObject.SetActive(false);
			T t = gameObject.AddComponent<T>();
			t.text = gameObject.transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>().First<TextMeshProUGUI>();
			t.decButton = gameObject.transform.GetChild(1).GetComponentsInChildren<Button>().First<Button>();
			t.incButton = gameObject.transform.GetChild(1).GetComponentsInChildren<Button>().Last<Button>();
			(gameObject.transform.GetChild(1) as RectTransform).sizeDelta = new Vector2(40f, 0f);
			t.text.overflowMode = 1;
			TextMeshProUGUI componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>();
			componentInChildren.text = "Default Text";
			gameObject.AddComponent<ExternalComponents>().components.Add(componentInChildren);
			Object.Destroy(componentInChildren.GetComponent<LocalizedTextMeshProUGUI>());
			gameObject.GetComponent<LayoutElement>().preferredWidth = 90f;
			gameObject.SetActive(true);
			return gameObject;
		}
	}
}
