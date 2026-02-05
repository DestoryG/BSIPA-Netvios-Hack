using System;
using System.Collections.Generic;
using System.Linq;
using HMUI;
using SongCore.OverrideClasses;
using UnityEngine;

namespace BeatSaverDownloader.Misc
{
	// Token: 0x02000021 RID: 33
	public static class CustomHelpers
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00006630 File Offset: 0x00004830
		public static void RefreshTable(this TableView tableView, bool callbackTable = true)
		{
			HashSet<int> hashSet = new HashSet<int>(tableView.GetPrivateField("_selectedCellIdxs"));
			tableView.ReloadData();
			if (hashSet.Count > 0)
			{
				tableView.SelectCellWithIdx(hashSet.First<int>(), callbackTable);
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000666C File Offset: 0x0000486C
		public static SongCoreCustomBeatmapLevelPack GetLevelPackWithLevels(CustomPreviewBeatmapLevel[] levels, string packName = null, Sprite packCover = null, string packID = null)
		{
			SongCoreCustomLevelCollection songCoreCustomLevelCollection = new SongCoreCustomLevelCollection(levels.ToArray<CustomPreviewBeatmapLevel>());
			return new SongCoreCustomBeatmapLevelPack(string.IsNullOrEmpty(packID) ? "" : packID, string.IsNullOrEmpty(packName) ? "Custom Songs" : packName, packCover ?? Sprites.BeastSaberLogo, songCoreCustomLevelCollection, "");
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000066BA File Offset: 0x000048BA
		public static string CheckHex(string input)
		{
			input = input.ToUpper();
			if (input.All((char x) => CustomHelpers.hexChars.Contains(x)))
			{
				return input;
			}
			return "";
		}

		// Token: 0x04000096 RID: 150
		private static char[] hexChars = new char[]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};
	}
}
