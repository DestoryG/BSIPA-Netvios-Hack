using System;

namespace Mono.Cecil
{
	// Token: 0x02000121 RID: 289
	internal interface IMemberDefinition : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060007FD RID: 2045
		// (set) Token: 0x060007FE RID: 2046
		string Name { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060007FF RID: 2047
		string FullName { get; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000800 RID: 2048
		// (set) Token: 0x06000801 RID: 2049
		bool IsSpecialName { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000802 RID: 2050
		// (set) Token: 0x06000803 RID: 2051
		bool IsRuntimeSpecialName { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000804 RID: 2052
		// (set) Token: 0x06000805 RID: 2053
		TypeDefinition DeclaringType { get; set; }
	}
}
