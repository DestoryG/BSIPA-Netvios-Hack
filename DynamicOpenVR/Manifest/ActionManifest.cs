using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynamicOpenVR.Manifest
{
	// Token: 0x020000CF RID: 207
	internal class ActionManifest
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00005E94 File Offset: 0x00004094
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00005E9C File Offset: 0x0000409C
		[JsonProperty(PropertyName = "version")]
		internal ulong version { get; set; } = 1UL;

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00005EA5 File Offset: 0x000040A5
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00005EAD File Offset: 0x000040AD
		[JsonProperty(PropertyName = "actions")]
		internal List<ManifestAction> actions { get; set; } = new List<ManifestAction>();

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00005EB6 File Offset: 0x000040B6
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00005EBE File Offset: 0x000040BE
		[JsonProperty(PropertyName = "action_sets")]
		internal List<ManifestActionSet> actionSets { get; set; } = new List<ManifestActionSet>();

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00005EC7 File Offset: 0x000040C7
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00005ECF File Offset: 0x000040CF
		[JsonProperty(PropertyName = "default_bindings")]
		internal List<ManifestDefaultBinding> defaultBindings { get; set; } = new List<ManifestDefaultBinding>();

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00005ED8 File Offset: 0x000040D8
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00005EE0 File Offset: 0x000040E0
		[JsonProperty(PropertyName = "localization")]
		internal List<Dictionary<string, string>> localization { get; set; } = new List<Dictionary<string, string>>();
	}
}
