using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200046E RID: 1134
	public class X509ChainElement
	{
		// Token: 0x06002A31 RID: 10801 RVA: 0x000C0BEA File Offset: 0x000BEDEA
		private X509ChainElement()
		{
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x000C0BF4 File Offset: 0x000BEDF4
		internal unsafe X509ChainElement(IntPtr pChainElement)
		{
			CAPIBase.CERT_CHAIN_ELEMENT cert_CHAIN_ELEMENT = new CAPIBase.CERT_CHAIN_ELEMENT(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_ELEMENT)));
			uint num = (uint)Marshal.ReadInt32(pChainElement);
			if ((ulong)num > (ulong)((long)Marshal.SizeOf(cert_CHAIN_ELEMENT)))
			{
				num = (uint)Marshal.SizeOf(cert_CHAIN_ELEMENT);
			}
			X509Utils.memcpy(pChainElement, new IntPtr((void*)(&cert_CHAIN_ELEMENT)), num);
			this.m_certificate = new X509Certificate2(cert_CHAIN_ELEMENT.pCertContext);
			if (cert_CHAIN_ELEMENT.pwszExtendedErrorInfo == IntPtr.Zero)
			{
				this.m_description = string.Empty;
			}
			else
			{
				this.m_description = Marshal.PtrToStringUni(cert_CHAIN_ELEMENT.pwszExtendedErrorInfo);
			}
			if (cert_CHAIN_ELEMENT.dwErrorStatus == 0U)
			{
				this.m_chainStatus = new X509ChainStatus[0];
				return;
			}
			this.m_chainStatus = X509Chain.GetChainStatusInformation(cert_CHAIN_ELEMENT.dwErrorStatus);
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06002A33 RID: 10803 RVA: 0x000C0CB6 File Offset: 0x000BEEB6
		public X509Certificate2 Certificate
		{
			get
			{
				return this.m_certificate;
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06002A34 RID: 10804 RVA: 0x000C0CBE File Offset: 0x000BEEBE
		public X509ChainStatus[] ChainElementStatus
		{
			get
			{
				return this.m_chainStatus;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06002A35 RID: 10805 RVA: 0x000C0CC6 File Offset: 0x000BEEC6
		public string Information
		{
			get
			{
				return this.m_description;
			}
		}

		// Token: 0x040025F9 RID: 9721
		private X509Certificate2 m_certificate;

		// Token: 0x040025FA RID: 9722
		private X509ChainStatus[] m_chainStatus;

		// Token: 0x040025FB RID: 9723
		private string m_description;
	}
}
