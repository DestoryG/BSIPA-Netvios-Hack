using System;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x02000098 RID: 152
	public struct RoomSettings
	{
		// Token: 0x06000997 RID: 2455 RVA: 0x00026D29 File Offset: 0x00024F29
		public RoomSettings(string name, string password, int maxPlayers, int resultsShowTime)
		{
			this.name = name;
			this.password = password;
			this.maxPlayers = maxPlayers;
			this.resultsShowTime = resultsShowTime;
		}

		// Token: 0x040004CD RID: 1229
		public string name;

		// Token: 0x040004CE RID: 1230
		public string password;

		// Token: 0x040004CF RID: 1231
		public int maxPlayers;

		// Token: 0x040004D0 RID: 1232
		public int resultsShowTime;
	}
}
