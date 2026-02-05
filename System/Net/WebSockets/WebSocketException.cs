using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.WebSockets
{
	// Token: 0x02000236 RID: 566
	[Serializable]
	public sealed class WebSocketException : Win32Exception
	{
		// Token: 0x06001531 RID: 5425 RVA: 0x0006E8A1 File Offset: 0x0006CAA1
		public WebSocketException()
			: this(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0006E8AE File Offset: 0x0006CAAE
		public WebSocketException(WebSocketError error)
			: this(error, WebSocketException.GetErrorMessage(error))
		{
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x0006E8BD File Offset: 0x0006CABD
		public WebSocketException(WebSocketError error, string message)
			: base(message)
		{
			this.m_WebSocketErrorCode = error;
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0006E8CD File Offset: 0x0006CACD
		public WebSocketException(WebSocketError error, Exception innerException)
			: this(error, WebSocketException.GetErrorMessage(error), innerException)
		{
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0006E8DD File Offset: 0x0006CADD
		public WebSocketException(WebSocketError error, string message, Exception innerException)
			: base(message, innerException)
		{
			this.m_WebSocketErrorCode = error;
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x0006E8EE File Offset: 0x0006CAEE
		public WebSocketException(int nativeError)
			: base(nativeError)
		{
			this.m_WebSocketErrorCode = ((!WebSocketProtocolComponent.Succeeded(nativeError)) ? WebSocketError.NativeError : WebSocketError.Success);
			this.SetErrorCodeOnError(nativeError);
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x0006E910 File Offset: 0x0006CB10
		public WebSocketException(int nativeError, string message)
			: base(nativeError, message)
		{
			this.m_WebSocketErrorCode = ((!WebSocketProtocolComponent.Succeeded(nativeError)) ? WebSocketError.NativeError : WebSocketError.Success);
			this.SetErrorCodeOnError(nativeError);
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x0006E933 File Offset: 0x0006CB33
		public WebSocketException(int nativeError, Exception innerException)
			: base(SR.GetString("net_WebSockets_Generic"), innerException)
		{
			this.m_WebSocketErrorCode = ((!WebSocketProtocolComponent.Succeeded(nativeError)) ? WebSocketError.NativeError : WebSocketError.Success);
			this.SetErrorCodeOnError(nativeError);
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0006E95F File Offset: 0x0006CB5F
		public WebSocketException(WebSocketError error, int nativeError)
			: this(error, nativeError, WebSocketException.GetErrorMessage(error))
		{
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0006E96F File Offset: 0x0006CB6F
		public WebSocketException(WebSocketError error, int nativeError, string message)
			: base(message)
		{
			this.m_WebSocketErrorCode = error;
			this.SetErrorCodeOnError(nativeError);
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0006E986 File Offset: 0x0006CB86
		public WebSocketException(WebSocketError error, int nativeError, Exception innerException)
			: this(error, nativeError, WebSocketException.GetErrorMessage(error), innerException)
		{
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0006E997 File Offset: 0x0006CB97
		public WebSocketException(WebSocketError error, int nativeError, string message, Exception innerException)
			: base(message, innerException)
		{
			this.m_WebSocketErrorCode = error;
			this.SetErrorCodeOnError(nativeError);
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0006E9B0 File Offset: 0x0006CBB0
		public WebSocketException(string message)
			: base(message)
		{
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0006E9B9 File Offset: 0x0006CBB9
		public WebSocketException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0006E9C3 File Offset: 0x0006CBC3
		private WebSocketException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x0006E9CD File Offset: 0x0006CBCD
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x0006E9D5 File Offset: 0x0006CBD5
		public WebSocketError WebSocketErrorCode
		{
			get
			{
				return this.m_WebSocketErrorCode;
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0006E9E0 File Offset: 0x0006CBE0
		private static string GetErrorMessage(WebSocketError error)
		{
			switch (error)
			{
			case WebSocketError.InvalidMessageType:
				return SR.GetString("net_WebSockets_InvalidMessageType_Generic", new object[]
				{
					typeof(WebSocket).Name + "CloseAsync",
					typeof(WebSocket).Name + "CloseOutputAsync"
				});
			case WebSocketError.Faulted:
				return SR.GetString("net_Websockets_WebSocketBaseFaulted");
			case WebSocketError.NotAWebSocket:
				return SR.GetString("net_WebSockets_NotAWebSocket_Generic");
			case WebSocketError.UnsupportedVersion:
				return SR.GetString("net_WebSockets_UnsupportedWebSocketVersion_Generic");
			case WebSocketError.UnsupportedProtocol:
				return SR.GetString("net_WebSockets_UnsupportedProtocol_Generic");
			case WebSocketError.HeaderError:
				return SR.GetString("net_WebSockets_HeaderError_Generic");
			case WebSocketError.ConnectionClosedPrematurely:
				return SR.GetString("net_WebSockets_ConnectionClosedPrematurely_Generic");
			case WebSocketError.InvalidState:
				return SR.GetString("net_WebSockets_InvalidState_Generic");
			}
			return SR.GetString("net_WebSockets_Generic");
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0006EABE File Offset: 0x0006CCBE
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("WebSocketErrorCode", this.m_WebSocketErrorCode);
			base.GetObjectData(info, context);
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0006EAEC File Offset: 0x0006CCEC
		private void SetErrorCodeOnError(int nativeError)
		{
			if (!WebSocketProtocolComponent.Succeeded(nativeError))
			{
				base.HResult = nativeError;
			}
		}

		// Token: 0x040016A0 RID: 5792
		private WebSocketError m_WebSocketErrorCode;
	}
}
