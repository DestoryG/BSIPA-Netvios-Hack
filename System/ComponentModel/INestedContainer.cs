using System;

namespace System.ComponentModel
{
	// Token: 0x02000566 RID: 1382
	public interface INestedContainer : IContainer, IDisposable
	{
		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x060033A6 RID: 13222
		IComponent Owner { get; }
	}
}
