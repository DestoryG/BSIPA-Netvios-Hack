using System;
using System.Configuration;
using System.Net.Cache;

namespace System.Net.Configuration
{
	// Token: 0x0200033F RID: 831
	public sealed class SettingsSection : ConfigurationSection
	{
		// Token: 0x06001DAB RID: 7595 RVA: 0x0008C524 File Offset: 0x0008A724
		internal static void EnsureConfigLoaded()
		{
			try
			{
				AuthenticationManager.EnsureConfigLoaded();
				object obj = RequestCacheManager.IsCachingEnabled;
				obj = global::System.Net.ServicePointManager.DefaultConnectionLimit;
				obj = global::System.Net.ServicePointManager.Expect100Continue;
				obj = WebRequest.PrefixList;
				obj = WebRequest.InternalDefaultWebProxy;
			}
			catch
			{
			}
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x0008C578 File Offset: 0x0008A778
		public SettingsSection()
		{
			this.properties.Add(this.httpWebRequest);
			this.properties.Add(this.ipv6);
			this.properties.Add(this.servicePointManager);
			this.properties.Add(this.socket);
			this.properties.Add(this.webProxyScript);
			this.properties.Add(this.performanceCounters);
			this.properties.Add(this.httpListener);
			this.properties.Add(this.webUtility);
			this.properties.Add(this.windowsAuthentication);
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06001DAD RID: 7597 RVA: 0x0008C72B File Offset: 0x0008A92B
		[ConfigurationProperty("httpWebRequest")]
		public HttpWebRequestElement HttpWebRequest
		{
			get
			{
				return (HttpWebRequestElement)base[this.httpWebRequest];
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06001DAE RID: 7598 RVA: 0x0008C73E File Offset: 0x0008A93E
		[ConfigurationProperty("ipv6")]
		public Ipv6Element Ipv6
		{
			get
			{
				return (Ipv6Element)base[this.ipv6];
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06001DAF RID: 7599 RVA: 0x0008C751 File Offset: 0x0008A951
		[ConfigurationProperty("servicePointManager")]
		public ServicePointManagerElement ServicePointManager
		{
			get
			{
				return (ServicePointManagerElement)base[this.servicePointManager];
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06001DB0 RID: 7600 RVA: 0x0008C764 File Offset: 0x0008A964
		[ConfigurationProperty("socket")]
		public SocketElement Socket
		{
			get
			{
				return (SocketElement)base[this.socket];
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06001DB1 RID: 7601 RVA: 0x0008C777 File Offset: 0x0008A977
		[ConfigurationProperty("webProxyScript")]
		public WebProxyScriptElement WebProxyScript
		{
			get
			{
				return (WebProxyScriptElement)base[this.webProxyScript];
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06001DB2 RID: 7602 RVA: 0x0008C78A File Offset: 0x0008A98A
		[ConfigurationProperty("performanceCounters")]
		public PerformanceCountersElement PerformanceCounters
		{
			get
			{
				return (PerformanceCountersElement)base[this.performanceCounters];
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x0008C79D File Offset: 0x0008A99D
		[ConfigurationProperty("httpListener")]
		public HttpListenerElement HttpListener
		{
			get
			{
				return (HttpListenerElement)base[this.httpListener];
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06001DB4 RID: 7604 RVA: 0x0008C7B0 File Offset: 0x0008A9B0
		[ConfigurationProperty("webUtility")]
		public WebUtilityElement WebUtility
		{
			get
			{
				return (WebUtilityElement)base[this.webUtility];
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06001DB5 RID: 7605 RVA: 0x0008C7C3 File Offset: 0x0008A9C3
		[ConfigurationProperty("windowsAuthentication")]
		public WindowsAuthenticationElement WindowsAuthentication
		{
			get
			{
				return (WindowsAuthenticationElement)base[this.windowsAuthentication];
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x0008C7D6 File Offset: 0x0008A9D6
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04001C5F RID: 7263
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C60 RID: 7264
		private readonly ConfigurationProperty httpWebRequest = new ConfigurationProperty("httpWebRequest", typeof(HttpWebRequestElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C61 RID: 7265
		private readonly ConfigurationProperty ipv6 = new ConfigurationProperty("ipv6", typeof(Ipv6Element), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C62 RID: 7266
		private readonly ConfigurationProperty servicePointManager = new ConfigurationProperty("servicePointManager", typeof(ServicePointManagerElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C63 RID: 7267
		private readonly ConfigurationProperty socket = new ConfigurationProperty("socket", typeof(SocketElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C64 RID: 7268
		private readonly ConfigurationProperty webProxyScript = new ConfigurationProperty("webProxyScript", typeof(WebProxyScriptElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C65 RID: 7269
		private readonly ConfigurationProperty performanceCounters = new ConfigurationProperty("performanceCounters", typeof(PerformanceCountersElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C66 RID: 7270
		private readonly ConfigurationProperty httpListener = new ConfigurationProperty("httpListener", typeof(HttpListenerElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C67 RID: 7271
		private readonly ConfigurationProperty webUtility = new ConfigurationProperty("webUtility", typeof(WebUtilityElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C68 RID: 7272
		private readonly ConfigurationProperty windowsAuthentication = new ConfigurationProperty("windowsAuthentication", typeof(WindowsAuthenticationElement), null, ConfigurationPropertyOptions.None);
	}
}
