using System;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x0200000F RID: 15
	public struct Stats
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00003706 File Offset: 0x00001906
		// (set) Token: 0x0600009C RID: 156 RVA: 0x0000370E File Offset: 0x0000190E
		[JsonProperty("downloads")]
		public int Downloads { readonly get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003717 File Offset: 0x00001917
		// (set) Token: 0x0600009E RID: 158 RVA: 0x0000371F File Offset: 0x0000191F
		[JsonProperty("plays")]
		public int Plays { readonly get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003728 File Offset: 0x00001928
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00003730 File Offset: 0x00001930
		[JsonProperty("upVotes")]
		public int UpVotes { readonly get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003739 File Offset: 0x00001939
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003741 File Offset: 0x00001941
		[JsonProperty("downVotes")]
		public int DownVotes { readonly get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000374A File Offset: 0x0000194A
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003752 File Offset: 0x00001952
		[JsonProperty("rating")]
		public float Rating { readonly get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000375B File Offset: 0x0000195B
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003763 File Offset: 0x00001963
		[JsonProperty("heat")]
		public float Heat { readonly get; private set; }
	}
}
