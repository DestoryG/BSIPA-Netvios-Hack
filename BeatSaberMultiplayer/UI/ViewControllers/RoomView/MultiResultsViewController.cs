using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using TMPro;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x0200005C RID: 92
	internal class MultiResultsViewController : BSMLResourceViewController
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x0001C5DE File Offset: 0x0001A7DE
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

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001EF6C File Offset: 0x0001D16C
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x0001EF74 File Offset: 0x0001D174
		public string TimeText
		{
			get
			{
				return this._timeText;
			}
			set
			{
				this._timeText = value;
				this.updateTimeText.text = this._timeText;
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000196A0 File Offset: 0x000178A0
		[UIAction("result-selected")]
		private void ResultSelected(TableView sender, ResultListObject obj)
		{
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001EF8E File Offset: 0x0001D18E
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			if (firstActivation)
			{
				this._resultInfos = new List<ResultInfo>();
			}
			this._resultsCellList.tableView.ClearSelection();
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0001EFB6 File Offset: 0x0001D1B6
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001EFC0 File Offset: 0x0001D1C0
		public void SetResults(long playerId, string playerName, int score)
		{
			List<ResultInfo> list = new List<ResultInfo>();
			foreach (ResultInfo resultInfo in this._resultInfos)
			{
				if (resultInfo.playerId != playerId)
				{
					list.Add(resultInfo);
				}
			}
			list.Add(new ResultInfo(playerId, playerName, score));
			base.StartCoroutine(this.UpdateResults(list));
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001F040 File Offset: 0x0001D240
		private IEnumerator UpdateResults(List<ResultInfo> results)
		{
			this._resultInfos = results;
			this._resultsList.Clear();
			IEnumerable<ResultInfo> enumerable = results.OrderByDescending((ResultInfo x) => x.score);
			int num = 1;
			foreach (ResultInfo resultInfo in enumerable)
			{
				this._resultsList.Add(new ResultListObject(num, resultInfo));
				num++;
			}
			this._resultsCellList.tableView.ReloadData();
			yield break;
		}

		// Token: 0x040003A5 RID: 933
		private List<ResultInfo> _resultInfos;

		// Token: 0x040003A6 RID: 934
		[UIComponent("results-list")]
		private CustomCellListTableData _resultsCellList;

		// Token: 0x040003A7 RID: 935
		[UIValue("results")]
		private List<object> _resultsList = new List<object>();

		// Token: 0x040003A8 RID: 936
		[UIComponent("update-time")]
		private TextMeshProUGUI updateTimeText;

		// Token: 0x040003A9 RID: 937
		[UIValue("tiem-text")]
		private string _timeText = "10";
	}
}
