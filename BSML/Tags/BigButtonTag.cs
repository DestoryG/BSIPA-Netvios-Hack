using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using Polyglot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200003E RID: 62
	public class BigButtonTag : BSMLTag
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000088B5 File Offset: 0x00006AB5
		public override string[] Aliases
		{
			get
			{
				return new string[] { "big-button" };
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000088C8 File Offset: 0x00006AC8
		public override GameObject CreateObject(Transform parent)
		{
			Button button = Object.Instantiate<Button>(Resources.FindObjectsOfTypeAll<Button>().Last((Button x) => x.name == "SoloFreePlayButton"), parent, false);
			button.name = "BSMLBigButton";
			button.interactable = true;
			Object.Destroy(button.GetComponent<HoverHint>());
			Object.Destroy(button.GetComponent<LocalizedHoverHint>());
			LocalizedTextMeshProUGUI componentInChildren = button.GetComponentInChildren<LocalizedTextMeshProUGUI>();
			if (componentInChildren != null)
			{
				Object.Destroy(componentInChildren);
			}
			button.gameObject.AddComponent<ExternalComponents>().components.Add(button.GetComponentInChildren<TextMeshProUGUI>());
			Image image = (from x in button.gameObject.GetComponentsInChildren<Image>(true)
				where x.gameObject.name == "Stroke"
				select x).FirstOrDefault<Image>();
			if (image != null)
			{
				Strokable strokable = button.gameObject.AddComponent<Strokable>();
				strokable.image = image;
				strokable.SetType(Strokable.StrokeType.Regular);
			}
			Image image2 = (from x in button.gameObject.GetComponentsInChildren<Image>(true)
				where x.gameObject.name == "Icon"
				select x).FirstOrDefault<Image>();
			if (image2 != null)
			{
				button.gameObject.AddComponent<ButtonIconImage>().image = image2;
			}
			Image image3 = (from x in button.gameObject.GetComponentsInChildren<Image>(true)
				where x.gameObject.name == "BGArtwork"
				select x).FirstOrDefault<Image>();
			if (image3 != null)
			{
				button.gameObject.AddComponent<ButtonArtworkImage>().image = image3;
			}
			return button.gameObject;
		}
	}
}
