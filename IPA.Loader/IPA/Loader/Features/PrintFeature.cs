using System;
using IPA.Logging;

namespace IPA.Loader.Features
{
	// Token: 0x02000051 RID: 81
	internal class PrintFeature : Feature
	{
		// Token: 0x0600024F RID: 591 RVA: 0x0000C7F7 File Offset: 0x0000A9F7
		public override bool Initialize(PluginMetadata meta, string[] parameters)
		{
			Logger.features.Info(meta.Name + ": " + string.Join(" ", parameters));
			return true;
		}
	}
}
