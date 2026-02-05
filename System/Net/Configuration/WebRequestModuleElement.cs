using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

namespace System.Net.Configuration
{
	// Token: 0x0200034A RID: 842
	public sealed class WebRequestModuleElement : ConfigurationElement
	{
		// Token: 0x06001E2F RID: 7727 RVA: 0x0008D904 File Offset: 0x0008BB04
		public WebRequestModuleElement()
		{
			this.properties.Add(this.prefix);
			this.properties.Add(this.type);
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0008D982 File Offset: 0x0008BB82
		public WebRequestModuleElement(string prefix, string type)
			: this()
		{
			this.Prefix = prefix;
			base[this.type] = new WebRequestModuleElement.TypeAndName(type);
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x0008D9A3 File Offset: 0x0008BBA3
		public WebRequestModuleElement(string prefix, Type type)
			: this()
		{
			this.Prefix = prefix;
			this.Type = type;
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06001E32 RID: 7730 RVA: 0x0008D9B9 File Offset: 0x0008BBB9
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06001E33 RID: 7731 RVA: 0x0008D9C1 File Offset: 0x0008BBC1
		// (set) Token: 0x06001E34 RID: 7732 RVA: 0x0008D9D4 File Offset: 0x0008BBD4
		[ConfigurationProperty("prefix", IsRequired = true, IsKey = true)]
		public string Prefix
		{
			get
			{
				return (string)base[this.prefix];
			}
			set
			{
				base[this.prefix] = value;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x0008D9E4 File Offset: 0x0008BBE4
		// (set) Token: 0x06001E36 RID: 7734 RVA: 0x0008DA0E File Offset: 0x0008BC0E
		[ConfigurationProperty("type")]
		[TypeConverter(typeof(WebRequestModuleElement.TypeTypeConverter))]
		public Type Type
		{
			get
			{
				WebRequestModuleElement.TypeAndName typeAndName = (WebRequestModuleElement.TypeAndName)base[this.type];
				if (typeAndName != null)
				{
					return typeAndName.type;
				}
				return null;
			}
			set
			{
				base[this.type] = new WebRequestModuleElement.TypeAndName(value);
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06001E37 RID: 7735 RVA: 0x0008DA22 File Offset: 0x0008BC22
		internal string Key
		{
			get
			{
				return this.Prefix;
			}
		}

		// Token: 0x04001CAF RID: 7343
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001CB0 RID: 7344
		private readonly ConfigurationProperty prefix = new ConfigurationProperty("prefix", typeof(string), null, ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001CB1 RID: 7345
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(WebRequestModuleElement.TypeAndName), null, new WebRequestModuleElement.TypeTypeConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x020007C8 RID: 1992
		private class TypeAndName
		{
			// Token: 0x0600438C RID: 17292 RVA: 0x0011CD33 File Offset: 0x0011AF33
			public TypeAndName(string name)
			{
				this.type = Type.GetType(name, true, true);
				this.name = name;
			}

			// Token: 0x0600438D RID: 17293 RVA: 0x0011CD50 File Offset: 0x0011AF50
			public TypeAndName(Type type)
			{
				this.type = type;
			}

			// Token: 0x0600438E RID: 17294 RVA: 0x0011CD5F File Offset: 0x0011AF5F
			public override int GetHashCode()
			{
				return this.type.GetHashCode();
			}

			// Token: 0x0600438F RID: 17295 RVA: 0x0011CD6C File Offset: 0x0011AF6C
			public override bool Equals(object comparand)
			{
				return this.type.Equals(((WebRequestModuleElement.TypeAndName)comparand).type);
			}

			// Token: 0x04003473 RID: 13427
			public readonly Type type;

			// Token: 0x04003474 RID: 13428
			public readonly string name;
		}

		// Token: 0x020007C9 RID: 1993
		private class TypeTypeConverter : TypeConverter
		{
			// Token: 0x06004390 RID: 17296 RVA: 0x0011CD84 File Offset: 0x0011AF84
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06004391 RID: 17297 RVA: 0x0011CDA2 File Offset: 0x0011AFA2
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is string)
				{
					return new WebRequestModuleElement.TypeAndName((string)value);
				}
				return base.ConvertFrom(context, culture, value);
			}

			// Token: 0x06004392 RID: 17298 RVA: 0x0011CDC4 File Offset: 0x0011AFC4
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (!(destinationType == typeof(string)))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				WebRequestModuleElement.TypeAndName typeAndName = (WebRequestModuleElement.TypeAndName)value;
				if (typeAndName.name != null)
				{
					return typeAndName.name;
				}
				return typeAndName.type.AssemblyQualifiedName;
			}
		}
	}
}
