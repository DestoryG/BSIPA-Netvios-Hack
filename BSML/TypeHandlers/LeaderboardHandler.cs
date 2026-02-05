using System;
using System.Collections.Generic;
using IPA.Utilities;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000027 RID: 39
	[ComponentHandler(typeof(LeaderboardTableView))]
	public class LeaderboardHandler : TypeHandler<LeaderboardTableView>
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00006ADC File Offset: 0x00004CDC
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"cellSize",
					new string[] { "cell-size" }
				} };
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00006B09 File Offset: 0x00004D09
		public override Dictionary<string, Action<LeaderboardTableView, string>> Setters
		{
			get
			{
				Dictionary<string, Action<LeaderboardTableView, string>> dictionary = new Dictionary<string, Action<LeaderboardTableView, string>>();
				dictionary.Add("cellSize", delegate(LeaderboardTableView component, string value)
				{
					component.SetField("_rowHeight", Parse.Float(value));
				});
				return dictionary;
			}
		}
	}
}
