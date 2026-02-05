using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002FC RID: 764
	internal class SystemMulticastIPAddressInformation : MulticastIPAddressInformation
	{
		// Token: 0x06001B05 RID: 6917 RVA: 0x00081361 File Offset: 0x0007F561
		private SystemMulticastIPAddressInformation()
		{
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x00081369 File Offset: 0x0007F569
		public SystemMulticastIPAddressInformation(SystemIPAddressInformation addressInfo)
		{
			this.innerInfo = addressInfo;
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x00081378 File Offset: 0x0007F578
		public override IPAddress Address
		{
			get
			{
				return this.innerInfo.Address;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001B08 RID: 6920 RVA: 0x00081385 File Offset: 0x0007F585
		public override bool IsTransient
		{
			get
			{
				return this.innerInfo.IsTransient;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x00081392 File Offset: 0x0007F592
		public override bool IsDnsEligible
		{
			get
			{
				return this.innerInfo.IsDnsEligible;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001B0A RID: 6922 RVA: 0x0008139F File Offset: 0x0007F59F
		public override PrefixOrigin PrefixOrigin
		{
			get
			{
				return PrefixOrigin.Other;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001B0B RID: 6923 RVA: 0x000813A2 File Offset: 0x0007F5A2
		public override SuffixOrigin SuffixOrigin
		{
			get
			{
				return SuffixOrigin.Other;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06001B0C RID: 6924 RVA: 0x000813A5 File Offset: 0x0007F5A5
		public override DuplicateAddressDetectionState DuplicateAddressDetectionState
		{
			get
			{
				return DuplicateAddressDetectionState.Invalid;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001B0D RID: 6925 RVA: 0x000813A8 File Offset: 0x0007F5A8
		public override long AddressValidLifetime
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001B0E RID: 6926 RVA: 0x000813AC File Offset: 0x0007F5AC
		public override long AddressPreferredLifetime
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001B0F RID: 6927 RVA: 0x000813B0 File Offset: 0x0007F5B0
		public override long DhcpLeaseLifetime
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x000813B4 File Offset: 0x0007F5B4
		internal static MulticastIPAddressInformationCollection ToMulticastIpAddressInformationCollection(IPAddressInformationCollection addresses)
		{
			MulticastIPAddressInformationCollection multicastIPAddressInformationCollection = new MulticastIPAddressInformationCollection();
			foreach (IPAddressInformation ipaddressInformation in addresses)
			{
				multicastIPAddressInformationCollection.InternalAdd(new SystemMulticastIPAddressInformation((SystemIPAddressInformation)ipaddressInformation));
			}
			return multicastIPAddressInformationCollection;
		}

		// Token: 0x04001ABC RID: 6844
		private SystemIPAddressInformation innerInfo;
	}
}
