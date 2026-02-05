using System;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x02000120 RID: 288
	internal static class ConfigurationStrings
	{
		// Token: 0x060011AF RID: 4527 RVA: 0x0004A908 File Offset: 0x00048B08
		private static string GetSectionPath(string sectionName)
		{
			return "system.runtime.serialization" + "/" + sectionName;
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x0004A91A File Offset: 0x00048B1A
		internal static string DataContractSerializerSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("dataContractSerializer");
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x0004A926 File Offset: 0x00048B26
		internal static string NetDataContractSerializerSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("netDataContractSerializer");
			}
		}

		// Token: 0x04000892 RID: 2194
		internal const string SectionGroupName = "system.runtime.serialization";

		// Token: 0x04000893 RID: 2195
		internal const string DefaultCollectionName = "";

		// Token: 0x04000894 RID: 2196
		internal const string DeclaredTypes = "declaredTypes";

		// Token: 0x04000895 RID: 2197
		internal const string Index = "index";

		// Token: 0x04000896 RID: 2198
		internal const string Parameter = "parameter";

		// Token: 0x04000897 RID: 2199
		internal const string Type = "type";

		// Token: 0x04000898 RID: 2200
		internal const string EnableUnsafeTypeForwarding = "enableUnsafeTypeForwarding";

		// Token: 0x04000899 RID: 2201
		internal const string DataContractSerializerSectionName = "dataContractSerializer";

		// Token: 0x0400089A RID: 2202
		internal const string NetDataContractSerializerSectionName = "netDataContractSerializer";
	}
}
