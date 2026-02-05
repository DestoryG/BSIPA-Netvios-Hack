using System;
using IPA.Logging;

namespace IPA.Loader.Features
{
	// Token: 0x02000052 RID: 82
	internal class DebugFeature : Feature
	{
		// Token: 0x06000251 RID: 593 RVA: 0x0000C827 File Offset: 0x0000AA27
		public override bool Initialize(PluginMetadata meta, string[] parameters)
		{
			Logger.features.Debug(meta.Name + ": " + string.Join(" ", parameters));
			return true;
		}
	}
}
