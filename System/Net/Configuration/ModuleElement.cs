using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000339 RID: 825
	public sealed class ModuleElement : ConfigurationElement
	{
		// Token: 0x06001D73 RID: 7539 RVA: 0x0008BCA8 File Offset: 0x00089EA8
		public ModuleElement()
		{
			this.properties.Add(this.type);
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06001D74 RID: 7540 RVA: 0x0008BCE8 File Offset: 0x00089EE8
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x0008BCF0 File Offset: 0x00089EF0
		// (set) Token: 0x06001D76 RID: 7542 RVA: 0x0008BD03 File Offset: 0x00089F03
		[ConfigurationProperty("type")]
		public string Type
		{
			get
			{
				return (string)base[this.type];
			}
			set
			{
				base[this.type] = value;
			}
		}

		// Token: 0x04001C44 RID: 7236
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C45 RID: 7237
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}
