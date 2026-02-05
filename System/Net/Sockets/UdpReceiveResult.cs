using System;

namespace System.Net.Sockets
{
	// Token: 0x0200038A RID: 906
	public struct UdpReceiveResult : IEquatable<UdpReceiveResult>
	{
		// Token: 0x06002205 RID: 8709 RVA: 0x000A2D8C File Offset: 0x000A0F8C
		public UdpReceiveResult(byte[] buffer, IPEndPoint remoteEndPoint)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (remoteEndPoint == null)
			{
				throw new ArgumentNullException("remoteEndPoint");
			}
			this.m_buffer = buffer;
			this.m_remoteEndPoint = remoteEndPoint;
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002206 RID: 8710 RVA: 0x000A2DB8 File Offset: 0x000A0FB8
		public byte[] Buffer
		{
			get
			{
				return this.m_buffer;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002207 RID: 8711 RVA: 0x000A2DC0 File Offset: 0x000A0FC0
		public IPEndPoint RemoteEndPoint
		{
			get
			{
				return this.m_remoteEndPoint;
			}
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000A2DC8 File Offset: 0x000A0FC8
		public override int GetHashCode()
		{
			if (this.m_buffer == null)
			{
				return 0;
			}
			return this.m_buffer.GetHashCode() ^ this.m_remoteEndPoint.GetHashCode();
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000A2DEB File Offset: 0x000A0FEB
		public override bool Equals(object obj)
		{
			return obj is UdpReceiveResult && this.Equals((UdpReceiveResult)obj);
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x000A2E03 File Offset: 0x000A1003
		public bool Equals(UdpReceiveResult other)
		{
			return object.Equals(this.m_buffer, other.m_buffer) && object.Equals(this.m_remoteEndPoint, other.m_remoteEndPoint);
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x000A2E2B File Offset: 0x000A102B
		public static bool operator ==(UdpReceiveResult left, UdpReceiveResult right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000A2E35 File Offset: 0x000A1035
		public static bool operator !=(UdpReceiveResult left, UdpReceiveResult right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04001F4D RID: 8013
		private byte[] m_buffer;

		// Token: 0x04001F4E RID: 8014
		private IPEndPoint m_remoteEndPoint;
	}
}
