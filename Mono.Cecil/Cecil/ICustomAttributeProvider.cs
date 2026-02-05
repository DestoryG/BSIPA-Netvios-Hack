using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000065 RID: 101
	public interface ICustomAttributeProvider : IMetadataTokenProvider
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600045C RID: 1116
		Collection<CustomAttribute> CustomAttributes { get; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600045D RID: 1117
		bool HasCustomAttributes { get; }
	}
}
