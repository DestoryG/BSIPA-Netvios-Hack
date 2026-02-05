using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002F7 RID: 759
	internal struct FixedInfo
	{
		// Token: 0x06001ABD RID: 6845 RVA: 0x00080E0C File Offset: 0x0007F00C
		internal FixedInfo(FIXED_INFO info)
		{
			this.info = info;
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x00080E15 File Offset: 0x0007F015
		internal string HostName
		{
			get
			{
				return this.info.hostName;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x00080E22 File Offset: 0x0007F022
		internal string DomainName
		{
			get
			{
				return this.info.domainName;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x00080E2F File Offset: 0x0007F02F
		internal NetBiosNodeType NodeType
		{
			get
			{
				return this.info.nodeType;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x00080E3C File Offset: 0x0007F03C
		internal string ScopeId
		{
			get
			{
				return this.info.scopeId;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x00080E49 File Offset: 0x0007F049
		internal bool EnableRouting
		{
			get
			{
				return this.info.enableRouting;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x00080E56 File Offset: 0x0007F056
		internal bool EnableProxy
		{
			get
			{
				return this.info.enableProxy;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x00080E63 File Offset: 0x0007F063
		internal bool EnableDns
		{
			get
			{
				return this.info.enableDns;
			}
		}

		// Token: 0x04001AAB RID: 6827
		internal FIXED_INFO info;
	}
}
