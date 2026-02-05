using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200046C RID: 1132
	public struct X509ChainStatus
	{
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06002A1B RID: 10779 RVA: 0x000C02A9 File Offset: 0x000BE4A9
		// (set) Token: 0x06002A1C RID: 10780 RVA: 0x000C02B1 File Offset: 0x000BE4B1
		public X509ChainStatusFlags Status
		{
			get
			{
				return this.m_status;
			}
			set
			{
				this.m_status = value;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06002A1D RID: 10781 RVA: 0x000C02BA File Offset: 0x000BE4BA
		// (set) Token: 0x06002A1E RID: 10782 RVA: 0x000C02D0 File Offset: 0x000BE4D0
		public string StatusInformation
		{
			get
			{
				if (this.m_statusInformation == null)
				{
					return string.Empty;
				}
				return this.m_statusInformation;
			}
			set
			{
				this.m_statusInformation = value;
			}
		}

		// Token: 0x040025EF RID: 9711
		private X509ChainStatusFlags m_status;

		// Token: 0x040025F0 RID: 9712
		private string m_statusInformation;
	}
}
