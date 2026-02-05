using System;
using System.Collections.Generic;
using System.Configuration;

namespace System.Security.Authentication.ExtendedProtection.Configuration
{
	// Token: 0x0200044A RID: 1098
	public sealed class ExtendedProtectionPolicyElement : ConfigurationElement
	{
		// Token: 0x0600289C RID: 10396 RVA: 0x000BA630 File Offset: 0x000B8830
		public ExtendedProtectionPolicyElement()
		{
			this.properties.Add(this.policyEnforcement);
			this.properties.Add(this.protectionScenario);
			this.properties.Add(this.customServiceNames);
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x0600289D RID: 10397 RVA: 0x000BA6E3 File Offset: 0x000B88E3
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x000BA6EB File Offset: 0x000B88EB
		// (set) Token: 0x0600289F RID: 10399 RVA: 0x000BA6FE File Offset: 0x000B88FE
		[ConfigurationProperty("policyEnforcement")]
		public PolicyEnforcement PolicyEnforcement
		{
			get
			{
				return (PolicyEnforcement)base[this.policyEnforcement];
			}
			set
			{
				base[this.policyEnforcement] = value;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x000BA712 File Offset: 0x000B8912
		// (set) Token: 0x060028A1 RID: 10401 RVA: 0x000BA725 File Offset: 0x000B8925
		[ConfigurationProperty("protectionScenario", DefaultValue = ProtectionScenario.TransportSelected)]
		public ProtectionScenario ProtectionScenario
		{
			get
			{
				return (ProtectionScenario)base[this.protectionScenario];
			}
			set
			{
				base[this.protectionScenario] = value;
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x000BA739 File Offset: 0x000B8939
		[ConfigurationProperty("customServiceNames")]
		public ServiceNameElementCollection CustomServiceNames
		{
			get
			{
				return (ServiceNameElementCollection)base[this.customServiceNames];
			}
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000BA74C File Offset: 0x000B894C
		public ExtendedProtectionPolicy BuildPolicy()
		{
			if (this.PolicyEnforcement == PolicyEnforcement.Never)
			{
				return new ExtendedProtectionPolicy(PolicyEnforcement.Never);
			}
			ServiceNameCollection serviceNameCollection = null;
			ServiceNameElementCollection serviceNameElementCollection = this.CustomServiceNames;
			if (serviceNameElementCollection != null && serviceNameElementCollection.Count > 0)
			{
				List<string> list = new List<string>(serviceNameElementCollection.Count);
				foreach (object obj in serviceNameElementCollection)
				{
					ServiceNameElement serviceNameElement = (ServiceNameElement)obj;
					list.Add(serviceNameElement.Name);
				}
				serviceNameCollection = new ServiceNameCollection(list);
			}
			return new ExtendedProtectionPolicy(this.PolicyEnforcement, this.ProtectionScenario, serviceNameCollection);
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x060028A4 RID: 10404 RVA: 0x000BA7F4 File Offset: 0x000B89F4
		private static PolicyEnforcement DefaultPolicyEnforcement
		{
			get
			{
				return PolicyEnforcement.Never;
			}
		}

		// Token: 0x0400226D RID: 8813
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x0400226E RID: 8814
		private readonly ConfigurationProperty policyEnforcement = new ConfigurationProperty("policyEnforcement", typeof(PolicyEnforcement), ExtendedProtectionPolicyElement.DefaultPolicyEnforcement, ConfigurationPropertyOptions.None);

		// Token: 0x0400226F RID: 8815
		private readonly ConfigurationProperty protectionScenario = new ConfigurationProperty("protectionScenario", typeof(ProtectionScenario), ProtectionScenario.TransportSelected, ConfigurationPropertyOptions.None);

		// Token: 0x04002270 RID: 8816
		private readonly ConfigurationProperty customServiceNames = new ConfigurationProperty("customServiceNames", typeof(ServiceNameElementCollection), null, ConfigurationPropertyOptions.None);
	}
}
