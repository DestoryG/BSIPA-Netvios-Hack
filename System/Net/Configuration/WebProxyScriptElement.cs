using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000349 RID: 841
	public sealed class WebProxyScriptElement : ConfigurationElement
	{
		// Token: 0x06001E28 RID: 7720 RVA: 0x0008D7A4 File Offset: 0x0008B9A4
		public WebProxyScriptElement()
		{
			this.properties.Add(this.autoConfigUrlRetryInterval);
			this.properties.Add(this.downloadTimeout);
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x0008D854 File Offset: 0x0008BA54
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_element_permission", new object[] { "webProxyScript" }), ex);
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06001E2A RID: 7722 RVA: 0x0008D8AC File Offset: 0x0008BAAC
		// (set) Token: 0x06001E2B RID: 7723 RVA: 0x0008D8BF File Offset: 0x0008BABF
		[ConfigurationProperty("autoConfigUrlRetryInterval", DefaultValue = 600)]
		public int AutoConfigUrlRetryInterval
		{
			get
			{
				return (int)base[this.autoConfigUrlRetryInterval];
			}
			set
			{
				base[this.autoConfigUrlRetryInterval] = value;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06001E2C RID: 7724 RVA: 0x0008D8D3 File Offset: 0x0008BAD3
		// (set) Token: 0x06001E2D RID: 7725 RVA: 0x0008D8E6 File Offset: 0x0008BAE6
		[ConfigurationProperty("downloadTimeout", DefaultValue = "00:01:00")]
		public TimeSpan DownloadTimeout
		{
			get
			{
				return (TimeSpan)base[this.downloadTimeout];
			}
			set
			{
				base[this.downloadTimeout] = value;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06001E2E RID: 7726 RVA: 0x0008D8FA File Offset: 0x0008BAFA
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04001CAC RID: 7340
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001CAD RID: 7341
		private readonly ConfigurationProperty autoConfigUrlRetryInterval = new ConfigurationProperty("autoConfigUrlRetryInterval", typeof(int), 600, null, new WebProxyScriptElement.RetryIntervalValidator(), ConfigurationPropertyOptions.None);

		// Token: 0x04001CAE RID: 7342
		private readonly ConfigurationProperty downloadTimeout = new ConfigurationProperty("downloadTimeout", typeof(TimeSpan), TimeSpan.FromMinutes(1.0), null, new TimeSpanValidator(new TimeSpan(0, 0, 0), TimeSpan.MaxValue, false), ConfigurationPropertyOptions.None);

		// Token: 0x020007C7 RID: 1991
		private class RetryIntervalValidator : ConfigurationValidatorBase
		{
			// Token: 0x06004389 RID: 17289 RVA: 0x0011CCCA File Offset: 0x0011AECA
			public override bool CanValidate(Type type)
			{
				return type == typeof(int);
			}

			// Token: 0x0600438A RID: 17290 RVA: 0x0011CCDC File Offset: 0x0011AEDC
			public override void Validate(object value)
			{
				int num = (int)value;
				if (num < 0)
				{
					throw new ArgumentOutOfRangeException("value", num, SR.GetString("ArgumentOutOfRange_Bounds_Lower_Upper", new object[] { 0, int.MaxValue }));
				}
			}
		}
	}
}
