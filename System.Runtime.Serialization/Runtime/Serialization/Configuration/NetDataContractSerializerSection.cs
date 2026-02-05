using System;
using System.Configuration;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x02000126 RID: 294
	public sealed class NetDataContractSerializerSection : ConfigurationSection
	{
		// Token: 0x060011D1 RID: 4561 RVA: 0x0004ACE4 File Offset: 0x00048EE4
		[SecurityCritical]
		[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static bool TryUnsafeGetSection(out NetDataContractSerializerSection section)
		{
			section = (NetDataContractSerializerSection)ConfigurationManager.GetSection(ConfigurationStrings.NetDataContractSerializerSectionPath);
			return section != null;
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x0004ACFC File Offset: 0x00048EFC
		[ConfigurationProperty("enableUnsafeTypeForwarding", DefaultValue = false)]
		public bool EnableUnsafeTypeForwarding
		{
			get
			{
				return (bool)base["enableUnsafeTypeForwarding"];
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x0004AD10 File Offset: 0x00048F10
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new ConfigurationPropertyCollection
					{
						new ConfigurationProperty("enableUnsafeTypeForwarding", typeof(bool), false, null, null, ConfigurationPropertyOptions.None)
					};
				}
				return this.properties;
			}
		}

		// Token: 0x0400089D RID: 2205
		private ConfigurationPropertyCollection properties;
	}
}
