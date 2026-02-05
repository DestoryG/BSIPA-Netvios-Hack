using System;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x0200038B RID: 907
	internal class AcceptOverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x0600220D RID: 8717 RVA: 0x000A2E42 File Offset: 0x000A1042
		internal AcceptOverlappedAsyncResult(Socket listenSocket, object asyncState, AsyncCallback asyncCallback)
			: base(listenSocket, asyncState, asyncCallback)
		{
			this.m_ListenSocket = listenSocket;
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000A2E54 File Offset: 0x000A1054
		internal override object PostCompletion(int numBytes)
		{
			SocketError socketError = (SocketError)base.ErrorCode;
			SocketAddress socketAddress = null;
			if (socketError == SocketError.Success)
			{
				this.m_LocalBytesTransferred = numBytes;
				if (Logging.On)
				{
					this.LogBuffer((long)numBytes);
				}
				socketAddress = this.m_ListenSocket.m_RightEndPoint.Serialize();
				try
				{
					IntPtr intPtr;
					int num;
					IntPtr intPtr2;
					this.m_ListenSocket.GetAcceptExSockaddrs(Marshal.UnsafeAddrOfPinnedArrayElement(this.m_Buffer, 0), this.m_Buffer.Length - this.m_AddressBufferLength * 2, this.m_AddressBufferLength, this.m_AddressBufferLength, out intPtr, out num, out intPtr2, out socketAddress.m_Size);
					Marshal.Copy(intPtr2, socketAddress.m_Buffer, 0, socketAddress.m_Size);
					IntPtr intPtr3 = this.m_ListenSocket.SafeHandle.DangerousGetHandle();
					socketError = UnsafeNclNativeMethods.OSSOCK.setsockopt(this.m_AcceptSocket.SafeHandle, SocketOptionLevel.Socket, SocketOptionName.UpdateAcceptContext, ref intPtr3, Marshal.SizeOf(intPtr3));
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
				return this.m_ListenSocket.UpdateAcceptSocket(this.m_AcceptSocket, this.m_ListenSocket.m_RightEndPoint.Create(socketAddress), false);
			}
			return null;
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x000A2F7C File Offset: 0x000A117C
		internal void SetUnmanagedStructures(byte[] buffer, int addressBufferLength)
		{
			base.SetUnmanagedStructures(buffer);
			this.m_AddressBufferLength = addressBufferLength;
			this.m_Buffer = buffer;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x000A2F94 File Offset: 0x000A1194
		private void LogBuffer(long size)
		{
			IntPtr intPtr = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_Buffer, 0);
			if (intPtr != IntPtr.Zero)
			{
				if (size > -1L)
				{
					Logging.Dump(Logging.Sockets, this.m_ListenSocket, "PostCompletion", intPtr, (int)Math.Min(size, (long)this.m_Buffer.Length));
					return;
				}
				Logging.Dump(Logging.Sockets, this.m_ListenSocket, "PostCompletion", intPtr, this.m_Buffer.Length);
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002211 RID: 8721 RVA: 0x000A3005 File Offset: 0x000A1205
		internal byte[] Buffer
		{
			get
			{
				return this.m_Buffer;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002212 RID: 8722 RVA: 0x000A300D File Offset: 0x000A120D
		internal int BytesTransferred
		{
			get
			{
				return this.m_LocalBytesTransferred;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (set) Token: 0x06002213 RID: 8723 RVA: 0x000A3015 File Offset: 0x000A1215
		internal Socket AcceptSocket
		{
			set
			{
				this.m_AcceptSocket = value;
			}
		}

		// Token: 0x04001F4F RID: 8015
		private int m_LocalBytesTransferred;

		// Token: 0x04001F50 RID: 8016
		private Socket m_ListenSocket;

		// Token: 0x04001F51 RID: 8017
		private Socket m_AcceptSocket;

		// Token: 0x04001F52 RID: 8018
		private int m_AddressBufferLength;

		// Token: 0x04001F53 RID: 8019
		private byte[] m_Buffer;
	}
}
