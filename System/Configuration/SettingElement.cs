using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020000B9 RID: 185
	public sealed class SettingElement : ConfigurationElement
	{
		// Token: 0x06000627 RID: 1575 RVA: 0x00023ED4 File Offset: 0x000220D4
		static SettingElement()
		{
			SettingElement._properties.Add(SettingElement._propName);
			SettingElement._properties.Add(SettingElement._propSerializeAs);
			SettingElement._properties.Add(SettingElement._propValue);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00023F7C File Offset: 0x0002217C
		public SettingElement()
		{
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00023F84 File Offset: 0x00022184
		public SettingElement(string name, SettingsSerializeAs serializeAs)
			: this()
		{
			this.Name = name;
			this.SerializeAs = serializeAs;
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00023F9A File Offset: 0x0002219A
		internal string Key
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00023FA4 File Offset: 0x000221A4
		public override bool Equals(object settings)
		{
			SettingElement settingElement = settings as SettingElement;
			return settingElement != null && base.Equals(settings) && object.Equals(settingElement.Value, this.Value);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00023FD7 File Offset: 0x000221D7
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.Value.GetHashCode();
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00023FEB File Offset: 0x000221EB
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SettingElement._properties;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00023FF2 File Offset: 0x000221F2
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x00024004 File Offset: 0x00022204
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string Name
		{
			get
			{
				return (string)base[SettingElement._propName];
			}
			set
			{
				base[SettingElement._propName] = value;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00024012 File Offset: 0x00022212
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x00024024 File Offset: 0x00022224
		[ConfigurationProperty("serializeAs", IsRequired = true, DefaultValue = SettingsSerializeAs.String)]
		public SettingsSerializeAs SerializeAs
		{
			get
			{
				return (SettingsSerializeAs)base[SettingElement._propSerializeAs];
			}
			set
			{
				base[SettingElement._propSerializeAs] = value;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00024037 File Offset: 0x00022237
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x00024049 File Offset: 0x00022249
		[ConfigurationProperty("value", IsRequired = true, DefaultValue = null)]
		public SettingValueElement Value
		{
			get
			{
				return (SettingValueElement)base[SettingElement._propValue];
			}
			set
			{
				base[SettingElement._propValue] = value;
			}
		}

		// Token: 0x04000C65 RID: 3173
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04000C66 RID: 3174
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04000C67 RID: 3175
		private static readonly ConfigurationProperty _propSerializeAs = new ConfigurationProperty("serializeAs", typeof(SettingsSerializeAs), SettingsSerializeAs.String, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04000C68 RID: 3176
		private static readonly ConfigurationProperty _propValue = new ConfigurationProperty("value", typeof(SettingValueElement), null, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04000C69 RID: 3177
		private static XmlDocument doc = new XmlDocument();
	}
}
