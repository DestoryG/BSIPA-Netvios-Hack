using System;

namespace System.Net.Sockets
{
	// Token: 0x0200039A RID: 922
	internal class SingleSocketMultipleConnectAsync : MultipleConnectAsync
	{
		// Token: 0x0600226E RID: 8814 RVA: 0x000A42F1 File Offset: 0x000A24F1
		public SingleSocketMultipleConnectAsync(Socket socket, bool userSocket)
		{
			this.socket = socket;
			this.userSocket = userSocket;
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000A4308 File Offset: 0x000A2508
		protected override IPAddress GetNextAddress(out Socket attemptSocket)
		{
			attemptSocket = this.socket;
			while (this.nextAddress < this.addressList.Length)
			{
				IPAddress ipaddress = this.addressList[this.nextAddress];
				this.nextAddress++;
				if (this.socket.CanTryAddressFamily(ipaddress.AddressFamily))
				{
					return ipaddress;
				}
			}
			return null;
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000A4361 File Offset: 0x000A2561
		protected override void OnFail(bool abortive)
		{
			if (abortive || !this.userSocket)
			{
				this.socket.Close();
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000A4379 File Offset: 0x000A2579
		protected override void OnSucceed()
		{
		}

		// Token: 0x04001F75 RID: 8053
		private Socket socket;

		// Token: 0x04001F76 RID: 8054
		private bool userSocket;
	}
}
