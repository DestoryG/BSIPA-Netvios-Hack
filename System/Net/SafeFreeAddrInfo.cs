using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x020001E7 RID: 487
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeAddrInfo : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060012E0 RID: 4832 RVA: 0x00063F30 File Offset: 0x00062130
		private SafeFreeAddrInfo()
			: base(true)
		{
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00063F39 File Offset: 0x00062139
		internal static int GetAddrInfo(string nodename, string servicename, ref AddressInfo hints, out SafeFreeAddrInfo outAddrInfo)
		{
			return UnsafeNclNativeMethods.SafeNetHandlesXPOrLater.GetAddrInfoW(nodename, servicename, ref hints, out outAddrInfo);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00063F44 File Offset: 0x00062144
		protected override bool ReleaseHandle()
		{
			UnsafeNclNativeMethods.SafeNetHandlesXPOrLater.freeaddrinfo(this.handle);
			return true;
		}

		// Token: 0x04001529 RID: 5417
		private const string WS2_32 = "ws2_32.dll";
	}
}
