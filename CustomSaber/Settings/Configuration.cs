using System;
using CustomSaber.Settings.Utilities;
using CustomSaber.Utilities;
using IPA.Config;
using IPA.Config.Stores;

namespace CustomSaber.Settings
{
	// Token: 0x02000013 RID: 19
	public class Configuration
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000042CE File Offset: 0x000024CE
		// (set) Token: 0x06000073 RID: 115 RVA: 0x000042D5 File Offset: 0x000024D5
		public static string CurrentlySelectedSaber { get; internal set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000042DD File Offset: 0x000024DD
		// (set) Token: 0x06000075 RID: 117 RVA: 0x000042E4 File Offset: 0x000024E4
		public static TrailType TrailType { get; internal set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000042EC File Offset: 0x000024EC
		// (set) Token: 0x06000077 RID: 119 RVA: 0x000042F3 File Offset: 0x000024F3
		public static bool CustomEventsEnabled { get; internal set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000078 RID: 120 RVA: 0x000042FB File Offset: 0x000024FB
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00004302 File Offset: 0x00002502
		public static bool RandomSabersEnabled { get; internal set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600007A RID: 122 RVA: 0x0000430A File Offset: 0x0000250A
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00004311 File Offset: 0x00002511
		public static bool ShowSabersInSaberMenu { get; internal set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00004319 File Offset: 0x00002519
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00004320 File Offset: 0x00002520
		public static bool OverrideTrailLength { get; internal set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00004328 File Offset: 0x00002528
		// (set) Token: 0x0600007F RID: 127 RVA: 0x0000432F File Offset: 0x0000252F
		public static float TrailLength { get; internal set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00004337 File Offset: 0x00002537
		// (set) Token: 0x06000081 RID: 129 RVA: 0x0000433E File Offset: 0x0000253E
		public static float SaberWidthAdjust { get; internal set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00004346 File Offset: 0x00002546
		// (set) Token: 0x06000083 RID: 131 RVA: 0x0000434D File Offset: 0x0000254D
		public static bool DisableWhitestep { get; internal set; }

		// Token: 0x06000084 RID: 132 RVA: 0x00004355 File Offset: 0x00002555
		internal static void Init(Config config)
		{
			PluginConfig.Instance = config.Generated(true);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004364 File Offset: 0x00002564
		internal static void Load()
		{
			Configuration.CurrentlySelectedSaber = PluginConfig.Instance.lastSaber;
			TrailType trailType;
			Configuration.TrailType = (Enum.TryParse<TrailType>(PluginConfig.Instance.trailType, out trailType) ? trailType : TrailType.Custom);
			Configuration.CustomEventsEnabled = PluginConfig.Instance.customEventsEnabled;
			Configuration.RandomSabersEnabled = PluginConfig.Instance.randomSabersEnabled;
			Configuration.ShowSabersInSaberMenu = PluginConfig.Instance.showSabersInSaberMenu;
			Configuration.OverrideTrailLength = PluginConfig.Instance.overrideCustomTrailLength;
			Configuration.TrailLength = PluginConfig.Instance.trailLength;
			Configuration.SaberWidthAdjust = PluginConfig.Instance.saberWidthAdjust;
			Configuration.DisableWhitestep = PluginConfig.Instance.disableWhitestep;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004410 File Offset: 0x00002610
		internal static void Save()
		{
			PluginConfig.Instance.lastSaber = Configuration.CurrentlySelectedSaber;
			PluginConfig.Instance.trailType = Configuration.TrailType.ToString();
			PluginConfig.Instance.customEventsEnabled = Configuration.CustomEventsEnabled;
			PluginConfig.Instance.randomSabersEnabled = Configuration.RandomSabersEnabled;
			PluginConfig.Instance.showSabersInSaberMenu = Configuration.ShowSabersInSaberMenu;
			PluginConfig.Instance.overrideCustomTrailLength = Configuration.OverrideTrailLength;
			PluginConfig.Instance.trailLength = Configuration.TrailLength;
			PluginConfig.Instance.saberWidthAdjust = Configuration.SaberWidthAdjust;
			PluginConfig.Instance.disableWhitestep = Configuration.DisableWhitestep;
		}
	}
}
