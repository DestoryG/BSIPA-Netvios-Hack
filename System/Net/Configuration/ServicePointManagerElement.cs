using System;
using System.Configuration;
using System.Net.Security;

namespace System.Net.Configuration
{
	// Token: 0x02000341 RID: 833
	public sealed class ServicePointManagerElement : ConfigurationElement
	{
		// Token: 0x06001DDC RID: 7644 RVA: 0x0008CB70 File Offset: 0x0008AD70
		public ServicePointManagerElement()
		{
			this.properties.Add(this.checkCertificateName);
			this.properties.Add(this.checkCertificateRevocationList);
			this.properties.Add(this.dnsRefreshTimeout);
			this.properties.Add(this.enableDnsRoundRobin);
			this.properties.Add(this.encryptionPolicy);
			this.properties.Add(this.expect100Continue);
			this.properties.Add(this.useNagleAlgorithm);
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0008CCF8 File Offset: 0x0008AEF8
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			PropertyInformation[] array = new PropertyInformation[]
			{
				base.ElementInformation.Properties["checkCertificateName"],
				base.ElementInformation.Properties["checkCertificateRevocationList"]
			};
			foreach (PropertyInformation propertyInformation in array)
			{
				if (propertyInformation.ValueOrigin == PropertyValueOrigin.SetHere)
				{
					try
					{
						ExceptionHelper.UnmanagedPermission.Demand();
					}
					catch (Exception ex)
					{
						throw new ConfigurationErrorsException(SR.GetString("net_config_property_permission", new object[] { propertyInformation.Name }), ex);
					}
				}
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001DDE RID: 7646 RVA: 0x0008CDA8 File Offset: 0x0008AFA8
		// (set) Token: 0x06001DDF RID: 7647 RVA: 0x0008CDBB File Offset: 0x0008AFBB
		[ConfigurationProperty("checkCertificateName", DefaultValue = true)]
		public bool CheckCertificateName
		{
			get
			{
				return (bool)base[this.checkCertificateName];
			}
			set
			{
				base[this.checkCertificateName] = value;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x0008CDCF File Offset: 0x0008AFCF
		// (set) Token: 0x06001DE1 RID: 7649 RVA: 0x0008CDE2 File Offset: 0x0008AFE2
		[ConfigurationProperty("checkCertificateRevocationList", DefaultValue = false)]
		public bool CheckCertificateRevocationList
		{
			get
			{
				return (bool)base[this.checkCertificateRevocationList];
			}
			set
			{
				base[this.checkCertificateRevocationList] = value;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x0008CDF6 File Offset: 0x0008AFF6
		// (set) Token: 0x06001DE3 RID: 7651 RVA: 0x0008CE09 File Offset: 0x0008B009
		[ConfigurationProperty("dnsRefreshTimeout", DefaultValue = 120000)]
		public int DnsRefreshTimeout
		{
			get
			{
				return (int)base[this.dnsRefreshTimeout];
			}
			set
			{
				base[this.dnsRefreshTimeout] = value;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x0008CE1D File Offset: 0x0008B01D
		// (set) Token: 0x06001DE5 RID: 7653 RVA: 0x0008CE30 File Offset: 0x0008B030
		[ConfigurationProperty("enableDnsRoundRobin", DefaultValue = false)]
		public bool EnableDnsRoundRobin
		{
			get
			{
				return (bool)base[this.enableDnsRoundRobin];
			}
			set
			{
				base[this.enableDnsRoundRobin] = value;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x0008CE44 File Offset: 0x0008B044
		// (set) Token: 0x06001DE7 RID: 7655 RVA: 0x0008CE57 File Offset: 0x0008B057
		[ConfigurationProperty("encryptionPolicy", DefaultValue = EncryptionPolicy.RequireEncryption)]
		public EncryptionPolicy EncryptionPolicy
		{
			get
			{
				return (EncryptionPolicy)base[this.encryptionPolicy];
			}
			set
			{
				base[this.encryptionPolicy] = value;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06001DE8 RID: 7656 RVA: 0x0008CE6B File Offset: 0x0008B06B
		// (set) Token: 0x06001DE9 RID: 7657 RVA: 0x0008CE7E File Offset: 0x0008B07E
		[ConfigurationProperty("expect100Continue", DefaultValue = true)]
		public bool Expect100Continue
		{
			get
			{
				return (bool)base[this.expect100Continue];
			}
			set
			{
				base[this.expect100Continue] = value;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x0008CE92 File Offset: 0x0008B092
		// (set) Token: 0x06001DEB RID: 7659 RVA: 0x0008CEA5 File Offset: 0x0008B0A5
		[ConfigurationProperty("useNagleAlgorithm", DefaultValue = true)]
		public bool UseNagleAlgorithm
		{
			get
			{
				return (bool)base[this.useNagleAlgorithm];
			}
			set
			{
				base[this.useNagleAlgorithm] = value;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x0008CEB9 File Offset: 0x0008B0B9
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04001C82 RID: 7298
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C83 RID: 7299
		private readonly ConfigurationProperty checkCertificateName = new ConfigurationProperty("checkCertificateName", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001C84 RID: 7300
		private readonly ConfigurationProperty checkCertificateRevocationList = new ConfigurationProperty("checkCertificateRevocationList", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001C85 RID: 7301
		private readonly ConfigurationProperty dnsRefreshTimeout = new ConfigurationProperty("dnsRefreshTimeout", typeof(int), 120000, null, new TimeoutValidator(true), ConfigurationPropertyOptions.None);

		// Token: 0x04001C86 RID: 7302
		private readonly ConfigurationProperty enableDnsRoundRobin = new ConfigurationProperty("enableDnsRoundRobin", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001C87 RID: 7303
		private readonly ConfigurationProperty encryptionPolicy = new ConfigurationProperty("encryptionPolicy", typeof(EncryptionPolicy), EncryptionPolicy.RequireEncryption, ConfigurationPropertyOptions.None);

		// Token: 0x04001C88 RID: 7304
		private readonly ConfigurationProperty expect100Continue = new ConfigurationProperty("expect100Continue", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001C89 RID: 7305
		private readonly ConfigurationProperty useNagleAlgorithm = new ConfigurationProperty("useNagleAlgorithm", typeof(bool), true, ConfigurationPropertyOptions.None);
	}
}
