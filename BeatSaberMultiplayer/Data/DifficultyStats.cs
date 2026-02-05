using System;
using System.Collections.Generic;

namespace BeatSaberMultiplayer.Data
{
	// Token: 0x0200009A RID: 154
	public struct DifficultyStats
	{
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x000270A2 File Offset: 0x000252A2
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x000270AA File Offset: 0x000252AA
		public string Diff { get; set; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x000270B3 File Offset: 0x000252B3
		// (set) Token: 0x060009B9 RID: 2489 RVA: 0x000270BB File Offset: 0x000252BB
		public int Scores { get; set; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x000270C4 File Offset: 0x000252C4
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x000270CC File Offset: 0x000252CC
		public float Stars { get; set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x000270D5 File Offset: 0x000252D5
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x000270DD File Offset: 0x000252DD
		public byte Ranked { get; set; }

		// Token: 0x060009BE RID: 2494 RVA: 0x000270E8 File Offset: 0x000252E8
		public override bool Equals(object obj)
		{
			if (obj is DifficultyStats)
			{
				DifficultyStats difficultyStats = (DifficultyStats)obj;
				if (this.Diff == difficultyStats.Diff && this.Scores == difficultyStats.Scores && this.Stars == difficultyStats.Stars)
				{
					return this.Ranked == difficultyStats.Ranked;
				}
			}
			return false;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00027148 File Offset: 0x00025348
		public override int GetHashCode()
		{
			return (((342751480 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Diff)) * -1521134295 + this.Scores.GetHashCode()) * -1521134295 + this.Stars.GetHashCode()) * -1521134295 + this.Ranked.GetHashCode();
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000271B0 File Offset: 0x000253B0
		public static bool operator ==(DifficultyStats c1, DifficultyStats c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000271C5 File Offset: 0x000253C5
		public static bool operator !=(DifficultyStats c1, DifficultyStats c2)
		{
			return !c1.Equals(c2);
		}
	}
}
