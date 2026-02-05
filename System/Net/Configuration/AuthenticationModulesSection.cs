using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000326 RID: 806
	public sealed class AuthenticationModulesSection : ConfigurationSection
	{
		// Token: 0x06001CEA RID: 7402 RVA: 0x0008A5F8 File Offset: 0x000887F8
		public AuthenticationModulesSection()
		{
			this.properties.Add(this.authenticationModules);
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x0008A634 File Offset: 0x00088834
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.UnmanagedPermission.Demand();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", new object[] { "authenticationModules" }), ex);
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x0008A68C File Offset: 0x0008888C
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public AuthenticationModuleElementCollection AuthenticationModules
		{
			get
			{
				return (AuthenticationModuleElementCollection)base[this.authenticationModules];
			}
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x0008A6A0 File Offset: 0x000888A0
		protected override void InitializeDefault()
		{
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(NegotiateClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(KerberosClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(NtlmClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(DigestClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(BasicClient).AssemblyQualifiedName));
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x0008A748 File Offset: 0x00088948
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04001BBA RID: 7098
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001BBB RID: 7099
		private readonly ConfigurationProperty authenticationModules = new ConfigurationProperty(null, typeof(AuthenticationModuleElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
