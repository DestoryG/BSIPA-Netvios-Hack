using System;

namespace Mono.Cecil
{
	// Token: 0x02000123 RID: 291
	internal interface IMetadataScope : IMetadataTokenProvider
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000806 RID: 2054
		MetadataScopeType MetadataScopeType { get; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000807 RID: 2055
		// (set) Token: 0x06000808 RID: 2056
		string Name { get; set; }
	}
}
