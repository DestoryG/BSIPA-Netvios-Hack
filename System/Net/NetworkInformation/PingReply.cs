using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002ED RID: 749
	public class PingReply
	{
		// Token: 0x06001A54 RID: 6740 RVA: 0x0007FB92 File Offset: 0x0007DD92
		internal PingReply()
		{
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0007FB9A File Offset: 0x0007DD9A
		internal PingReply(IPStatus ipStatus)
		{
			this.ipStatus = ipStatus;
			this.buffer = new byte[0];
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x0007FBB8 File Offset: 0x0007DDB8
		internal PingReply(byte[] data, int dataLength, IPAddress address, int time)
		{
			this.address = address;
			this.rtt = (long)time;
			this.ipStatus = this.GetIPStatus((IcmpV4Type)data[20], (IcmpV4Code)data[21]);
			if (this.ipStatus == IPStatus.Success)
			{
				this.buffer = new byte[dataLength - 28];
				Array.Copy(data, 28, this.buffer, 0, dataLength - 28);
				return;
			}
			this.buffer = new byte[0];
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x0007FC28 File Offset: 0x0007DE28
		internal PingReply(IcmpEchoReply reply)
		{
			this.address = new IPAddress((long)((ulong)reply.address));
			this.ipStatus = (IPStatus)reply.status;
			if (this.ipStatus == IPStatus.Success)
			{
				this.rtt = (long)((ulong)reply.roundTripTime);
				this.buffer = new byte[(int)reply.dataSize];
				Marshal.Copy(reply.data, this.buffer, 0, (int)reply.dataSize);
				this.options = new PingOptions(reply.options);
				return;
			}
			this.buffer = new byte[0];
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x0007FCB8 File Offset: 0x0007DEB8
		internal PingReply(Icmp6EchoReply reply, IntPtr dataPtr, int sendSize)
		{
			this.address = new IPAddress(reply.Address.Address, (long)((ulong)reply.Address.ScopeID));
			this.ipStatus = (IPStatus)reply.Status;
			if (this.ipStatus == IPStatus.Success)
			{
				this.rtt = (long)((ulong)reply.RoundTripTime);
				this.buffer = new byte[sendSize];
				Marshal.Copy(IntPtrHelper.Add(dataPtr, 36), this.buffer, 0, sendSize);
				return;
			}
			this.buffer = new byte[0];
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x0007FD3C File Offset: 0x0007DF3C
		private IPStatus GetIPStatus(IcmpV4Type type, IcmpV4Code code)
		{
			switch (type)
			{
			case IcmpV4Type.ICMP4_ECHO_REPLY:
				return IPStatus.Success;
			case (IcmpV4Type)1:
			case (IcmpV4Type)2:
				break;
			case IcmpV4Type.ICMP4_DST_UNREACH:
				switch (code)
				{
				case IcmpV4Code.ICMP4_UNREACH_NET:
					return IPStatus.DestinationNetworkUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_HOST:
					return IPStatus.DestinationHostUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_PROTOCOL:
					return IPStatus.DestinationProtocolUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_PORT:
					return IPStatus.DestinationPortUnreachable;
				case IcmpV4Code.ICMP4_UNREACH_FRAG_NEEDED:
					return IPStatus.PacketTooBig;
				default:
					return IPStatus.DestinationUnreachable;
				}
				break;
			case IcmpV4Type.ICMP4_SOURCE_QUENCH:
				return IPStatus.SourceQuench;
			default:
				if (type == IcmpV4Type.ICMP4_TIME_EXCEEDED)
				{
					return IPStatus.TtlExpired;
				}
				if (type == IcmpV4Type.ICMP4_PARAM_PROB)
				{
					return IPStatus.ParameterProblem;
				}
				break;
			}
			return IPStatus.Unknown;
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x0007FDC4 File Offset: 0x0007DFC4
		public IPStatus Status
		{
			get
			{
				return this.ipStatus;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x0007FDCC File Offset: 0x0007DFCC
		public IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001A5C RID: 6748 RVA: 0x0007FDD4 File Offset: 0x0007DFD4
		public long RoundtripTime
		{
			get
			{
				return this.rtt;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x0007FDDC File Offset: 0x0007DFDC
		public PingOptions Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001A5E RID: 6750 RVA: 0x0007FDE4 File Offset: 0x0007DFE4
		public byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x04001A80 RID: 6784
		private IPAddress address;

		// Token: 0x04001A81 RID: 6785
		private PingOptions options;

		// Token: 0x04001A82 RID: 6786
		private IPStatus ipStatus;

		// Token: 0x04001A83 RID: 6787
		private long rtt;

		// Token: 0x04001A84 RID: 6788
		private byte[] buffer;
	}
}
