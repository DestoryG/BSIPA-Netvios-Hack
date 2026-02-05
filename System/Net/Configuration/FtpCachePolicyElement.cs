using System;
using System.Configuration;
using System.Net.Cache;
using System.Xml;

namespace System.Net.Configuration
{
	// Token: 0x02000335 RID: 821
	public sealed class FtpCachePolicyElement : ConfigurationElement
	{
		// Token: 0x06001D63 RID: 7523 RVA: 0x0008BB24 File Offset: 0x00089D24
		public FtpCachePolicyElement()
		{
			this.properties.Add(this.policyLevel);
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001D64 RID: 7524 RVA: 0x0008BB74 File Offset: 0x00089D74
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001D65 RID: 7525 RVA: 0x0008BB7C File Offset: 0x00089D7C
		// (set) Token: 0x06001D66 RID: 7526 RVA: 0x0008BB8F File Offset: 0x00089D8F
		[ConfigurationProperty("policyLevel", DefaultValue = RequestCacheLevel.Default)]
		public RequestCacheLevel PolicyLevel
		{
			get
			{
				return (RequestCacheLevel)base[this.policyLevel];
			}
			set
			{
				base[this.policyLevel] = value;
			}
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x0008BBA3 File Offset: 0x00089DA3
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			this.wasReadFromConfig = true;
			base.DeserializeElement(reader, serializeCollectionKey);
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x0008BBB4 File Offset: 0x00089DB4
		protected override void Reset(ConfigurationElement parentElement)
		{
			if (parentElement != null)
			{
				FtpCachePolicyElement ftpCachePolicyElement = (FtpCachePolicyElement)parentElement;
				this.wasReadFromConfig = ftpCachePolicyElement.wasReadFromConfig;
			}
			base.Reset(parentElement);
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x0008BBDE File Offset: 0x00089DDE
		internal bool WasReadFromConfig
		{
			get
			{
				return this.wasReadFromConfig;
			}
		}

		// Token: 0x04001C3E RID: 7230
		private bool wasReadFromConfig;

		// Token: 0x04001C3F RID: 7231
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001C40 RID: 7232
		private readonly ConfigurationProperty policyLevel = new ConfigurationProperty("policyLevel", typeof(RequestCacheLevel), RequestCacheLevel.Default, ConfigurationPropertyOptions.None);
	}
}
