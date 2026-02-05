using System;
using System.Linq;
using IPA.Utilities;
using TMPro;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x0200004E RID: 78
	public class LeaderboardTag : BSMLTag
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00009965 File Offset: 0x00007B65
		public override string[] Aliases
		{
			get
			{
				return new string[] { "leaderboard" };
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00009978 File Offset: 0x00007B78
		public override GameObject CreateObject(Transform parent)
		{
			LeaderboardTableView leaderboardTableView = Object.Instantiate<LeaderboardTableView>(Resources.FindObjectsOfTypeAll<LeaderboardTableView>().First((LeaderboardTableView x) => x.name == "LeaderboardTableView"), parent, false);
			leaderboardTableView.name = "BSMLLeaderboard";
			leaderboardTableView.GetField("_cellPrefab").GetField("_scoreText").enableWordWrapping = false;
			foreach (object obj in leaderboardTableView.transform.GetChild(0).GetChild(0))
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			return leaderboardTableView.gameObject;
		}
	}
}
