using System;

namespace Mono.Cecil
{
	// Token: 0x02000120 RID: 288
	internal interface IMarshalInfoProvider : IMetadataTokenProvider
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060007FA RID: 2042
		bool HasMarshalInfo { get; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060007FB RID: 2043
		// (set) Token: 0x060007FC RID: 2044
		MarshalInfo MarshalInfo { get; set; }
	}
}
