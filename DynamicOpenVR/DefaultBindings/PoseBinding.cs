using System;
using Newtonsoft.Json;

namespace DynamicOpenVR.DefaultBindings
{
	// Token: 0x020000E6 RID: 230
	internal class PoseBinding
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00006831 File Offset: 0x00004A31
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00006839 File Offset: 0x00004A39
		[JsonProperty(PropertyName = "output")]
		public string output { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00006842 File Offset: 0x00004A42
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000684A File Offset: 0x00004A4A
		[JsonProperty(PropertyName = "path")]
		public string path { get; set; }
	}
}
