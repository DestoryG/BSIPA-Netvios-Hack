using System;
using Newtonsoft.Json;

namespace DynamicOpenVR.DefaultBindings
{
	// Token: 0x020000E5 RID: 229
	internal class HapticBinding
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00006807 File Offset: 0x00004A07
		// (set) Token: 0x06000244 RID: 580 RVA: 0x0000680F File Offset: 0x00004A0F
		[JsonProperty(PropertyName = "output")]
		public string output { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00006818 File Offset: 0x00004A18
		// (set) Token: 0x06000246 RID: 582 RVA: 0x00006820 File Offset: 0x00004A20
		[JsonProperty(PropertyName = "path")]
		public string path { get; set; }
	}
}
