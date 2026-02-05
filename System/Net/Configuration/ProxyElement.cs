using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200033C RID: 828
	public sealed class ProxyElement : ConfigurationElement
	{
		// Token: 0x06001D84 RID: 7556 RVA: 0x0008BE5C File Offset: 0x0008A05C
		public ProxyElement()
		{
			this.properties.Add(this.autoDetect);
			this.properties.Add(this.scriptLocation);
			this.properties.Add(this.bypassonlocal);
			this.properties.Add(this.proxyaddress);
			this.properties.Add(this.usesystemdefault);
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06001D85 RID: 7557 RVA: 0x0008BFA8 File Offset: 0x0008A1A8
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06001D86 RID: 7558 RVA: 0x0008BFB0 File Offset: 0x0008A1B0
		// (set) Token: 0x06001D87 RID: 7559 RVA: 0x0008BFC3 File Offset: 0x0008A1C3
		[ConfigurationProperty("autoDetect", DefaultValue = ProxyElement.AutoDetectValues.Unspecified)]
		public ProxyElement.AutoDetectValues AutoDetect
		{
			get
			{
				return (ProxyElement.AutoDetectValues)base[this.autoDetect];
			}
			set
			{
				base[this.autoDetect] = value;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x0008BFD7 File Offset: 0x0008A1D7
		// (set) Token: 0x06001D89 RID: 7561 RVA: 0x0008BFEA File Offset: 0x0008A1EA
		[ConfigurationProperty("scriptLocation")]
		public Uri ScriptLocation
		{
			get
			{
				return (Uri)base[this.scriptLocation];
			}
			set
			{
				base[this.scriptLocation] = value;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x0008BFF9 File Offset: 0x0008A1F9
		// (set) Token: 0x06001D8B RID: 7563 RVA: 0x0008C00C File Offset: 0x0008A20C
		[ConfigurationProperty("bypassonlocal", DefaultValue = ProxyElement.BypassOnLocalValues.Unspecified)]
		public ProxyElement.BypassOnLocalValues BypassOnLocal
		{
			get
			{
				return (ProxyElement.BypassOnLocalValues)base[this.bypassonlocal];
			}
			set
			{
				base[this.bypassonlocal] = value;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x0008C020 File Offset: 0x0008A220
		// (set) Token: 0x06001D8D RID: 7565 RVA: 0x0008C033 File Offset: 0x0008A233
		[ConfigurationProperty("proxyaddress")]
		public Uri ProxyAddress
		{
			get
			{
				return (Uri)base[this.proxyaddress];
			}
			set
			{
				base[this.proxyaddress] = value;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06001D8E RID: 7566 RVA: 0x0008C042 File Offset: 0x0008A242
		// (set) Token: 0x06001D8F RID: 7567 RVA: 0x0008C055 File Offset: 0x0008A255
		[ConfigurationProperty("usesystemdefault", DefaultValue = ProxyElement.UseSystemDefaultValues.Unspecified)]
		public ProxyElement.UseSystemDefaultValues UseSystemDefault
		{
			get
			{
				return (ProxyElement.UseSystemDefaultValues)base[this.usesystemdefault];
			}
			set
			{
				base[this.usesystemdefault] = value;
			}
		}

		// Token: 0x04001C48 RID: 7240
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C49 RID: 7241
		private readonly ConfigurationProperty autoDetect = new ConfigurationProperty("autoDetect", typeof(ProxyElement.AutoDetectValues), ProxyElement.AutoDetectValues.Unspecified, new EnumConverter(typeof(ProxyElement.AutoDetectValues)), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C4A RID: 7242
		private readonly ConfigurationProperty scriptLocation = new ConfigurationProperty("scriptLocation", typeof(Uri), null, new UriTypeConverter(UriKind.Absolute), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C4B RID: 7243
		private readonly ConfigurationProperty bypassonlocal = new ConfigurationProperty("bypassonlocal", typeof(ProxyElement.BypassOnLocalValues), ProxyElement.BypassOnLocalValues.Unspecified, new EnumConverter(typeof(ProxyElement.BypassOnLocalValues)), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C4C RID: 7244
		private readonly ConfigurationProperty proxyaddress = new ConfigurationProperty("proxyaddress", typeof(Uri), null, new UriTypeConverter(UriKind.Absolute), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C4D RID: 7245
		private readonly ConfigurationProperty usesystemdefault = new ConfigurationProperty("usesystemdefault", typeof(ProxyElement.UseSystemDefaultValues), ProxyElement.UseSystemDefaultValues.Unspecified, new EnumConverter(typeof(ProxyElement.UseSystemDefaultValues)), null, ConfigurationPropertyOptions.None);

		// Token: 0x020007C2 RID: 1986
		public enum BypassOnLocalValues
		{
			// Token: 0x04003468 RID: 13416
			Unspecified = -1,
			// Token: 0x04003469 RID: 13417
			False,
			// Token: 0x0400346A RID: 13418
			True
		}

		// Token: 0x020007C3 RID: 1987
		public enum UseSystemDefaultValues
		{
			// Token: 0x0400346C RID: 13420
			Unspecified = -1,
			// Token: 0x0400346D RID: 13421
			False,
			// Token: 0x0400346E RID: 13422
			True
		}

		// Token: 0x020007C4 RID: 1988
		public enum AutoDetectValues
		{
			// Token: 0x04003470 RID: 13424
			Unspecified = -1,
			// Token: 0x04003471 RID: 13425
			False,
			// Token: 0x04003472 RID: 13426
			True
		}
	}
}
