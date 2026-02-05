using System;
using Newtonsoft.Json;

namespace SongCore.Data
{
	// Token: 0x0200002D RID: 45
	[Serializable]
	public class SongHashData
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x000084BA File Offset: 0x000066BA
		[JsonConstructor]
		public SongHashData(long directoryHash, string songHash)
		{
			this.directoryHash = directoryHash;
			this.songHash = songHash;
		}

		// Token: 0x04000097 RID: 151
		public long directoryHash;

		// Token: 0x04000098 RID: 152
		public string songHash;
	}
}
