using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200032F RID: 815
	public sealed class DefaultProxySection : ConfigurationSection
	{
		// Token: 0x06001D2C RID: 7468 RVA: 0x0008AE20 File Offset: 0x00089020
		public DefaultProxySection()
		{
			this.properties.Add(this.bypasslist);
			this.properties.Add(this.module);
			this.properties.Add(this.proxy);
			this.properties.Add(this.enabled);
			this.properties.Add(this.useDefaultCredentials);
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x0008AF2C File Offset: 0x0008912C
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", new object[] { "defaultProxy" }), ex);
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001D2E RID: 7470 RVA: 0x0008AF84 File Offset: 0x00089184
		[ConfigurationProperty("bypasslist")]
		public BypassElementCollection BypassList
		{
			get
			{
				return (BypassElementCollection)base[this.bypasslist];
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06001D2F RID: 7471 RVA: 0x0008AF97 File Offset: 0x00089197
		[ConfigurationProperty("module")]
		public ModuleElement Module
		{
			get
			{
				return (ModuleElement)base[this.module];
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06001D30 RID: 7472 RVA: 0x0008AFAA File Offset: 0x000891AA
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001D31 RID: 7473 RVA: 0x0008AFB2 File Offset: 0x000891B2
		[ConfigurationProperty("proxy")]
		public ProxyElement Proxy
		{
			get
			{
				return (ProxyElement)base[this.proxy];
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001D32 RID: 7474 RVA: 0x0008AFC5 File Offset: 0x000891C5
		// (set) Token: 0x06001D33 RID: 7475 RVA: 0x0008AFD8 File Offset: 0x000891D8
		[ConfigurationProperty("enabled", DefaultValue = true)]
		public bool Enabled
		{
			get
			{
				return (bool)base[this.enabled];
			}
			set
			{
				base[this.enabled] = value;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06001D34 RID: 7476 RVA: 0x0008AFEC File Offset: 0x000891EC
		// (set) Token: 0x06001D35 RID: 7477 RVA: 0x0008AFFF File Offset: 0x000891FF
		[ConfigurationProperty("useDefaultCredentials", DefaultValue = false)]
		public bool UseDefaultCredentials
		{
			get
			{
				return (bool)base[this.useDefaultCredentials];
			}
			set
			{
				base[this.useDefaultCredentials] = value;
			}
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x0008B014 File Offset: 0x00089214
		protected override void Reset(ConfigurationElement parentElement)
		{
			DefaultProxySection defaultProxySection = new DefaultProxySection();
			defaultProxySection.InitializeDefault();
			base.Reset(defaultProxySection);
		}

		// Token: 0x04001C20 RID: 7200
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C21 RID: 7201
		private readonly ConfigurationProperty bypasslist = new ConfigurationProperty("bypasslist", typeof(BypassElementCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C22 RID: 7202
		private readonly ConfigurationProperty module = new ConfigurationProperty("module", typeof(ModuleElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C23 RID: 7203
		private readonly ConfigurationProperty proxy = new ConfigurationProperty("proxy", typeof(ProxyElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C24 RID: 7204
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001C25 RID: 7205
		private readonly ConfigurationProperty useDefaultCredentials = new ConfigurationProperty("useDefaultCredentials", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
