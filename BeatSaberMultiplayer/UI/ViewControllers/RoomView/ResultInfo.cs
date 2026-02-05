using System;

namespace BeatSaberMultiplayer.UI.ViewControllers.RoomView
{
	// Token: 0x0200005D RID: 93
	public class ResultInfo
	{
		// Token: 0x06000768 RID: 1896 RVA: 0x0001F074 File Offset: 0x0001D274
		public ResultInfo(long playerId, string name, int score)
		{
			this.playerId = playerId;
			this.playerName = name;
			this.score = score;
		}

		// Token: 0x040003AA RID: 938
		public long playerId;

		// Token: 0x040003AB RID: 939
		public string playerName;

		// Token: 0x040003AC RID: 940
		public int score;
	}
}
