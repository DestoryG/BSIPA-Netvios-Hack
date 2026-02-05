using System;

namespace System.ComponentModel
{
	// Token: 0x02000560 RID: 1376
	public interface IDataErrorInfo
	{
		// Token: 0x17000C9C RID: 3228
		string this[string columnName] { get; }

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06003397 RID: 13207
		string Error { get; }
	}
}
