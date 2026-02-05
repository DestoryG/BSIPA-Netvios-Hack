using System;

namespace Mono.Cecil
{
	// Token: 0x02000064 RID: 100
	public interface IConstantProvider : IMetadataTokenProvider
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000458 RID: 1112
		// (set) Token: 0x06000459 RID: 1113
		bool HasConstant { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600045A RID: 1114
		// (set) Token: 0x0600045B RID: 1115
		object Constant { get; set; }
	}
}
