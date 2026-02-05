using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002E3 RID: 739
	[global::__DynamicallyInvokable]
	public abstract class NetworkInterface
	{
		// Token: 0x060019FC RID: 6652 RVA: 0x0007E6B3 File Offset: 0x0007C8B3
		[global::__DynamicallyInvokable]
		public static NetworkInterface[] GetAllNetworkInterfaces()
		{
			new NetworkInformationPermission(NetworkInformationAccess.Read).Demand();
			return SystemNetworkInterface.GetNetworkInterfaces();
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x0007E6C5 File Offset: 0x0007C8C5
		[global::__DynamicallyInvokable]
		public static bool GetIsNetworkAvailable()
		{
			return SystemNetworkInterface.InternalGetIsNetworkAvailable();
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x0007E6CC File Offset: 0x0007C8CC
		[global::__DynamicallyInvokable]
		public static int LoopbackInterfaceIndex
		{
			[global::__DynamicallyInvokable]
			get
			{
				return SystemNetworkInterface.InternalLoopbackInterfaceIndex;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x0007E6D3 File Offset: 0x0007C8D3
		[global::__DynamicallyInvokable]
		public static int IPv6LoopbackInterfaceIndex
		{
			[global::__DynamicallyInvokable]
			get
			{
				return SystemNetworkInterface.InternalIPv6LoopbackInterfaceIndex;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x0007E6DA File Offset: 0x0007C8DA
		[global::__DynamicallyInvokable]
		public virtual string Id
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x0007E6E1 File Offset: 0x0007C8E1
		[global::__DynamicallyInvokable]
		public virtual string Name
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x0007E6E8 File Offset: 0x0007C8E8
		[global::__DynamicallyInvokable]
		public virtual string Description
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x0007E6EF File Offset: 0x0007C8EF
		[global::__DynamicallyInvokable]
		public virtual IPInterfaceProperties GetIPProperties()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0007E6F6 File Offset: 0x0007C8F6
		[global::__DynamicallyInvokable]
		public virtual IPv4InterfaceStatistics GetIPv4Statistics()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0007E6FD File Offset: 0x0007C8FD
		[global::__DynamicallyInvokable]
		public virtual IPInterfaceStatistics GetIPStatistics()
		{
			throw new NotImplementedException();
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001A06 RID: 6662 RVA: 0x0007E704 File Offset: 0x0007C904
		[global::__DynamicallyInvokable]
		public virtual OperationalStatus OperationalStatus
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0007E70B File Offset: 0x0007C90B
		[global::__DynamicallyInvokable]
		public virtual long Speed
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x0007E712 File Offset: 0x0007C912
		[global::__DynamicallyInvokable]
		public virtual bool IsReceiveOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x0007E719 File Offset: 0x0007C919
		[global::__DynamicallyInvokable]
		public virtual bool SupportsMulticast
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x0007E720 File Offset: 0x0007C920
		[global::__DynamicallyInvokable]
		public virtual PhysicalAddress GetPhysicalAddress()
		{
			throw new NotImplementedException();
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001A0B RID: 6667 RVA: 0x0007E727 File Offset: 0x0007C927
		[global::__DynamicallyInvokable]
		public virtual NetworkInterfaceType NetworkInterfaceType
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0007E72E File Offset: 0x0007C92E
		[global::__DynamicallyInvokable]
		public virtual bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x0007E735 File Offset: 0x0007C935
		[global::__DynamicallyInvokable]
		protected NetworkInterface()
		{
		}
	}
}
