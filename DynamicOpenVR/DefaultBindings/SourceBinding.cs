using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynamicOpenVR.DefaultBindings
{
	// Token: 0x020000E7 RID: 231
	internal class SourceBinding
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000685B File Offset: 0x00004A5B
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00006863 File Offset: 0x00004A63
		[JsonProperty(PropertyName = "path")]
		public string path { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000686C File Offset: 0x00004A6C
		// (set) Token: 0x06000250 RID: 592 RVA: 0x00006874 File Offset: 0x00004A74
		[JsonProperty(PropertyName = "mode")]
		public string mode { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000687D File Offset: 0x00004A7D
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00006885 File Offset: 0x00004A85
		[JsonProperty(PropertyName = "inputs")]
		public Dictionary<string, SourceInput> inputs { get; set; }
	}
}
