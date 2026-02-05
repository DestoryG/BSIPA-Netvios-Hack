using System;
using Newtonsoft.Json;

namespace DynamicOpenVR.DefaultBindings
{
	// Token: 0x020000E3 RID: 227
	internal class SkeletonBinding
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00006780 File Offset: 0x00004980
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00006788 File Offset: 0x00004988
		[JsonProperty(PropertyName = "output")]
		public string output { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00006791 File Offset: 0x00004991
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00006799 File Offset: 0x00004999
		[JsonProperty(PropertyName = "path")]
		public string path { get; set; }
	}
}
