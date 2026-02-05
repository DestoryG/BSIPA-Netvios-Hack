using System;
using Newtonsoft.Json;

namespace DynamicOpenVR.Manifest
{
	// Token: 0x020000D2 RID: 210
	internal class ManifestDefaultBinding
	{
		// Token: 0x0400087C RID: 2172
		[JsonProperty(PropertyName = "controller_type")]
		internal string ControllerType;

		// Token: 0x0400087D RID: 2173
		[JsonProperty(PropertyName = "binding_url")]
		internal string BindingUrl;
	}
}
