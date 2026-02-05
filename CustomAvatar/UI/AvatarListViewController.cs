using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomAvatar.Avatar;
using CustomAvatar.Utilities;
using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CustomAvatar.UI
{
	// Token: 0x02000032 RID: 50
	internal class AvatarListViewController : BSMLResourceViewController, TableView.IDataSource
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00006F34 File Offset: 0x00005134
		public override string ResourceName
		{
			get
			{
				return "CustomAvatar.Views.AvatarListViewController.bsml";
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006F3C File Offset: 0x0000513C
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			AvatarManager.instance.avatarChanged += this.OnAvatarChanged;
			this._blankAvatarIcon = this.LoadTextureFromResource("CustomAvatar.Resources.mystery-man.png");
			this._noAvatarIcon = this.LoadTextureFromResource("CustomAvatar.Resources.ban.png");
			if (firstActivation)
			{
				this.FirstActivation();
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006F98 File Offset: 0x00005198
		private Texture2D LoadTextureFromResource(string resourceName)
		{
			Texture2D texture2D = new Texture2D(0, 0);
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			{
				byte[] array = new byte[manifestResourceStream.Length];
				manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
				ImageConversion.LoadImage(texture2D, array);
			}
			return texture2D;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007004 File Offset: 0x00005204
		private void FirstActivation()
		{
			this._tableCellTemplate = Resources.FindObjectsOfTypeAll<LevelListTableCell>().First((LevelListTableCell x) => x.name == "LevelListTableCell");
			this.avatarList.tableView.SetPrivateField("_pageUpButton", this.upButton);
			this.avatarList.tableView.SetPrivateField("_pageDownButton", this.downButton);
			this.avatarList.tableView.SetPrivateField("_hideScrollButtonsIfNotNeeded", false);
			TableViewScroller scroller = this.avatarList.tableView.GetPrivateField("_scroller");
			this.upButton.onClick.AddListener(delegate
			{
				scroller.PageScrollUp();
				this.avatarList.tableView.InvokePrivateMethod("RefreshScrollButtons", new object[] { false });
			});
			this.downButton.onClick.AddListener(delegate
			{
				scroller.PageScrollDown();
				this.avatarList.tableView.InvokePrivateMethod("RefreshScrollButtons", new object[] { false });
			});
			this.avatarList.tableView.dataSource = this;
			this._avatars.Add(new AvatarListItem("No Avatar", this._noAvatarIcon));
			AvatarManager.instance.GetAvatarsAsync(delegate(LoadedAvatar avatar)
			{
				Plugin.logger.Info("Loaded avatar " + avatar.descriptor.name);
				this._avatars.Add(new AvatarListItem(avatar));
				this.ReloadData();
			}, delegate(Exception ex)
			{
				Plugin.logger.Error("Failed to load avatar: " + ex.Message);
			});
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000715B File Offset: 0x0000535B
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
			AvatarManager.instance.avatarChanged -= this.OnAvatarChanged;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000717D File Offset: 0x0000537D
		[UIAction("avatar-click")]
		private void OnAvatarClicked(TableView table, int row)
		{
			AvatarManager.instance.SwitchToAvatar(this._avatars[row].avatar);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000719C File Offset: 0x0000539C
		private void OnAvatarChanged(SpawnedAvatar avatar)
		{
			this.ReloadData();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000071A8 File Offset: 0x000053A8
		private void ReloadData()
		{
			this._avatars.Sort(delegate(AvatarListItem a, AvatarListItem b)
			{
				bool flag = a.avatar == null;
				int num2;
				if (flag)
				{
					num2 = -1;
				}
				else
				{
					bool flag2 = b.avatar == null;
					if (flag2)
					{
						num2 = 1;
					}
					else
					{
						num2 = string.Compare(a.name, b.name, StringComparison.CurrentCulture);
					}
				}
				return num2;
			});
			int num = this._avatars.FindIndex(delegate(AvatarListItem a)
			{
				LoadedAvatar avatar = a.avatar;
				string text = ((avatar != null) ? avatar.fullPath : null);
				SpawnedAvatar currentlySpawnedAvatar = AvatarManager.instance.currentlySpawnedAvatar;
				return text == ((currentlySpawnedAvatar != null) ? currentlySpawnedAvatar.customAvatar.fullPath : null);
			});
			this.avatarList.tableView.ReloadData();
			this.avatarList.tableView.ScrollToCellWithIdx(num, 1, true);
			this.avatarList.tableView.SelectCellWithIdx(num, false);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00007244 File Offset: 0x00005444
		public float CellSize()
		{
			return 8.5f;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000725C File Offset: 0x0000545C
		public int NumberOfCells()
		{
			return this._avatars.Count;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000727C File Offset: 0x0000547C
		public TableCell CellForIdx(TableView tableView, int idx)
		{
			LevelListTableCell levelListTableCell = this.avatarList.tableView.DequeueReusableCellForIdentifier("CustomAvatarsTableCell") as LevelListTableCell;
			bool flag = !levelListTableCell;
			if (flag)
			{
				levelListTableCell = Object.Instantiate<LevelListTableCell>(this._tableCellTemplate);
				foreach (Image image in levelListTableCell.GetPrivateField("_beatmapCharacteristicImages"))
				{
					Object.DestroyImmediate(image);
				}
				levelListTableCell.SetPrivateField("_beatmapCharacteristicImages", new Image[0]);
				levelListTableCell.GetPrivateField("_favoritesBadgeImage").enabled = false;
				levelListTableCell.reuseIdentifier = "CustomAvatarsTableCell";
			}
			AvatarListItem avatarListItem = this._avatars[idx];
			levelListTableCell.GetPrivateField("_songNameText").text = avatarListItem.name;
			levelListTableCell.GetPrivateField("_authorText").text = avatarListItem.author;
			levelListTableCell.GetPrivateField("_coverRawImage").texture = avatarListItem.icon ?? this._blankAvatarIcon;
			return levelListTableCell;
		}

		// Token: 0x04000177 RID: 375
		private const string kTableCellReuseIdentifier = "CustomAvatarsTableCell";

		// Token: 0x04000178 RID: 376
		[UIComponent("avatar-list")]
		public CustomListTableData avatarList;

		// Token: 0x04000179 RID: 377
		[UIComponent("up-button")]
		public Button upButton;

		// Token: 0x0400017A RID: 378
		[UIComponent("down-button")]
		public Button downButton;

		// Token: 0x0400017B RID: 379
		private readonly List<AvatarListItem> _avatars = new List<AvatarListItem>();

		// Token: 0x0400017C RID: 380
		private LevelListTableCell _tableCellTemplate;

		// Token: 0x0400017D RID: 381
		private Texture2D _blankAvatarIcon;

		// Token: 0x0400017E RID: 382
		private Texture2D _noAvatarIcon;
	}
}
