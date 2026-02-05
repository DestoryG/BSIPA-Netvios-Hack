using System;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using UnityEngine;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x02000056 RID: 86
	internal class LevelPacksViewController : BSMLResourceViewController
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001C5DE File Offset: 0x0001A7DE
		public override string ResourceName
		{
			get
			{
				return string.Join(".", new string[]
				{
					base.GetType().Namespace,
					base.GetType().Name,
					"bsml"
				});
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000714 RID: 1812 RVA: 0x0001DD34 File Offset: 0x0001BF34
		// (remove) Token: 0x06000715 RID: 1813 RVA: 0x0001DD6C File Offset: 0x0001BF6C
		public event Action<IAnnotatedBeatmapLevelCollection> packSelected;

		// Token: 0x06000716 RID: 1814 RVA: 0x0001DDA1 File Offset: 0x0001BFA1
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			this.Initialize();
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001DDB4 File Offset: 0x0001BFB4
		public void Initialize()
		{
			if (this._initialized)
			{
				return;
			}
			if (this._beatmapLevelsModel == null)
			{
				this._beatmapLevelsModel = Resources.FindObjectsOfTypeAll<BeatmapLevelsModel>().First<BeatmapLevelsModel>();
			}
			IAnnotatedBeatmapLevelCollection[] beatmapLevelPacks = this._beatmapLevelsModel.allLoadedBeatmapLevelPackCollection.beatmapLevelPacks;
			this._visiblePacks = beatmapLevelPacks;
			this.SetPacks(this._visiblePacks, 0);
			this._initialized = true;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001DE14 File Offset: 0x0001C014
		public void SetSelectedPack(IAnnotatedBeatmapLevelCollection pack)
		{
			for (int i = 0; i < this._visiblePacks.Length; i++)
			{
				if (this._visiblePacks[i].collectionName == pack.collectionName)
				{
					this.levelPacksTableData.tableView.SelectCellWithIdx(i, false);
					return;
				}
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001DE64 File Offset: 0x0001C064
		public void SetPacks(IAnnotatedBeatmapLevelCollection[] packs, int idx = 0)
		{
			this.levelPacksTableData.data.Clear();
			foreach (IAnnotatedBeatmapLevelCollection annotatedBeatmapLevelCollection in packs)
			{
				this.levelPacksTableData.data.Add(new CustomListTableData.CustomCellInfo(annotatedBeatmapLevelCollection.collectionName, string.Format("{0} levels", annotatedBeatmapLevelCollection.beatmapLevelCollection.beatmapLevels.Length), annotatedBeatmapLevelCollection.coverImage.texture));
			}
			this.levelPacksTableData.tableView.ReloadData();
			this.levelPacksTableData.tableView.SelectCellWithIdx(idx, false);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001DEF9 File Offset: 0x0001C0F9
		[UIAction("pack-selected")]
		public void PackSelected(TableView sender, int index)
		{
			if (Client.Instance.isHost)
			{
				Action<IAnnotatedBeatmapLevelCollection> action = this.packSelected;
				if (action == null)
				{
					return;
				}
				action(this._visiblePacks[index]);
			}
		}

		// Token: 0x04000376 RID: 886
		[UIComponent("packs-list-table")]
		private CustomListTableData levelPacksTableData;

		// Token: 0x04000377 RID: 887
		private bool _initialized;

		// Token: 0x04000378 RID: 888
		private BeatmapLevelsModel _beatmapLevelsModel;

		// Token: 0x04000379 RID: 889
		private IAnnotatedBeatmapLevelCollection[] _visiblePacks;
	}
}
