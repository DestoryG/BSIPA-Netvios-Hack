using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200032B RID: 811
	public sealed class ConnectionManagementElement : ConfigurationElement
	{
		// Token: 0x06001D10 RID: 7440 RVA: 0x0008AAE0 File Offset: 0x00088CE0
		public ConnectionManagementElement()
		{
			this.properties.Add(this.address);
			this.properties.Add(this.maxconnection);
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x0008AB5D File Offset: 0x00088D5D
		public ConnectionManagementElement(string address, int maxConnection)
			: this()
		{
			this.Address = address;
			this.MaxConnection = maxConnection;
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06001D12 RID: 7442 RVA: 0x0008AB73 File Offset: 0x00088D73
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001D13 RID: 7443 RVA: 0x0008AB7B File Offset: 0x00088D7B
		// (set) Token: 0x06001D14 RID: 7444 RVA: 0x0008AB8E File Offset: 0x00088D8E
		[ConfigurationProperty("address", IsRequired = true, IsKey = true)]
		public string Address
		{
			get
			{
				return (string)base[this.address];
			}
			set
			{
				base[this.address] = value;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x0008AB9D File Offset: 0x00088D9D
		// (set) Token: 0x06001D16 RID: 7446 RVA: 0x0008ABB0 File Offset: 0x00088DB0
		[ConfigurationProperty("maxconnection", IsRequired = true, DefaultValue = 1)]
		public int MaxConnection
		{
			get
			{
				return (int)base[this.maxconnection];
			}
			set
			{
				base[this.maxconnection] = value;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x0008ABC4 File Offset: 0x00088DC4
		internal string Key
		{
			get
			{
				return this.Address;
			}
		}

		// Token: 0x04001C19 RID: 7193
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C1A RID: 7194
		private readonly ConfigurationProperty address = new ConfigurationProperty("address", typeof(string), null, ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001C1B RID: 7195
		private readonly ConfigurationProperty maxconnection = new ConfigurationProperty("maxconnection", typeof(int), 1, ConfigurationPropertyOptions.None);
	}
}
