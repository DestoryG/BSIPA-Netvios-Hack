using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002FE RID: 766
	internal class SystemIPv4InterfaceProperties : IPv4InterfaceProperties
	{
		// Token: 0x06001B1F RID: 6943 RVA: 0x000815D4 File Offset: 0x0007F7D4
		internal SystemIPv4InterfaceProperties(FixedInfo fixedInfo, IpAdapterAddresses ipAdapterAddresses)
		{
			this.index = ipAdapterAddresses.index;
			this.routingEnabled = fixedInfo.EnableRouting;
			this.dhcpEnabled = (ipAdapterAddresses.flags & AdapterFlags.DhcpEnabled) > (AdapterFlags)0;
			this.haveWins = ipAdapterAddresses.firstWinsServerAddress != IntPtr.Zero;
			this.mtu = ipAdapterAddresses.mtu;
			this.GetPerAdapterInfo(ipAdapterAddresses.index);
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001B20 RID: 6944 RVA: 0x0008163F File Offset: 0x0007F83F
		public override bool UsesWins
		{
			get
			{
				return this.haveWins;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x00081647 File Offset: 0x0007F847
		public override bool IsDhcpEnabled
		{
			get
			{
				return this.dhcpEnabled;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001B22 RID: 6946 RVA: 0x0008164F File Offset: 0x0007F84F
		public override bool IsForwardingEnabled
		{
			get
			{
				return this.routingEnabled;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x00081657 File Offset: 0x0007F857
		public override bool IsAutomaticPrivateAddressingEnabled
		{
			get
			{
				return this.autoConfigEnabled;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001B24 RID: 6948 RVA: 0x0008165F File Offset: 0x0007F85F
		public override bool IsAutomaticPrivateAddressingActive
		{
			get
			{
				return this.autoConfigActive;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x00081667 File Offset: 0x0007F867
		public override int Mtu
		{
			get
			{
				return (int)this.mtu;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x0008166F File Offset: 0x0007F86F
		public override int Index
		{
			get
			{
				return (int)this.index;
			}
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x00081678 File Offset: 0x0007F878
		private void GetPerAdapterInfo(uint index)
		{
			if (index != 0U)
			{
				uint num = 0U;
				SafeLocalFree safeLocalFree = null;
				uint num2 = UnsafeNetInfoNativeMethods.GetPerAdapterInfo(index, SafeLocalFree.Zero, ref num);
				while (num2 == 111U)
				{
					try
					{
						safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
						num2 = UnsafeNetInfoNativeMethods.GetPerAdapterInfo(index, safeLocalFree, ref num);
						if (num2 == 0U)
						{
							IpPerAdapterInfo ipPerAdapterInfo = (IpPerAdapterInfo)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IpPerAdapterInfo));
							this.autoConfigEnabled = ipPerAdapterInfo.autoconfigEnabled;
							this.autoConfigActive = ipPerAdapterInfo.autoconfigActive;
						}
					}
					finally
					{
						if (safeLocalFree != null)
						{
							safeLocalFree.Close();
						}
					}
				}
				if (num2 != 0U)
				{
					throw new NetworkInformationException((int)num2);
				}
			}
		}

		// Token: 0x04001AC6 RID: 6854
		private bool haveWins;

		// Token: 0x04001AC7 RID: 6855
		private bool dhcpEnabled;

		// Token: 0x04001AC8 RID: 6856
		private bool routingEnabled;

		// Token: 0x04001AC9 RID: 6857
		private bool autoConfigEnabled;

		// Token: 0x04001ACA RID: 6858
		private bool autoConfigActive;

		// Token: 0x04001ACB RID: 6859
		private uint index;

		// Token: 0x04001ACC RID: 6860
		private uint mtu;
	}
}
