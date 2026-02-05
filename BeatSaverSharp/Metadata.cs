using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x0200000B RID: 11
	public struct Metadata
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003590 File Offset: 0x00001790
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00003598 File Offset: 0x00001798
		[JsonProperty("songName")]
		public string SongName { readonly get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000035A1 File Offset: 0x000017A1
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000035A9 File Offset: 0x000017A9
		[JsonProperty("songSubName")]
		public string SongSubName { readonly get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000035B2 File Offset: 0x000017B2
		// (set) Token: 0x06000074 RID: 116 RVA: 0x000035BA File Offset: 0x000017BA
		[JsonProperty("songAuthorName")]
		public string SongAuthorName { readonly get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000035C3 File Offset: 0x000017C3
		// (set) Token: 0x06000076 RID: 118 RVA: 0x000035CB File Offset: 0x000017CB
		[JsonProperty("levelAuthorName")]
		public string LevelAuthorName { readonly get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000035D4 File Offset: 0x000017D4
		// (set) Token: 0x06000078 RID: 120 RVA: 0x000035DC File Offset: 0x000017DC
		[JsonProperty("duration")]
		public long Duration { readonly get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000035E5 File Offset: 0x000017E5
		// (set) Token: 0x0600007A RID: 122 RVA: 0x000035ED File Offset: 0x000017ED
		[JsonProperty("bpm")]
		public float BPM { readonly get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000035F6 File Offset: 0x000017F6
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000035FE File Offset: 0x000017FE
		[JsonProperty("difficulties")]
		public Difficulties Difficulties { readonly get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003607 File Offset: 0x00001807
		// (set) Token: 0x0600007E RID: 126 RVA: 0x0000360F File Offset: 0x0000180F
		[JsonProperty("characteristics")]
		public ReadOnlyCollection<BeatmapCharacteristic> Characteristics { readonly get; private set; }
	}
}
