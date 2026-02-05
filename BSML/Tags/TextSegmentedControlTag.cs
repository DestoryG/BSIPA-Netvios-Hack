using System;
using System.Linq;
using HMUI;
using IPA.Utilities;
using UnityEngine;
using Zenject;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200005D RID: 93
	internal class TextSegmentedControlTag : BSMLTag
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000AB45 File Offset: 0x00008D45
		public override string[] Aliases
		{
			get
			{
				return new string[] { "text-segments" };
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000AB58 File Offset: 0x00008D58
		public override GameObject CreateObject(Transform parent)
		{
			TextSegmentedControl textSegmentedControl = Resources.FindObjectsOfTypeAll<TextSegmentedControl>().First((TextSegmentedControl x) => x.name == "BeatmapDifficultySegmentedControl" && x.GetField("_container") != null);
			TextSegmentedControl textSegmentedControl2 = Object.Instantiate<TextSegmentedControl>(textSegmentedControl, parent, false);
			textSegmentedControl2.name = "BSMLTextSegmentedControl";
			textSegmentedControl2.SetField("_container", textSegmentedControl.GetField("_container"));
			(textSegmentedControl2.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
			foreach (object obj in textSegmentedControl2.transform)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			Object.Destroy(textSegmentedControl2.GetComponent<BeatmapDifficultySegmentedControlController>());
			return textSegmentedControl2.gameObject;
		}
	}
}
