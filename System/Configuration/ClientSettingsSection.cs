using System;

namespace System.Configuration
{
	// Token: 0x020000B7 RID: 183
	public sealed class ClientSettingsSection : ConfigurationSection
	{
		// Token: 0x0600061A RID: 1562 RVA: 0x00023E2A File Offset: 0x0002202A
		static ClientSettingsSection()
		{
			ClientSettingsSection._properties.Add(ClientSettingsSection._propSettings);
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00023E64 File Offset: 0x00022064
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ClientSettingsSection._properties;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x00023E6B File Offset: 0x0002206B
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public SettingElementCollection Settings
		{
			get
			{
				return (SettingElementCollection)base[ClientSettingsSection._propSettings];
			}
		}

		// Token: 0x04000C63 RID: 3171
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04000C64 RID: 3172
		private static readonly ConfigurationProperty _propSettings = new ConfigurationProperty(null, typeof(SettingElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
