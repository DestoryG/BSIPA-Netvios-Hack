using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000332 RID: 818
	public sealed class HttpListenerElement : ConfigurationElement
	{
		// Token: 0x06001D46 RID: 7494 RVA: 0x0008B694 File Offset: 0x00089894
		static HttpListenerElement()
		{
			HttpListenerElement.properties.Add(HttpListenerElement.unescapeRequestUrl);
			HttpListenerElement.properties.Add(HttpListenerElement.timeouts);
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06001D47 RID: 7495 RVA: 0x0008B704 File Offset: 0x00089904
		[ConfigurationProperty("unescapeRequestUrl", DefaultValue = true, IsRequired = false)]
		public bool UnescapeRequestUrl
		{
			get
			{
				return (bool)base[HttpListenerElement.unescapeRequestUrl];
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06001D48 RID: 7496 RVA: 0x0008B716 File Offset: 0x00089916
		[ConfigurationProperty("timeouts")]
		public HttpListenerTimeoutsElement Timeouts
		{
			get
			{
				return (HttpListenerTimeoutsElement)base[HttpListenerElement.timeouts];
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x0008B728 File Offset: 0x00089928
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpListenerElement.properties;
			}
		}

		// Token: 0x04001C2D RID: 7213
		internal const bool UnescapeRequestUrlDefaultValue = true;

		// Token: 0x04001C2E RID: 7214
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C2F RID: 7215
		private static readonly ConfigurationProperty unescapeRequestUrl = new ConfigurationProperty("unescapeRequestUrl", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001C30 RID: 7216
		private static readonly ConfigurationProperty timeouts = new ConfigurationProperty("timeouts", typeof(HttpListenerTimeoutsElement), null, ConfigurationPropertyOptions.None);
	}
}
