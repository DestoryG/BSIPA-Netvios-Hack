using System;
using System.Configuration;
using System.Net.Cache;
using System.Xml;

namespace System.Net.Configuration
{
	// Token: 0x0200033D RID: 829
	public sealed class RequestCachingSection : ConfigurationSection
	{
		// Token: 0x06001D90 RID: 7568 RVA: 0x0008C06C File Offset: 0x0008A26C
		public RequestCachingSection()
		{
			this.properties.Add(this.disableAllCaching);
			this.properties.Add(this.defaultPolicyLevel);
			this.properties.Add(this.isPrivateCache);
			this.properties.Add(this.defaultHttpCachePolicy);
			this.properties.Add(this.defaultFtpCachePolicy);
			this.properties.Add(this.unspecifiedMaximumAge);
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001D91 RID: 7569 RVA: 0x0008C1B9 File Offset: 0x0008A3B9
		[ConfigurationProperty("defaultHttpCachePolicy")]
		public HttpCachePolicyElement DefaultHttpCachePolicy
		{
			get
			{
				return (HttpCachePolicyElement)base[this.defaultHttpCachePolicy];
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001D92 RID: 7570 RVA: 0x0008C1CC File Offset: 0x0008A3CC
		[ConfigurationProperty("defaultFtpCachePolicy")]
		public FtpCachePolicyElement DefaultFtpCachePolicy
		{
			get
			{
				return (FtpCachePolicyElement)base[this.defaultFtpCachePolicy];
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001D93 RID: 7571 RVA: 0x0008C1DF File Offset: 0x0008A3DF
		// (set) Token: 0x06001D94 RID: 7572 RVA: 0x0008C1F2 File Offset: 0x0008A3F2
		[ConfigurationProperty("defaultPolicyLevel", DefaultValue = RequestCacheLevel.BypassCache)]
		public RequestCacheLevel DefaultPolicyLevel
		{
			get
			{
				return (RequestCacheLevel)base[this.defaultPolicyLevel];
			}
			set
			{
				base[this.defaultPolicyLevel] = value;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001D95 RID: 7573 RVA: 0x0008C206 File Offset: 0x0008A406
		// (set) Token: 0x06001D96 RID: 7574 RVA: 0x0008C219 File Offset: 0x0008A419
		[ConfigurationProperty("disableAllCaching", DefaultValue = false)]
		public bool DisableAllCaching
		{
			get
			{
				return (bool)base[this.disableAllCaching];
			}
			set
			{
				base[this.disableAllCaching] = value;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001D97 RID: 7575 RVA: 0x0008C22D File Offset: 0x0008A42D
		// (set) Token: 0x06001D98 RID: 7576 RVA: 0x0008C240 File Offset: 0x0008A440
		[ConfigurationProperty("isPrivateCache", DefaultValue = true)]
		public bool IsPrivateCache
		{
			get
			{
				return (bool)base[this.isPrivateCache];
			}
			set
			{
				base[this.isPrivateCache] = value;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06001D99 RID: 7577 RVA: 0x0008C254 File Offset: 0x0008A454
		// (set) Token: 0x06001D9A RID: 7578 RVA: 0x0008C267 File Offset: 0x0008A467
		[ConfigurationProperty("unspecifiedMaximumAge", DefaultValue = "1.00:00:00")]
		public TimeSpan UnspecifiedMaximumAge
		{
			get
			{
				return (TimeSpan)base[this.unspecifiedMaximumAge];
			}
			set
			{
				base[this.unspecifiedMaximumAge] = value;
			}
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x0008C27C File Offset: 0x0008A47C
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			bool flag = this.DisableAllCaching;
			base.DeserializeElement(reader, serializeCollectionKey);
			if (flag)
			{
				this.DisableAllCaching = true;
			}
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x0008C2A4 File Offset: 0x0008A4A4
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", new object[] { "requestCaching" }), ex);
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x0008C2FC File Offset: 0x0008A4FC
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04001C4E RID: 7246
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C4F RID: 7247
		private readonly ConfigurationProperty defaultHttpCachePolicy = new ConfigurationProperty("defaultHttpCachePolicy", typeof(HttpCachePolicyElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C50 RID: 7248
		private readonly ConfigurationProperty defaultFtpCachePolicy = new ConfigurationProperty("defaultFtpCachePolicy", typeof(FtpCachePolicyElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001C51 RID: 7249
		private readonly ConfigurationProperty defaultPolicyLevel = new ConfigurationProperty("defaultPolicyLevel", typeof(RequestCacheLevel), RequestCacheLevel.BypassCache, ConfigurationPropertyOptions.None);

		// Token: 0x04001C52 RID: 7250
		private readonly ConfigurationProperty disableAllCaching = new ConfigurationProperty("disableAllCaching", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001C53 RID: 7251
		private readonly ConfigurationProperty isPrivateCache = new ConfigurationProperty("isPrivateCache", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001C54 RID: 7252
		private readonly ConfigurationProperty unspecifiedMaximumAge = new ConfigurationProperty("unspecifiedMaximumAge", typeof(TimeSpan), TimeSpan.FromDays(1.0), ConfigurationPropertyOptions.None);
	}
}
