using System;

namespace System.Net.Sockets
{
	// Token: 0x020003A0 RID: 928
	public struct IPPacketInformation
	{
		// Token: 0x06002299 RID: 8857 RVA: 0x000A4D4E File Offset: 0x000A2F4E
		internal IPPacketInformation(IPAddress address, int networkInterface)
		{
			this.address = address;
			this.networkInterface = networkInterface;
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x0600229A RID: 8858 RVA: 0x000A4D5E File Offset: 0x000A2F5E
		public IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x000A4D66 File Offset: 0x000A2F66
		public int Interface
		{
			get
			{
				return this.networkInterface;
			}
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x000A4D6E File Offset: 0x000A2F6E
		public static bool operator ==(IPPacketInformation packetInformation1, IPPacketInformation packetInformation2)
		{
			return packetInformation1.Equals(packetInformation2);
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x000A4D83 File Offset: 0x000A2F83
		public static bool operator !=(IPPacketInformation packetInformation1, IPPacketInformation packetInformation2)
		{
			return !packetInformation1.Equals(packetInformation2);
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x000A4D9C File Offset: 0x000A2F9C
		public override bool Equals(object comparand)
		{
			if (comparand == null)
			{
				return false;
			}
			if (!(comparand is IPPacketInformation))
			{
				return false;
			}
			IPPacketInformation ippacketInformation = (IPPacketInformation)comparand;
			return this.address.Equals(ippacketInformation.address) && this.networkInterface == ippacketInformation.networkInterface;
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x000A4DE3 File Offset: 0x000A2FE3
		public override int GetHashCode()
		{
			return this.address.GetHashCode() + this.networkInterface.GetHashCode();
		}

		// Token: 0x04001F8D RID: 8077
		private IPAddress address;

		// Token: 0x04001F8E RID: 8078
		private int networkInterface;
	}
}
