using System;
using System.Configuration.Provider;

namespace System.Configuration
{
	// Token: 0x020000B0 RID: 176
	public abstract class SettingsProvider : ProviderBase
	{
		// Token: 0x0600060D RID: 1549
		public abstract SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection);

		// Token: 0x0600060E RID: 1550
		public abstract void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection);

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600060F RID: 1551
		// (set) Token: 0x06000610 RID: 1552
		public abstract string ApplicationName { get; set; }
	}
}
