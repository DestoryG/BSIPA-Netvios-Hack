using System;
using BeatSaberMarkupLanguage.Attributes;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x0200005E RID: 94
	public class ResultListObject
	{
		// Token: 0x06000769 RID: 1897 RVA: 0x0001F091 File Offset: 0x0001D291
		public ResultListObject(int rank, ResultInfo result)
		{
			this._rank = rank.ToString();
			this._playerName = result.playerName;
			this._score = result.score;
		}

		// Token: 0x040003AD RID: 941
		[UIValue("rank")]
		private string _rank;

		// Token: 0x040003AE RID: 942
		[UIValue("player-name")]
		private string _playerName;

		// Token: 0x040003AF RID: 943
		[UIValue("score")]
		private int _score;
	}
}
