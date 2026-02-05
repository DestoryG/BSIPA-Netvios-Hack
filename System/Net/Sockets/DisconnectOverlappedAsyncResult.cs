using System;

namespace System.Net.Sockets
{
	// Token: 0x0200038F RID: 911
	internal class DisconnectOverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x06002232 RID: 8754 RVA: 0x000A3870 File Offset: 0x000A1A70
		internal DisconnectOverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback)
			: base(socket, asyncState, asyncCallback)
		{
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000A387C File Offset: 0x000A1A7C
		internal override object PostCompletion(int numBytes)
		{
			if (base.ErrorCode == 0)
			{
				Socket socket = (Socket)base.AsyncObject;
				socket.SetToDisconnected();
				socket.m_RemoteEndPoint = null;
			}
			return base.PostCompletion(numBytes);
		}
	}
}
