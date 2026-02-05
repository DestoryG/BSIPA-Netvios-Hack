using System;

namespace System.Net.Sockets
{
	// Token: 0x02000383 RID: 899
	public struct SocketReceiveMessageFromResult
	{
		// Token: 0x04001F2B RID: 7979
		public int ReceivedBytes;

		// Token: 0x04001F2C RID: 7980
		public SocketFlags SocketFlags;

		// Token: 0x04001F2D RID: 7981
		public EndPoint RemoteEndPoint;

		// Token: 0x04001F2E RID: 7982
		public IPPacketInformation PacketInformation;
	}
}
