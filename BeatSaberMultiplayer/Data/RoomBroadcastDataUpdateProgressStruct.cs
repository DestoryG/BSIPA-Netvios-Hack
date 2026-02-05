using System;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x02000094 RID: 148
	public struct RoomBroadcastDataUpdateProgressStruct
	{
		// Token: 0x06000985 RID: 2437 RVA: 0x00026C13 File Offset: 0x00024E13
		public RoomBroadcastDataUpdateProgressStruct(long playerId, float progress)
		{
			this.playerId = playerId;
			this.progress = progress;
		}

		// Token: 0x040004C1 RID: 1217
		public long playerId;

		// Token: 0x040004C2 RID: 1218
		public float progress;
	}
}
