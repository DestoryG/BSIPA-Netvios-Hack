using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200006F RID: 111
	public interface IMethodSignature : IMetadataTokenProvider
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000479 RID: 1145
		// (set) Token: 0x0600047A RID: 1146
		bool HasThis { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600047B RID: 1147
		// (set) Token: 0x0600047C RID: 1148
		bool ExplicitThis { get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600047D RID: 1149
		// (set) Token: 0x0600047E RID: 1150
		MethodCallingConvention CallingConvention { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600047F RID: 1151
		bool HasParameters { get; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000480 RID: 1152
		Collection<ParameterDefinition> Parameters { get; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000481 RID: 1153
		// (set) Token: 0x06000482 RID: 1154
		TypeReference ReturnType { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000483 RID: 1155
		MethodReturnType MethodReturnType { get; }
	}
}
