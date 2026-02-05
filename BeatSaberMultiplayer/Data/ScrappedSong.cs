using System;
using System.Collections.Generic;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x02000099 RID: 153
	public struct ScrappedSong
	{
		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x00026D48 File Offset: 0x00024F48
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x00026D50 File Offset: 0x00024F50
		public string Key { get; set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x00026D59 File Offset: 0x00024F59
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x00026D61 File Offset: 0x00024F61
		public string Hash { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x00026D6A File Offset: 0x00024F6A
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x00026D72 File Offset: 0x00024F72
		public string SongName { get; set; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x00026D7B File Offset: 0x00024F7B
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x00026D83 File Offset: 0x00024F83
		public string SongSubName { get; set; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x00026D8C File Offset: 0x00024F8C
		// (set) Token: 0x060009A1 RID: 2465 RVA: 0x00026D94 File Offset: 0x00024F94
		public string LevelAuthorName { get; set; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x00026D9D File Offset: 0x00024F9D
		// (set) Token: 0x060009A3 RID: 2467 RVA: 0x00026DA5 File Offset: 0x00024FA5
		public string SongAuthorName { get; set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x00026DAE File Offset: 0x00024FAE
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x00026DB6 File Offset: 0x00024FB6
		public List<DifficultyStats> Diffs { get; set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x00026DBF File Offset: 0x00024FBF
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x00026DC7 File Offset: 0x00024FC7
		public float Bpm { get; set; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x00026DD0 File Offset: 0x00024FD0
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x00026DD8 File Offset: 0x00024FD8
		public int PlayedCount { get; set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x00026DE1 File Offset: 0x00024FE1
		// (set) Token: 0x060009AB RID: 2475 RVA: 0x00026DE9 File Offset: 0x00024FE9
		public int Upvotes { get; set; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x00026DF2 File Offset: 0x00024FF2
		// (set) Token: 0x060009AD RID: 2477 RVA: 0x00026DFA File Offset: 0x00024FFA
		public int Downvotes { get; set; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x00026E03 File Offset: 0x00025003
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x00026E0B File Offset: 0x0002500B
		public float Heat { get; set; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00026E14 File Offset: 0x00025014
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x00026E1C File Offset: 0x0002501C
		public float Rating { get; set; }

		// Token: 0x060009B2 RID: 2482 RVA: 0x00026E28 File Offset: 0x00025028
		public override bool Equals(object obj)
		{
			if (obj is ScrappedSong)
			{
				ScrappedSong scrappedSong = (ScrappedSong)obj;
				if (this.Key == scrappedSong.Key && this.Hash == scrappedSong.Hash && this.SongName == scrappedSong.SongName && this.SongSubName == scrappedSong.SongSubName && this.LevelAuthorName == scrappedSong.LevelAuthorName && this.SongAuthorName == scrappedSong.SongAuthorName && EqualityComparer<List<DifficultyStats>>.Default.Equals(this.Diffs, scrappedSong.Diffs) && this.Bpm == scrappedSong.Bpm && this.PlayedCount == scrappedSong.PlayedCount && this.Upvotes == scrappedSong.Upvotes && this.Downvotes == scrappedSong.Downvotes && this.Heat == scrappedSong.Heat)
				{
					return this.Rating == scrappedSong.Rating;
				}
			}
			return false;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00026F44 File Offset: 0x00025144
		public override int GetHashCode()
		{
			return ((((((((((((505786036 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Key)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Hash)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.SongName)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.SongSubName)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.LevelAuthorName)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.SongAuthorName)) * -1521134295 + EqualityComparer<List<DifficultyStats>>.Default.GetHashCode(this.Diffs)) * -1521134295 + this.Bpm.GetHashCode()) * -1521134295 + this.PlayedCount.GetHashCode()) * -1521134295 + this.Upvotes.GetHashCode()) * -1521134295 + this.Downvotes.GetHashCode()) * -1521134295 + this.Heat.GetHashCode()) * -1521134295 + this.Rating.GetHashCode();
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00027075 File Offset: 0x00025275
		public static bool operator ==(ScrappedSong c1, ScrappedSong c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0002708A File Offset: 0x0002528A
		public static bool operator !=(ScrappedSong c1, ScrappedSong c2)
		{
			return !c1.Equals(c2);
		}
	}
}
