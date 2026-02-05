using System;

namespace Mono.Cecil
{
	// Token: 0x0200006B RID: 107
	public interface IMemberDefinition : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600046B RID: 1131
		// (set) Token: 0x0600046C RID: 1132
		string Name { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600046D RID: 1133
		string FullName { get; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600046E RID: 1134
		// (set) Token: 0x0600046F RID: 1135
		bool IsSpecialName { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000470 RID: 1136
		// (set) Token: 0x06000471 RID: 1137
		bool IsRuntimeSpecialName { get; set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000472 RID: 1138
		// (set) Token: 0x06000473 RID: 1139
		TypeDefinition DeclaringType { get; set; }
	}
}
