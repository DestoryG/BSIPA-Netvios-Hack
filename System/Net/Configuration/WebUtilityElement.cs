using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

namespace System.Net.Configuration
{
	// Token: 0x0200034E RID: 846
	public sealed class WebUtilityElement : ConfigurationElement
	{
		// Token: 0x06001E4E RID: 7758 RVA: 0x0008DD90 File Offset: 0x0008BF90
		public WebUtilityElement()
		{
			this.properties.Add(this.unicodeDecodingConformance);
			this.properties.Add(this.unicodeEncodingConformance);
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06001E4F RID: 7759 RVA: 0x0008DE1E File Offset: 0x0008C01E
		// (set) Token: 0x06001E50 RID: 7760 RVA: 0x0008DE31 File Offset: 0x0008C031
		[ConfigurationProperty("unicodeDecodingConformance", DefaultValue = UnicodeDecodingConformance.Auto)]
		public UnicodeDecodingConformance UnicodeDecodingConformance
		{
			get
			{
				return (UnicodeDecodingConformance)base[this.unicodeDecodingConformance];
			}
			set
			{
				base[this.unicodeDecodingConformance] = value;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x0008DE45 File Offset: 0x0008C045
		// (set) Token: 0x06001E52 RID: 7762 RVA: 0x0008DE58 File Offset: 0x0008C058
		[ConfigurationProperty("unicodeEncodingConformance", DefaultValue = UnicodeEncodingConformance.Auto)]
		public UnicodeEncodingConformance UnicodeEncodingConformance
		{
			get
			{
				return (UnicodeEncodingConformance)base[this.unicodeEncodingConformance];
			}
			set
			{
				base[this.unicodeEncodingConformance] = value;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06001E53 RID: 7763 RVA: 0x0008DE6C File Offset: 0x0008C06C
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04001CB6 RID: 7350
		private readonly ConfigurationProperty unicodeDecodingConformance = new ConfigurationProperty("unicodeDecodingConformance", typeof(UnicodeDecodingConformance), UnicodeDecodingConformance.Auto, new WebUtilityElement.EnumTypeConverter<UnicodeDecodingConformance>(), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001CB7 RID: 7351
		private readonly ConfigurationProperty unicodeEncodingConformance = new ConfigurationProperty("unicodeEncodingConformance", typeof(UnicodeEncodingConformance), UnicodeEncodingConformance.Auto, new WebUtilityElement.EnumTypeConverter<UnicodeEncodingConformance>(), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001CB8 RID: 7352
		private readonly ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x020007CA RID: 1994
		private class EnumTypeConverter<TEnum> : TypeConverter where TEnum : struct
		{
			// Token: 0x06004394 RID: 17300 RVA: 0x0011CE19 File Offset: 0x0011B019
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06004395 RID: 17301 RVA: 0x0011CE38 File Offset: 0x0011B038
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				TEnum tenum;
				if (text != null && Enum.TryParse<TEnum>(text, true, out tenum))
				{
					return tenum;
				}
				return base.ConvertFrom(context, culture, value);
			}
		}
	}
}
