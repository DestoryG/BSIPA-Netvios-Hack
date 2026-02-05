using System;

namespace System.ComponentModel
{
	// Token: 0x0200055B RID: 1371
	[global::__DynamicallyInvokable]
	public interface IChangeTracking
	{
		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06003374 RID: 13172
		[global::__DynamicallyInvokable]
		bool IsChanged
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x06003375 RID: 13173
		[global::__DynamicallyInvokable]
		void AcceptChanges();
	}
}
