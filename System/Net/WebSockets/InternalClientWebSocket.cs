using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Net.WebSockets
{
	// Token: 0x0200022C RID: 556
	internal sealed class InternalClientWebSocket : WebSocketBase
	{
		// Token: 0x06001497 RID: 5271 RVA: 0x0006C5F4 File Offset: 0x0006A7F4
		public InternalClientWebSocket(Stream innerStream, string subProtocol, int receiveBufferSize, int sendBufferSize, TimeSpan keepAliveInterval, bool useZeroMaskingKey, ArraySegment<byte> internalBuffer)
			: base(innerStream, subProtocol, keepAliveInterval, WebSocketBuffer.CreateClientBuffer(internalBuffer, receiveBufferSize, sendBufferSize))
		{
			this.m_Properties = base.InternalBuffer.CreateProperties(useZeroMaskingKey);
			this.m_SessionHandle = this.CreateWebSocketHandle();
			if (this.m_SessionHandle == null || this.m_SessionHandle.IsInvalid)
			{
				WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
			}
			base.StartKeepAliveTimer();
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x0006C654 File Offset: 0x0006A854
		internal override SafeHandle SessionHandle
		{
			get
			{
				return this.m_SessionHandle;
			}
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0006C65C File Offset: 0x0006A85C
		private SafeHandle CreateWebSocketHandle()
		{
			SafeWebSocketHandle safeWebSocketHandle;
			WebSocketProtocolComponent.WebSocketCreateClientHandle(this.m_Properties, out safeWebSocketHandle);
			return safeWebSocketHandle;
		}

		// Token: 0x0400163B RID: 5691
		private readonly SafeHandle m_SessionHandle;

		// Token: 0x0400163C RID: 5692
		private readonly WebSocketProtocolComponent.Property[] m_Properties;
	}
}
