using System;

namespace System.ComponentModel
{
	// Token: 0x02000575 RID: 1397
	[global::__DynamicallyInvokable]
	public interface IRevertibleChangeTracking : IChangeTracking
	{
		// Token: 0x060033D8 RID: 13272
		[global::__DynamicallyInvokable]
		void RejectChanges();
	}
}
