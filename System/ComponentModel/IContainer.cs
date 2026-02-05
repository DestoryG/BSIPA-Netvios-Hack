using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x0200055E RID: 1374
	[ComVisible(true)]
	public interface IContainer : IDisposable
	{
		// Token: 0x06003386 RID: 13190
		void Add(IComponent component);

		// Token: 0x06003387 RID: 13191
		void Add(IComponent component, string name);

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06003388 RID: 13192
		ComponentCollection Components { get; }

		// Token: 0x06003389 RID: 13193
		void Remove(IComponent component);
	}
}
