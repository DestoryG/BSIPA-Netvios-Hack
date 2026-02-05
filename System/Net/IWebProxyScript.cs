using System;

namespace System.Net
{
	// Token: 0x02000193 RID: 403
	public interface IWebProxyScript
	{
		// Token: 0x06000F9D RID: 3997
		bool Load(Uri scriptLocation, string script, Type helperType);

		// Token: 0x06000F9E RID: 3998
		string Run(string url, string host);

		// Token: 0x06000F9F RID: 3999
		void Close();
	}
}
