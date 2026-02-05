using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000324 RID: 804
	public sealed class AuthenticationModuleElement : ConfigurationElement
	{
		// Token: 0x06001CD7 RID: 7383 RVA: 0x0008A49E File Offset: 0x0008869E
		public AuthenticationModuleElement()
		{
			this.properties.Add(this.type);
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x0008A4DE File Offset: 0x000886DE
		public AuthenticationModuleElement(string typeName)
			: this()
		{
			if (typeName != (string)this.type.DefaultValue)
			{
				this.Type = typeName;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x0008A505 File Offset: 0x00088705
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06001CDA RID: 7386 RVA: 0x0008A50D File Offset: 0x0008870D
		// (set) Token: 0x06001CDB RID: 7387 RVA: 0x0008A520 File Offset: 0x00088720
		[ConfigurationProperty("type", IsRequired = true, IsKey = true)]
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

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06001CDC RID: 7388 RVA: 0x0008A52F File Offset: 0x0008872F
		internal string Key
		{
			get
			{
				return this.Type;
			}
		}

		// Token: 0x04001BB8 RID: 7096
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001BB9 RID: 7097
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(string), null, ConfigurationPropertyOptions.IsKey);
	}
}
