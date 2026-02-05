using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000200 RID: 512
	[SuppressUnmanagedCodeSecurity]
	internal class SafeCloseSocket : SafeHandleMinusOneIsInvalid
	{
		// Token: 0x06001344 RID: 4932 RVA: 0x00065BDA File Offset: 0x00063DDA
		protected SafeCloseSocket()
			: base(true)
		{
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001345 RID: 4933 RVA: 0x00065BE3 File Offset: 0x00063DE3
		public override bool IsInvalid
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return base.IsClosed || base.IsInvalid;
			}
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00065BF5 File Offset: 0x00063DF5
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void SetInnerSocket(SafeCloseSocket.InnerSafeCloseSocket socket)
		{
			this.m_InnerSocket = socket;
			base.SetHandle(socket.DangerousGetHandle());
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00065C0C File Offset: 0x00063E0C
		private static SafeCloseSocket CreateSocket(SafeCloseSocket.InnerSafeCloseSocket socket)
		{
			SafeCloseSocket safeCloseSocket = new SafeCloseSocket();
			SafeCloseSocket.CreateSocket(socket, safeCloseSocket);
			return safeCloseSocket;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00065C28 File Offset: 0x00063E28
		protected static void CreateSocket(SafeCloseSocket.InnerSafeCloseSocket socket, SafeCloseSocket target)
		{
			if (socket != null && socket.IsInvalid)
			{
				target.SetHandleAsInvalid();
				return;
			}
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				socket.DangerousAddRef(ref flag);
			}
			catch
			{
				if (flag)
				{
					socket.DangerousRelease();
					flag = false;
				}
			}
			finally
			{
				if (flag)
				{
					target.SetInnerSocket(socket);
					socket.Close();
				}
				else
				{
					target.SetHandleAsInvalid();
				}
			}
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00065C9C File Offset: 0x00063E9C
		internal unsafe static SafeCloseSocket CreateWSASocket(byte* pinnedBuffer)
		{
			return SafeCloseSocket.CreateSocket(SafeCloseSocket.InnerSafeCloseSocket.CreateWSASocket(pinnedBuffer));
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00065CA9 File Offset: 0x00063EA9
		internal static SafeCloseSocket CreateWSASocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
		{
			return SafeCloseSocket.CreateSocket(SafeCloseSocket.InnerSafeCloseSocket.CreateWSASocket(addressFamily, socketType, protocolType));
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00065CB8 File Offset: 0x00063EB8
		internal static SafeCloseSocket Accept(SafeCloseSocket socketHandle, byte[] socketAddress, ref int socketAddressSize)
		{
			return SafeCloseSocket.CreateSocket(SafeCloseSocket.InnerSafeCloseSocket.Accept(socketHandle, socketAddress, ref socketAddressSize));
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00065CC8 File Offset: 0x00063EC8
		protected override bool ReleaseHandle()
		{
			this.m_Released = true;
			SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = ((this.m_InnerSocket == null) ? null : Interlocked.Exchange<SafeCloseSocket.InnerSafeCloseSocket>(ref this.m_InnerSocket, null));
			if (innerSafeCloseSocket != null)
			{
				innerSafeCloseSocket.DangerousRelease();
			}
			return true;
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00065D00 File Offset: 0x00063F00
		internal void CloseAsIs()
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = ((this.m_InnerSocket == null) ? null : Interlocked.Exchange<SafeCloseSocket.InnerSafeCloseSocket>(ref this.m_InnerSocket, null));
				base.Close();
				if (innerSafeCloseSocket != null)
				{
					while (!this.m_Released)
					{
						Thread.SpinWait(1);
					}
					innerSafeCloseSocket.BlockingRelease();
				}
			}
		}

		// Token: 0x0400154F RID: 5455
		private SafeCloseSocket.InnerSafeCloseSocket m_InnerSocket;

		// Token: 0x04001550 RID: 5456
		private volatile bool m_Released;

		// Token: 0x02000757 RID: 1879
		internal class InnerSafeCloseSocket : SafeHandleMinusOneIsInvalid
		{
			// Token: 0x060041FF RID: 16895 RVA: 0x00112246 File Offset: 0x00110446
			protected InnerSafeCloseSocket()
				: base(true)
			{
			}

			// Token: 0x17000F15 RID: 3861
			// (get) Token: 0x06004200 RID: 16896 RVA: 0x0011224F File Offset: 0x0011044F
			public override bool IsInvalid
			{
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
				get
				{
					return base.IsClosed || base.IsInvalid;
				}
			}

			// Token: 0x06004201 RID: 16897 RVA: 0x00112264 File Offset: 0x00110464
			protected override bool ReleaseHandle()
			{
				SocketError socketError;
				if (this.m_Blockable)
				{
					socketError = UnsafeNclNativeMethods.SafeNetHandles.closesocket(this.handle);
					if (socketError == SocketError.SocketError)
					{
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
					if (socketError != SocketError.WouldBlock)
					{
						return socketError == SocketError.Success;
					}
					int num = 0;
					socketError = UnsafeNclNativeMethods.SafeNetHandles.ioctlsocket(this.handle, -2147195266, ref num);
					if (socketError == SocketError.SocketError)
					{
						socketError = (SocketError)Marshal.GetLastWin32Error();
					}
					if (socketError == SocketError.InvalidArgument)
					{
						socketError = UnsafeNclNativeMethods.SafeNetHandles.WSAEventSelect(this.handle, IntPtr.Zero, AsyncEventBits.FdNone);
						socketError = UnsafeNclNativeMethods.SafeNetHandles.ioctlsocket(this.handle, -2147195266, ref num);
					}
					if (socketError == SocketError.Success)
					{
						socketError = UnsafeNclNativeMethods.SafeNetHandles.closesocket(this.handle);
						if (socketError == SocketError.SocketError)
						{
							socketError = (SocketError)Marshal.GetLastWin32Error();
						}
						if (socketError != SocketError.WouldBlock)
						{
							return socketError == SocketError.Success;
						}
					}
				}
				Linger linger;
				linger.OnOff = 1;
				linger.Time = 0;
				socketError = UnsafeNclNativeMethods.SafeNetHandles.setsockopt(this.handle, SocketOptionLevel.Socket, SocketOptionName.Linger, ref linger, 4);
				if (socketError == SocketError.SocketError)
				{
					socketError = (SocketError)Marshal.GetLastWin32Error();
				}
				if (socketError != SocketError.Success && socketError != SocketError.InvalidArgument && socketError != SocketError.ProtocolOption)
				{
					return false;
				}
				socketError = UnsafeNclNativeMethods.SafeNetHandles.closesocket(this.handle);
				return socketError == SocketError.Success;
			}

			// Token: 0x06004202 RID: 16898 RVA: 0x00112373 File Offset: 0x00110573
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			internal void BlockingRelease()
			{
				this.m_Blockable = true;
				base.DangerousRelease();
			}

			// Token: 0x06004203 RID: 16899 RVA: 0x00112384 File Offset: 0x00110584
			internal unsafe static SafeCloseSocket.InnerSafeCloseSocket CreateWSASocket(byte* pinnedBuffer)
			{
				SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = UnsafeNclNativeMethods.OSSOCK.WSASocket(AddressFamily.Unknown, SocketType.Unknown, ProtocolType.Unknown, pinnedBuffer, 0U, SocketConstructorFlags.WSA_FLAG_OVERLAPPED);
				if (innerSafeCloseSocket.IsInvalid)
				{
					innerSafeCloseSocket.SetHandleAsInvalid();
				}
				return innerSafeCloseSocket;
			}

			// Token: 0x06004204 RID: 16900 RVA: 0x001123AC File Offset: 0x001105AC
			internal static SafeCloseSocket.InnerSafeCloseSocket CreateWSASocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
			{
				SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = UnsafeNclNativeMethods.OSSOCK.WSASocket(addressFamily, socketType, protocolType, IntPtr.Zero, 0U, SocketConstructorFlags.WSA_FLAG_OVERLAPPED);
				if (innerSafeCloseSocket.IsInvalid)
				{
					innerSafeCloseSocket.SetHandleAsInvalid();
				}
				return innerSafeCloseSocket;
			}

			// Token: 0x06004205 RID: 16901 RVA: 0x001123D8 File Offset: 0x001105D8
			internal static SafeCloseSocket.InnerSafeCloseSocket Accept(SafeCloseSocket socketHandle, byte[] socketAddress, ref int socketAddressSize)
			{
				SafeCloseSocket.InnerSafeCloseSocket innerSafeCloseSocket = UnsafeNclNativeMethods.SafeNetHandles.accept(socketHandle.DangerousGetHandle(), socketAddress, ref socketAddressSize);
				if (innerSafeCloseSocket.IsInvalid)
				{
					innerSafeCloseSocket.SetHandleAsInvalid();
				}
				return innerSafeCloseSocket;
			}

			// Token: 0x04003212 RID: 12818
			private static readonly byte[] tempBuffer = new byte[1];

			// Token: 0x04003213 RID: 12819
			private bool m_Blockable;
		}
	}
}
