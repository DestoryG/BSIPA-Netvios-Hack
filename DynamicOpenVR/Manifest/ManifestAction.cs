using System;
using Newtonsoft.Json;

namespace DynamicOpenVR.Manifest
{
	// Token: 0x020000D0 RID: 208
	internal class ManifestAction
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00005F25 File Offset: 0x00004125
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00005F2D File Offset: 0x0000412D
		[JsonProperty(PropertyName = "name")]
		internal string Name { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00005F36 File Offset: 0x00004136
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00005F3E File Offset: 0x0000413E
		[JsonProperty(PropertyName = "requirement")]
		internal string Requirement { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00005F47 File Offset: 0x00004147
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00005F4F File Offset: 0x0000414F
		[JsonProperty(PropertyName = "type")]
		internal string Type { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00005F58 File Offset: 0x00004158
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x00005F60 File Offset: 0x00004160
		[JsonProperty(PropertyName = "skeleton", NullValueHandling = NullValueHandling.Ignore)]
		internal string Skeleton { get; set; }
	}
}
