using System;
using System.Net.Configuration;

namespace System.Configuration.Internal
{
	// Token: 0x020000BD RID: 189
	internal sealed class ConfigurationManagerHelper : IConfigurationManagerHelper
	{
		// Token: 0x06000647 RID: 1607 RVA: 0x000242DB File Offset: 0x000224DB
		private ConfigurationManagerHelper()
		{
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x000242E3 File Offset: 0x000224E3
		void IConfigurationManagerHelper.EnsureNetConfigLoaded()
		{
			SettingsSection.EnsureConfigLoaded();
		}
	}
}
