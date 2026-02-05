using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000125 RID: 293
	internal interface IMethodSignature : IMetadataTokenProvider
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600080B RID: 2059
		// (set) Token: 0x0600080C RID: 2060
		bool HasThis { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600080D RID: 2061
		// (set) Token: 0x0600080E RID: 2062
		bool ExplicitThis { get; set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600080F RID: 2063
		// (set) Token: 0x06000810 RID: 2064
		MethodCallingConvention CallingConvention { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000811 RID: 2065
		bool HasParameters { get; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000812 RID: 2066
		Collection<ParameterDefinition> Parameters { get; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000813 RID: 2067
		// (set) Token: 0x06000814 RID: 2068
		TypeReference ReturnType { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000815 RID: 2069
		MethodReturnType MethodReturnType { get; }
	}
}
