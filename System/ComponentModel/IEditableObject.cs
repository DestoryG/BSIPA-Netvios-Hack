using System;

namespace System.ComponentModel
{
	// Token: 0x02000561 RID: 1377
	[global::__DynamicallyInvokable]
	public interface IEditableObject
	{
		// Token: 0x06003398 RID: 13208
		[global::__DynamicallyInvokable]
		void BeginEdit();

		// Token: 0x06003399 RID: 13209
		[global::__DynamicallyInvokable]
		void EndEdit();

		// Token: 0x0600339A RID: 13210
		[global::__DynamicallyInvokable]
		void CancelEdit();
	}
}
