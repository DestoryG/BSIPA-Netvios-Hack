using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000336 RID: 822
	public sealed class Ipv6Element : ConfigurationElement
	{
		// Token: 0x06001D6A RID: 7530 RVA: 0x0008BBE8 File Offset: 0x00089DE8
		public Ipv6Element()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x0008BC38 File Offset: 0x00089E38
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x0008BC40 File Offset: 0x00089E40
		// (set) Token: 0x06001D6D RID: 7533 RVA: 0x0008BC53 File Offset: 0x00089E53
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

		// Token: 0x04001C41 RID: 7233
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C42 RID: 7234
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
