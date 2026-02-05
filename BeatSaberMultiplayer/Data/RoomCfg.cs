using System;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x02000096 RID: 150
	public class RoomCfg
	{
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x00026C23 File Offset: 0x00024E23
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x00026C2B File Offset: 0x00024E2B
		public string roomId { get; private set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x00026C34 File Offset: 0x00024E34
		// (set) Token: 0x06000989 RID: 2441 RVA: 0x00026C3C File Offset: 0x00024E3C
		public bool isPrivate { get; private set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x00026C45 File Offset: 0x00024E45
		// (set) Token: 0x0600098B RID: 2443 RVA: 0x00026C4D File Offset: 0x00024E4D
		public int maxPlayers { get; private set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x00026C56 File Offset: 0x00024E56
		// (set) Token: 0x0600098D RID: 2445 RVA: 0x00026C5E File Offset: 0x00024E5E
		public float resultsShowTime { get; private set; }

		// Token: 0x0600098E RID: 2446 RVA: 0x00026C67 File Offset: 0x00024E67
		public RoomCfg(string roomId, string roomName)
		{
			this.roomId = roomId;
			this.roomName = roomName;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00026C7D File Offset: 0x00024E7D
		public RoomCfg(string roomId, string roomName, bool isPrivate, int maxPlayers, float resultsShowTime)
		{
			this.roomId = roomId;
			this.roomName = roomName;
			this.isPrivate = isPrivate;
			this.maxPlayers = maxPlayers;
			this.resultsShowTime = resultsShowTime;
		}

		// Token: 0x040004C7 RID: 1223
		public string roomName;
	}
}
