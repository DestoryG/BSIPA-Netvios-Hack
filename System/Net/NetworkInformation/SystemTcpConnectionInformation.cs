using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000301 RID: 769
	internal class SystemTcpConnectionInformation : TcpConnectionInformation
	{
		// Token: 0x06001B3F RID: 6975 RVA: 0x00081A9C File Offset: 0x0007FC9C
		internal SystemTcpConnectionInformation(MibTcpRow row)
		{
			this.state = row.state;
			int num = ((int)row.localPort1 << 8) | (int)row.localPort2;
			int num2 = ((this.state == TcpState.Listen) ? 0 : (((int)row.remotePort1 << 8) | (int)row.remotePort2));
			this.localEndPoint = new IPEndPoint((long)((ulong)row.localAddr), num);
			this.remoteEndPoint = new IPEndPoint((long)((ulong)row.remoteAddr), num2);
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x00081B10 File Offset: 0x0007FD10
		internal SystemTcpConnectionInformation(MibTcp6RowOwnerPid row)
		{
			this.state = row.state;
			int num = ((int)row.localPort1 << 8) | (int)row.localPort2;
			int num2 = ((this.state == TcpState.Listen) ? 0 : (((int)row.remotePort1 << 8) | (int)row.remotePort2));
			this.localEndPoint = new IPEndPoint(new IPAddress(row.localAddr, (long)((ulong)row.localScopeId)), num);
			this.remoteEndPoint = new IPEndPoint(new IPAddress(row.remoteAddr, (long)((ulong)row.remoteScopeId)), num2);
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001B41 RID: 6977 RVA: 0x00081B97 File Offset: 0x0007FD97
		public override TcpState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001B42 RID: 6978 RVA: 0x00081B9F File Offset: 0x0007FD9F
		public override IPEndPoint LocalEndPoint
		{
			get
			{
				return this.localEndPoint;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x00081BA7 File Offset: 0x0007FDA7
		public override IPEndPoint RemoteEndPoint
		{
			get
			{
				return this.remoteEndPoint;
			}
		}

		// Token: 0x04001ADC RID: 6876
		private IPEndPoint localEndPoint;

		// Token: 0x04001ADD RID: 6877
		private IPEndPoint remoteEndPoint;

		// Token: 0x04001ADE RID: 6878
		private TcpState state;
	}
}
