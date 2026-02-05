using System;

namespace BeatSaverSharp
{
	// Token: 0x02000005 RID: 5
	public struct HttpOptions
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002944 File Offset: 0x00000B44
		// (set) Token: 0x06000021 RID: 33 RVA: 0x0000294C File Offset: 0x00000B4C
		public string ApplicationName { readonly get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002955 File Offset: 0x00000B55
		// (set) Token: 0x06000023 RID: 35 RVA: 0x0000295D File Offset: 0x00000B5D
		public Version Version { readonly get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002966 File Offset: 0x00000B66
		// (set) Token: 0x06000025 RID: 37 RVA: 0x0000296E File Offset: 0x00000B6E
		public TimeSpan? Timeout { readonly get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002977 File Offset: 0x00000B77
		// (set) Token: 0x06000027 RID: 39 RVA: 0x0000297F File Offset: 0x00000B7F
		public bool HandleRateLimits { readonly get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002988 File Offset: 0x00000B88
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002990 File Offset: 0x00000B90
		public ApplicationAgent[] Agents { readonly get; set; }
	}
}
