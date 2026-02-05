using System;
using System.Linq;
using HMUI;
using IPA.Utilities;
using UnityEngine;
using Zenject;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000048 RID: 72
	internal class IconSegmentedControlTag : BSMLTag
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00009321 File Offset: 0x00007521
		public override string[] Aliases
		{
			get
			{
				return new string[] { "icon-segments" };
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00009334 File Offset: 0x00007534
		public override GameObject CreateObject(Transform parent)
		{
			IconSegmentedControl iconSegmentedControl = Resources.FindObjectsOfTypeAll<IconSegmentedControl>().First((IconSegmentedControl x) => x.name == "BeatmapCharacteristicSegmentedControl" && x.GetField("_container") != null);
			IconSegmentedControl iconSegmentedControl2 = Object.Instantiate<IconSegmentedControl>(iconSegmentedControl, parent, false);
			iconSegmentedControl2.name = "BSMLIconSegmentedControl";
			iconSegmentedControl2.SetField("_container", iconSegmentedControl.GetField("_container"));
			(iconSegmentedControl2.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
			foreach (object obj in iconSegmentedControl2.transform)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			Object.Destroy(iconSegmentedControl2.GetComponent<BeatmapCharacteristicSegmentedControlController>());
			return iconSegmentedControl2.gameObject;
		}
	}
}
