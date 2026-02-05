using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using IPA.Utilities;
using UnityEngine;
using Zenject;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200005A RID: 90
	public class TabSelectorTag : BSMLTag
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000A9AF File Offset: 0x00008BAF
		public override string[] Aliases
		{
			get
			{
				return new string[] { "tab-select", "tab-selector" };
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000A9C8 File Offset: 0x00008BC8
		public override GameObject CreateObject(Transform parent)
		{
			TextSegmentedControl textSegmentedControl = Resources.FindObjectsOfTypeAll<TextSegmentedControl>().First((TextSegmentedControl x) => x.transform.parent.name == "PlayerStatisticsViewController" && x.GetField("_container") != null);
			TextSegmentedControl textSegmentedControl2 = Object.Instantiate<TextSegmentedControl>(textSegmentedControl, parent, false);
			textSegmentedControl2.name = "BSMLTabSelector";
			textSegmentedControl2.SetField("_container", textSegmentedControl.GetField("_container"));
			(textSegmentedControl2.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
			foreach (object obj in textSegmentedControl2.transform)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			textSegmentedControl2.gameObject.AddComponent<TabSelector>().textSegmentedControl = textSegmentedControl2;
			return textSegmentedControl2.gameObject;
		}
	}
}
