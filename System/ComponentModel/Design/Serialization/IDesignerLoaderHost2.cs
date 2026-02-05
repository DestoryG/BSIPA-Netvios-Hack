using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000609 RID: 1545
	public interface IDesignerLoaderHost2 : IDesignerLoaderHost, IDesignerHost, IServiceContainer, IServiceProvider
	{
		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x060038AC RID: 14508
		// (set) Token: 0x060038AD RID: 14509
		bool IgnoreErrorsDuringReload { get; set; }

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x060038AE RID: 14510
		// (set) Token: 0x060038AF RID: 14511
		bool CanReloadWithErrors { get; set; }
	}
}
