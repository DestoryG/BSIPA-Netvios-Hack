using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200011B RID: 283
	internal interface ICustomAttributeProvider : IMetadataTokenProvider
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060007EE RID: 2030
		Collection<CustomAttribute> CustomAttributes { get; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060007EF RID: 2031
		bool HasCustomAttributes { get; }
	}
}
