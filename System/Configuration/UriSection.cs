using System;

namespace System.Configuration
{
	// Token: 0x02000075 RID: 117
	public sealed class UriSection : ConfigurationSection
	{
		// Token: 0x060004B4 RID: 1204 RVA: 0x0001FA8C File Offset: 0x0001DC8C
		static UriSection()
		{
			UriSection.properties.Add(UriSection.idn);
			UriSection.properties.Add(UriSection.iriParsing);
			UriSection.properties.Add(UriSection.schemeSettings);
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0001FB21 File Offset: 0x0001DD21
		[ConfigurationProperty("idn")]
		public IdnElement Idn
		{
			get
			{
				return (IdnElement)base[UriSection.idn];
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x0001FB33 File Offset: 0x0001DD33
		[ConfigurationProperty("iriParsing")]
		public IriParsingElement IriParsing
		{
			get
			{
				return (IriParsingElement)base[UriSection.iriParsing];
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0001FB45 File Offset: 0x0001DD45
		[ConfigurationProperty("schemeSettings")]
		public SchemeSettingElementCollection SchemeSettings
		{
			get
			{
				return (SchemeSettingElementCollection)base[UriSection.schemeSettings];
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0001FB57 File Offset: 0x0001DD57
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return UriSection.properties;
			}
		}

		// Token: 0x04000BF3 RID: 3059
		private static readonly ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04000BF4 RID: 3060
		private static readonly ConfigurationProperty idn = new ConfigurationProperty("idn", typeof(IdnElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000BF5 RID: 3061
		private static readonly ConfigurationProperty iriParsing = new ConfigurationProperty("iriParsing", typeof(IriParsingElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000BF6 RID: 3062
		private static readonly ConfigurationProperty schemeSettings = new ConfigurationProperty("schemeSettings", typeof(SchemeSettingElementCollection), null, ConfigurationPropertyOptions.None);
	}
}
