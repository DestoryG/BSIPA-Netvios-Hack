using System;
using System.Collections.Generic;
using System.Linq;
using HMUI;
using IPA.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000AB RID: 171
	public class CustomListTableData : MonoBehaviour, TableView.IDataSource
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00010E19 File Offset: 0x0000F019
		// (set) Token: 0x0600037B RID: 891 RVA: 0x00010E24 File Offset: 0x0000F024
		public CustomListTableData.ListStyle Style
		{
			get
			{
				return this.listStyle;
			}
			set
			{
				switch (value)
				{
				case CustomListTableData.ListStyle.List:
					this.cellSize = 8.5f;
					break;
				case CustomListTableData.ListStyle.Box:
					this.cellSize = ((this.tableView.tableType == 1) ? 30f : 35f);
					break;
				case CustomListTableData.ListStyle.Simple:
					this.cellSize = 8f;
					break;
				}
				this.listStyle = value;
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00010E88 File Offset: 0x0000F088
		public LevelListTableCell GetTableCell(bool beatmapCharacteristicImages = false)
		{
			LevelListTableCell levelListTableCell = (LevelListTableCell)this.tableView.DequeueReusableCellForIdentifier(this.reuseIdentifier);
			if (!levelListTableCell)
			{
				if (this.songListTableCellInstance == null)
				{
					this.songListTableCellInstance = Resources.FindObjectsOfTypeAll<LevelListTableCell>().First((LevelListTableCell x) => x.name == "LevelListTableCell");
				}
				levelListTableCell = Object.Instantiate<LevelListTableCell>(this.songListTableCellInstance);
			}
			if (!beatmapCharacteristicImages)
			{
				Image[] field = levelListTableCell.GetField("_beatmapCharacteristicImages");
				for (int i = 0; i < field.Length; i++)
				{
					field[i].enabled = false;
				}
			}
			levelListTableCell.transform.Find("FavoritesIcon").gameObject.SetActive(false);
			levelListTableCell.SetField("_bought", true);
			levelListTableCell.reuseIdentifier = this.reuseIdentifier;
			return levelListTableCell;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00010F58 File Offset: 0x0000F158
		public AnnotatedBeatmapLevelCollectionTableCell GetLevelPackTableCell()
		{
			AnnotatedBeatmapLevelCollectionTableCell annotatedBeatmapLevelCollectionTableCell = (AnnotatedBeatmapLevelCollectionTableCell)this.tableView.DequeueReusableCellForIdentifier(this.reuseIdentifier);
			if (!annotatedBeatmapLevelCollectionTableCell)
			{
				if (this.levelPackTableCellInstance == null)
				{
					this.levelPackTableCellInstance = Resources.FindObjectsOfTypeAll<AnnotatedBeatmapLevelCollectionTableCell>().First((AnnotatedBeatmapLevelCollectionTableCell x) => x.name == "AnnotatedBeatmapLevelCollectionTableCell");
				}
				annotatedBeatmapLevelCollectionTableCell = Object.Instantiate<AnnotatedBeatmapLevelCollectionTableCell>(this.levelPackTableCellInstance);
			}
			annotatedBeatmapLevelCollectionTableCell.reuseIdentifier = this.reuseIdentifier;
			return annotatedBeatmapLevelCollectionTableCell;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00010FDC File Offset: 0x0000F1DC
		public MainSettingsTableCell GetMainSettingsTableCell()
		{
			MainSettingsTableCell mainSettingsTableCell = (MainSettingsTableCell)this.tableView.DequeueReusableCellForIdentifier(this.reuseIdentifier);
			if (!mainSettingsTableCell)
			{
				if (this.mainSettingsTableCellInstance == null)
				{
					this.mainSettingsTableCellInstance = Resources.FindObjectsOfTypeAll<MainSettingsTableCell>().First((MainSettingsTableCell x) => x.name == "MainSettingsTableCell");
				}
				mainSettingsTableCell = Object.Instantiate<MainSettingsTableCell>(this.mainSettingsTableCellInstance);
			}
			mainSettingsTableCell.reuseIdentifier = this.reuseIdentifier;
			return mainSettingsTableCell;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00011060 File Offset: 0x0000F260
		public virtual TableCell CellForIdx(TableView tableView, int idx)
		{
			switch (this.listStyle)
			{
			case CustomListTableData.ListStyle.List:
			{
				LevelListTableCell tableCell = this.GetTableCell(false);
				TextMeshProUGUI field = tableCell.GetField("_songNameText");
				TextMeshProUGUI field2 = tableCell.GetField("_authorText");
				if (this.expandCell)
				{
					field.rectTransform.anchorMax = new Vector3(2f, 1f, 0f);
					field2.rectTransform.anchorMax = new Vector3(2f, 1f, 0f);
				}
				field.text = this.data[idx].text;
				field2.text = this.data[idx].subtext;
				tableCell.GetField("_coverRawImage").texture = ((this.data[idx].icon == null) ? Texture2D.blackTexture : this.data[idx].icon);
				float num = -1f;
				Image[] field3 = tableCell.GetField("_beatmapCharacteristicImages");
				IEnumerable<Sprite> characteristicSprites = this.data[idx].characteristicSprites;
				if (characteristicSprites != null)
				{
					Sprite[] array = characteristicSprites.ToArray<Sprite>();
					if (array.Length > field3.Length)
					{
						Logger.log.Warn(string.Format("List cell specifies {0} characteristic sprites, where only {1} are supported", array.Length, field3.Length));
					}
					foreach (ValueTuple<Sprite, Image> valueTuple in characteristicSprites.Zip(field3, (Sprite s, Image img) => new ValueTuple<Sprite, Image>(s, img)))
					{
						Sprite item = valueTuple.Item1;
						Image item2 = valueTuple.Item2;
						item2.enabled = true;
						item2.rectTransform.sizeDelta = new Vector2(2.625f, 4.5f);
						item2.rectTransform.anchoredPosition = new Vector2(num, 0f);
						num -= item2.rectTransform.sizeDelta.x + 0.5f;
						item2.sprite = item;
					}
				}
				field.rectTransform.offsetMax = new Vector2(num, field.rectTransform.offsetMax.y);
				field2.rectTransform.offsetMax = new Vector2(num, field2.rectTransform.offsetMax.y);
				return tableCell;
			}
			case CustomListTableData.ListStyle.Box:
			{
				AnnotatedBeatmapLevelCollectionTableCell levelPackTableCell = this.GetLevelPackTableCell();
				levelPackTableCell.showNewRibbon = false;
				levelPackTableCell.GetField("_infoText").text = this.data[idx].text + "\n" + this.data[idx].subtext;
				Image field4 = levelPackTableCell.GetField("_coverImage");
				Texture2D texture2D = ((this.data[idx].icon == null) ? Texture2D.blackTexture : this.data[idx].icon);
				field4.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.one * 0.5f, 100f, 1U);
				field4.mainTexture.wrapMode = 1;
				return levelPackTableCell;
			}
			case CustomListTableData.ListStyle.Simple:
			{
				MainSettingsTableCell mainSettingsTableCell = this.GetMainSettingsTableCell();
				mainSettingsTableCell.settingsSubMenuText = this.data[idx].text;
				return mainSettingsTableCell;
			}
			default:
				return null;
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000113D4 File Offset: 0x0000F5D4
		public float CellSize()
		{
			return this.cellSize;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000113DC File Offset: 0x0000F5DC
		public int NumberOfCells()
		{
			return this.data.Count<CustomListTableData.CustomCellInfo>();
		}

		// Token: 0x04000108 RID: 264
		private CustomListTableData.ListStyle listStyle;

		// Token: 0x04000109 RID: 265
		private LevelListTableCell songListTableCellInstance;

		// Token: 0x0400010A RID: 266
		private AnnotatedBeatmapLevelCollectionTableCell levelPackTableCellInstance;

		// Token: 0x0400010B RID: 267
		private MainSettingsTableCell mainSettingsTableCellInstance;

		// Token: 0x0400010C RID: 268
		public List<CustomListTableData.CustomCellInfo> data = new List<CustomListTableData.CustomCellInfo>();

		// Token: 0x0400010D RID: 269
		public float cellSize = 8.5f;

		// Token: 0x0400010E RID: 270
		public string reuseIdentifier = "BSMLListTableCell";

		// Token: 0x0400010F RID: 271
		public TableView tableView;

		// Token: 0x04000110 RID: 272
		public bool expandCell;

		// Token: 0x0200014A RID: 330
		public enum ListStyle
		{
			// Token: 0x040002DD RID: 733
			List,
			// Token: 0x040002DE RID: 734
			Box,
			// Token: 0x040002DF RID: 735
			Simple
		}

		// Token: 0x0200014B RID: 331
		public class CustomCellInfo
		{
			// Token: 0x0600068B RID: 1675 RVA: 0x00016EEA File Offset: 0x000150EA
			public CustomCellInfo(string text, string subtext, Texture2D icon)
				: this(text, subtext, icon, null)
			{
			}

			// Token: 0x0600068C RID: 1676 RVA: 0x00016EF6 File Offset: 0x000150F6
			public CustomCellInfo(string text, string subtext = null, Texture2D icon = null, IEnumerable<Sprite> characteristicSprites = null)
			{
				this.text = text;
				this.subtext = subtext;
				this.icon = icon;
				this.characteristicSprites = characteristicSprites;
			}

			// Token: 0x040002E0 RID: 736
			public string text;

			// Token: 0x040002E1 RID: 737
			public string subtext;

			// Token: 0x040002E2 RID: 738
			public Texture2D icon;

			// Token: 0x040002E3 RID: 739
			public IEnumerable<Sprite> characteristicSprites;
		}
	}
}
