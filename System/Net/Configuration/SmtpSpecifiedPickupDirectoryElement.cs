using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000346 RID: 838
	public sealed class SmtpSpecifiedPickupDirectoryElement : ConfigurationElement
	{
		// Token: 0x06001E19 RID: 7705 RVA: 0x0008D591 File Offset: 0x0008B791
		public SmtpSpecifiedPickupDirectoryElement()
		{
			this.properties.Add(this.pickupDirectoryLocation);
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06001E1A RID: 7706 RVA: 0x0008D5D1 File Offset: 0x0008B7D1
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06001E1B RID: 7707 RVA: 0x0008D5D9 File Offset: 0x0008B7D9
		// (set) Token: 0x06001E1C RID: 7708 RVA: 0x0008D5EC File Offset: 0x0008B7EC
		[ConfigurationProperty("pickupDirectoryLocation")]
		public string PickupDirectoryLocation
		{
			get
			{
				return (string)base[this.pickupDirectoryLocation];
			}
			set
			{
				base[this.pickupDirectoryLocation] = value;
			}
		}

		// Token: 0x04001CA5 RID: 7333
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001CA6 RID: 7334
		private readonly ConfigurationProperty pickupDirectoryLocation = new ConfigurationProperty("pickupDirectoryLocation", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}
