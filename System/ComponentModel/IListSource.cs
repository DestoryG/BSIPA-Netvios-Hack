using System;
using System.Collections;

namespace System.ComponentModel
{
	// Token: 0x02000564 RID: 1380
	[TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Editor("System.Windows.Forms.Design.DataSourceListEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[MergableProperty(false)]
	public interface IListSource
	{
		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x0600339E RID: 13214
		bool ContainsListCollection { get; }

		// Token: 0x0600339F RID: 13215
		IList GetList();
	}
}
