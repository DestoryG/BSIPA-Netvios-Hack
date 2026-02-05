using System;
using System.Configuration.Provider;

namespace System.Configuration
{
	// Token: 0x020000B1 RID: 177
	public class SettingsProviderCollection : ProviderCollection
	{
		// Token: 0x06000612 RID: 1554 RVA: 0x00023D00 File Offset: 0x00021F00
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (!(provider is SettingsProvider))
			{
				throw new ArgumentException(SR.GetString("Config_provider_must_implement_type", new object[] { typeof(SettingsProvider).ToString() }), "provider");
			}
			base.Add(provider);
		}

		// Token: 0x170000FD RID: 253
		public SettingsProvider this[string name]
		{
			get
			{
				return (SettingsProvider)base[name];
			}
		}
	}
}
