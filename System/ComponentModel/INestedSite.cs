using System;

namespace System.ComponentModel
{
	// Token: 0x02000567 RID: 1383
	public interface INestedSite : ISite, IServiceProvider
	{
		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x060033A7 RID: 13223
		string FullName { get; }
	}
}
