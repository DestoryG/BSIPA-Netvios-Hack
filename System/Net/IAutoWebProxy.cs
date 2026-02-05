using System;

namespace System.Net
{
	// Token: 0x020001DE RID: 478
	internal interface IAutoWebProxy : IWebProxy
	{
		// Token: 0x060012B9 RID: 4793
		ProxyChain GetProxies(Uri destination);
	}
}
