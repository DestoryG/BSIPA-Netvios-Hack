using System;
using System.Globalization;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000474 RID: 1140
	public sealed class X509ChainPolicy
	{
		// Token: 0x06002A46 RID: 10822 RVA: 0x000C0F0F File Offset: 0x000BF10F
		public X509ChainPolicy()
		{
			this.Reset();
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06002A47 RID: 10823 RVA: 0x000C0F1D File Offset: 0x000BF11D
		public OidCollection ApplicationPolicy
		{
			get
			{
				return this.m_applicationPolicy;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x000C0F25 File Offset: 0x000BF125
		public OidCollection CertificatePolicy
		{
			get
			{
				return this.m_certificatePolicy;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06002A49 RID: 10825 RVA: 0x000C0F2D File Offset: 0x000BF12D
		// (set) Token: 0x06002A4A RID: 10826 RVA: 0x000C0F35 File Offset: 0x000BF135
		public X509RevocationMode RevocationMode
		{
			get
			{
				return this.m_revocationMode;
			}
			set
			{
				if (value < X509RevocationMode.NoCheck || value > X509RevocationMode.Offline)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "value" }));
				}
				this.m_revocationMode = value;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06002A4B RID: 10827 RVA: 0x000C0F6E File Offset: 0x000BF16E
		// (set) Token: 0x06002A4C RID: 10828 RVA: 0x000C0F76 File Offset: 0x000BF176
		public X509RevocationFlag RevocationFlag
		{
			get
			{
				return this.m_revocationFlag;
			}
			set
			{
				if (value < X509RevocationFlag.EndCertificateOnly || value > X509RevocationFlag.ExcludeRoot)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "value" }));
				}
				this.m_revocationFlag = value;
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06002A4D RID: 10829 RVA: 0x000C0FAF File Offset: 0x000BF1AF
		// (set) Token: 0x06002A4E RID: 10830 RVA: 0x000C0FB7 File Offset: 0x000BF1B7
		public X509VerificationFlags VerificationFlags
		{
			get
			{
				return this.m_verificationFlags;
			}
			set
			{
				if (value < X509VerificationFlags.NoFlag || value > X509VerificationFlags.AllFlags)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "value" }));
				}
				this.m_verificationFlags = value;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06002A4F RID: 10831 RVA: 0x000C0FF4 File Offset: 0x000BF1F4
		// (set) Token: 0x06002A50 RID: 10832 RVA: 0x000C0FFC File Offset: 0x000BF1FC
		public DateTime VerificationTime
		{
			get
			{
				return this.m_verificationTime;
			}
			set
			{
				this.m_verificationTime = value;
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06002A51 RID: 10833 RVA: 0x000C1005 File Offset: 0x000BF205
		// (set) Token: 0x06002A52 RID: 10834 RVA: 0x000C100D File Offset: 0x000BF20D
		public TimeSpan UrlRetrievalTimeout
		{
			get
			{
				return this.m_timeout;
			}
			set
			{
				this.m_timeout = value;
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06002A53 RID: 10835 RVA: 0x000C1016 File Offset: 0x000BF216
		public X509Certificate2Collection ExtraStore
		{
			get
			{
				return this.m_extraStore;
			}
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000C1020 File Offset: 0x000BF220
		public void Reset()
		{
			this.m_applicationPolicy = new OidCollection();
			this.m_certificatePolicy = new OidCollection();
			this.m_revocationMode = X509RevocationMode.Online;
			this.m_revocationFlag = X509RevocationFlag.ExcludeRoot;
			this.m_verificationFlags = X509VerificationFlags.NoFlag;
			this.m_verificationTime = DateTime.Now;
			this.m_timeout = new TimeSpan(0, 0, 0);
			this.m_extraStore = new X509Certificate2Collection();
		}

		// Token: 0x04002616 RID: 9750
		private OidCollection m_applicationPolicy;

		// Token: 0x04002617 RID: 9751
		private OidCollection m_certificatePolicy;

		// Token: 0x04002618 RID: 9752
		private X509RevocationMode m_revocationMode;

		// Token: 0x04002619 RID: 9753
		private X509RevocationFlag m_revocationFlag;

		// Token: 0x0400261A RID: 9754
		private DateTime m_verificationTime;

		// Token: 0x0400261B RID: 9755
		private TimeSpan m_timeout;

		// Token: 0x0400261C RID: 9756
		private X509Certificate2Collection m_extraStore;

		// Token: 0x0400261D RID: 9757
		private X509VerificationFlags m_verificationFlags;
	}
}
