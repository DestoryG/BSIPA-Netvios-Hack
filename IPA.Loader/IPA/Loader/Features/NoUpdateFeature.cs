using System;

namespace IPA.Loader.Features
{
	// Token: 0x02000050 RID: 80
	internal class NoUpdateFeature : Feature
	{
		// Token: 0x0600024B RID: 587 RVA: 0x0000C7C8 File Offset: 0x0000A9C8
		public override bool Initialize(PluginMetadata meta, string[] parameters)
		{
			return meta.Id != null;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000C7D3 File Offset: 0x0000A9D3
		// (set) Token: 0x0600024D RID: 589 RVA: 0x0000C7DB File Offset: 0x0000A9DB
		public override string InvalidMessage { get; protected set; } = "No ID specified; cannot update anyway";
	}
}
