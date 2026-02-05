using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using HMUI;
using Polyglot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags.Settings
{
	// Token: 0x02000064 RID: 100
	public abstract class GenericSliderSettingTag<T> : BSMLTag where T : GenericSliderSetting
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x0000B2E4 File Offset: 0x000094E4
		public override GameObject CreateObject(Transform parent)
		{
			BoolSettingsController boolSettingsController = Object.Instantiate<BoolSettingsController>(Resources.FindObjectsOfTypeAll<BoolSettingsController>().First((BoolSettingsController x) => x.name == "Fullscreen"), parent, false);
			boolSettingsController.name = "BSMLSliderSetting";
			GameObject gameObject = boolSettingsController.gameObject;
			T t = gameObject.AddComponent<T>();
			Transform transform = gameObject.transform.Find("ValuePicker");
			t.slider = Object.Instantiate<TimeSlider>(Resources.FindObjectsOfTypeAll<TimeSlider>().First((TimeSlider s) => s.name != "BSMLSlider"), transform, false);
			t.slider.name = "BSMLSlider";
			t.slider.GetComponentInChildren<TextMeshProUGUI>().enableWordWrapping = false;
			(t.slider.transform as RectTransform).anchorMin = new Vector2(-0.2f, 0.4f);
			(t.slider.transform as RectTransform).anchorMax = new Vector2(1f, 1.2f);
			(t.slider.transform as RectTransform).sizeDelta = new Vector2(0f, 0f);
			Object.Destroy(boolSettingsController);
			Object.Destroy(transform.GetComponentsInChildren<TextMeshProUGUI>().First<TextMeshProUGUI>().transform.parent.gameObject);
			Object.Destroy(transform.GetComponentsInChildren<Button>().First<Button>().gameObject);
			Object.Destroy(transform.GetComponentsInChildren<Button>().Last<Button>().gameObject);
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
