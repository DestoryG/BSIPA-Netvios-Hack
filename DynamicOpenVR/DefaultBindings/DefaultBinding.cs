using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynamicOpenVR.DefaultBindings
{
	// Token: 0x020000E4 RID: 228
	internal class DefaultBinding
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000238 RID: 568 RVA: 0x000067AA File Offset: 0x000049AA
		// (set) Token: 0x06000239 RID: 569 RVA: 0x000067B2 File Offset: 0x000049B2
		[JsonProperty(PropertyName = "name")]
		public string name { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600023A RID: 570 RVA: 0x000067BB File Offset: 0x000049BB
		// (set) Token: 0x0600023B RID: 571 RVA: 0x000067C3 File Offset: 0x000049C3
		[JsonProperty(PropertyName = "description")]
		public string description { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600023C RID: 572 RVA: 0x000067CC File Offset: 0x000049CC
		// (set) Token: 0x0600023D RID: 573 RVA: 0x000067D4 File Offset: 0x000049D4
		[JsonProperty(PropertyName = "controller_type")]
		public string controllerType { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600023E RID: 574 RVA: 0x000067DD File Offset: 0x000049DD
		// (set) Token: 0x0600023F RID: 575 RVA: 0x000067E5 File Offset: 0x000049E5
		[JsonProperty(PropertyName = "category")]
		public string category { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000240 RID: 576 RVA: 0x000067EE File Offset: 0x000049EE
		// (set) Token: 0x06000241 RID: 577 RVA: 0x000067F6 File Offset: 0x000049F6
		[JsonProperty(PropertyName = "bindings")]
		public Dictionary<string, BindingCollection> bindings { get; set; }

		// Token: 0x0400089A RID: 2202
		[JsonProperty(PropertyName = "action_manifest_version")]
		public int actionManifestVersion;
	}
}
