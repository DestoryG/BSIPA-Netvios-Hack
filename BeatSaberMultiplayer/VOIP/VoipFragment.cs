using System;
using NSpeex;

namespace BeatSaberMultiplayer.VOIP
{
	// Token: 0x02000088 RID: 136
	public class VoipFragment
	{
		// Token: 0x06000953 RID: 2387 RVA: 0x00026048 File Offset: 0x00024248
		public VoipFragment(long playerId, int index, byte[] data, BandMode mode)
		{
			this.playerId = playerId;
			this.index = index;
			this.data = data;
			this.mode = mode;
		}

		// Token: 0x04000476 RID: 1142
		public long playerId;

		// Token: 0x04000477 RID: 1143
		public readonly byte[] data;

		// Token: 0x04000478 RID: 1144
		public readonly int index;

		// Token: 0x04000479 RID: 1145
		public readonly BandMode mode;
	}
}
