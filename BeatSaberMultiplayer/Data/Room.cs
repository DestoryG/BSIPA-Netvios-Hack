using System;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x02000091 RID: 145
	public class Room
	{
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x00026B66 File Offset: 0x00024D66
		// (set) Token: 0x0600097F RID: 2431 RVA: 0x00026B6E File Offset: 0x00024D6E
		public string roomId { get; private set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x00026B77 File Offset: 0x00024D77
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x00026B7F File Offset: 0x00024D7F
		public Room.RoomStatus roomStatus { get; private set; }

		// Token: 0x06000982 RID: 2434 RVA: 0x00026B88 File Offset: 0x00024D88
		public Room(string roomId, long roomOwner, string roomOwnerName, string roomStatus, RoomCfg roomCfg, Player[] players = null, SongCfg songCfg = null)
		{
			this.roomId = roomId;
			this.roomOwner = roomOwner;
			this.roomOwnerName = roomOwnerName;
			this.roomStatus = (Room.RoomStatus)Enum.Parse(typeof(Room.RoomStatus), roomStatus, true);
			this.roomCfg = roomCfg;
			this.players = players;
			this.songCfg = songCfg;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00026BE5 File Offset: 0x00024DE5
		public void SetRoomStatus(string status)
		{
			this.roomStatus = (Room.RoomStatus)Enum.Parse(typeof(Room.RoomStatus), status, true);
		}

		// Token: 0x040004B5 RID: 1205
		public long roomOwner;

		// Token: 0x040004B6 RID: 1206
		public string roomOwnerName;

		// Token: 0x040004B8 RID: 1208
		public RoomCfg roomCfg;

		// Token: 0x040004B9 RID: 1209
		public SongCfg songCfg;

		// Token: 0x040004BA RID: 1210
		public Player[] players;

		// Token: 0x02000119 RID: 281
		[Flags]
		public enum RoomStatus
		{
			// Token: 0x0400064D RID: 1613
			Waiting = 1,
			// Token: 0x0400064E RID: 1614
			Playing = 2
		}
	}
}
