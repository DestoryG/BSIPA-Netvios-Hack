using System;
using System.Configuration;
using System.Net.Mail;

namespace System.Net.Configuration
{
	// Token: 0x02000344 RID: 836
	public sealed class SmtpNetworkElement : ConfigurationElement
	{
		// Token: 0x06001DFF RID: 7679 RVA: 0x0008D170 File Offset: 0x0008B370
		public SmtpNetworkElement()
		{
			this.properties.Add(this.defaultCredentials);
			this.properties.Add(this.host);
			this.properties.Add(this.clientDomain);
			this.properties.Add(this.password);
			this.properties.Add(this.port);
			this.properties.Add(this.userName);
			this.properties.Add(this.targetName);
			this.properties.Add(this.enableSsl);
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0008D314 File Offset: 0x0008B514
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			PropertyInformation propertyInformation = base.ElementInformation.Properties["port"];
			if (propertyInformation.ValueOrigin == PropertyValueOrigin.SetHere && (int)propertyInformation.Value != (int)propertyInformation.DefaultValue)
			{
				try
				{
					new SmtpPermission(SmtpAccess.ConnectToUnrestrictedPort).Demand();
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("net_config_property_permission", new object[] { propertyInformation.Name }), ex);
				}
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06001E01 RID: 7681 RVA: 0x0008D3A4 File Offset: 0x0008B5A4
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06001E02 RID: 7682 RVA: 0x0008D3AC File Offset: 0x0008B5AC
		// (set) Token: 0x06001E03 RID: 7683 RVA: 0x0008D3BF File Offset: 0x0008B5BF
		[ConfigurationProperty("defaultCredentials", DefaultValue = false)]
		public bool DefaultCredentials
		{
			get
			{
				return (bool)base[this.defaultCredentials];
			}
			set
			{
				base[this.defaultCredentials] = value;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06001E04 RID: 7684 RVA: 0x0008D3D3 File Offset: 0x0008B5D3
		// (set) Token: 0x06001E05 RID: 7685 RVA: 0x0008D3E6 File Offset: 0x0008B5E6
		[ConfigurationProperty("host")]
		public string Host
		{
			get
			{
				return (string)base[this.host];
			}
			set
			{
				base[this.host] = value;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06001E06 RID: 7686 RVA: 0x0008D3F5 File Offset: 0x0008B5F5
		// (set) Token: 0x06001E07 RID: 7687 RVA: 0x0008D408 File Offset: 0x0008B608
		[ConfigurationProperty("targetName")]
		public string TargetName
		{
			get
			{
				return (string)base[this.targetName];
			}
			set
			{
				base[this.targetName] = value;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x0008D417 File Offset: 0x0008B617
		// (set) Token: 0x06001E09 RID: 7689 RVA: 0x0008D42A File Offset: 0x0008B62A
		[ConfigurationProperty("clientDomain")]
		public string ClientDomain
		{
			get
			{
				return (string)base[this.clientDomain];
			}
			set
			{
				base[this.clientDomain] = value;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x0008D439 File Offset: 0x0008B639
		// (set) Token: 0x06001E0B RID: 7691 RVA: 0x0008D44C File Offset: 0x0008B64C
		[ConfigurationProperty("password")]
		public string Password
		{
			get
			{
				return (string)base[this.password];
			}
			set
			{
				base[this.password] = value;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x0008D45B File Offset: 0x0008B65B
		// (set) Token: 0x06001E0D RID: 7693 RVA: 0x0008D46E File Offset: 0x0008B66E
		[ConfigurationProperty("port", DefaultValue = 25)]
		public int Port
		{
			get
			{
				return (int)base[this.port];
			}
			set
			{
				base[this.port] = value;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06001E0E RID: 7694 RVA: 0x0008D482 File Offset: 0x0008B682
		// (set) Token: 0x06001E0F RID: 7695 RVA: 0x0008D495 File Offset: 0x0008B695
		[ConfigurationProperty("userName")]
		public string UserName
		{
			get
			{
				return (string)base[this.userName];
			}
			set
			{
				base[this.userName] = value;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06001E10 RID: 7696 RVA: 0x0008D4A4 File Offset: 0x0008B6A4
		// (set) Token: 0x06001E11 RID: 7697 RVA: 0x0008D4B7 File Offset: 0x0008B6B7
		[ConfigurationProperty("enableSsl", DefaultValue = false)]
		public bool EnableSsl
		{
			get
			{
				return (bool)base[this.enableSsl];
			}
			set
			{
				base[this.enableSsl] = value;
			}
		}

		// Token: 0x04001C96 RID: 7318
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C97 RID: 7319
		private readonly ConfigurationProperty defaultCredentials = new ConfigurationProperty("defaultCredentials", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001C98 RID: 7320
		private readonly ConfigurationProperty host = new ConfigurationProperty("host", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C99 RID: 7321
		private readonly ConfigurationProperty clientDomain = new ConfigurationProperty("clientDomain", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C9A RID: 7322
		private readonly ConfigurationProperty password = new ConfigurationProperty("password", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C9B RID: 7323
		private readonly ConfigurationProperty port = new ConfigurationProperty("port", typeof(int), 25, null, new IntegerValidator(1, 65535), ConfigurationPropertyOptions.None);

		// Token: 0x04001C9C RID: 7324
		private readonly ConfigurationProperty userName = new ConfigurationProperty("userName", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C9D RID: 7325
		private readonly ConfigurationProperty targetName = new ConfigurationProperty("targetName", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C9E RID: 7326
		private readonly ConfigurationProperty enableSsl = new ConfigurationProperty("enableSsl", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
