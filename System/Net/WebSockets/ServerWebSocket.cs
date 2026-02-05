using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Net.WebSockets
{
	// Token: 0x0200022E RID: 558
	internal sealed class ServerWebSocket : WebSocketBase
	{
		// Token: 0x060014A8 RID: 5288 RVA: 0x0006C7D0 File Offset: 0x0006A9D0
		public ServerWebSocket(Stream innerStream, string subProtocol, int receiveBufferSize, TimeSpan keepAliveInterval, ArraySegment<byte> internalBuffer)
			: base(innerStream, subProtocol, keepAliveInterval, WebSocketBuffer.CreateServerBuffer(internalBuffer, receiveBufferSize))
		{
			this.m_Properties = base.InternalBuffer.CreateProperties(false);
			this.m_SessionHandle = this.CreateWebSocketHandle();
			if (this.m_SessionHandle == null || this.m_SessionHandle.IsInvalid)
			{
				WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
			}
			base.StartKeepAliveTimer();
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x060014A9 RID: 5289 RVA: 0x0006C82D File Offset: 0x0006AA2D
		internal override SafeHandle SessionHandle
		{
			get
			{
				return this.m_SessionHandle;
			}
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0006C838 File Offset: 0x0006AA38
		private SafeHandle CreateWebSocketHandle()
		{
			SafeWebSocketHandle safeWebSocketHandle;
			WebSocketProtocolComponent.WebSocketCreateServerHandle(this.m_Properties, this.m_Properties.Length, out safeWebSocketHandle);
			return safeWebSocketHandle;
		}

		// Token: 0x04001649 RID: 5705
		private readonly SafeHandle m_SessionHandle;

		// Token: 0x0400164A RID: 5706
		private readonly WebSocketProtocolComponent.Property[] m_Properties;
	}
}
