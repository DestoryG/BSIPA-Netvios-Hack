using System;

namespace CustomSaber.Settings.Utilities
{
	// Token: 0x02000014 RID: 20
	public class PluginConfig
	{
		// Token: 0x04000057 RID: 87
		public static PluginConfig Instance;

		// Token: 0x04000058 RID: 88
		public string lastSaber;

		// Token: 0x04000059 RID: 89
		public string trailType;

		// Token: 0x0400005A RID: 90
		public bool customEventsEnabled = true;

		// Token: 0x0400005B RID: 91
		public bool randomSabersEnabled = false;

		// Token: 0x0400005C RID: 92
		public bool showSabersInSaberMenu = false;

		// Token: 0x0400005D RID: 93
		public bool overrideCustomTrailLength = false;

		// Token: 0x0400005E RID: 94
		public float trailLength = 1f;

		// Token: 0x0400005F RID: 95
		public float saberWidthAdjust = 1f;

		// Token: 0x04000060 RID: 96
		public bool disableWhitestep = false;
	}
}
