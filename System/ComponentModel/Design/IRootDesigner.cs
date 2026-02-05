using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x020005F5 RID: 1525
	[ComVisible(true)]
	public interface IRootDesigner : IDesigner, IDisposable
	{
		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06003840 RID: 14400
		ViewTechnology[] SupportedTechnologies { get; }

		// Token: 0x06003841 RID: 14401
		object GetView(ViewTechnology technology);
	}
}
