using System;

namespace Mono.Cecil
{
	// Token: 0x0200006A RID: 106
	public interface IMarshalInfoProvider : IMetadataTokenProvider
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000468 RID: 1128
		bool HasMarshalInfo { get; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000469 RID: 1129
		// (set) Token: 0x0600046A RID: 1130
		MarshalInfo MarshalInfo { get; set; }
	}
}
