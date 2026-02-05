using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using TMPro;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000043 RID: 67
	public class ClickableTextTag : BSMLTag
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00008D64 File Offset: 0x00006F64
		public override string[] Aliases
		{
			get
			{
				return new string[] { "clickable-text" };
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00008D74 File Offset: 0x00006F74
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "BSMLClickableText";
			gameObject.SetActive(false);
			ClickableText clickableText = gameObject.AddComponent<ClickableText>();
			clickableText.font = Object.Instantiate<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First((TMP_FontAsset t) => t.name == "Teko-Medium SDF No Glow"));
			clickableText.rectTransform.SetParent(parent, false);
			clickableText.text = "Default Text";
			clickableText.fontSize = 5f;
			clickableText.color = Color.white;
			clickableText.rectTransform.sizeDelta = new Vector2(90f, 8f);
			gameObject.SetActive(true);
			return gameObject;
		}
	}
}
