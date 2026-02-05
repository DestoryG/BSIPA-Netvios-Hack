using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using Polyglot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000041 RID: 65
	public class ButtonTag : BSMLTag
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00008B83 File Offset: 0x00006D83
		public override string[] Aliases
		{
			get
			{
				return new string[] { "button" };
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00008B94 File Offset: 0x00006D94
		public override GameObject CreateObject(Transform parent)
		{
			Button button = Object.Instantiate<Button>(Resources.FindObjectsOfTypeAll<Button>().Last((Button x) => x.name == ((parent.GetComponent<StartMiddleEndButtonsGroup>() == null) ? "PlayButton" : "CreditsButton")), parent, false);
			button.name = "BSMLButton";
			button.interactable = true;
			LocalizedTextMeshProUGUI componentInChildren = button.GetComponentInChildren<LocalizedTextMeshProUGUI>();
			if (componentInChildren != null)
			{
				Object.Destroy(componentInChildren);
			}
			ExternalComponents externalComponents = button.gameObject.AddComponent<ExternalComponents>();
			externalComponents.components.Add(button.GetComponentInChildren<TextMeshProUGUI>());
			HorizontalLayoutGroup componentInChildren2 = button.GetComponentInChildren<HorizontalLayoutGroup>();
			if (componentInChildren2 != null)
			{
				externalComponents.components.Add(componentInChildren2);
			}
			Image image = (from x in button.gameObject.GetComponentsInChildren<Image>(true)
				where x.gameObject.name == "Glow"
				select x).FirstOrDefault<Image>();
			if (image != null)
			{
				Glowable glowable = button.gameObject.AddComponent<Glowable>();
				glowable.image = image;
				glowable.SetGlow("none");
			}
			Image image2 = (from x in button.gameObject.GetComponentsInChildren<Image>(true)
				where x.gameObject.name == "Stroke"
				select x).FirstOrDefault<Image>();
			if (image2 != null)
			{
				Strokable strokable = button.gameObject.AddComponent<Strokable>();
				strokable.image = image2;
				strokable.SetType(Strokable.StrokeType.Regular);
			}
			return button.gameObject;
		}
	}
}
