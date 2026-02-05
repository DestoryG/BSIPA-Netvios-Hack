using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000328 RID: 808
	public sealed class BypassElement : ConfigurationElement
	{
		// Token: 0x06001CF3 RID: 7411 RVA: 0x0008A8F8 File Offset: 0x00088AF8
		public BypassElement()
		{
			this.properties.Add(this.address);
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x0008A938 File Offset: 0x00088B38
		public BypassElement(string address)
			: this()
		{
			this.Address = address;
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x0008A947 File Offset: 0x00088B47
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x0008A94F File Offset: 0x00088B4F
		// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x0008A962 File Offset: 0x00088B62
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

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x0008A971 File Offset: 0x00088B71
		internal string Key
		{
			get
			{
				return this.Address;
			}
		}

		// Token: 0x04001BBE RID: 7102
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001BBF RID: 7103
		private readonly ConfigurationProperty address = new ConfigurationProperty("address", typeof(string), null, ConfigurationPropertyOptions.IsKey);
	}
}
