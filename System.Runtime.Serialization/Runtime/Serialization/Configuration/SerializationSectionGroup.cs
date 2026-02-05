using System;
using System.Configuration;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x0200012A RID: 298
	public sealed class SerializationSectionGroup : ConfigurationSectionGroup
	{
		// Token: 0x060011FA RID: 4602 RVA: 0x0004B353 File Offset: 0x00049553
		public static SerializationSectionGroup GetSectionGroup(Configuration config)
		{
			if (config == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("config");
			}
			return (SerializationSectionGroup)config.SectionGroups["system.runtime.serialization"];
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060011FB RID: 4603 RVA: 0x0004B378 File Offset: 0x00049578
		public DataContractSerializerSection DataContractSerializer
		{
			get
			{
				return (DataContractSerializerSection)base.Sections["dataContractSerializer"];
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x0004B38F File Offset: 0x0004958F
		public NetDataContractSerializerSection NetDataContractSerializer
		{
			get
			{
				return (NetDataContractSerializerSection)base.Sections["netDataContractSerializer"];
			}
		}
	}
}
