using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200005E RID: 94
	public class TextTag : BSMLTag
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000AC38 File Offset: 0x00008E38
		public override string[] Aliases
		{
			get
			{
				return new string[] { "text", "label" };
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000AC50 File Offset: 0x00008E50
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = new GameObject("BSMLText");
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.font = Object.Instantiate<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First((TMP_FontAsset t) => t.name == "Teko-Medium SDF No Glow"));
			textMeshProUGUI.fontSize = 4f;
			textMeshProUGUI.color = Color.white;
			textMeshProUGUI.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			textMeshProUGUI.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			return gameObject;
		}
	}
}
