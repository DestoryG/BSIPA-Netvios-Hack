using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200011D RID: 285
	internal interface IGenericParameterProvider : IMetadataTokenProvider
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060007F2 RID: 2034
		bool HasGenericParameters { get; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060007F3 RID: 2035
		bool IsDefinition { get; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060007F4 RID: 2036
		ModuleDefinition Module { get; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060007F5 RID: 2037
		Collection<GenericParameter> GenericParameters { get; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060007F6 RID: 2038
		GenericParameterType GenericParameterType { get; }
	}
}
