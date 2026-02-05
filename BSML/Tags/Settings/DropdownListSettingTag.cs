using System;
using System.Linq;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using HMUI;
using IPA.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Tags.Settings
{
	// Token: 0x02000063 RID: 99
	public class DropdownListSettingTag : BSMLTag
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000B195 File Offset: 0x00009395
		public override string[] Aliases
		{
			get
			{
				return new string[] { "dropdown-list-setting" };
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000B1A8 File Offset: 0x000093A8
		public override void Setup()
		{
			this.safePrefab = Object.Instantiate<LabelAndValueDropdownWithTableView>(Resources.FindObjectsOfTypeAll<LabelAndValueDropdownWithTableView>().First((LabelAndValueDropdownWithTableView x) => x.name == "NormalLevels"), PersistentSingleton<BSMLParser>.instance.transform, false);
			this.safePrefab.gameObject.SetActive(false);
			this.safePrefab.name = "BSMLDropDownListPrefab";
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000B218 File Offset: 0x00009418
		public override GameObject CreateObject(Transform parent)
		{
			LabelAndValueDropdownWithTableView labelAndValueDropdownWithTableView = Object.Instantiate<LabelAndValueDropdownWithTableView>(this.safePrefab, parent, false);
			labelAndValueDropdownWithTableView.gameObject.SetActive(false);
			labelAndValueDropdownWithTableView.name = "BSMLDropDownList";
			TextMeshProUGUI field = labelAndValueDropdownWithTableView.GetField("_labelText");
			field.fontSize = 5f;
			labelAndValueDropdownWithTableView.gameObject.AddComponent<ExternalComponents>().components.Add(field);
			LayoutElement layoutElement = labelAndValueDropdownWithTableView.gameObject.AddComponent<LayoutElement>();
			layoutElement.preferredHeight = 8f;
			layoutElement.preferredWidth = 90f;
			labelAndValueDropdownWithTableView.SetLabelText("Default Text");
			labelAndValueDropdownWithTableView.SetValueText("Default Text");
			DropDownListSetting dropDownListSetting = labelAndValueDropdownWithTableView.gameObject.AddComponent<DropDownListSetting>();
			dropDownListSetting.tableView = labelAndValueDropdownWithTableView.GetField("_tableView");
			dropDownListSetting.dropdown = labelAndValueDropdownWithTableView;
			dropDownListSetting.tableView.dataSource = dropDownListSetting;
			return labelAndValueDropdownWithTableView.gameObject;
		}

		// Token: 0x0400003A RID: 58
		private LabelAndValueDropdownWithTableView safePrefab;
	}
}
