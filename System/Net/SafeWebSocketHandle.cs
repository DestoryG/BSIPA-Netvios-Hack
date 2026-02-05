using System;
using System.Net.WebSockets;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000207 RID: 519
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeWebSocketHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600136C RID: 4972 RVA: 0x000661A6 File Offset: 0x000643A6
		internal SafeWebSocketHandle()
			: base(true)
		{
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x000661AF File Offset: 0x000643AF
		protected override bool ReleaseHandle()
		{
			if (this.IsInvalid)
			{
				return true;
			}
			WebSocketProtocolComponent.WebSocketDeleteHandle(this.handle);
			return true;
		}
	}
}
