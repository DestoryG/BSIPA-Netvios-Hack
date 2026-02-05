using System;

namespace Mono.Cecil
{
	// Token: 0x0200011A RID: 282
	internal interface IConstantProvider : IMetadataTokenProvider
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060007EA RID: 2026
		// (set) Token: 0x060007EB RID: 2027
		bool HasConstant { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060007EC RID: 2028
		// (set) Token: 0x060007ED RID: 2029
		object Constant { get; set; }
	}
}
