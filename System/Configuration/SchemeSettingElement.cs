using System;

namespace System.Configuration
{
	// Token: 0x02000070 RID: 112
	public sealed class SchemeSettingElement : ConfigurationElement
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0001F448 File Offset: 0x0001D648
		static SchemeSettingElement()
		{
			SchemeSettingElement.properties.Add(SchemeSettingElement.name);
			SchemeSettingElement.properties.Add(SchemeSettingElement.genericUriParserOptions);
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x0001F4B8 File Offset: 0x0001D6B8
		[ConfigurationProperty("name", DefaultValue = null, IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base[SchemeSettingElement.name];
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0001F4CA File Offset: 0x0001D6CA
		[ConfigurationProperty("genericUriParserOptions", DefaultValue = ConfigurationPropertyOptions.None, IsRequired = true)]
		public GenericUriParserOptions GenericUriParserOptions
		{
			get
			{
				return (GenericUriParserOptions)base[SchemeSettingElement.genericUriParserOptions];
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x0001F4DC File Offset: 0x0001D6DC
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SchemeSettingElement.properties;
			}
		}

		// Token: 0x04000BE4 RID: 3044
		private static readonly ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04000BE5 RID: 3045
		private static readonly ConfigurationProperty name = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04000BE6 RID: 3046
		private static readonly ConfigurationProperty genericUriParserOptions = new ConfigurationProperty("genericUriParserOptions", typeof(GenericUriParserOptions), GenericUriParserOptions.Default, ConfigurationPropertyOptions.IsRequired);
	}
}
