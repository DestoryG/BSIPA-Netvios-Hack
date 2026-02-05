using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x0200057A RID: 1402
	[ComVisible(true)]
	public interface ITypeDescriptorContext : IServiceProvider
	{
		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x060033E7 RID: 13287
		IContainer Container { get; }

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x060033E8 RID: 13288
		object Instance { get; }

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x060033E9 RID: 13289
		PropertyDescriptor PropertyDescriptor { get; }

		// Token: 0x060033EA RID: 13290
		bool OnComponentChanging();

		// Token: 0x060033EB RID: 13291
		void OnComponentChanged();
	}
}
