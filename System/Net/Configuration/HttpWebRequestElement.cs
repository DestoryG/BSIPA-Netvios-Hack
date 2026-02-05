using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000331 RID: 817
	public sealed class HttpWebRequestElement : ConfigurationElement
	{
		// Token: 0x06001D3B RID: 7483 RVA: 0x0008B458 File Offset: 0x00089658
		public HttpWebRequestElement()
		{
			this.properties.Add(this.maximumResponseHeadersLength);
			this.properties.Add(this.maximumErrorResponseLength);
			this.properties.Add(this.maximumUnauthorizedUploadLength);
			this.properties.Add(this.useUnsafeHeaderParsing);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x0008B540 File Offset: 0x00089740
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			PropertyInformation[] array = new PropertyInformation[]
			{
				base.ElementInformation.Properties["maximumResponseHeadersLength"],
				base.ElementInformation.Properties["maximumErrorResponseLength"]
			};
			foreach (PropertyInformation propertyInformation in array)
			{
				if (propertyInformation.ValueOrigin == PropertyValueOrigin.SetHere)
				{
					try
					{
						ExceptionHelper.WebPermissionUnrestricted.Demand();
					}
					catch (Exception ex)
					{
						throw new ConfigurationErrorsException(SR.GetString("net_config_property_permission", new object[] { propertyInformation.Name }), ex);
					}
				}
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001D3D RID: 7485 RVA: 0x0008B5F0 File Offset: 0x000897F0
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001D3E RID: 7486 RVA: 0x0008B5F8 File Offset: 0x000897F8
		// (set) Token: 0x06001D3F RID: 7487 RVA: 0x0008B60B File Offset: 0x0008980B
		[ConfigurationProperty("maximumUnauthorizedUploadLength", DefaultValue = -1)]
		public int MaximumUnauthorizedUploadLength
		{
			get
			{
				return (int)base[this.maximumUnauthorizedUploadLength];
			}
			set
			{
				base[this.maximumUnauthorizedUploadLength] = value;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001D40 RID: 7488 RVA: 0x0008B61F File Offset: 0x0008981F
		// (set) Token: 0x06001D41 RID: 7489 RVA: 0x0008B632 File Offset: 0x00089832
		[ConfigurationProperty("maximumErrorResponseLength", DefaultValue = 64)]
		public int MaximumErrorResponseLength
		{
			get
			{
				return (int)base[this.maximumErrorResponseLength];
			}
			set
			{
				base[this.maximumErrorResponseLength] = value;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001D42 RID: 7490 RVA: 0x0008B646 File Offset: 0x00089846
		// (set) Token: 0x06001D43 RID: 7491 RVA: 0x0008B659 File Offset: 0x00089859
		[ConfigurationProperty("maximumResponseHeadersLength", DefaultValue = 64)]
		public int MaximumResponseHeadersLength
		{
			get
			{
				return (int)base[this.maximumResponseHeadersLength];
			}
			set
			{
				base[this.maximumResponseHeadersLength] = value;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06001D44 RID: 7492 RVA: 0x0008B66D File Offset: 0x0008986D
		// (set) Token: 0x06001D45 RID: 7493 RVA: 0x0008B680 File Offset: 0x00089880
		[ConfigurationProperty("useUnsafeHeaderParsing", DefaultValue = false)]
		public bool UseUnsafeHeaderParsing
		{
			get
			{
				return (bool)base[this.useUnsafeHeaderParsing];
			}
			set
			{
				base[this.useUnsafeHeaderParsing] = value;
			}
		}

		// Token: 0x04001C28 RID: 7208
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C29 RID: 7209
		private readonly ConfigurationProperty maximumResponseHeadersLength = new ConfigurationProperty("maximumResponseHeadersLength", typeof(int), 64, ConfigurationPropertyOptions.None);

		// Token: 0x04001C2A RID: 7210
		private readonly ConfigurationProperty maximumErrorResponseLength = new ConfigurationProperty("maximumErrorResponseLength", typeof(int), 64, ConfigurationPropertyOptions.None);

		// Token: 0x04001C2B RID: 7211
		private readonly ConfigurationProperty maximumUnauthorizedUploadLength = new ConfigurationProperty("maximumUnauthorizedUploadLength", typeof(int), -1, ConfigurationPropertyOptions.None);

		// Token: 0x04001C2C RID: 7212
		private readonly ConfigurationProperty useUnsafeHeaderParsing = new ConfigurationProperty("useUnsafeHeaderParsing", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
