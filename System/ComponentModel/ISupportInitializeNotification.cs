using System;

namespace System.ComponentModel
{
	// Token: 0x02000578 RID: 1400
	public interface ISupportInitializeNotification : ISupportInitialize
	{
		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x060033E0 RID: 13280
		bool IsInitialized { get; }

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x060033E1 RID: 13281
		// (remove) Token: 0x060033E2 RID: 13282
		event EventHandler Initialized;
	}
}
