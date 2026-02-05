using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Specialized
{
	// Token: 0x020003AB RID: 939
	[TypeForwardedFrom("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
	[global::__DynamicallyInvokable]
	public interface INotifyCollectionChanged
	{
		// Token: 0x1400002A RID: 42
		// (add) Token: 0x0600230D RID: 8973
		// (remove) Token: 0x0600230E RID: 8974
		[global::__DynamicallyInvokable]
		event NotifyCollectionChangedEventHandler CollectionChanged;
	}
}
