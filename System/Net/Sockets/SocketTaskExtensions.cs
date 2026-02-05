using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Net.Sockets
{
	// Token: 0x02000384 RID: 900
	public static class SocketTaskExtensions
	{
		// Token: 0x06002182 RID: 8578 RVA: 0x000A0D33 File Offset: 0x0009EF33
		public static Task<Socket> AcceptAsync(this Socket socket)
		{
			return socket.AcceptAsync(null);
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x000A0D3C File Offset: 0x0009EF3C
		public static Task<Socket> AcceptAsync(this Socket socket, Socket acceptSocket)
		{
			return socket.AcceptAsync(acceptSocket);
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x000A0D45 File Offset: 0x0009EF45
		public static Task ConnectAsync(this Socket socket, EndPoint remoteEP)
		{
			return socket.ConnectAsync(remoteEP);
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x000A0D4E File Offset: 0x0009EF4E
		public static Task ConnectAsync(this Socket socket, IPAddress address, int port)
		{
			return socket.ConnectAsync(address, port);
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x000A0D58 File Offset: 0x0009EF58
		public static Task ConnectAsync(this Socket socket, IPAddress[] addresses, int port)
		{
			return socket.ConnectAsync(addresses, port);
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x000A0D62 File Offset: 0x0009EF62
		public static Task ConnectAsync(this Socket socket, string host, int port)
		{
			return socket.ConnectAsync(host, port);
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x000A0D6C File Offset: 0x0009EF6C
		public static Task<int> ReceiveAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags)
		{
			return socket.ReceiveAsync(buffer, socketFlags, false);
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x000A0D77 File Offset: 0x0009EF77
		public static Task<int> ReceiveAsync(this Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			return socket.ReceiveAsync(buffers, socketFlags);
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x000A0D81 File Offset: 0x0009EF81
		public static Task<SocketReceiveFromResult> ReceiveFromAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint)
		{
			return socket.ReceiveFromAsync(buffer, socketFlags, remoteEndPoint);
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000A0D8C File Offset: 0x0009EF8C
		public static Task<SocketReceiveMessageFromResult> ReceiveMessageFromAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint)
		{
			return socket.ReceiveMessageFromAsync(buffer, socketFlags, remoteEndPoint);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000A0D97 File Offset: 0x0009EF97
		public static Task<int> SendAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags)
		{
			return socket.SendAsync(buffer, socketFlags, false);
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000A0DA2 File Offset: 0x0009EFA2
		public static Task<int> SendAsync(this Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			return socket.SendAsync(buffers, socketFlags);
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000A0DAC File Offset: 0x0009EFAC
		public static Task<int> SendToAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEP)
		{
			return socket.SendToAsync(buffer, socketFlags, remoteEP);
		}
	}
}
