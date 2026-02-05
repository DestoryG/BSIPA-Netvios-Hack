using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200032D RID: 813
	public sealed class ConnectionManagementSection : ConfigurationSection
	{
		// Token: 0x06001D25 RID: 7461 RVA: 0x0008AC8D File Offset: 0x00088E8D
		public ConnectionManagementSection()
		{
			this.properties.Add(this.connectionManagement);
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06001D26 RID: 7462 RVA: 0x0008ACC9 File Offset: 0x00088EC9
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public ConnectionManagementElementCollection ConnectionManagement
		{
			get
			{
				return (ConnectionManagementElementCollection)base[this.connectionManagement];
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001D27 RID: 7463 RVA: 0x0008ACDC File Offset: 0x00088EDC
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04001C1C RID: 7196
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C1D RID: 7197
		private readonly ConfigurationProperty connectionManagement = new ConfigurationProperty(null, typeof(ConnectionManagementElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
