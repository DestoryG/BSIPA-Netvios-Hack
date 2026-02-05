using System;
using System.Configuration;

namespace System.Security.Authentication.ExtendedProtection.Configuration
{
	// Token: 0x0200044C RID: 1100
	public sealed class ServiceNameElement : ConfigurationElement
	{
		// Token: 0x060028B2 RID: 10418 RVA: 0x000BA8B8 File Offset: 0x000B8AB8
		public ServiceNameElement()
		{
			this.properties.Add(this.name);
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060028B3 RID: 10419 RVA: 0x000BA8F8 File Offset: 0x000B8AF8
		// (set) Token: 0x060028B4 RID: 10420 RVA: 0x000BA90B File Offset: 0x000B8B0B
		[ConfigurationProperty("name")]
		public string Name
		{
			get
			{
				return (string)base[this.name];
			}
			set
			{
				base[this.name] = value;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x000BA91A File Offset: 0x000B8B1A
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x000BA922 File Offset: 0x000B8B22
		internal string Key
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x04002271 RID: 8817
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002272 RID: 8818
		private readonly ConfigurationProperty name = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
	}
}
