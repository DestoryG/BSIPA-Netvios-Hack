using System;
using System.Configuration.Internal;

namespace System.Configuration
{
	// Token: 0x02000082 RID: 130
	internal static class ConfigurationManagerInternalFactory
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00021477 File Offset: 0x0001F677
		internal static IConfigurationManagerInternal Instance
		{
			get
			{
				if (ConfigurationManagerInternalFactory.s_instance == null)
				{
					ConfigurationManagerInternalFactory.s_instance = (IConfigurationManagerInternal)TypeUtil.CreateInstanceWithReflectionPermission("System.Configuration.Internal.ConfigurationManagerInternal, System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				}
				return ConfigurationManagerInternalFactory.s_instance;
			}
		}

		// Token: 0x04000C1D RID: 3101
		private const string ConfigurationManagerInternalTypeString = "System.Configuration.Internal.ConfigurationManagerInternal, System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		// Token: 0x04000C1E RID: 3102
		private static volatile IConfigurationManagerInternal s_instance;
	}
}
