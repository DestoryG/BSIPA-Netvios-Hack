using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x02000390 RID: 912
	internal sealed class DynamicWinsockMethods
	{
		// Token: 0x06002234 RID: 8756 RVA: 0x000A38B4 File Offset: 0x000A1AB4
		public static DynamicWinsockMethods GetMethods(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
		{
			List<DynamicWinsockMethods> list = DynamicWinsockMethods.s_MethodTable;
			DynamicWinsockMethods dynamicWinsockMethods2;
			lock (list)
			{
				DynamicWinsockMethods dynamicWinsockMethods;
				for (int i = 0; i < DynamicWinsockMethods.s_MethodTable.Count; i++)
				{
					dynamicWinsockMethods = DynamicWinsockMethods.s_MethodTable[i];
					if (dynamicWinsockMethods.addressFamily == addressFamily && dynamicWinsockMethods.socketType == socketType && dynamicWinsockMethods.protocolType == protocolType)
					{
						return dynamicWinsockMethods;
					}
				}
				dynamicWinsockMethods = new DynamicWinsockMethods(addressFamily, socketType, protocolType);
				DynamicWinsockMethods.s_MethodTable.Add(dynamicWinsockMethods);
				dynamicWinsockMethods2 = dynamicWinsockMethods;
			}
			return dynamicWinsockMethods2;
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000A3948 File Offset: 0x000A1B48
		private DynamicWinsockMethods(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
		{
			this.addressFamily = addressFamily;
			this.socketType = socketType;
			this.protocolType = protocolType;
			this.lockObject = new object();
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000A3970 File Offset: 0x000A1B70
		public T GetDelegate<T>(SafeCloseSocket socketHandle) where T : class
		{
			if (typeof(T) == typeof(AcceptExDelegate))
			{
				this.EnsureAcceptEx(socketHandle);
				return (T)((object)this.acceptEx);
			}
			if (typeof(T) == typeof(GetAcceptExSockaddrsDelegate))
			{
				this.EnsureGetAcceptExSockaddrs(socketHandle);
				return (T)((object)this.getAcceptExSockaddrs);
			}
			if (typeof(T) == typeof(ConnectExDelegate))
			{
				this.EnsureConnectEx(socketHandle);
				return (T)((object)this.connectEx);
			}
			if (typeof(T) == typeof(DisconnectExDelegate))
			{
				this.EnsureDisconnectEx(socketHandle);
				return (T)((object)this.disconnectEx);
			}
			if (typeof(T) == typeof(DisconnectExDelegate_Blocking))
			{
				this.EnsureDisconnectEx(socketHandle);
				return (T)((object)this.disconnectEx_Blocking);
			}
			if (typeof(T) == typeof(WSARecvMsgDelegate))
			{
				this.EnsureWSARecvMsg(socketHandle);
				return (T)((object)this.recvMsg);
			}
			if (typeof(T) == typeof(WSARecvMsgDelegate_Blocking))
			{
				this.EnsureWSARecvMsg(socketHandle);
				return (T)((object)this.recvMsg_Blocking);
			}
			if (typeof(T) == typeof(TransmitPacketsDelegate))
			{
				this.EnsureTransmitPackets(socketHandle);
				return (T)((object)this.transmitPackets);
			}
			return default(T);
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x000A3AF8 File Offset: 0x000A1CF8
		private unsafe IntPtr LoadDynamicFunctionPointer(SafeCloseSocket socketHandle, ref Guid guid)
		{
			IntPtr zero = IntPtr.Zero;
			int num;
			SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl(socketHandle, -939524090, ref guid, sizeof(Guid), out zero, sizeof(IntPtr), out num, IntPtr.Zero, IntPtr.Zero);
			if (socketError != SocketError.Success)
			{
				throw new SocketException();
			}
			return zero;
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x000A3B3C File Offset: 0x000A1D3C
		private void EnsureAcceptEx(SafeCloseSocket socketHandle)
		{
			if (this.acceptEx == null)
			{
				object obj = this.lockObject;
				lock (obj)
				{
					if (this.acceptEx == null)
					{
						Guid guid = new Guid("{0xb5367df1,0xcbac,0x11cf,{0x95, 0xca, 0x00, 0x80, 0x5f, 0x48, 0xa1, 0x92}}");
						IntPtr intPtr = this.LoadDynamicFunctionPointer(socketHandle, ref guid);
						this.acceptEx = (AcceptExDelegate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(AcceptExDelegate));
					}
				}
			}
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000A3BB8 File Offset: 0x000A1DB8
		private void EnsureGetAcceptExSockaddrs(SafeCloseSocket socketHandle)
		{
			if (this.getAcceptExSockaddrs == null)
			{
				object obj = this.lockObject;
				lock (obj)
				{
					if (this.getAcceptExSockaddrs == null)
					{
						Guid guid = new Guid("{0xb5367df2,0xcbac,0x11cf,{0x95, 0xca, 0x00, 0x80, 0x5f, 0x48, 0xa1, 0x92}}");
						IntPtr intPtr = this.LoadDynamicFunctionPointer(socketHandle, ref guid);
						this.getAcceptExSockaddrs = (GetAcceptExSockaddrsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(GetAcceptExSockaddrsDelegate));
					}
				}
			}
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000A3C34 File Offset: 0x000A1E34
		private void EnsureConnectEx(SafeCloseSocket socketHandle)
		{
			if (this.connectEx == null)
			{
				object obj = this.lockObject;
				lock (obj)
				{
					if (this.connectEx == null)
					{
						Guid guid = new Guid("{0x25a207b9,0x0ddf3,0x4660,{0x8e,0xe9,0x76,0xe5,0x8c,0x74,0x06,0x3e}}");
						IntPtr intPtr = this.LoadDynamicFunctionPointer(socketHandle, ref guid);
						this.connectEx = (ConnectExDelegate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(ConnectExDelegate));
					}
				}
			}
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000A3CB0 File Offset: 0x000A1EB0
		private void EnsureDisconnectEx(SafeCloseSocket socketHandle)
		{
			if (this.disconnectEx == null)
			{
				object obj = this.lockObject;
				lock (obj)
				{
					if (this.disconnectEx == null)
					{
						Guid guid = new Guid("{0x7fda2e11,0x8630,0x436f,{0xa0, 0x31, 0xf5, 0x36, 0xa6, 0xee, 0xc1, 0x57}}");
						IntPtr intPtr = this.LoadDynamicFunctionPointer(socketHandle, ref guid);
						this.disconnectEx = (DisconnectExDelegate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(DisconnectExDelegate));
						this.disconnectEx_Blocking = (DisconnectExDelegate_Blocking)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(DisconnectExDelegate_Blocking));
					}
				}
			}
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000A3D48 File Offset: 0x000A1F48
		private void EnsureWSARecvMsg(SafeCloseSocket socketHandle)
		{
			if (this.recvMsg == null)
			{
				object obj = this.lockObject;
				lock (obj)
				{
					if (this.recvMsg == null)
					{
						Guid guid = new Guid("{0xf689d7c8,0x6f1f,0x436b,{0x8a,0x53,0xe5,0x4f,0xe3,0x51,0xc3,0x22}}");
						IntPtr intPtr = this.LoadDynamicFunctionPointer(socketHandle, ref guid);
						this.recvMsg = (WSARecvMsgDelegate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WSARecvMsgDelegate));
						this.recvMsg_Blocking = (WSARecvMsgDelegate_Blocking)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(WSARecvMsgDelegate_Blocking));
					}
				}
			}
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000A3DE0 File Offset: 0x000A1FE0
		private void EnsureTransmitPackets(SafeCloseSocket socketHandle)
		{
			if (this.transmitPackets == null)
			{
				object obj = this.lockObject;
				lock (obj)
				{
					if (this.transmitPackets == null)
					{
						Guid guid = new Guid("{0xd9689da0,0x1f90,0x11d3,{0x99,0x71,0x00,0xc0,0x4f,0x68,0xc8,0x76}}");
						IntPtr intPtr = this.LoadDynamicFunctionPointer(socketHandle, ref guid);
						this.transmitPackets = (TransmitPacketsDelegate)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(TransmitPacketsDelegate));
					}
				}
			}
		}

		// Token: 0x04001F61 RID: 8033
		private static List<DynamicWinsockMethods> s_MethodTable = new List<DynamicWinsockMethods>();

		// Token: 0x04001F62 RID: 8034
		private AddressFamily addressFamily;

		// Token: 0x04001F63 RID: 8035
		private SocketType socketType;

		// Token: 0x04001F64 RID: 8036
		private ProtocolType protocolType;

		// Token: 0x04001F65 RID: 8037
		private object lockObject;

		// Token: 0x04001F66 RID: 8038
		private AcceptExDelegate acceptEx;

		// Token: 0x04001F67 RID: 8039
		private GetAcceptExSockaddrsDelegate getAcceptExSockaddrs;

		// Token: 0x04001F68 RID: 8040
		private ConnectExDelegate connectEx;

		// Token: 0x04001F69 RID: 8041
		private TransmitPacketsDelegate transmitPackets;

		// Token: 0x04001F6A RID: 8042
		private DisconnectExDelegate disconnectEx;

		// Token: 0x04001F6B RID: 8043
		private DisconnectExDelegate_Blocking disconnectEx_Blocking;

		// Token: 0x04001F6C RID: 8044
		private WSARecvMsgDelegate recvMsg;

		// Token: 0x04001F6D RID: 8045
		private WSARecvMsgDelegate_Blocking recvMsg_Blocking;
	}
}
