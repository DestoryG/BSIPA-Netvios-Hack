using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Net.Sockets
{
	// Token: 0x02000396 RID: 918
	// (Invoke) Token: 0x06002254 RID: 8788
	[SuppressUnmanagedCodeSecurity]
	internal delegate SocketError WSARecvMsgDelegate(SafeCloseSocket socketHandle, IntPtr msg, out int bytesTransferred, SafeHandle overlapped, IntPtr completionRoutine);
}
