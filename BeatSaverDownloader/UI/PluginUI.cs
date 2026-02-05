using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using HMUI;
using UnityEngine;

namespace BeatSaverDownloader.UI
{
	// Token: 0x02000011 RID: 17
	public class PluginUI : PersistentSingleton<PluginUI>
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x00003D4C File Offset: 0x00001F4C
		internal void Setup()
		{
			this.moreSongsButton = new MenuButton("更多歌曲", "从这里下载更多歌曲!", new Action(this.MoreSongsButtonPressed), false);
			PersistentSingleton<MenuButtons>.instance.RegisterButton(this.moreSongsButton);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003D80 File Offset: 0x00001F80
		internal static void SetupLevelDetailClone()
		{
			PluginUI._levelDetailClone = Object.Instantiate<GameObject>(Resources.FindObjectsOfTypeAll<StandardLevelDetailView>().First((StandardLevelDetailView x) => x.gameObject.name == "LevelDetail").gameObject);
			PluginUI._levelDetailClone.gameObject.SetActive(false);
			Object.Destroy(PluginUI._levelDetailClone.GetComponent<StandardLevelDetailView>());
			IEnumerable<RectTransform> enumerable = from x in PluginUI._levelDetailClone.GetComponentsInChildren<RectTransform>()
				where x.gameObject.name.StartsWith("BSML")
				select x;
			HoverHint[] componentsInChildren = PluginUI._levelDetailClone.GetComponentsInChildren<HoverHint>();
			LocalizedHoverHint[] componentsInChildren2 = PluginUI._levelDetailClone.GetComponentsInChildren<LocalizedHoverHint>();
			foreach (RectTransform rectTransform in enumerable)
			{
				Object.Destroy(rectTransform.gameObject);
			}
			HoverHint[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i]);
			}
			LocalizedHoverHint[] array2 = componentsInChildren2;
			for (int i = 0; i < array2.Length; i++)
			{
				Object.Destroy(array2[i]);
			}
			Object.Destroy(PluginUI._levelDetailClone.transform.Find("LevelInfo").Find("FavoritesToggle").gameObject);
			Object.Destroy(PluginUI._levelDetailClone.transform.Find("PlayContainer").Find("PlayButtons").gameObject);
			Object.Destroy(PluginUI._levelDetailClone.transform.Find("Stats").Find("MaxCombo").gameObject);
			Object.Destroy(PluginUI._levelDetailClone.transform.Find("Stats").Find("Highscore").gameObject);
			Object.Destroy(PluginUI._levelDetailClone.transform.Find("Stats").Find("MaxRank").gameObject);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003F6C File Offset: 0x0000216C
		internal void MoreSongsButtonPressed()
		{
			this.ShowMoreSongsFlow();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003F74 File Offset: 0x00002174
		internal void ShowMoreSongsFlow()
		{
			if (this._moreSongsFlowCooridinator == null)
			{
				this._moreSongsFlowCooridinator = BeatSaberUI.CreateFlowCoordinator<MoreSongsFlowCoordinator>();
			}
			this._moreSongsFlowCooridinator.SetParentFlowCoordinator(BeatSaberUI.MainFlowCoordinator);
			BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(this._moreSongsFlowCooridinator, null, false, false);
		}

		// Token: 0x0400002B RID: 43
		public MenuButton moreSongsButton;

		// Token: 0x0400002C RID: 44
		internal MoreSongsFlowCoordinator _moreSongsFlowCooridinator;

		// Token: 0x0400002D RID: 45
		public static GameObject _levelDetailClone;
	}
}
