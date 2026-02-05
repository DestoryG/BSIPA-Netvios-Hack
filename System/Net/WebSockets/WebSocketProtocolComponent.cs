using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Net.WebSockets
{
	// Token: 0x0200023A RID: 570
	internal static class WebSocketProtocolComponent
	{
		// Token: 0x06001582 RID: 5506 RVA: 0x0006FD54 File Offset: 0x0006DF54
		[SecuritySafeCritical]
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		static WebSocketProtocolComponent()
		{
			if (!WebSocketProtocolComponent.s_WebSocketDllHandle.IsInvalid)
			{
				WebSocketProtocolComponent.s_SupportedVersion = WebSocketProtocolComponent.GetSupportedVersion();
				WebSocketProtocolComponent.s_ServerFakeRequestHeaders = new WebSocketProtocolComponent.HttpHeader[]
				{
					new WebSocketProtocolComponent.HttpHeader
					{
						Name = "Connection",
						NameLength = (uint)"Connection".Length,
						Value = "Upgrade",
						ValueLength = (uint)"Upgrade".Length
					},
					new WebSocketProtocolComponent.HttpHeader
					{
						Name = "Upgrade",
						NameLength = (uint)"Upgrade".Length,
						Value = "websocket",
						ValueLength = (uint)"websocket".Length
					},
					new WebSocketProtocolComponent.HttpHeader
					{
						Name = "Host",
						NameLength = (uint)"Host".Length,
						Value = string.Empty,
						ValueLength = 0U
					},
					new WebSocketProtocolComponent.HttpHeader
					{
						Name = "Sec-WebSocket-Version",
						NameLength = (uint)"Sec-WebSocket-Version".Length,
						Value = WebSocketProtocolComponent.s_SupportedVersion,
						ValueLength = (uint)WebSocketProtocolComponent.s_SupportedVersion.Length
					},
					new WebSocketProtocolComponent.HttpHeader
					{
						Name = "Sec-WebSocket-Key",
						NameLength = (uint)"Sec-WebSocket-Key".Length,
						Value = WebSocketProtocolComponent.s_DummyWebsocketKeyBase64,
						ValueLength = (uint)WebSocketProtocolComponent.s_DummyWebsocketKeyBase64.Length
					}
				};
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001583 RID: 5507 RVA: 0x0006FFC1 File Offset: 0x0006E1C1
		internal static string SupportedVersion
		{
			get
			{
				if (WebSocketProtocolComponent.s_WebSocketDllHandle.IsInvalid)
				{
					WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
				}
				return WebSocketProtocolComponent.s_SupportedVersion;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x0006FFD9 File Offset: 0x0006E1D9
		internal static bool IsSupported
		{
			get
			{
				return !WebSocketProtocolComponent.s_WebSocketDllHandle.IsInvalid;
			}
		}

		// Token: 0x06001585 RID: 5509
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketCreateClientHandle", ExactSpelling = true)]
		private static extern int WebSocketCreateClientHandle_Raw([In] WebSocketProtocolComponent.Property[] properties, [In] uint propertyCount, out SafeWebSocketHandle webSocketHandle);

		// Token: 0x06001586 RID: 5510
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketBeginClientHandshake", ExactSpelling = true)]
		private static extern int WebSocketBeginClientHandshake_Raw([In] SafeHandle webSocketHandle, [In] IntPtr subProtocols, [In] uint subProtocolCount, [In] IntPtr extensions, [In] uint extensionCount, [In] WebSocketProtocolComponent.HttpHeader[] initialHeaders, [In] uint initialHeaderCount, out IntPtr additionalHeadersPtr, out uint additionalHeaderCount);

		// Token: 0x06001587 RID: 5511
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketEndClientHandshake", ExactSpelling = true)]
		private static extern int WebSocketEndClientHandshake_Raw([In] SafeHandle webSocketHandle, [In] WebSocketProtocolComponent.HttpHeader[] responseHeaders, [In] uint responseHeaderCount, [In] [Out] IntPtr selectedExtensions, [In] IntPtr selectedExtensionCount, [In] IntPtr selectedSubProtocol);

		// Token: 0x06001588 RID: 5512
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketBeginServerHandshake", ExactSpelling = true)]
		private static extern int WebSocketBeginServerHandshake_Raw([In] SafeHandle webSocketHandle, [In] IntPtr subProtocol, [In] IntPtr extensions, [In] uint extensionCount, [In] WebSocketProtocolComponent.HttpHeader[] requestHeaders, [In] uint requestHeaderCount, out IntPtr responseHeadersPtr, out uint responseHeaderCount);

		// Token: 0x06001589 RID: 5513
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketEndServerHandshake", ExactSpelling = true)]
		private static extern int WebSocketEndServerHandshake_Raw([In] SafeHandle webSocketHandle);

		// Token: 0x0600158A RID: 5514
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketCreateServerHandle", ExactSpelling = true)]
		private static extern int WebSocketCreateServerHandle_Raw([In] WebSocketProtocolComponent.Property[] properties, [In] uint propertyCount, out SafeWebSocketHandle webSocketHandle);

		// Token: 0x0600158B RID: 5515
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketAbortHandle", ExactSpelling = true)]
		private static extern void WebSocketAbortHandle_Raw([In] SafeHandle webSocketHandle);

		// Token: 0x0600158C RID: 5516
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketDeleteHandle", ExactSpelling = true)]
		private static extern void WebSocketDeleteHandle_Raw([In] IntPtr webSocketHandle);

		// Token: 0x0600158D RID: 5517
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketSend", ExactSpelling = true)]
		private static extern int WebSocketSend_Raw([In] SafeHandle webSocketHandle, [In] WebSocketProtocolComponent.BufferType bufferType, [In] ref WebSocketProtocolComponent.Buffer buffer, [In] IntPtr applicationContext);

		// Token: 0x0600158E RID: 5518
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketSend", ExactSpelling = true)]
		private static extern int WebSocketSendWithoutBody_Raw([In] SafeHandle webSocketHandle, [In] WebSocketProtocolComponent.BufferType bufferType, [In] IntPtr buffer, [In] IntPtr applicationContext);

		// Token: 0x0600158F RID: 5519
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketReceive", ExactSpelling = true)]
		private static extern int WebSocketReceive_Raw([In] SafeHandle webSocketHandle, [In] IntPtr buffers, [In] IntPtr applicationContext);

		// Token: 0x06001590 RID: 5520
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketGetAction", ExactSpelling = true)]
		private static extern int WebSocketGetAction_Raw([In] SafeHandle webSocketHandle, [In] WebSocketProtocolComponent.ActionQueue actionQueue, [In] [Out] WebSocketProtocolComponent.Buffer[] dataBuffers, [In] [Out] ref uint dataBufferCount, out WebSocketProtocolComponent.Action action, out WebSocketProtocolComponent.BufferType bufferType, out IntPtr applicationContext, out IntPtr actionContext);

		// Token: 0x06001591 RID: 5521
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketCompleteAction", ExactSpelling = true)]
		private static extern void WebSocketCompleteAction_Raw([In] SafeHandle webSocketHandle, [In] IntPtr actionContext, [In] uint bytesTransferred);

		// Token: 0x06001592 RID: 5522
		[SuppressUnmanagedCodeSecurity]
		[DllImport("websocket.dll", EntryPoint = "WebSocketGetGlobalProperty", ExactSpelling = true)]
		private static extern int WebSocketGetGlobalProperty_Raw([In] WebSocketProtocolComponent.PropertyType property, [In] [Out] ref uint value, [In] [Out] ref uint size);

		// Token: 0x06001593 RID: 5523 RVA: 0x0006FFE8 File Offset: 0x0006E1E8
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static string GetSupportedVersion()
		{
			if (WebSocketProtocolComponent.s_WebSocketDllHandle.IsInvalid)
			{
				WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
			}
			SafeWebSocketHandle safeWebSocketHandle = null;
			string text2;
			try
			{
				int num = WebSocketProtocolComponent.WebSocketCreateClientHandle_Raw(null, 0U, out safeWebSocketHandle);
				WebSocketProtocolComponent.ThrowOnError(num);
				if (safeWebSocketHandle == null || safeWebSocketHandle.IsInvalid)
				{
					WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
				}
				IntPtr intPtr;
				uint num2;
				num = WebSocketProtocolComponent.WebSocketBeginClientHandshake_Raw(safeWebSocketHandle, IntPtr.Zero, 0U, IntPtr.Zero, 0U, WebSocketProtocolComponent.s_InitialClientRequestHeaders, (uint)WebSocketProtocolComponent.s_InitialClientRequestHeaders.Length, out intPtr, out num2);
				WebSocketProtocolComponent.ThrowOnError(num);
				WebSocketProtocolComponent.HttpHeader[] array = WebSocketProtocolComponent.MarshalHttpHeaders(intPtr, (int)num2);
				string text = null;
				foreach (WebSocketProtocolComponent.HttpHeader httpHeader in array)
				{
					if (string.Compare(httpHeader.Name, "Sec-WebSocket-Version", StringComparison.OrdinalIgnoreCase) == 0)
					{
						text = httpHeader.Value;
						break;
					}
				}
				text2 = text;
			}
			finally
			{
				if (safeWebSocketHandle != null)
				{
					safeWebSocketHandle.Dispose();
				}
			}
			return text2;
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x000700C0 File Offset: 0x0006E2C0
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void WebSocketCreateClientHandle(WebSocketProtocolComponent.Property[] properties, out SafeWebSocketHandle webSocketHandle)
		{
			uint num = (uint)((properties == null) ? 0 : properties.Length);
			if (WebSocketProtocolComponent.s_WebSocketDllHandle.IsInvalid)
			{
				WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
			}
			int num2 = WebSocketProtocolComponent.WebSocketCreateClientHandle_Raw(properties, num, out webSocketHandle);
			WebSocketProtocolComponent.ThrowOnError(num2);
			if (webSocketHandle == null || webSocketHandle.IsInvalid)
			{
				WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
			}
			IntPtr intPtr;
			uint num3;
			num2 = WebSocketProtocolComponent.WebSocketBeginClientHandshake_Raw(webSocketHandle, IntPtr.Zero, 0U, IntPtr.Zero, 0U, WebSocketProtocolComponent.s_InitialClientRequestHeaders, (uint)WebSocketProtocolComponent.s_InitialClientRequestHeaders.Length, out intPtr, out num3);
			WebSocketProtocolComponent.ThrowOnError(num2);
			WebSocketProtocolComponent.HttpHeader[] array = WebSocketProtocolComponent.MarshalHttpHeaders(intPtr, (int)num3);
			string text = null;
			foreach (WebSocketProtocolComponent.HttpHeader httpHeader in array)
			{
				if (string.Compare(httpHeader.Name, "Sec-WebSocket-Key", StringComparison.OrdinalIgnoreCase) == 0)
				{
					text = httpHeader.Value;
					break;
				}
			}
			string secWebSocketAcceptString = WebSocketHelpers.GetSecWebSocketAcceptString(text);
			WebSocketProtocolComponent.HttpHeader[] array3 = new WebSocketProtocolComponent.HttpHeader[]
			{
				new WebSocketProtocolComponent.HttpHeader
				{
					Name = "Connection",
					NameLength = (uint)"Connection".Length,
					Value = "Upgrade",
					ValueLength = (uint)"Upgrade".Length
				},
				new WebSocketProtocolComponent.HttpHeader
				{
					Name = "Upgrade",
					NameLength = (uint)"Upgrade".Length,
					Value = "websocket",
					ValueLength = (uint)"websocket".Length
				},
				new WebSocketProtocolComponent.HttpHeader
				{
					Name = "Sec-WebSocket-Accept",
					NameLength = (uint)"Sec-WebSocket-Accept".Length,
					Value = secWebSocketAcceptString,
					ValueLength = (uint)secWebSocketAcceptString.Length
				}
			};
			num2 = WebSocketProtocolComponent.WebSocketEndClientHandshake_Raw(webSocketHandle, array3, (uint)array3.Length, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			WebSocketProtocolComponent.ThrowOnError(num2);
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x00070290 File Offset: 0x0006E490
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void WebSocketCreateServerHandle(WebSocketProtocolComponent.Property[] properties, int propertyCount, out SafeWebSocketHandle webSocketHandle)
		{
			if (WebSocketProtocolComponent.s_WebSocketDllHandle.IsInvalid)
			{
				WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
			}
			int num = WebSocketProtocolComponent.WebSocketCreateServerHandle_Raw(properties, (uint)propertyCount, out webSocketHandle);
			WebSocketProtocolComponent.ThrowOnError(num);
			if (webSocketHandle == null || webSocketHandle.IsInvalid)
			{
				WebSocketHelpers.ThrowPlatformNotSupportedException_WSPC();
			}
			IntPtr intPtr;
			uint num2;
			num = WebSocketProtocolComponent.WebSocketBeginServerHandshake_Raw(webSocketHandle, IntPtr.Zero, IntPtr.Zero, 0U, WebSocketProtocolComponent.s_ServerFakeRequestHeaders, (uint)WebSocketProtocolComponent.s_ServerFakeRequestHeaders.Length, out intPtr, out num2);
			WebSocketProtocolComponent.ThrowOnError(num);
			WebSocketProtocolComponent.HttpHeader[] array = WebSocketProtocolComponent.MarshalHttpHeaders(intPtr, (int)num2);
			num = WebSocketProtocolComponent.WebSocketEndServerHandshake_Raw(webSocketHandle);
			WebSocketProtocolComponent.ThrowOnError(num);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0007030E File Offset: 0x0006E50E
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void WebSocketAbortHandle(SafeHandle webSocketHandle)
		{
			WebSocketProtocolComponent.WebSocketAbortHandle_Raw(webSocketHandle);
			WebSocketProtocolComponent.DrainActionQueue(webSocketHandle, WebSocketProtocolComponent.ActionQueue.Send);
			WebSocketProtocolComponent.DrainActionQueue(webSocketHandle, WebSocketProtocolComponent.ActionQueue.Receive);
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00070324 File Offset: 0x0006E524
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void WebSocketDeleteHandle(IntPtr webSocketPtr)
		{
			WebSocketProtocolComponent.WebSocketDeleteHandle_Raw(webSocketPtr);
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0007032C File Offset: 0x0006E52C
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void WebSocketSend(WebSocketBase webSocket, WebSocketProtocolComponent.BufferType bufferType, WebSocketProtocolComponent.Buffer buffer)
		{
			WebSocketProtocolComponent.ThrowIfSessionHandleClosed(webSocket);
			int num;
			try
			{
				num = WebSocketProtocolComponent.WebSocketSend_Raw(webSocket.SessionHandle, bufferType, ref buffer, IntPtr.Zero);
			}
			catch (ObjectDisposedException ex)
			{
				throw WebSocketProtocolComponent.ConvertObjectDisposedException(webSocket, ex);
			}
			WebSocketProtocolComponent.ThrowOnError(num);
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00070374 File Offset: 0x0006E574
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void WebSocketSendWithoutBody(WebSocketBase webSocket, WebSocketProtocolComponent.BufferType bufferType)
		{
			WebSocketProtocolComponent.ThrowIfSessionHandleClosed(webSocket);
			int num;
			try
			{
				num = WebSocketProtocolComponent.WebSocketSendWithoutBody_Raw(webSocket.SessionHandle, bufferType, IntPtr.Zero, IntPtr.Zero);
			}
			catch (ObjectDisposedException ex)
			{
				throw WebSocketProtocolComponent.ConvertObjectDisposedException(webSocket, ex);
			}
			WebSocketProtocolComponent.ThrowOnError(num);
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x000703C0 File Offset: 0x0006E5C0
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void WebSocketReceive(WebSocketBase webSocket)
		{
			WebSocketProtocolComponent.ThrowIfSessionHandleClosed(webSocket);
			int num;
			try
			{
				num = WebSocketProtocolComponent.WebSocketReceive_Raw(webSocket.SessionHandle, IntPtr.Zero, IntPtr.Zero);
			}
			catch (ObjectDisposedException ex)
			{
				throw WebSocketProtocolComponent.ConvertObjectDisposedException(webSocket, ex);
			}
			WebSocketProtocolComponent.ThrowOnError(num);
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x0007040C File Offset: 0x0006E60C
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void WebSocketGetAction(WebSocketBase webSocket, WebSocketProtocolComponent.ActionQueue actionQueue, WebSocketProtocolComponent.Buffer[] dataBuffers, ref uint dataBufferCount, out WebSocketProtocolComponent.Action action, out WebSocketProtocolComponent.BufferType bufferType, out IntPtr actionContext)
		{
			action = WebSocketProtocolComponent.Action.NoAction;
			bufferType = WebSocketProtocolComponent.BufferType.None;
			actionContext = IntPtr.Zero;
			WebSocketProtocolComponent.ThrowIfSessionHandleClosed(webSocket);
			int num;
			try
			{
				IntPtr intPtr;
				num = WebSocketProtocolComponent.WebSocketGetAction_Raw(webSocket.SessionHandle, actionQueue, dataBuffers, ref dataBufferCount, out action, out bufferType, out intPtr, out actionContext);
			}
			catch (ObjectDisposedException ex)
			{
				throw WebSocketProtocolComponent.ConvertObjectDisposedException(webSocket, ex);
			}
			WebSocketProtocolComponent.ThrowOnError(num);
			webSocket.ValidateNativeBuffers(action, bufferType, dataBuffers, dataBufferCount);
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x00070478 File Offset: 0x0006E678
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static void WebSocketCompleteAction(WebSocketBase webSocket, IntPtr actionContext, int bytesTransferred)
		{
			if (webSocket.SessionHandle.IsClosed)
			{
				return;
			}
			try
			{
				WebSocketProtocolComponent.WebSocketCompleteAction_Raw(webSocket.SessionHandle, actionContext, (uint)bytesTransferred);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x000704B8 File Offset: 0x0006E6B8
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal static TimeSpan WebSocketGetDefaultKeepAliveInterval()
		{
			uint num = 0U;
			uint num2 = 4U;
			int num3 = WebSocketProtocolComponent.WebSocketGetGlobalProperty_Raw(WebSocketProtocolComponent.PropertyType.KeepAliveInterval, ref num, ref num2);
			if (!WebSocketProtocolComponent.Succeeded(num3))
			{
				return Timeout.InfiniteTimeSpan;
			}
			return TimeSpan.FromMilliseconds(num);
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x000704EC File Offset: 0x0006E6EC
		private static void DrainActionQueue(SafeHandle webSocketHandle, WebSocketProtocolComponent.ActionQueue actionQueue)
		{
			for (;;)
			{
				WebSocketProtocolComponent.Buffer[] array = new WebSocketProtocolComponent.Buffer[1];
				uint num = 1U;
				WebSocketProtocolComponent.Action action;
				WebSocketProtocolComponent.BufferType bufferType;
				IntPtr intPtr;
				IntPtr intPtr2;
				int num2 = WebSocketProtocolComponent.WebSocketGetAction_Raw(webSocketHandle, actionQueue, array, ref num, out action, out bufferType, out intPtr, out intPtr2);
				if (!WebSocketProtocolComponent.Succeeded(num2))
				{
					break;
				}
				if (action == WebSocketProtocolComponent.Action.NoAction)
				{
					return;
				}
				WebSocketProtocolComponent.WebSocketCompleteAction_Raw(webSocketHandle, intPtr2, 0U);
			}
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x00070530 File Offset: 0x0006E730
		private static void MarshalAndVerifyHttpHeader(IntPtr httpHeaderPtr, ref WebSocketProtocolComponent.HttpHeader httpHeader)
		{
			IntPtr intPtr = Marshal.ReadIntPtr(httpHeaderPtr);
			IntPtr intPtr2 = IntPtr.Add(httpHeaderPtr, IntPtr.Size);
			int num = Marshal.ReadInt32(intPtr2);
			if (intPtr != IntPtr.Zero)
			{
				httpHeader.Name = Marshal.PtrToStringAnsi(intPtr, num);
			}
			if ((httpHeader.Name == null && num != 0) || (httpHeader.Name != null && num != httpHeader.Name.Length))
			{
				throw new AccessViolationException();
			}
			int num2 = 2 * IntPtr.Size;
			int num3 = 3 * IntPtr.Size;
			IntPtr intPtr3 = Marshal.ReadIntPtr(IntPtr.Add(httpHeaderPtr, num2));
			intPtr2 = IntPtr.Add(httpHeaderPtr, num3);
			num = Marshal.ReadInt32(intPtr2);
			httpHeader.Value = Marshal.PtrToStringAnsi(intPtr3, num);
			if ((httpHeader.Value == null && num != 0) || (httpHeader.Value != null && num != httpHeader.Value.Length))
			{
				throw new AccessViolationException();
			}
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x000705FC File Offset: 0x0006E7FC
		private static WebSocketProtocolComponent.HttpHeader[] MarshalHttpHeaders(IntPtr nativeHeadersPtr, int nativeHeaderCount)
		{
			WebSocketProtocolComponent.HttpHeader[] array = new WebSocketProtocolComponent.HttpHeader[nativeHeaderCount];
			int num = 4 * IntPtr.Size;
			for (int i = 0; i < nativeHeaderCount; i++)
			{
				int num2 = num * i;
				IntPtr intPtr = IntPtr.Add(nativeHeadersPtr, num2);
				WebSocketProtocolComponent.MarshalAndVerifyHttpHeader(intPtr, ref array[i]);
			}
			return array;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00070640 File Offset: 0x0006E840
		public static bool Succeeded(int hr)
		{
			return hr >= 0;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00070649 File Offset: 0x0006E849
		private static void ThrowOnError(int errorCode)
		{
			if (WebSocketProtocolComponent.Succeeded(errorCode))
			{
				return;
			}
			throw new WebSocketException(errorCode);
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0007065C File Offset: 0x0006E85C
		private static void ThrowIfSessionHandleClosed(WebSocketBase webSocket)
		{
			if (webSocket.SessionHandle.IsClosed)
			{
				throw new WebSocketException(WebSocketError.InvalidState, SR.GetString("net_WebSockets_InvalidState_ClosedOrAborted", new object[]
				{
					webSocket.GetType().FullName,
					webSocket.State
				}));
			}
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x000706AA File Offset: 0x0006E8AA
		private static WebSocketException ConvertObjectDisposedException(WebSocketBase webSocket, ObjectDisposedException innerException)
		{
			return new WebSocketException(WebSocketError.InvalidState, SR.GetString("net_WebSockets_InvalidState_ClosedOrAborted", new object[]
			{
				webSocket.GetType().FullName,
				webSocket.State
			}), innerException);
		}

		// Token: 0x040016C1 RID: 5825
		private const string WEBSOCKET = "websocket.dll";

		// Token: 0x040016C2 RID: 5826
		private static readonly string s_DllFileName = Path.Combine(Environment.SystemDirectory, "websocket.dll");

		// Token: 0x040016C3 RID: 5827
		private static readonly string s_DummyWebsocketKeyBase64 = Convert.ToBase64String(new byte[16]);

		// Token: 0x040016C4 RID: 5828
		private static readonly SafeLoadLibrary s_WebSocketDllHandle = SafeLoadLibrary.LoadLibraryEx(WebSocketProtocolComponent.s_DllFileName);

		// Token: 0x040016C5 RID: 5829
		private static readonly string s_SupportedVersion;

		// Token: 0x040016C6 RID: 5830
		private static readonly WebSocketProtocolComponent.HttpHeader[] s_InitialClientRequestHeaders = new WebSocketProtocolComponent.HttpHeader[]
		{
			new WebSocketProtocolComponent.HttpHeader
			{
				Name = "Connection",
				NameLength = (uint)"Connection".Length,
				Value = "Upgrade",
				ValueLength = (uint)"Upgrade".Length
			},
			new WebSocketProtocolComponent.HttpHeader
			{
				Name = "Upgrade",
				NameLength = (uint)"Upgrade".Length,
				Value = "websocket",
				ValueLength = (uint)"websocket".Length
			}
		};

		// Token: 0x040016C7 RID: 5831
		private static readonly WebSocketProtocolComponent.HttpHeader[] s_ServerFakeRequestHeaders;

		// Token: 0x02000788 RID: 1928
		internal static class Errors
		{
			// Token: 0x04003348 RID: 13128
			internal const int E_INVALID_OPERATION = -2147483568;

			// Token: 0x04003349 RID: 13129
			internal const int E_INVALID_PROTOCOL_OPERATION = -2147483567;

			// Token: 0x0400334A RID: 13130
			internal const int E_INVALID_PROTOCOL_FORMAT = -2147483566;

			// Token: 0x0400334B RID: 13131
			internal const int E_NUMERIC_OVERFLOW = -2147483565;

			// Token: 0x0400334C RID: 13132
			internal const int E_FAIL = -2147467259;
		}

		// Token: 0x02000789 RID: 1929
		internal enum Action
		{
			// Token: 0x0400334E RID: 13134
			NoAction,
			// Token: 0x0400334F RID: 13135
			SendToNetwork,
			// Token: 0x04003350 RID: 13136
			IndicateSendComplete,
			// Token: 0x04003351 RID: 13137
			ReceiveFromNetwork,
			// Token: 0x04003352 RID: 13138
			IndicateReceiveComplete
		}

		// Token: 0x0200078A RID: 1930
		internal enum BufferType : uint
		{
			// Token: 0x04003354 RID: 13140
			None,
			// Token: 0x04003355 RID: 13141
			UTF8Message = 2147483648U,
			// Token: 0x04003356 RID: 13142
			UTF8Fragment,
			// Token: 0x04003357 RID: 13143
			BinaryMessage,
			// Token: 0x04003358 RID: 13144
			BinaryFragment,
			// Token: 0x04003359 RID: 13145
			Close,
			// Token: 0x0400335A RID: 13146
			PingPong,
			// Token: 0x0400335B RID: 13147
			UnsolicitedPong
		}

		// Token: 0x0200078B RID: 1931
		internal enum PropertyType
		{
			// Token: 0x0400335D RID: 13149
			ReceiveBufferSize,
			// Token: 0x0400335E RID: 13150
			SendBufferSize,
			// Token: 0x0400335F RID: 13151
			DisableMasking,
			// Token: 0x04003360 RID: 13152
			AllocatedBuffer,
			// Token: 0x04003361 RID: 13153
			DisableUtf8Verification,
			// Token: 0x04003362 RID: 13154
			KeepAliveInterval
		}

		// Token: 0x0200078C RID: 1932
		internal enum ActionQueue
		{
			// Token: 0x04003364 RID: 13156
			Send = 1,
			// Token: 0x04003365 RID: 13157
			Receive
		}

		// Token: 0x0200078D RID: 1933
		internal struct Property
		{
			// Token: 0x04003366 RID: 13158
			internal WebSocketProtocolComponent.PropertyType Type;

			// Token: 0x04003367 RID: 13159
			internal IntPtr PropertyData;

			// Token: 0x04003368 RID: 13160
			internal uint PropertySize;
		}

		// Token: 0x0200078E RID: 1934
		[StructLayout(LayoutKind.Explicit)]
		internal struct Buffer
		{
			// Token: 0x04003369 RID: 13161
			[FieldOffset(0)]
			internal WebSocketProtocolComponent.DataBuffer Data;

			// Token: 0x0400336A RID: 13162
			[FieldOffset(0)]
			internal WebSocketProtocolComponent.CloseBuffer CloseStatus;
		}

		// Token: 0x0200078F RID: 1935
		internal struct DataBuffer
		{
			// Token: 0x0400336B RID: 13163
			internal IntPtr BufferData;

			// Token: 0x0400336C RID: 13164
			internal uint BufferLength;
		}

		// Token: 0x02000790 RID: 1936
		internal struct CloseBuffer
		{
			// Token: 0x0400336D RID: 13165
			internal IntPtr ReasonData;

			// Token: 0x0400336E RID: 13166
			internal uint ReasonLength;

			// Token: 0x0400336F RID: 13167
			internal ushort CloseStatus;
		}

		// Token: 0x02000791 RID: 1937
		internal struct HttpHeader
		{
			// Token: 0x04003370 RID: 13168
			[MarshalAs(UnmanagedType.LPStr)]
			internal string Name;

			// Token: 0x04003371 RID: 13169
			internal uint NameLength;

			// Token: 0x04003372 RID: 13170
			[MarshalAs(UnmanagedType.LPStr)]
			internal string Value;

			// Token: 0x04003373 RID: 13171
			internal uint ValueLength;
		}
	}
}
