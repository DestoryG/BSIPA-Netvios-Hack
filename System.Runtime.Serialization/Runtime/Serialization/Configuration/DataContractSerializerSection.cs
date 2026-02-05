using System;
using System.Configuration;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x02000121 RID: 289
	public sealed class DataContractSerializerSection : ConfigurationSection
	{
		// Token: 0x060011B3 RID: 4531 RVA: 0x0004A93A File Offset: 0x00048B3A
		[SecurityCritical]
		[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static DataContractSerializerSection UnsafeGetSection()
		{
			DataContractSerializerSection dataContractSerializerSection = (DataContractSerializerSection)ConfigurationManager.GetSection(ConfigurationStrings.DataContractSerializerSectionPath);
			if (dataContractSerializerSection == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ConfigurationErrorsException(SR.GetString("Failed to load configuration section for dataContractSerializer.")));
			}
			return dataContractSerializerSection;
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x0004A963 File Offset: 0x00048B63
		[ConfigurationProperty("declaredTypes", DefaultValue = null)]
		public DeclaredTypeElementCollection DeclaredTypes
		{
			get
			{
				return (DeclaredTypeElementCollection)base["declaredTypes"];
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x0004A978 File Offset: 0x00048B78
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new ConfigurationPropertyCollection
					{
						new ConfigurationProperty("declaredTypes", typeof(DeclaredTypeElementCollection), null, null, null, ConfigurationPropertyOptions.None)
					};
				}
				return this.properties;
			}
		}

		// Token: 0x0400089B RID: 2203
		private ConfigurationPropertyCollection properties;
	}
}
