using System;
using System.Runtime.Serialization;

namespace System.Net.Sockets
{
	// Token: 0x02000375 RID: 885
	[Serializable]
	public struct SocketInformation
	{
		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06002012 RID: 8210 RVA: 0x00095F0E File Offset: 0x0009410E
		// (set) Token: 0x06002013 RID: 8211 RVA: 0x00095F16 File Offset: 0x00094116
		public byte[] ProtocolInformation
		{
			get
			{
				return this.protocolInformation;
			}
			set
			{
				this.protocolInformation = value;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x00095F1F File Offset: 0x0009411F
		// (set) Token: 0x06002015 RID: 8213 RVA: 0x00095F27 File Offset: 0x00094127
		public SocketInformationOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002016 RID: 8214 RVA: 0x00095F30 File Offset: 0x00094130
		// (set) Token: 0x06002017 RID: 8215 RVA: 0x00095F3D File Offset: 0x0009413D
		internal bool IsNonBlocking
		{
			get
			{
				return (this.options & SocketInformationOptions.NonBlocking) > (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.NonBlocking;
					return;
				}
				this.options &= ~SocketInformationOptions.NonBlocking;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x00095F60 File Offset: 0x00094160
		// (set) Token: 0x06002019 RID: 8217 RVA: 0x00095F6D File Offset: 0x0009416D
		internal bool IsConnected
		{
			get
			{
				return (this.options & SocketInformationOptions.Connected) > (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.Connected;
					return;
				}
				this.options &= ~SocketInformationOptions.Connected;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x00095F90 File Offset: 0x00094190
		// (set) Token: 0x0600201B RID: 8219 RVA: 0x00095F9D File Offset: 0x0009419D
		internal bool IsListening
		{
			get
			{
				return (this.options & SocketInformationOptions.Listening) > (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.Listening;
					return;
				}
				this.options &= ~SocketInformationOptions.Listening;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x00095FC0 File Offset: 0x000941C0
		// (set) Token: 0x0600201D RID: 8221 RVA: 0x00095FCD File Offset: 0x000941CD
		internal bool UseOnlyOverlappedIO
		{
			get
			{
				return (this.options & SocketInformationOptions.UseOnlyOverlappedIO) > (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.UseOnlyOverlappedIO;
					return;
				}
				this.options &= ~SocketInformationOptions.UseOnlyOverlappedIO;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x00095FF0 File Offset: 0x000941F0
		// (set) Token: 0x0600201F RID: 8223 RVA: 0x00095FF8 File Offset: 0x000941F8
		internal EndPoint RemoteEndPoint
		{
			get
			{
				return this.remoteEndPoint;
			}
			set
			{
				this.remoteEndPoint = value;
			}
		}

		// Token: 0x04001E29 RID: 7721
		private byte[] protocolInformation;

		// Token: 0x04001E2A RID: 7722
		private SocketInformationOptions options;

		// Token: 0x04001E2B RID: 7723
		[OptionalField]
		private EndPoint remoteEndPoint;
	}
}
