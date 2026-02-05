using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000067 RID: 103
	public interface IGenericParameterProvider : IMetadataTokenProvider
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000460 RID: 1120
		bool HasGenericParameters { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000461 RID: 1121
		bool IsDefinition { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000462 RID: 1122
		ModuleDefinition Module { get; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000463 RID: 1123
		Collection<GenericParameter> GenericParameters { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000464 RID: 1124
		GenericParameterType GenericParameterType { get; }
	}
}
