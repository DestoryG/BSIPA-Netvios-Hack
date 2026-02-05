using System;

namespace System.Net.Sockets
{
	// Token: 0x0200039B RID: 923
	internal class MultipleSocketMultipleConnectAsync : MultipleConnectAsync
	{
		// Token: 0x06002272 RID: 8818 RVA: 0x000A437B File Offset: 0x000A257B
		public MultipleSocketMultipleConnectAsync(SocketType socketType, ProtocolType protocolType)
		{
			if (Socket.OSSupportsIPv4)
			{
				this.socket4 = new Socket(AddressFamily.InterNetwork, socketType, protocolType);
			}
			if (Socket.OSSupportsIPv6)
			{
				this.socket6 = new Socket(AddressFamily.InterNetworkV6, socketType, protocolType);
			}
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000A43B0 File Offset: 0x000A25B0
		protected override IPAddress GetNextAddress(out Socket attemptSocket)
		{
			IPAddress ipaddress = null;
			attemptSocket = null;
			while (attemptSocket == null)
			{
				if (this.nextAddress >= this.addressList.Length)
				{
					return null;
				}
				ipaddress = this.addressList[this.nextAddress];
				this.nextAddress++;
				if (ipaddress.AddressFamily == AddressFamily.InterNetworkV6)
				{
					attemptSocket = this.socket6;
				}
				else if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
				{
					attemptSocket = this.socket4;
				}
			}
			return ipaddress;
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000A441C File Offset: 0x000A261C
		protected override void OnSucceed()
		{
			if (this.socket4 != null && !this.socket4.Connected)
			{
				this.socket4.Close();
			}
			if (this.socket6 != null && !this.socket6.Connected)
			{
				this.socket6.Close();
			}
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000A4469 File Offset: 0x000A2669
		protected override void OnFail(bool abortive)
		{
			if (this.socket4 != null)
			{
				this.socket4.Close();
			}
			if (this.socket6 != null)
			{
				this.socket6.Close();
			}
		}

		// Token: 0x04001F77 RID: 8055
		private Socket socket4;

		// Token: 0x04001F78 RID: 8056
		private Socket socket6;
	}
}
