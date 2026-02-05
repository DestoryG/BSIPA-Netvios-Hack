using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002F5 RID: 757
	internal class SystemIPAddressInformation : IPAddressInformation
	{
		// Token: 0x06001A9F RID: 6815 RVA: 0x000803A7 File Offset: 0x0007E5A7
		internal SystemIPAddressInformation(IPAddress address, AdapterAddressFlags flags)
		{
			this.address = address;
			this.transient = (flags & AdapterAddressFlags.Transient) > (AdapterAddressFlags)0;
			this.dnsEligible = (flags & AdapterAddressFlags.DnsEligible) > (AdapterAddressFlags)0;
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x000803D5 File Offset: 0x0007E5D5
		public override IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001AA1 RID: 6817 RVA: 0x000803DD File Offset: 0x0007E5DD
		public override bool IsTransient
		{
			get
			{
				return this.transient;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001AA2 RID: 6818 RVA: 0x000803E5 File Offset: 0x0007E5E5
		public override bool IsDnsEligible
		{
			get
			{
				return this.dnsEligible;
			}
		}

		// Token: 0x04001AA3 RID: 6819
		private IPAddress address;

		// Token: 0x04001AA4 RID: 6820
		internal bool transient;

		// Token: 0x04001AA5 RID: 6821
		internal bool dnsEligible = true;
	}
}
