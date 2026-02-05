using System;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x0200000E RID: 14
	public struct BeatmapCharacteristicDifficulty
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000368F File Offset: 0x0000188F
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00003697 File Offset: 0x00001897
		[JsonProperty("duration")]
		public float? Duration { readonly get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000036A0 File Offset: 0x000018A0
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000036A8 File Offset: 0x000018A8
		[JsonProperty("length")]
		public long? Length { readonly get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000036B1 File Offset: 0x000018B1
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000036B9 File Offset: 0x000018B9
		[JsonProperty("bombs")]
		public int? Bombs { readonly get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000036C2 File Offset: 0x000018C2
		// (set) Token: 0x06000094 RID: 148 RVA: 0x000036CA File Offset: 0x000018CA
		[JsonProperty("notes")]
		public int? Notes { readonly get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000036D3 File Offset: 0x000018D3
		// (set) Token: 0x06000096 RID: 150 RVA: 0x000036DB File Offset: 0x000018DB
		[JsonProperty("obstacles")]
		public int? Obstacles { readonly get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000036E4 File Offset: 0x000018E4
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000036EC File Offset: 0x000018EC
		[JsonProperty("njs")]
		public float? NoteJumpSpeed { readonly get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000036F5 File Offset: 0x000018F5
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000036FD File Offset: 0x000018FD
		[JsonProperty("njsOffset")]
		public float? NoteJumpSpeedOffset { readonly get; private set; }
	}
}
