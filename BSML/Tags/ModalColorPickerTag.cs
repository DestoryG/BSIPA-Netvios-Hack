using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200004A RID: 74
	public class ModalColorPickerTag : ModalTag
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000948C File Offset: 0x0000768C
		public override string[] Aliases
		{
			get
			{
				return new string[] { "modal-color-picker" };
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000949C File Offset: 0x0000769C
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = base.CreateObject(parent);
			ExternalComponents component = gameObject.GetComponent<ExternalComponents>();
			RectTransform rectTransform = component.Get<RectTransform>();
			rectTransform.name = "BSMLModalColorPicker";
			rectTransform.sizeDelta = new Vector2(135f, 75f);
			ModalColorPicker modalColorPicker = gameObject.AddComponent<ModalColorPicker>();
			modalColorPicker.modalView = component.Get<ModalView>();
			RGBPanelController rgbpanelController = Object.Instantiate<RGBPanelController>(Resources.FindObjectsOfTypeAll<RGBPanelController>().First((RGBPanelController x) => x.name == "RGBColorPicker"), gameObject.transform, false);
			rgbpanelController.name = "BSMLRGBPanel";
			(rgbpanelController.gameObject.transform as RectTransform).anchoredPosition = new Vector2(0f, 3f);
			(rgbpanelController.gameObject.transform as RectTransform).anchorMin = new Vector2(0.1f, 0.73f);
			(rgbpanelController.gameObject.transform as RectTransform).anchorMax = new Vector2(0.1f, 0.73f);
			modalColorPicker.rgbPanel = rgbpanelController;
			rgbpanelController.colorDidChangeEvent += modalColorPicker.OnChange;
			HSVPanelController hsvpanelController = Object.Instantiate<HSVPanelController>(Resources.FindObjectsOfTypeAll<HSVPanelController>().First((HSVPanelController x) => x.name == "HSVColorPicker"), gameObject.transform, false);
			hsvpanelController.name = "BSMLHSVPanel";
			(hsvpanelController.gameObject.transform as RectTransform).anchoredPosition = new Vector2(0f, 3f);
			(hsvpanelController.gameObject.transform as RectTransform).anchorMin = new Vector2(0.75f, 0.5f);
			(hsvpanelController.gameObject.transform as RectTransform).anchorMax = new Vector2(0.75f, 0.5f);
			modalColorPicker.hsvPanel = hsvpanelController;
			hsvpanelController.colorDidChangeEvent += modalColorPicker.OnChange;
			Image image = Object.Instantiate<Image>(Resources.FindObjectsOfTypeAll<Image>().First(delegate(Image x)
			{
				if (x.gameObject.name == "ColorImage")
				{
					Sprite sprite = x.sprite;
					return ((sprite != null) ? sprite.name : null) == "NoteCircle";
				}
				return false;
			}), gameObject.transform, false);
			image.name = "BSMLCurrentColor";
			(image.gameObject.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
			(image.gameObject.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
			(image.gameObject.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
			modalColorPicker.colorImage = image;
			PersistentSingleton<BSMLParser>.instance.Parse("<horizontal anchor-pos-y='-30' spacing='2' horizontal-fit='PreferredSize'><button text='Cancel' on-click='cancel' pref-width='30'/><button text='Done' on-click='done' pref-width='30'/></horizontal>", gameObject, modalColorPicker);
			return gameObject;
		}
	}
}
