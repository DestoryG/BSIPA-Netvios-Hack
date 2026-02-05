using System;

namespace System.Net
{
	// Token: 0x020001A5 RID: 421
	internal struct WriteHeadersCallbackState
	{
		// Token: 0x06001056 RID: 4182 RVA: 0x000575C8 File Offset: 0x000557C8
		internal WriteHeadersCallbackState(HttpWebRequest request, ConnectStream stream)
		{
			this.request = request;
			this.stream = stream;
		}

		// Token: 0x04001375 RID: 4981
		internal HttpWebRequest request;

		// Token: 0x04001376 RID: 4982
		internal ConnectStream stream;
	}
}
