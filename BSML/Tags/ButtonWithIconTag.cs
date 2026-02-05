using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000047 RID: 71
	public class ButtonWithIconTag : BSMLTag
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000915C File Offset: 0x0000735C
		public override string[] Aliases
		{
			get
			{
				return new string[] { "button-with-icon", "icon-button" };
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00009174 File Offset: 0x00007374
		public override GameObject CreateObject(Transform parent)
		{
			Button button = Object.Instantiate<Button>(Resources.FindObjectsOfTypeAll<Button>().Last((Button x) => x.name == "PracticeButton" && x.transform.parent.name == "PlayButtons"), parent, false);
			button.name = "BSMLIconButton";
			button.interactable = true;
			Object.Destroy(button.GetComponent<HoverHint>());
			Object.Destroy(button.GetComponent<LocalizedHoverHint>());
			button.gameObject.AddComponent<ExternalComponents>().components.Add(button.GetComponentsInChildren<HorizontalLayoutGroup>().First((HorizontalLayoutGroup x) => x.name == "Content"));
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
			Image image3 = (from x in button.gameObject.GetComponentsInChildren<Image>(true)
				where x.gameObject.name == "Icon"
				select x).FirstOrDefault<Image>();
			if (image3 != null)
			{
				button.gameObject.AddComponent<ButtonIconImage>().image = image3;
			}
			return button.gameObject;
		}
	}
}
