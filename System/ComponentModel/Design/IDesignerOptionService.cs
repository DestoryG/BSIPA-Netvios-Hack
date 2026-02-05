using System;

namespace System.ComponentModel.Design
{
	// Token: 0x020005EB RID: 1515
	public interface IDesignerOptionService
	{
		// Token: 0x06003818 RID: 14360
		object GetOptionValue(string pageName, string valueName);

		// Token: 0x06003819 RID: 14361
		void SetOptionValue(string pageName, string valueName, object value);
	}
}
