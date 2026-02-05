using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x02000188 RID: 392
	internal class WebProxyData
	{
		// Token: 0x04001278 RID: 4728
		internal bool bypassOnLocal;

		// Token: 0x04001279 RID: 4729
		internal bool automaticallyDetectSettings;

		// Token: 0x0400127A RID: 4730
		internal Uri proxyAddress;

		// Token: 0x0400127B RID: 4731
		internal Hashtable proxyHostAddresses;

		// Token: 0x0400127C RID: 4732
		internal Uri scriptLocation;

		// Token: 0x0400127D RID: 4733
		internal ArrayList bypassList;
	}
}
