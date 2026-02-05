using System;
using Newtonsoft.Json;

namespace DynamicOpenVR.Manifest
{
	// Token: 0x020000D1 RID: 209
	internal class ManifestActionSet
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00005F71 File Offset: 0x00004171
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00005F79 File Offset: 0x00004179
		[JsonProperty(PropertyName = "name")]
		internal string Name { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00005F82 File Offset: 0x00004182
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00005F8A File Offset: 0x0000418A
		[JsonProperty(PropertyName = "usage")]
		internal string Usage { get; set; }
	}
}
