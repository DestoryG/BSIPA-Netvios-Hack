using System;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x0200038E RID: 910
	internal class ConnectOverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x0600222F RID: 8751 RVA: 0x000A37E0 File Offset: 0x000A19E0
		internal ConnectOverlappedAsyncResult(Socket socket, EndPoint endPoint, object asyncState, AsyncCallback asyncCallback)
			: base(socket, asyncState, asyncCallback)
		{
			this.m_EndPoint = endPoint;
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000A37F4 File Offset: 0x000A19F4
		internal override object PostCompletion(int numBytes)
		{
			SocketError socketError = (SocketError)base.ErrorCode;
			Socket socket = (Socket)base.AsyncObject;
			if (socketError == SocketError.Success)
			{
				try
				{
					socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(socket.SafeHandle, SocketOptionLevel.Socket, SocketOptionName.UpdateConnectContext, null, 0);
					if (socketError == SocketError.SocketError)
					{
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
				}
				catch (ObjectDisposedException)
				{
					socketError = SocketError.OperationAborted;
				}
				base.ErrorCode = (int)socketError;
			}
			if (socketError == SocketError.Success)
			{
				socket.SetToConnected();
				return socket;
			}
			return null;
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002231 RID: 8753 RVA: 0x000A3868 File Offset: 0x000A1A68
		internal EndPoint RemoteEndPoint
		{
			get
			{
				return this.m_EndPoint;
			}
		}

		// Token: 0x04001F60 RID: 8032
		private EndPoint m_EndPoint;
	}
}
