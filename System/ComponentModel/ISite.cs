using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x02000576 RID: 1398
	[ComVisible(true)]
	public interface ISite : IServiceProvider
	{
		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x060033D9 RID: 13273
		IComponent Component { get; }

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x060033DA RID: 13274
		IContainer Container { get; }

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x060033DB RID: 13275
		bool DesignMode { get; }

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x060033DC RID: 13276
		// (set) Token: 0x060033DD RID: 13277
		string Name { get; set; }
	}
}
