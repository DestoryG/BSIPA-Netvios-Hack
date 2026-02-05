using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x02000078 RID: 120
	public sealed class IdnElement : ConfigurationElement
	{
		// Token: 0x060004C9 RID: 1225 RVA: 0x0001FF1C File Offset: 0x0001E11C
		public IdnElement()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0001FF72 File Offset: 0x0001E172
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0001FF7A File Offset: 0x0001E17A
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x0001FF8D File Offset: 0x0001E18D
		[ConfigurationProperty("enabled", DefaultValue = UriIdnScope.None)]
		public UriIdnScope Enabled
		{
			get
			{
				return (UriIdnScope)base[this.enabled];
			}
			set
			{
				base[this.enabled] = value;
			}
		}

		// Token: 0x04000BFE RID: 3070
		internal const UriIdnScope EnabledDefaultValue = UriIdnScope.None;

		// Token: 0x04000BFF RID: 3071
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04000C00 RID: 3072
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(UriIdnScope), UriIdnScope.None, new IdnElement.UriIdnScopeTypeConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x020006E7 RID: 1767
		private class UriIdnScopeTypeConverter : TypeConverter
		{
			// Token: 0x06004040 RID: 16448 RVA: 0x0010DD2C File Offset: 0x0010BF2C
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06004041 RID: 16449 RVA: 0x0010DD4C File Offset: 0x0010BF4C
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				if (text != null)
				{
					text = text.ToLower(CultureInfo.InvariantCulture);
					if (text == "all")
					{
						return UriIdnScope.All;
					}
					if (text == "none")
					{
						return UriIdnScope.None;
					}
					if (text == "allexceptintranet")
					{
						return UriIdnScope.AllExceptIntranet;
					}
				}
				return base.ConvertFrom(context, culture, value);
			}
		}
	}
}
