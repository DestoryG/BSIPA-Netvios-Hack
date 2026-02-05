using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E5 RID: 485
	internal interface ICustomDebugInformationProvider : IMetadataTokenProvider
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000EF8 RID: 3832
		bool HasCustomDebugInformations { get; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000EF9 RID: 3833
		Collection<CustomDebugInformation> CustomDebugInformations { get; }
	}
}
