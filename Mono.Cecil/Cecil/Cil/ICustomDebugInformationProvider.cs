using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000121 RID: 289
	public interface ICustomDebugInformationProvider : IMetadataTokenProvider
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000B11 RID: 2833
		bool HasCustomDebugInformations { get; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000B12 RID: 2834
		Collection<CustomDebugInformation> CustomDebugInformations { get; }
	}
}
