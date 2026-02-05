using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200034F RID: 847
	public sealed class WindowsAuthenticationElement : ConfigurationElement
	{
		// Token: 0x06001E54 RID: 7764 RVA: 0x0008DE74 File Offset: 0x0008C074
		public WindowsAuthenticationElement()
		{
			this.defaultCredentialsHandleCacheSize = new ConfigurationProperty("defaultCredentialsHandleCacheSize", typeof(int), 0, null, new WindowsAuthenticationElement.CacheSizeValidator(), ConfigurationPropertyOptions.None);
			this.properties = new ConfigurationPropertyCollection();
			this.properties.Add(this.defaultCredentialsHandleCacheSize);
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x0008DECA File Offset: 0x0008C0CA
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06001E56 RID: 7766 RVA: 0x0008DED2 File Offset: 0x0008C0D2
		// (set) Token: 0x06001E57 RID: 7767 RVA: 0x0008DEE5 File Offset: 0x0008C0E5
		[ConfigurationProperty("defaultCredentialsHandleCacheSize", DefaultValue = 0)]
		public int DefaultCredentialsHandleCacheSize
		{
			get
			{
				return (int)base[this.defaultCredentialsHandleCacheSize];
			}
			set
			{
				base[this.defaultCredentialsHandleCacheSize] = value;
			}
		}

		// Token: 0x04001CB9 RID: 7353
		private ConfigurationPropertyCollection properties;

		// Token: 0x04001CBA RID: 7354
		private readonly ConfigurationProperty defaultCredentialsHandleCacheSize;

		// Token: 0x020007CB RID: 1995
		private class CacheSizeValidator : ConfigurationValidatorBase
		{
			// Token: 0x06004397 RID: 17303 RVA: 0x0011CE72 File Offset: 0x0011B072
			public override bool CanValidate(Type type)
			{
				return type == typeof(int);
			}

			// Token: 0x06004398 RID: 17304 RVA: 0x0011CE84 File Offset: 0x0011B084
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
