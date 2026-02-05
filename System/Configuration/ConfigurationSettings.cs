using System;
using System.Collections.Specialized;

namespace System.Configuration
{
	// Token: 0x02000083 RID: 131
	public sealed class ConfigurationSettings
	{
		// Token: 0x06000525 RID: 1317 RVA: 0x0002149F File Offset: 0x0001F69F
		private ConfigurationSettings()
		{
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x000214A7 File Offset: 0x0001F6A7
		[Obsolete("This method is obsolete, it has been replaced by System.Configuration!System.Configuration.ConfigurationManager.AppSettings")]
		public static NameValueCollection AppSettings
		{
			get
			{
				return ConfigurationManager.AppSettings;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000214AE File Offset: 0x0001F6AE
		[Obsolete("This method is obsolete, it has been replaced by System.Configuration!System.Configuration.ConfigurationManager.GetSection")]
		public static object GetConfig(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
