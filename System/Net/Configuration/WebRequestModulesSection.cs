using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200034C RID: 844
	public sealed class WebRequestModulesSection : ConfigurationSection
	{
		// Token: 0x06001E45 RID: 7749 RVA: 0x0008DAEB File Offset: 0x0008BCEB
		public WebRequestModulesSection()
		{
			this.properties.Add(this.webRequestModules);
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x0008DB28 File Offset: 0x0008BD28
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
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", new object[] { "webRequestModules" }), ex);
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0008DB80 File Offset: 0x0008BD80
		protected override void InitializeDefault()
		{
			this.WebRequestModules.Add(new WebRequestModuleElement("https:", typeof(HttpRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("http:", typeof(HttpRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("file:", typeof(FileWebRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("ftp:", typeof(FtpWebRequestCreator)));
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x0008DC09 File Offset: 0x0008BE09
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06001E49 RID: 7753 RVA: 0x0008DC11 File Offset: 0x0008BE11
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public WebRequestModuleElementCollection WebRequestModules
		{
			get
			{
				return (WebRequestModuleElementCollection)base[this.webRequestModules];
			}
		}

		// Token: 0x04001CB2 RID: 7346
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001CB3 RID: 7347
		private readonly ConfigurationProperty webRequestModules = new ConfigurationProperty(null, typeof(WebRequestModuleElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
