using System;
using Newtonsoft.Json;

namespace DynamicOpenVR.DefaultBindings
{
	// Token: 0x020000E8 RID: 232
	internal class SourceInput
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00006896 File Offset: 0x00004A96
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000689E File Offset: 0x00004A9E
		[JsonProperty(PropertyName = "output")]
		public string output { get; set; }
	}
}
