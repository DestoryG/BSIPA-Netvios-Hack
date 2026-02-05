using System;
using System.Collections;

namespace System.ComponentModel
{
	// Token: 0x02000569 RID: 1385
	[global::__DynamicallyInvokable]
	public interface INotifyDataErrorInfo
	{
		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x060033AA RID: 13226
		[global::__DynamicallyInvokable]
		bool HasErrors
		{
			[global::__DynamicallyInvokable]
			get;
		}

		// Token: 0x060033AB RID: 13227
		[global::__DynamicallyInvokable]
		IEnumerable GetErrors(string propertyName);

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x060033AC RID: 13228
		// (remove) Token: 0x060033AD RID: 13229
		[global::__DynamicallyInvokable]
		event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
	}
}
