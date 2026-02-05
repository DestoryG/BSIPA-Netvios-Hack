using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Net.Mail;

namespace System.Net.Configuration
{
	// Token: 0x02000342 RID: 834
	public sealed class SmtpSection : ConfigurationSection
	{
		// Token: 0x06001DED RID: 7661 RVA: 0x0008CEC4 File Offset: 0x0008B0C4
		public SmtpSection()
		{
			this.properties.Add(this.deliveryMethod);
			this.properties.Add(this.deliveryFormat);
			this.properties.Add(this.from);
			this.properties.Add(this.network);
			this.properties.Add(this.specifiedPickupDirectory);
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x0008CFD9 File Offset: 0x0008B1D9
		// (set) Token: 0x06001DEF RID: 7663 RVA: 0x0008CFEC File Offset: 0x0008B1EC
		[ConfigurationProperty("deliveryMethod", DefaultValue = SmtpDeliveryMethod.Network)]
		public SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return (SmtpDeliveryMethod)base[this.deliveryMethod];
			}
			set
			{
				base[this.deliveryMethod] = value;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x0008D000 File Offset: 0x0008B200
		// (set) Token: 0x06001DF1 RID: 7665 RVA: 0x0008D013 File Offset: 0x0008B213
		[ConfigurationProperty("deliveryFormat", DefaultValue = SmtpDeliveryFormat.SevenBit)]
		public SmtpDeliveryFormat DeliveryFormat
		{
			get
			{
				return (SmtpDeliveryFormat)base[this.deliveryFormat];
			}
			set
			{
				base[this.deliveryFormat] = value;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x0008D027 File Offset: 0x0008B227
		// (set) Token: 0x06001DF3 RID: 7667 RVA: 0x0008D03A File Offset: 0x0008B23A
		[ConfigurationProperty("from")]
		public string From
		{
			get
			{
				return (string)base[this.from];
			}
			set
			{
				base[this.from] = value;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x0008D049 File Offset: 0x0008B249
		[ConfigurationProperty("network")]
		public SmtpNetworkElement Network
		{
			get
			{
				return (SmtpNetworkElement)base[this.network];
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06001DF5 RID: 7669 RVA: 0x0008D05C File Offset: 0x0008B25C
		[ConfigurationProperty("specifiedPickupDirectory")]
		public SmtpSpecifiedPickupDirectoryElement SpecifiedPickupDirectory
		{
			get
			{
				return (SmtpSpecifiedPickupDirectoryElement)base[this.specifiedPickupDirectory];
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x0008D06F File Offset: 0x0008B26F
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04001C8A RID: 7306
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C8B RID: 7307
		private readonly ConfigurationProperty from = new ConfigurationProperty("from", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C8C RID: 7308
		private readonly ConfigurationProperty network = new ConfigurationProperty("network", typeof(SmtpNetworkElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C8D RID: 7309
		private readonly ConfigurationProperty specifiedPickupDirectory = new ConfigurationProperty("specifiedPickupDirectory", typeof(SmtpSpecifiedPickupDirectoryElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C8E RID: 7310
		private readonly ConfigurationProperty deliveryMethod = new ConfigurationProperty("deliveryMethod", typeof(SmtpDeliveryMethod), SmtpDeliveryMethod.Network, new SmtpSection.SmtpDeliveryMethodTypeConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C8F RID: 7311
		private readonly ConfigurationProperty deliveryFormat = new ConfigurationProperty("deliveryFormat", typeof(SmtpDeliveryFormat), SmtpDeliveryFormat.SevenBit, new SmtpSection.SmtpDeliveryFormatTypeConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x020007C5 RID: 1989
		private class SmtpDeliveryMethodTypeConverter : TypeConverter
		{
			// Token: 0x06004383 RID: 17283 RVA: 0x0011CBBD File Offset: 0x0011ADBD
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06004384 RID: 17284 RVA: 0x0011CBDC File Offset: 0x0011ADDC
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				if (text != null)
				{
					text = text.ToLower(CultureInfo.InvariantCulture);
					if (text == "network")
					{
						return SmtpDeliveryMethod.Network;
					}
					if (text == "specifiedpickupdirectory")
					{
						return SmtpDeliveryMethod.SpecifiedPickupDirectory;
					}
					if (text == "pickupdirectoryfromiis")
					{
						return SmtpDeliveryMethod.PickupDirectoryFromIis;
					}
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		// Token: 0x020007C6 RID: 1990
		private class SmtpDeliveryFormatTypeConverter : TypeConverter
		{
			// Token: 0x06004386 RID: 17286 RVA: 0x0011CC4E File Offset: 0x0011AE4E
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06004387 RID: 17287 RVA: 0x0011CC6C File Offset: 0x0011AE6C
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				if (text != null)
				{
					text = text.ToLower(CultureInfo.InvariantCulture);
					if (text == "sevenbit")
					{
						return SmtpDeliveryFormat.SevenBit;
					}
					if (text == "international")
					{
						return SmtpDeliveryFormat.International;
					}
				}
				return base.ConvertFrom(context, culture, value);
			}
		}
	}
}
