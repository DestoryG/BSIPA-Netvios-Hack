using System;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x0200014F RID: 335
	internal interface IWebProxyFinder : IDisposable
	{
		// Token: 0x06000BAB RID: 2987
		bool GetProxies(Uri destination, out IList<string> proxyList);

		// Token: 0x06000BAC RID: 2988
		void Abort();

		// Token: 0x06000BAD RID: 2989
		void Reset();

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000BAE RID: 2990
		bool IsValid { get; }
	}
}
