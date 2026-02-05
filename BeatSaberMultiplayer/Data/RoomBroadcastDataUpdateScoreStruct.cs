using System;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x02000093 RID: 147
	public struct RoomBroadcastDataUpdateScoreStruct
	{
		// Token: 0x06000984 RID: 2436 RVA: 0x00026C03 File Offset: 0x00024E03
		public RoomBroadcastDataUpdateScoreStruct(long playerId, int score)
		{
			this.playerId = playerId;
			this.score = score;
		}

		// Token: 0x040004BF RID: 1215
		public long playerId;

		// Token: 0x040004C0 RID: 1216
		public int score;
	}
}
