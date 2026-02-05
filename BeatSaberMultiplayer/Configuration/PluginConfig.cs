using System;

namespace BeatSaberMultiplayer.Configuration
{
	// Token: 0x0200006D RID: 109
	internal class PluginConfig
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x0002353B File Offset: 0x0002173B
		// (set) Token: 0x0600083C RID: 2108 RVA: 0x00023542 File Offset: 0x00021742
		public static PluginConfig Instance { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0002354A File Offset: 0x0002174A
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x00023552 File Offset: 0x00021752
		public virtual bool ExposesStatusEnabled { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0002355B File Offset: 0x0002175B
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x00023563 File Offset: 0x00021763
		public virtual string ExposesUrl { get; set; } = "http://127.0.0.1/status";

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0002356C File Offset: 0x0002176C
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x00023574 File Offset: 0x00021774
		public virtual string Nickname { get; set; } = "Swordsman";

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0002357D File Offset: 0x0002177D
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x00023585 File Offset: 0x00021785
		public virtual bool SpatialAudio { get; set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x0002358E File Offset: 0x0002178E
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x00023596 File Offset: 0x00021796
		public virtual bool CustomVoiceEnabled { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x0002359F File Offset: 0x0002179F
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x000235A7 File Offset: 0x000217A7
		public virtual float VolumeAdjust { get; set; } = 0.8f;

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x000235B0 File Offset: 0x000217B0
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x000235B8 File Offset: 0x000217B8
		public virtual bool MicphoneEnabled { get; set; } = true;

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x000235C1 File Offset: 0x000217C1
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x000235C9 File Offset: 0x000217C9
		public string MicphoneName { get; set; } = "auto";

		// Token: 0x0600084D RID: 2125 RVA: 0x000196A0 File Offset: 0x000178A0
		public virtual void OnReload()
		{
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000196A0 File Offset: 0x000178A0
		public virtual void Changed()
		{
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000196A0 File Offset: 0x000178A0
		public virtual void CopyFrom(PluginConfig other)
		{
		}
	}
}
