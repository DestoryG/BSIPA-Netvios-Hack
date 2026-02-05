using System;
using System.Configuration;
using System.Security;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x02000122 RID: 290
	public sealed class DeclaredTypeElement : ConfigurationElement
	{
		// Token: 0x060011B6 RID: 4534 RVA: 0x0004A9BE File Offset: 0x00048BBE
		public DeclaredTypeElement()
		{
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0004A9C6 File Offset: 0x00048BC6
		public DeclaredTypeElement(string typeName)
			: this()
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("typeName");
			}
			this.Type = typeName;
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060011B8 RID: 4536 RVA: 0x0004A9E8 File Offset: 0x00048BE8
		[ConfigurationProperty("", DefaultValue = null, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
		public TypeElementCollection KnownTypes
		{
			get
			{
				return (TypeElementCollection)base[""];
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x0004A9FA File Offset: 0x00048BFA
		// (set) Token: 0x060011BA RID: 4538 RVA: 0x0004AA0C File Offset: 0x00048C0C
		[ConfigurationProperty("type", DefaultValue = "", Options = ConfigurationPropertyOptions.IsKey)]
		[DeclaredTypeValidator]
		public string Type
		{
			get
			{
				return (string)base["type"];
			}
			set
			{
				base["type"] = value;
			}
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0004AA1A File Offset: 0x00048C1A
		[SecuritySafeCritical]
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			if (!PartialTrustHelpers.IsInFullTrust())
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ConfigurationErrorsException(SR.GetString("Failed to load configuration section for dataContractSerializer.")));
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x0004AA48 File Offset: 0x00048C48
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new ConfigurationPropertyCollection
					{
						new ConfigurationProperty("", typeof(TypeElementCollection), null, null, null, ConfigurationPropertyOptions.IsDefaultCollection),
						new ConfigurationProperty("type", typeof(string), string.Empty, null, new DeclaredTypeValidator(), ConfigurationPropertyOptions.IsKey)
					};
				}
				return this.properties;
			}
		}

		// Token: 0x0400089C RID: 2204
		private ConfigurationPropertyCollection properties;
	}
}
