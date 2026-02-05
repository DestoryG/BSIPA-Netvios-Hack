using System;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x0200039E RID: 926
	internal class ReceiveMessageOverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x0600228C RID: 8844 RVA: 0x000A491A File Offset: 0x000A2B1A
		internal ReceiveMessageOverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback)
			: base(socket, asyncState, asyncCallback)
		{
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000A4925 File Offset: 0x000A2B25
		internal IntPtr GetSocketAddressSizePtr()
		{
			return Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, this.m_SocketAddress.GetAddressSizeOffset());
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x0600228E RID: 8846 RVA: 0x000A4942 File Offset: 0x000A2B42
		internal SocketAddress SocketAddress
		{
			get
			{
				return this.m_SocketAddress;
			}
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x000A494C File Offset: 0x000A2B4C
		internal unsafe void SetUnmanagedStructures(byte[] buffer, int offset, int size, SocketAddress socketAddress, SocketFlags socketFlags)
		{
			this.m_MessageBuffer = new byte[ReceiveMessageOverlappedAsyncResult.s_WSAMsgSize];
			this.m_WSABufferArray = new byte[ReceiveMessageOverlappedAsyncResult.s_WSABufferSize];
			IPAddress ipaddress = ((socketAddress.Family == AddressFamily.InterNetworkV6) ? socketAddress.GetIPAddress() : null);
			bool flag = ((Socket)base.AsyncObject).AddressFamily == AddressFamily.InterNetwork || (ipaddress != null && ipaddress.IsIPv4MappedToIPv6);
			bool flag2 = ((Socket)base.AsyncObject).AddressFamily == AddressFamily.InterNetworkV6;
			if (flag)
			{
				this.m_ControlBuffer = new byte[ReceiveMessageOverlappedAsyncResult.s_ControlDataSize];
			}
			else if (flag2)
			{
				this.m_ControlBuffer = new byte[ReceiveMessageOverlappedAsyncResult.s_ControlDataIPv6Size];
			}
			object[] array = new object[(this.m_ControlBuffer != null) ? 5 : 4];
			array[0] = buffer;
			array[1] = this.m_MessageBuffer;
			array[2] = this.m_WSABufferArray;
			this.m_SocketAddress = socketAddress;
			this.m_SocketAddress.CopyAddressSizeIntoBuffer();
			array[3] = this.m_SocketAddress.m_Buffer;
			if (this.m_ControlBuffer != null)
			{
				array[4] = this.m_ControlBuffer;
			}
			base.SetUnmanagedStructures(array);
			this.m_WSABuffer = (WSABuffer*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSABufferArray, 0);
			this.m_WSABuffer->Length = size;
			this.m_WSABuffer->Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
			this.m_Message = (UnsafeNclNativeMethods.OSSOCK.WSAMsg*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(this.m_MessageBuffer, 0);
			this.m_Message->socketAddress = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, 0);
			this.m_Message->addressLength = (uint)this.m_SocketAddress.Size;
			this.m_Message->buffers = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_WSABufferArray, 0);
			this.m_Message->count = 1U;
			if (this.m_ControlBuffer != null)
			{
				this.m_Message->controlBuffer.Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(this.m_ControlBuffer, 0);
				this.m_Message->controlBuffer.Length = this.m_ControlBuffer.Length;
			}
			this.m_Message->flags = socketFlags;
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x000A4B3A File Offset: 0x000A2D3A
		internal void SetUnmanagedStructures(byte[] buffer, int offset, int size, SocketAddress socketAddress, SocketFlags socketFlags, ref OverlappedCache overlappedCache)
		{
			base.SetupCache(ref overlappedCache);
			this.SetUnmanagedStructures(buffer, offset, size, socketAddress, socketFlags);
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x000A4B54 File Offset: 0x000A2D54
		private unsafe void InitIPPacketInformation()
		{
			IPAddress ipaddress = null;
			if (this.m_ControlBuffer.Length == ReceiveMessageOverlappedAsyncResult.s_ControlDataSize)
			{
				UnsafeNclNativeMethods.OSSOCK.ControlData controlData = (UnsafeNclNativeMethods.OSSOCK.ControlData)Marshal.PtrToStructure(this.m_Message->controlBuffer.Pointer, typeof(UnsafeNclNativeMethods.OSSOCK.ControlData));
				if (controlData.length != UIntPtr.Zero)
				{
					ipaddress = new IPAddress((long)((ulong)controlData.address));
				}
				this.m_IPPacketInformation = new IPPacketInformation((ipaddress != null) ? ipaddress : IPAddress.None, (int)controlData.index);
				return;
			}
			if (this.m_ControlBuffer.Length == ReceiveMessageOverlappedAsyncResult.s_ControlDataIPv6Size)
			{
				UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6 controlDataIPv = (UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6)Marshal.PtrToStructure(this.m_Message->controlBuffer.Pointer, typeof(UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6));
				if (controlDataIPv.length != UIntPtr.Zero)
				{
					ipaddress = new IPAddress(controlDataIPv.address);
				}
				this.m_IPPacketInformation = new IPPacketInformation((ipaddress != null) ? ipaddress : IPAddress.IPv6None, (int)controlDataIPv.index);
				return;
			}
			this.m_IPPacketInformation = default(IPPacketInformation);
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x000A4C4E File Offset: 0x000A2E4E
		internal void SyncReleaseUnmanagedStructures()
		{
			this.InitIPPacketInformation();
			this.ForceReleaseUnmanagedStructures();
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x000A4C5C File Offset: 0x000A2E5C
		protected unsafe override void ForceReleaseUnmanagedStructures()
		{
			this.m_flags = this.m_Message->flags;
			base.ForceReleaseUnmanagedStructures();
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x000A4C75 File Offset: 0x000A2E75
		internal override object PostCompletion(int numBytes)
		{
			this.InitIPPacketInformation();
			if (base.ErrorCode == 0 && Logging.On)
			{
				this.LogBuffer(numBytes);
			}
			return numBytes;
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x000A4C99 File Offset: 0x000A2E99
		private unsafe void LogBuffer(int size)
		{
			Logging.Dump(Logging.Sockets, base.AsyncObject, "PostCompletion", this.m_WSABuffer->Pointer, Math.Min(this.m_WSABuffer->Length, size));
		}

		// Token: 0x04001F80 RID: 8064
		private unsafe UnsafeNclNativeMethods.OSSOCK.WSAMsg* m_Message;

		// Token: 0x04001F81 RID: 8065
		internal SocketAddress SocketAddressOriginal;

		// Token: 0x04001F82 RID: 8066
		internal SocketAddress m_SocketAddress;

		// Token: 0x04001F83 RID: 8067
		private unsafe WSABuffer* m_WSABuffer;

		// Token: 0x04001F84 RID: 8068
		private byte[] m_WSABufferArray;

		// Token: 0x04001F85 RID: 8069
		private byte[] m_ControlBuffer;

		// Token: 0x04001F86 RID: 8070
		internal byte[] m_MessageBuffer;

		// Token: 0x04001F87 RID: 8071
		internal SocketFlags m_flags;

		// Token: 0x04001F88 RID: 8072
		private static readonly int s_ControlDataSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.ControlData));

		// Token: 0x04001F89 RID: 8073
		private static readonly int s_ControlDataIPv6Size = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.ControlDataIPv6));

		// Token: 0x04001F8A RID: 8074
		private static readonly int s_WSABufferSize = Marshal.SizeOf(typeof(WSABuffer));

		// Token: 0x04001F8B RID: 8075
		private static readonly int s_WSAMsgSize = Marshal.SizeOf(typeof(UnsafeNclNativeMethods.OSSOCK.WSAMsg));

		// Token: 0x04001F8C RID: 8076
		internal IPPacketInformation m_IPPacketInformation;
	}
}
