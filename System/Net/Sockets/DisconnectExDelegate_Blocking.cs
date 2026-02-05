using System;
using System.Security;

namespace System.Net.Sockets
{
	// Token: 0x02000395 RID: 917
	// (Invoke) Token: 0x06002250 RID: 8784
	[SuppressUnmanagedCodeSecurity]
	internal delegate bool DisconnectExDelegate_Blocking(IntPtr socketHandle, IntPtr overlapped, int flags, int reserved);
}
