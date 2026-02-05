using System;
using System.Configuration;
using System.Net.Cache;
using System.Xml;

namespace System.Net.Configuration
{
	// Token: 0x02000334 RID: 820
	public sealed class HttpCachePolicyElement : ConfigurationElement
	{
		// Token: 0x06001D56 RID: 7510 RVA: 0x0008B948 File Offset: 0x00089B48
		public HttpCachePolicyElement()
		{
			this.properties.Add(this.maximumAge);
			this.properties.Add(this.maximumStale);
			this.properties.Add(this.minimumFresh);
			this.properties.Add(this.policyLevel);
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001D57 RID: 7511 RVA: 0x0008BA3A File Offset: 0x00089C3A
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06001D58 RID: 7512 RVA: 0x0008BA42 File Offset: 0x00089C42
		// (set) Token: 0x06001D59 RID: 7513 RVA: 0x0008BA55 File Offset: 0x00089C55
		[ConfigurationProperty("maximumAge", DefaultValue = "10675199.02:48:05.4775807")]
		public TimeSpan MaximumAge
		{
			get
			{
				return (TimeSpan)base[this.maximumAge];
			}
			set
			{
				base[this.maximumAge] = value;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06001D5A RID: 7514 RVA: 0x0008BA69 File Offset: 0x00089C69
		// (set) Token: 0x06001D5B RID: 7515 RVA: 0x0008BA7C File Offset: 0x00089C7C
		[ConfigurationProperty("maximumStale", DefaultValue = "-10675199.02:48:05.4775808")]
		public TimeSpan MaximumStale
		{
			get
			{
				return (TimeSpan)base[this.maximumStale];
			}
			set
			{
				base[this.maximumStale] = value;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001D5C RID: 7516 RVA: 0x0008BA90 File Offset: 0x00089C90
		// (set) Token: 0x06001D5D RID: 7517 RVA: 0x0008BAA3 File Offset: 0x00089CA3
		[ConfigurationProperty("minimumFresh", DefaultValue = "-10675199.02:48:05.4775808")]
		public TimeSpan MinimumFresh
		{
			get
			{
				return (TimeSpan)base[this.minimumFresh];
			}
			set
			{
				base[this.minimumFresh] = value;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06001D5E RID: 7518 RVA: 0x0008BAB7 File Offset: 0x00089CB7
		// (set) Token: 0x06001D5F RID: 7519 RVA: 0x0008BACA File Offset: 0x00089CCA
		[ConfigurationProperty("policyLevel", IsRequired = true, DefaultValue = HttpRequestCacheLevel.Default)]
		public HttpRequestCacheLevel PolicyLevel
		{
			get
			{
				return (HttpRequestCacheLevel)base[this.policyLevel];
			}
			set
			{
				base[this.policyLevel] = value;
			}
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x0008BADE File Offset: 0x00089CDE
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			this.wasReadFromConfig = true;
			base.DeserializeElement(reader, serializeCollectionKey);
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x0008BAF0 File Offset: 0x00089CF0
		protected override void Reset(ConfigurationElement parentElement)
		{
			if (parentElement != null)
			{
				HttpCachePolicyElement httpCachePolicyElement = (HttpCachePolicyElement)parentElement;
				this.wasReadFromConfig = httpCachePolicyElement.wasReadFromConfig;
			}
			base.Reset(parentElement);
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06001D62 RID: 7522 RVA: 0x0008BB1A File Offset: 0x00089D1A
		internal bool WasReadFromConfig
		{
			get
			{
				return this.wasReadFromConfig;
			}
		}

		// Token: 0x04001C38 RID: 7224
		private bool wasReadFromConfig;

		// Token: 0x04001C39 RID: 7225
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C3A RID: 7226
		private readonly ConfigurationProperty maximumAge = new ConfigurationProperty("maximumAge", typeof(TimeSpan), TimeSpan.MaxValue, ConfigurationPropertyOptions.None);

		// Token: 0x04001C3B RID: 7227
		private readonly ConfigurationProperty maximumStale = new ConfigurationProperty("maximumStale", typeof(TimeSpan), TimeSpan.MinValue, ConfigurationPropertyOptions.None);

		// Token: 0x04001C3C RID: 7228
		private readonly ConfigurationProperty minimumFresh = new ConfigurationProperty("minimumFresh", typeof(TimeSpan), TimeSpan.MinValue, ConfigurationPropertyOptions.None);

		// Token: 0x04001C3D RID: 7229
		private readonly ConfigurationProperty policyLevel = new ConfigurationProperty("policyLevel", typeof(HttpRequestCacheLevel), HttpRequestCacheLevel.Default, ConfigurationPropertyOptions.None);
	}
}
