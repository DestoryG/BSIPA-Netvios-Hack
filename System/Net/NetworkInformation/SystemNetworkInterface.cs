using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000300 RID: 768
	internal class SystemNetworkInterface : NetworkInterface
	{
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x00081762 File Offset: 0x0007F962
		internal static int InternalLoopbackInterfaceIndex
		{
			get
			{
				return SystemNetworkInterface.GetBestInterfaceForAddress(IPAddress.Loopback);
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x0008176E File Offset: 0x0007F96E
		internal static int InternalIPv6LoopbackInterfaceIndex
		{
			get
			{
				return SystemNetworkInterface.GetBestInterfaceForAddress(IPAddress.IPv6Loopback);
			}
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0008177C File Offset: 0x0007F97C
		private static int GetBestInterfaceForAddress(IPAddress addr)
		{
			SocketAddress socketAddress = new SocketAddress(addr);
			int num;
			int bestInterfaceEx = (int)UnsafeNetInfoNativeMethods.GetBestInterfaceEx(socketAddress.m_Buffer, out num);
			if (bestInterfaceEx != 0)
			{
				throw new NetworkInformationException(bestInterfaceEx);
			}
			return num;
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000817AC File Offset: 0x0007F9AC
		internal static bool InternalGetIsNetworkAvailable()
		{
			try
			{
				NetworkInterface[] networkInterfaces = SystemNetworkInterface.GetNetworkInterfaces();
				foreach (NetworkInterface networkInterface in networkInterfaces)
				{
					if (networkInterface.OperationalStatus == OperationalStatus.Up && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
					{
						return true;
					}
				}
			}
			catch (NetworkInformationException ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.Web, "SystemNetworkInterface", "InternalGetIsNetworkAvailable", ex);
				}
			}
			return false;
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x00081830 File Offset: 0x0007FA30
		internal static NetworkInterface[] GetNetworkInterfaces()
		{
			AddressFamily addressFamily = AddressFamily.Unspecified;
			uint num = 0U;
			SafeLocalFree safeLocalFree = null;
			FixedInfo fixedInfo = SystemIPGlobalProperties.GetFixedInfo();
			List<SystemNetworkInterface> list = new List<SystemNetworkInterface>();
			GetAdaptersAddressesFlags getAdaptersAddressesFlags = GetAdaptersAddressesFlags.IncludeWins | GetAdaptersAddressesFlags.IncludeGateways;
			uint num2 = UnsafeNetInfoNativeMethods.GetAdaptersAddresses(addressFamily, (uint)getAdaptersAddressesFlags, IntPtr.Zero, SafeLocalFree.Zero, ref num);
			while (num2 == 111U)
			{
				try
				{
					safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
					num2 = UnsafeNetInfoNativeMethods.GetAdaptersAddresses(addressFamily, (uint)getAdaptersAddressesFlags, IntPtr.Zero, safeLocalFree, ref num);
					if (num2 == 0U)
					{
						IntPtr intPtr = safeLocalFree.DangerousGetHandle();
						while (intPtr != IntPtr.Zero)
						{
							IpAdapterAddresses ipAdapterAddresses = (IpAdapterAddresses)Marshal.PtrToStructure(intPtr, typeof(IpAdapterAddresses));
							list.Add(new SystemNetworkInterface(fixedInfo, ipAdapterAddresses));
							intPtr = ipAdapterAddresses.next;
						}
					}
				}
				finally
				{
					if (safeLocalFree != null)
					{
						safeLocalFree.Close();
					}
					safeLocalFree = null;
				}
			}
			if (num2 == 232U || num2 == 87U)
			{
				return new SystemNetworkInterface[0];
			}
			if (num2 != 0U)
			{
				throw new NetworkInformationException((int)num2);
			}
			return list.ToArray();
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0008192C File Offset: 0x0007FB2C
		internal SystemNetworkInterface(FixedInfo fixedInfo, IpAdapterAddresses ipAdapterAddresses)
		{
			this.id = ipAdapterAddresses.AdapterName;
			this.name = ipAdapterAddresses.friendlyName;
			this.description = ipAdapterAddresses.description;
			this.index = ipAdapterAddresses.index;
			this.physicalAddress = ipAdapterAddresses.address;
			this.addressLength = ipAdapterAddresses.addressLength;
			this.type = ipAdapterAddresses.type;
			this.operStatus = ipAdapterAddresses.operStatus;
			this.speed = (long)ipAdapterAddresses.receiveLinkSpeed;
			this.ipv6Index = ipAdapterAddresses.ipv6Index;
			this.adapterFlags = ipAdapterAddresses.flags;
			this.interfaceProperties = new SystemIPInterfaceProperties(fixedInfo, ipAdapterAddresses);
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001B32 RID: 6962 RVA: 0x000819D0 File Offset: 0x0007FBD0
		public override string Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001B33 RID: 6963 RVA: 0x000819D8 File Offset: 0x0007FBD8
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001B34 RID: 6964 RVA: 0x000819E0 File Offset: 0x0007FBE0
		public override string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000819E8 File Offset: 0x0007FBE8
		public override PhysicalAddress GetPhysicalAddress()
		{
			byte[] array = new byte[this.addressLength];
			Array.Copy(this.physicalAddress, array, (long)((ulong)this.addressLength));
			return new PhysicalAddress(array);
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001B36 RID: 6966 RVA: 0x00081A1A File Offset: 0x0007FC1A
		public override NetworkInterfaceType NetworkInterfaceType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x00081A22 File Offset: 0x0007FC22
		public override IPInterfaceProperties GetIPProperties()
		{
			return this.interfaceProperties;
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x00081A2A File Offset: 0x0007FC2A
		public override IPv4InterfaceStatistics GetIPv4Statistics()
		{
			return new SystemIPv4InterfaceStatistics((long)((ulong)this.index));
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x00081A38 File Offset: 0x0007FC38
		public override IPInterfaceStatistics GetIPStatistics()
		{
			return new SystemIPInterfaceStatistics((long)((ulong)this.index));
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x00081A46 File Offset: 0x0007FC46
		public override bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
		{
			return (networkInterfaceComponent == NetworkInterfaceComponent.IPv6 && (this.adapterFlags & AdapterFlags.IPv6Enabled) != (AdapterFlags)0) || (networkInterfaceComponent == NetworkInterfaceComponent.IPv4 && (this.adapterFlags & AdapterFlags.IPv4Enabled) != (AdapterFlags)0);
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001B3B RID: 6971 RVA: 0x00081A70 File Offset: 0x0007FC70
		public override OperationalStatus OperationalStatus
		{
			get
			{
				return this.operStatus;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001B3C RID: 6972 RVA: 0x00081A78 File Offset: 0x0007FC78
		public override long Speed
		{
			get
			{
				return this.speed;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001B3D RID: 6973 RVA: 0x00081A80 File Offset: 0x0007FC80
		public override bool IsReceiveOnly
		{
			get
			{
				return (this.adapterFlags & AdapterFlags.ReceiveOnly) > (AdapterFlags)0;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x00081A8D File Offset: 0x0007FC8D
		public override bool SupportsMulticast
		{
			get
			{
				return (this.adapterFlags & AdapterFlags.NoMulticast) == (AdapterFlags)0;
			}
		}

		// Token: 0x04001AD0 RID: 6864
		private string name;

		// Token: 0x04001AD1 RID: 6865
		private string id;

		// Token: 0x04001AD2 RID: 6866
		private string description;

		// Token: 0x04001AD3 RID: 6867
		private byte[] physicalAddress;

		// Token: 0x04001AD4 RID: 6868
		private uint addressLength;

		// Token: 0x04001AD5 RID: 6869
		private NetworkInterfaceType type;

		// Token: 0x04001AD6 RID: 6870
		private OperationalStatus operStatus;

		// Token: 0x04001AD7 RID: 6871
		private long speed;

		// Token: 0x04001AD8 RID: 6872
		private uint index;

		// Token: 0x04001AD9 RID: 6873
		private uint ipv6Index;

		// Token: 0x04001ADA RID: 6874
		private AdapterFlags adapterFlags;

		// Token: 0x04001ADB RID: 6875
		private SystemIPInterfaceProperties interfaceProperties;
	}
}
