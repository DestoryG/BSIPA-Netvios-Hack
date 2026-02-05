using System;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x0200009B RID: 155
	public class Song
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x000271E0 File Offset: 0x000253E0
		public Song(string songId, string songName, string album, string singer, long publishDate, int duration, string coverImage, int seq)
		{
			this.songId = songId;
			this.songName = songName;
			this.album = album;
			this.singer = singer;
			this.publishDate = publishDate;
			this.duration = duration;
			this.coverImage = coverImage;
			this.seq = seq;
		}

		// Token: 0x040004E2 RID: 1250
		public string songId;

		// Token: 0x040004E3 RID: 1251
		public string songName;

		// Token: 0x040004E4 RID: 1252
		public string album;

		// Token: 0x040004E5 RID: 1253
		public string singer;

		// Token: 0x040004E6 RID: 1254
		public long publishDate;

		// Token: 0x040004E7 RID: 1255
		public int duration;

		// Token: 0x040004E8 RID: 1256
		public string coverImage;

		// Token: 0x040004E9 RID: 1257
		public int seq;
	}
}
