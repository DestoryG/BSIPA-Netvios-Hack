using System;
using IPA.Logging;

namespace IPA.Loader.Features
{
	// Token: 0x02000053 RID: 83
	internal class WarnFeature : Feature
	{
		// Token: 0x06000253 RID: 595 RVA: 0x0000C857 File Offset: 0x0000AA57
		public override bool Initialize(PluginMetadata meta, string[] parameters)
		{
			Logger.features.Warn(meta.Name + ": " + string.Join(" ", parameters));
			return true;
		}
	}
}
