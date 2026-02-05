using System;
using System.Collections.Specialized;
using System.Configuration;

namespace System.Runtime.Serialization
{
	// Token: 0x02000063 RID: 99
	internal static class AppSettings
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x000206D7 File Offset: 0x0001E8D7
		internal static int MaxMimeParts
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings.maxMimeParts;
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x000206E4 File Offset: 0x0001E8E4
		private static void EnsureSettingsLoaded()
		{
			if (!AppSettings.settingsInitalized)
			{
				object obj = AppSettings.appSettingsLock;
				lock (obj)
				{
					if (!AppSettings.settingsInitalized)
					{
						NameValueCollection nameValueCollection = null;
						try
						{
							nameValueCollection = ConfigurationManager.AppSettings;
						}
						catch (ConfigurationErrorsException)
						{
						}
						finally
						{
							if (nameValueCollection == null || !int.TryParse(nameValueCollection["microsoft:xmldictionaryreader:maxmimeparts"], out AppSettings.maxMimeParts))
							{
								AppSettings.maxMimeParts = 1000;
							}
							AppSettings.settingsInitalized = true;
						}
					}
				}
			}
		}

		// Token: 0x040002CA RID: 714
		internal const string MaxMimePartsAppSettingsString = "microsoft:xmldictionaryreader:maxmimeparts";

		// Token: 0x040002CB RID: 715
		private const int DefaultMaxMimeParts = 1000;

		// Token: 0x040002CC RID: 716
		private static int maxMimeParts;

		// Token: 0x040002CD RID: 717
		private static volatile bool settingsInitalized = false;

		// Token: 0x040002CE RID: 718
		private static object appSettingsLock = new object();
	}
}
