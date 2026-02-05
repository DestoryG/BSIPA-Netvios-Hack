using System;

namespace System.Configuration
{
	// Token: 0x02000077 RID: 119
	public sealed class IriParsingElement : ConfigurationElement
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x0001FE9C File Offset: 0x0001E09C
		public IriParsingElement()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0001FEEC File Offset: 0x0001E0EC
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0001FEF4 File Offset: 0x0001E0F4
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x0001FF07 File Offset: 0x0001E107
		[ConfigurationProperty("enabled", DefaultValue = false)]
		public bool Enabled
		{
			get
			{
				return (bool)base[this.enabled];
			}
			set
			{
				base[this.enabled] = value;
			}
		}

		// Token: 0x04000BFB RID: 3067
		internal const bool EnabledDefaultValue = false;

		// Token: 0x04000BFC RID: 3068
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04000BFD RID: 3069
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
