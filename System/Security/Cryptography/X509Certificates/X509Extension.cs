using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000475 RID: 1141
	public class X509Extension : AsnEncodedData
	{
		// Token: 0x06002A55 RID: 10837 RVA: 0x000C107C File Offset: 0x000BF27C
		internal X509Extension(string oid)
			: base(new Oid(oid, OidGroup.ExtensionOrAttribute, false))
		{
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000C108C File Offset: 0x000BF28C
		internal X509Extension(IntPtr pExtension)
		{
			CAPIBase.CERT_EXTENSION cert_EXTENSION = (CAPIBase.CERT_EXTENSION)Marshal.PtrToStructure(pExtension, typeof(CAPIBase.CERT_EXTENSION));
			this.m_critical = cert_EXTENSION.fCritical;
			string pszObjId = cert_EXTENSION.pszObjId;
			this.m_oid = new Oid(pszObjId, OidGroup.ExtensionOrAttribute, false);
			byte[] array = new byte[cert_EXTENSION.Value.cbData];
			if (cert_EXTENSION.Value.pbData != IntPtr.Zero)
			{
				Marshal.Copy(cert_EXTENSION.Value.pbData, array, 0, array.Length);
			}
			this.m_rawData = array;
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000C111A File Offset: 0x000BF31A
		protected X509Extension()
		{
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000C1122 File Offset: 0x000BF322
		public X509Extension(string oid, byte[] rawData, bool critical)
			: this(new Oid(oid, OidGroup.ExtensionOrAttribute, true), rawData, critical)
		{
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x000C1134 File Offset: 0x000BF334
		public X509Extension(AsnEncodedData encodedExtension, bool critical)
			: this(encodedExtension.Oid, encodedExtension.RawData, critical)
		{
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x000C114C File Offset: 0x000BF34C
		public X509Extension(Oid oid, byte[] rawData, bool critical)
			: base(oid, rawData)
		{
			if (base.Oid == null || base.Oid.Value == null)
			{
				throw new ArgumentNullException("oid");
			}
			if (base.Oid.Value.Length == 0)
			{
				throw new ArgumentException(SR.GetString("Arg_EmptyOrNullString"), "oid.Value");
			}
			this.m_critical = critical;
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06002A5B RID: 10843 RVA: 0x000C11AF File Offset: 0x000BF3AF
		// (set) Token: 0x06002A5C RID: 10844 RVA: 0x000C11B7 File Offset: 0x000BF3B7
		public bool Critical
		{
			get
			{
				return this.m_critical;
			}
			set
			{
				this.m_critical = value;
			}
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000C11C0 File Offset: 0x000BF3C0
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			X509Extension x509Extension = asnEncodedData as X509Extension;
			if (x509Extension == null)
			{
				throw new ArgumentException(SR.GetString("Cryptography_X509_ExtensionMismatch"));
			}
			base.CopyFrom(asnEncodedData);
			this.m_critical = x509Extension.Critical;
		}

		// Token: 0x0400261E RID: 9758
		private bool m_critical;
	}
}
