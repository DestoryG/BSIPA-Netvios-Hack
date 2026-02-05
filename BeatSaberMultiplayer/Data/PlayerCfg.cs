using System;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x02000090 RID: 144
	public class PlayerCfg
	{
		// Token: 0x0600097D RID: 2429 RVA: 0x00026B49 File Offset: 0x00024D49
		public PlayerCfg(long playerId, bool headphoneOn, bool microphoneOn)
		{
			this.playerId = playerId;
			this.headphoneOn = headphoneOn;
			this.microphoneOn = microphoneOn;
		}

		// Token: 0x040004B1 RID: 1201
		public long playerId;

		// Token: 0x040004B2 RID: 1202
		public bool headphoneOn;

		// Token: 0x040004B3 RID: 1203
		public bool microphoneOn;
	}
}
