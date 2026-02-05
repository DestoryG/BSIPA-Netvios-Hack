using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200004C RID: 76
	public class ModifierTag : BSMLTag
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00009835 File Offset: 0x00007A35
		public override string[] Aliases
		{
			get
			{
				return new string[] { "modifier" };
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00009848 File Offset: 0x00007A48
		public override GameObject CreateObject(Transform parent)
		{
			GameplayModifierToggle gameplayModifierToggle = Object.Instantiate<GameplayModifierToggle>(Resources.FindObjectsOfTypeAll<GameplayModifierToggle>().First((GameplayModifierToggle x) => x.name == "InstaFail"), parent, false);
			gameplayModifierToggle.name = "BSMLModifier";
			GameObject gameObject = gameplayModifierToggle.gameObject;
			Object.Destroy(gameplayModifierToggle);
			Object.Destroy(gameObject.GetComponent<HoverHint>());
			ExternalComponents externalComponents = gameObject.AddComponent<ExternalComponents>();
			externalComponents.components.Add(gameObject.GetComponentInChildren<TextMeshProUGUI>());
			externalComponents.components.Add(gameObject.transform.Find("Icon").GetComponent<Image>());
			gameObject.AddComponent<CheckboxSetting>().checkbox = gameObject.GetComponent<Toggle>();
			return gameObject;
		}
	}
}
