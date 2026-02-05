using System;

namespace System.Net.Sockets
{
	// Token: 0x0200039F RID: 927
	internal class ReceiveFromOverlappedAsyncResult : OverlappedAsyncResult
	{
		// Token: 0x06002297 RID: 8855 RVA: 0x000A4D29 File Offset: 0x000A2F29
		internal ReceiveFromOverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback)
			: base(socket, asyncState, asyncCallback)
		{
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x000A4D34 File Offset: 0x000A2F34
		internal override object PostCompletion(int numBytes)
		{
			base.SocketAddress.SetSize(base.GetSocketAddressSizePtr());
			return base.PostCompletion(numBytes);
		}
	}
}
