using System;

namespace System.ComponentModel
{
	// Token: 0x02000563 RID: 1379
	public interface IIntellisenseBuilder
	{
		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x0600339C RID: 13212
		string Name { get; }

		// Token: 0x0600339D RID: 13213
		bool Show(string language, string value, ref string newValue);
	}
}
