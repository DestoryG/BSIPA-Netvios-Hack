using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200033B RID: 827
	public sealed class PerformanceCountersElement : ConfigurationElement
	{
		// Token: 0x06001D80 RID: 7552 RVA: 0x0008BDDC File Offset: 0x00089FDC
		public PerformanceCountersElement()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x0008BE2C File Offset: 0x0008A02C
		// (set) Token: 0x06001D82 RID: 7554 RVA: 0x0008BE3F File Offset: 0x0008A03F
		[ConfigurationProperty("enabled", DefaultValue = false)]
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

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06001D83 RID: 7555 RVA: 0x0008BE53 File Offset: 0x0008A053
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04001C46 RID: 7238
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C47 RID: 7239
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
