using System;

namespace Mono.Cecil
{
	// Token: 0x0200006D RID: 109
	public interface IMetadataScope : IMetadataTokenProvider
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000474 RID: 1140
		MetadataScopeType MetadataScopeType { get; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000475 RID: 1141
		// (set) Token: 0x06000476 RID: 1142
		string Name { get; set; }
	}
}
