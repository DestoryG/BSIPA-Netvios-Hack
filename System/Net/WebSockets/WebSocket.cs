using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets
{
	// Token: 0x0200022F RID: 559
	public abstract class WebSocket : IDisposable
	{
		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x060014AB RID: 5291
		public abstract WebSocketCloseStatus? CloseStatus { get; }

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060014AC RID: 5292
		public abstract string CloseStatusDescription { get; }

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060014AD RID: 5293
		public abstract string SubProtocol { get; }

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060014AE RID: 5294
		public abstract WebSocketState State { get; }

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0006C85B File Offset: 0x0006AA5B
		public static TimeSpan DefaultKeepAliveInterval
		{
			get
			{
				if (WebSocket.defaultKeepAliveInterval == null)
				{
					if (WebSocketProtocolComponent.IsSupported)
					{
						WebSocket.defaultKeepAliveInterval = new TimeSpan?(WebSocketProtocolComponent.WebSocketGetDefaultKeepAliveInterval());
					}
					else
					{
						WebSocket.defaultKeepAliveInterval = new TimeSpan?(Timeout.InfiniteTimeSpan);
					}
				}
				return WebSocket.defaultKeepAliveInterval.Value;
			}
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x0006C89A File Offset: 0x0006AA9A
		public static ArraySegment<byte> CreateClientBuffer(int receiveBufferSize, int sendBufferSize)
		{
			WebSocketHelpers.ValidateBufferSizes(receiveBufferSize, sendBufferSize);
			return WebSocketBuffer.CreateInternalBufferArraySegment(receiveBufferSize, sendBufferSize, false);
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0006C8AB File Offset: 0x0006AAAB
		public static ArraySegment<byte> CreateServerBuffer(int receiveBufferSize)
		{
			WebSocketHelpers.ValidateBufferSizes(receiveBufferSize, 16);
			return WebSocketBuffer.CreateInternalBufferArraySegment(receiveBufferSize, 16, true);
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x0006C8C0 File Offset: 0x0006AAC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static WebSocket CreateClientWebSocket(Stream innerStream, string subProtocol, int receiveBufferSize, int sendBufferSize, TimeSpan keepAliveInterval, bool useZeroMaskingKey, ArraySegment<byte> internalBuffer)
		{
			if (!WebSocketProtocolComponent.IsSupported)
			{
				WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
			}
			WebSocketHelpers.ValidateInnerStream(innerStream);
			WebSocketHelpers.ValidateOptions(subProtocol, receiveBufferSize, sendBufferSize, keepAliveInterval);
			WebSocketHelpers.ValidateArraySegment<byte>(internalBuffer, "internalBuffer");
			WebSocketBuffer.Validate(internalBuffer.Count, receiveBufferSize, sendBufferSize, false);
			return new InternalClientWebSocket(innerStream, subProtocol, receiveBufferSize, sendBufferSize, keepAliveInterval, useZeroMaskingKey, internalBuffer);
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x0006C914 File Offset: 0x0006AB14
		internal static WebSocket CreateServerWebSocket(Stream innerStream, string subProtocol, int receiveBufferSize, TimeSpan keepAliveInterval, ArraySegment<byte> internalBuffer)
		{
			if (!WebSocketProtocolComponent.IsSupported)
			{
				WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
			}
			WebSocketHelpers.ValidateInnerStream(innerStream);
			WebSocketHelpers.ValidateOptions(subProtocol, receiveBufferSize, 16, keepAliveInterval);
			WebSocketHelpers.ValidateArraySegment<byte>(internalBuffer, "internalBuffer");
			WebSocketBuffer.Validate(internalBuffer.Count, receiveBufferSize, 16, true);
			return new ServerWebSocket(innerStream, subProtocol, receiveBufferSize, keepAliveInterval, internalBuffer);
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0006C964 File Offset: 0x0006AB64
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RegisterPrefixes()
		{
			WebRequest.RegisterPrefix(Uri.UriSchemeWs + ":", new WebSocketHttpRequestCreator(false));
			WebRequest.RegisterPrefix(Uri.UriSchemeWss + ":", new WebSocketHttpRequestCreator(true));
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0006C99C File Offset: 0x0006AB9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.")]
		public static bool IsApplicationTargeting45()
		{
			return BinaryCompatibility.TargetsAtLeast_Desktop_V4_5;
		}

		// Token: 0x060014B6 RID: 5302
		public abstract void Abort();

		// Token: 0x060014B7 RID: 5303
		public abstract Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);

		// Token: 0x060014B8 RID: 5304
		public abstract Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);

		// Token: 0x060014B9 RID: 5305
		public abstract void Dispose();

		// Token: 0x060014BA RID: 5306
		public abstract Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);

		// Token: 0x060014BB RID: 5307
		public abstract Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken);

		// Token: 0x060014BC RID: 5308 RVA: 0x0006C9A4 File Offset: 0x0006ABA4
		protected static void ThrowOnInvalidState(WebSocketState state, params WebSocketState[] validStates)
		{
			string text = string.Empty;
			if (validStates != null && validStates.Length != 0)
			{
				foreach (WebSocketState webSocketState in validStates)
				{
					if (state == webSocketState)
					{
						return;
					}
				}
				text = string.Join<WebSocketState>(", ", validStates);
			}
			throw new WebSocketException(SR.GetString("net_WebSockets_InvalidState", new object[] { state, text }));
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0006CA05 File Offset: 0x0006AC05
		protected static bool IsStateTerminal(WebSocketState state)
		{
			return state == WebSocketState.Closed || state == WebSocketState.Aborted;
		}

		// Token: 0x0400164B RID: 5707
		private static TimeSpan? defaultKeepAliveInterval;
	}
}
