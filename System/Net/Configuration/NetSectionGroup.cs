using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200033A RID: 826
	public sealed class NetSectionGroup : ConfigurationSectionGroup
	{
		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06001D78 RID: 7544 RVA: 0x0008BD1A File Offset: 0x00089F1A
		[ConfigurationProperty("authenticationModules")]
		public AuthenticationModulesSection AuthenticationModules
		{
			get
			{
				return (AuthenticationModulesSection)base.Sections["authenticationModules"];
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06001D79 RID: 7545 RVA: 0x0008BD31 File Offset: 0x00089F31
		[ConfigurationProperty("connectionManagement")]
		public ConnectionManagementSection ConnectionManagement
		{
			get
			{
				return (ConnectionManagementSection)base.Sections["connectionManagement"];
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06001D7A RID: 7546 RVA: 0x0008BD48 File Offset: 0x00089F48
		[ConfigurationProperty("defaultProxy")]
		public DefaultProxySection DefaultProxy
		{
			get
			{
				return (DefaultProxySection)base.Sections["defaultProxy"];
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06001D7B RID: 7547 RVA: 0x0008BD5F File Offset: 0x00089F5F
		public MailSettingsSectionGroup MailSettings
		{
			get
			{
				return (MailSettingsSectionGroup)base.SectionGroups["mailSettings"];
			}
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0008BD76 File Offset: 0x00089F76
		public static NetSectionGroup GetSectionGroup(Configuration config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			return config.GetSectionGroup("system.net") as NetSectionGroup;
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x0008BD96 File Offset: 0x00089F96
		[ConfigurationProperty("requestCaching")]
		public RequestCachingSection RequestCaching
		{
			get
			{
				return (RequestCachingSection)base.Sections["requestCaching"];
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06001D7E RID: 7550 RVA: 0x0008BDAD File Offset: 0x00089FAD
		[ConfigurationProperty("settings")]
		public SettingsSection Settings
		{
			get
			{
				return (SettingsSection)base.Sections["settings"];
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06001D7F RID: 7551 RVA: 0x0008BDC4 File Offset: 0x00089FC4
		[ConfigurationProperty("webRequestModules")]
		public WebRequestModulesSection WebRequestModules
		{
			get
			{
				return (WebRequestModulesSection)base.Sections["webRequestModules"];
			}
		}
	}
}
