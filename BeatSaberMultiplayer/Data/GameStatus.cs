using System;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x0200008D RID: 141
	public struct GameStatus
	{
		// Token: 0x06000975 RID: 2421 RVA: 0x000269E6 File Offset: 0x00024BE6
		public GameStatus(string roomId, string playerName, int combo, int maxCombo, int noteWasCutCounter, int noteMissedCounter, int score)
		{
			this.roomId = roomId;
			this.playerName = playerName;
			this.combo = combo;
			this.maxCombo = maxCombo;
			this.noteWasCutCounter = noteWasCutCounter;
			this.noteMissedCounter = noteMissedCounter;
			this.score = score;
		}

		// Token: 0x04000498 RID: 1176
		public string roomId;

		// Token: 0x04000499 RID: 1177
		public string playerName;

		// Token: 0x0400049A RID: 1178
		public int combo;

		// Token: 0x0400049B RID: 1179
		public int maxCombo;

		// Token: 0x0400049C RID: 1180
		public int noteWasCutCounter;

		// Token: 0x0400049D RID: 1181
		public int noteMissedCounter;

		// Token: 0x0400049E RID: 1182
		public int score;
	}
}
