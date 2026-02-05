using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynamicOpenVR.DefaultBindings
{
	// Token: 0x020000E1 RID: 225
	internal class BindingCollection
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000223 RID: 547 RVA: 0x000066C2 File Offset: 0x000048C2
		// (set) Token: 0x06000224 RID: 548 RVA: 0x000066CA File Offset: 0x000048CA
		[JsonProperty(PropertyName = "sources")]
		public List<SourceBinding> sources { get; set; } = new List<SourceBinding>();

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000225 RID: 549 RVA: 0x000066D3 File Offset: 0x000048D3
		// (set) Token: 0x06000226 RID: 550 RVA: 0x000066DB File Offset: 0x000048DB
		[JsonProperty(PropertyName = "haptics")]
		public List<HapticBinding> haptics { get; set; } = new List<HapticBinding>();

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000227 RID: 551 RVA: 0x000066E4 File Offset: 0x000048E4
		// (set) Token: 0x06000228 RID: 552 RVA: 0x000066EC File Offset: 0x000048EC
		[JsonProperty(PropertyName = "poses")]
		public List<PoseBinding> poses { get; set; } = new List<PoseBinding>();

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000066F5 File Offset: 0x000048F5
		// (set) Token: 0x0600022A RID: 554 RVA: 0x000066FD File Offset: 0x000048FD
		[JsonProperty(PropertyName = "skeleton")]
		public List<SkeletonBinding> skeleton { get; set; } = new List<SkeletonBinding>();

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00006706 File Offset: 0x00004906
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0000670E File Offset: 0x0000490E
		[JsonProperty(PropertyName = "chords")]
		public List<ChordBinding> chords { get; set; } = new List<ChordBinding>();
	}
}
