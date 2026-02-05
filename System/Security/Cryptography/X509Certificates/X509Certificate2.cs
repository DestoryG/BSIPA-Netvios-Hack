using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000466 RID: 1126
	[Serializable]
	public class X509Certificate2 : X509Certificate
	{
		// Token: 0x060029B2 RID: 10674 RVA: 0x000BD172 File Offset: 0x000BB372
		public X509Certificate2()
		{
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000BD185 File Offset: 0x000BB385
		public X509Certificate2(byte[] rawData)
			: base(rawData)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x000BD1AA File Offset: 0x000BB3AA
		public X509Certificate2(byte[] rawData, string password)
			: base(rawData, password)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x000BD1D0 File Offset: 0x000BB3D0
		public X509Certificate2(byte[] rawData, SecureString password)
			: base(rawData, password)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x000BD1F6 File Offset: 0x000BB3F6
		public X509Certificate2(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
			: base(rawData, password, keyStorageFlags)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000BD21D File Offset: 0x000BB41D
		public X509Certificate2(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
			: base(rawData, password, keyStorageFlags)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x000BD244 File Offset: 0x000BB444
		public X509Certificate2(string fileName)
			: base(fileName)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x000BD269 File Offset: 0x000BB469
		public X509Certificate2(string fileName, string password)
			: base(fileName, password)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x000BD28F File Offset: 0x000BB48F
		public X509Certificate2(string fileName, SecureString password)
			: base(fileName, password)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x000BD2B5 File Offset: 0x000BB4B5
		public X509Certificate2(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
			: base(fileName, password, keyStorageFlags)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x000BD2DC File Offset: 0x000BB4DC
		public X509Certificate2(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
			: base(fileName, password, keyStorageFlags)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x000BD303 File Offset: 0x000BB503
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public X509Certificate2(IntPtr handle)
			: base(handle)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000BD328 File Offset: 0x000BB528
		public X509Certificate2(X509Certificate certificate)
			: base(certificate)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x000BD34D File Offset: 0x000BB54D
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected X509Certificate2(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x000BD373 File Offset: 0x000BB573
		public override string ToString()
		{
			return base.ToString(true);
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x000BD37C File Offset: 0x000BB57C
		public override string ToString(bool verbose)
		{
			if (!verbose || this.m_safeCertContext.IsInvalid)
			{
				return this.ToString();
			}
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Environment.NewLine;
			string text = newLine + newLine;
			string text2 = newLine + "  ";
			stringBuilder.Append("[Version]");
			stringBuilder.Append(text2);
			stringBuilder.Append("V" + this.Version.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("[Subject]");
			stringBuilder.Append(text2);
			stringBuilder.Append(this.SubjectName.Name);
			string text3 = this.GetNameInfo(X509NameType.SimpleName, false);
			if (text3.Length > 0)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append("Simple Name: ");
				stringBuilder.Append(text3);
			}
			string text4 = this.GetNameInfo(X509NameType.EmailName, false);
			if (text4.Length > 0)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append("Email Name: ");
				stringBuilder.Append(text4);
			}
			string text5 = this.GetNameInfo(X509NameType.UpnName, false);
			if (text5.Length > 0)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append("UPN Name: ");
				stringBuilder.Append(text5);
			}
			string text6 = this.GetNameInfo(X509NameType.DnsName, false);
			if (text6.Length > 0)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append("DNS Name: ");
				stringBuilder.Append(text6);
			}
			stringBuilder.Append(text);
			stringBuilder.Append("[Issuer]");
			stringBuilder.Append(text2);
			stringBuilder.Append(this.IssuerName.Name);
			text3 = this.GetNameInfo(X509NameType.SimpleName, true);
			if (text3.Length > 0)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append("Simple Name: ");
				stringBuilder.Append(text3);
			}
			text4 = this.GetNameInfo(X509NameType.EmailName, true);
			if (text4.Length > 0)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append("Email Name: ");
				stringBuilder.Append(text4);
			}
			text5 = this.GetNameInfo(X509NameType.UpnName, true);
			if (text5.Length > 0)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append("UPN Name: ");
				stringBuilder.Append(text5);
			}
			text6 = this.GetNameInfo(X509NameType.DnsName, true);
			if (text6.Length > 0)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append("DNS Name: ");
				stringBuilder.Append(text6);
			}
			stringBuilder.Append(text);
			stringBuilder.Append("[Serial Number]");
			stringBuilder.Append(text2);
			stringBuilder.Append(this.SerialNumber);
			stringBuilder.Append(text);
			stringBuilder.Append("[Not Before]");
			stringBuilder.Append(text2);
			stringBuilder.Append(X509Certificate.FormatDate(this.NotBefore));
			stringBuilder.Append(text);
			stringBuilder.Append("[Not After]");
			stringBuilder.Append(text2);
			stringBuilder.Append(X509Certificate.FormatDate(this.NotAfter));
			stringBuilder.Append(text);
			stringBuilder.Append("[Thumbprint]");
			stringBuilder.Append(text2);
			stringBuilder.Append(this.Thumbprint);
			stringBuilder.Append(text);
			stringBuilder.Append("[Signature Algorithm]");
			stringBuilder.Append(text2);
			stringBuilder.Append(this.SignatureAlgorithm.FriendlyName + "(" + this.SignatureAlgorithm.Value + ")");
			stringBuilder.Append(text);
			stringBuilder.Append("[Public Key]");
			try
			{
				PublicKey publicKey = this.PublicKey;
				string text7 = publicKey.Oid.FriendlyName;
				stringBuilder.Append(text2);
				stringBuilder.Append("Algorithm: ");
				stringBuilder.Append(text7);
				try
				{
					text7 = publicKey.Key.KeySize.ToString();
					stringBuilder.Append(text2);
					stringBuilder.Append("Length: ");
					stringBuilder.Append(text7);
				}
				catch (NotSupportedException)
				{
				}
				text7 = publicKey.EncodedKeyValue.Format(true);
				stringBuilder.Append(text2);
				stringBuilder.Append("Key Blob: ");
				stringBuilder.Append(text7);
				text7 = publicKey.EncodedParameters.Format(true);
				stringBuilder.Append(text2);
				stringBuilder.Append("Parameters: ");
				stringBuilder.Append(text7);
			}
			catch (CryptographicException)
			{
			}
			this.AppendPrivateKeyInfo(stringBuilder);
			X509ExtensionCollection extensions = this.Extensions;
			if (extensions.Count > 0)
			{
				stringBuilder.Append(text);
				stringBuilder.Append("[Extensions]");
				foreach (X509Extension x509Extension in extensions)
				{
					try
					{
						string text8 = x509Extension.Oid.FriendlyName;
						stringBuilder.Append(newLine);
						stringBuilder.Append("* " + text8);
						stringBuilder.Append("(" + x509Extension.Oid.Value + "):");
						text8 = x509Extension.Format(true);
						stringBuilder.Append(text2);
						stringBuilder.Append(text8);
					}
					catch (CryptographicException)
					{
					}
				}
			}
			stringBuilder.Append(newLine);
			return stringBuilder.ToString();
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x060029C2 RID: 10690 RVA: 0x000BD89C File Offset: 0x000BBA9C
		// (set) Token: 0x060029C3 RID: 10691 RVA: 0x000BD8E4 File Offset: 0x000BBAE4
		public bool Archived
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				uint num = 0U;
				return CAPISafe.CertGetCertificateContextProperty(this.m_safeCertContext, 19U, SafeLocalAllocHandle.InvalidHandle, ref num);
			}
			set
			{
				SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
				if (value)
				{
					safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CRYPTOAPI_BLOB))));
				}
				if (!CAPI.CertSetCertificateContextProperty(this.m_safeCertContext, 19U, 0U, safeLocalAllocHandle))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				safeLocalAllocHandle.Dispose();
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x060029C4 RID: 10692 RVA: 0x000BD938 File Offset: 0x000BBB38
		public X509ExtensionCollection Extensions
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_extensions == null)
				{
					this.m_extensions = new X509ExtensionCollection(this.m_safeCertContext);
				}
				return this.m_extensions;
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060029C5 RID: 10693 RVA: 0x000BD988 File Offset: 0x000BBB88
		// (set) Token: 0x060029C6 RID: 10694 RVA: 0x000BDA10 File Offset: 0x000BBC10
		public string FriendlyName
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
				uint num = 0U;
				if (!CAPISafe.CertGetCertificateContextProperty(this.m_safeCertContext, 11U, safeLocalAllocHandle, ref num))
				{
					return string.Empty;
				}
				safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
				if (!CAPISafe.CertGetCertificateContextProperty(this.m_safeCertContext, 11U, safeLocalAllocHandle, ref num))
				{
					return string.Empty;
				}
				string text = Marshal.PtrToStringUni(safeLocalAllocHandle.DangerousGetHandle());
				safeLocalAllocHandle.Dispose();
				return text;
			}
			set
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (value == null)
				{
					value = string.Empty;
				}
				X509Certificate2.SetFriendlyNameExtendedProperty(this.m_safeCertContext, value);
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060029C7 RID: 10695 RVA: 0x000BDA4C File Offset: 0x000BBC4C
		public unsafe X500DistinguishedName IssuerName
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_issuerName == null)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)this.m_safeCertContext.DangerousGetHandle();
					CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
					this.m_issuerName = new X500DistinguishedName(cert_INFO.Issuer);
				}
				return this.m_issuerName;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060029C8 RID: 10696 RVA: 0x000BDACC File Offset: 0x000BBCCC
		public unsafe DateTime NotAfter
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_notAfter == DateTime.MinValue)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)this.m_safeCertContext.DangerousGetHandle();
					CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
					long num = (long)(((ulong)cert_INFO.NotAfter.dwHighDateTime << 32) | (ulong)cert_INFO.NotAfter.dwLowDateTime);
					this.m_notAfter = DateTime.FromFileTime(num);
				}
				return this.m_notAfter;
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x000BDB70 File Offset: 0x000BBD70
		public unsafe DateTime NotBefore
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_notBefore == DateTime.MinValue)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)this.m_safeCertContext.DangerousGetHandle();
					CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
					long num = (long)(((ulong)cert_INFO.NotBefore.dwHighDateTime << 32) | (ulong)cert_INFO.NotBefore.dwLowDateTime);
					this.m_notBefore = DateTime.FromFileTime(num);
				}
				return this.m_notBefore;
			}
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060029CA RID: 10698 RVA: 0x000BDC14 File Offset: 0x000BBE14
		public bool HasPrivateKey
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				uint num = 0U;
				bool flag;
				using (SafeLocalAllocHandle invalidHandle = SafeLocalAllocHandle.InvalidHandle)
				{
					flag = CAPISafe.CertGetCertificateContextProperty(this.m_safeCertContext, 5U, invalidHandle, ref num);
					if (!flag)
					{
						flag = CAPISafe.CertGetCertificateContextProperty(this.m_safeCertContext, 2U, invalidHandle, ref num);
					}
				}
				return flag;
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060029CB RID: 10699 RVA: 0x000BDC8C File Offset: 0x000BBE8C
		// (set) Token: 0x060029CC RID: 10700 RVA: 0x000BDD28 File Offset: 0x000BBF28
		public AsymmetricAlgorithm PrivateKey
		{
			get
			{
				if (!this.HasPrivateKey)
				{
					return null;
				}
				if (this.m_privateKey == null)
				{
					CspParameters cspParameters = new CspParameters();
					if (!X509Certificate2.GetPrivateKeyInfo(this.m_safeCertContext, ref cspParameters))
					{
						return null;
					}
					cspParameters.Flags |= CspProviderFlags.UseExistingKey;
					uint algorithmId = this.PublicKey.AlgorithmId;
					if (algorithmId != 8704U)
					{
						if (algorithmId != 9216U && algorithmId != 41984U)
						{
							throw new NotSupportedException(SR.GetString("NotSupported_KeyAlgorithm"));
						}
						this.m_privateKey = new RSACryptoServiceProvider(cspParameters);
					}
					else
					{
						this.m_privateKey = new DSACryptoServiceProvider(cspParameters);
					}
				}
				return this.m_privateKey;
			}
			set
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				ICspAsymmetricAlgorithm cspAsymmetricAlgorithm = value as ICspAsymmetricAlgorithm;
				if (value != null && cspAsymmetricAlgorithm == null)
				{
					throw new NotSupportedException(SR.GetString("NotSupported_InvalidKeyImpl"));
				}
				if (cspAsymmetricAlgorithm != null)
				{
					if (cspAsymmetricAlgorithm.CspKeyContainerInfo == null)
					{
						throw new ArgumentException("CspKeyContainerInfo");
					}
					if (X509Certificate2.s_publicKeyOffset == 0)
					{
						X509Certificate2.s_publicKeyOffset = Marshal.SizeOf(typeof(CAPIBase.BLOBHEADER));
					}
					ICspAsymmetricAlgorithm cspAsymmetricAlgorithm2 = this.PublicKey.Key as ICspAsymmetricAlgorithm;
					byte[] array = cspAsymmetricAlgorithm2.ExportCspBlob(false);
					byte[] array2 = cspAsymmetricAlgorithm.ExportCspBlob(false);
					if (array == null || array2 == null || array.Length != array2.Length || array.Length <= X509Certificate2.s_publicKeyOffset)
					{
						throw new CryptographicUnexpectedOperationException(SR.GetString("Cryptography_X509_KeyMismatch"));
					}
					for (int i = X509Certificate2.s_publicKeyOffset; i < array.Length; i++)
					{
						if (array[i] != array2[i])
						{
							throw new CryptographicUnexpectedOperationException(SR.GetString("Cryptography_X509_KeyMismatch"));
						}
					}
				}
				X509Certificate2.SetPrivateKeyProperty(this.m_safeCertContext, cspAsymmetricAlgorithm);
				this.m_privateKey = value;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x060029CD RID: 10701 RVA: 0x000BDE34 File Offset: 0x000BC034
		public PublicKey PublicKey
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_publicKey == null)
				{
					string keyAlgorithm = this.GetKeyAlgorithm();
					byte[] keyAlgorithmParameters = this.GetKeyAlgorithmParameters();
					byte[] publicKey = this.GetPublicKey();
					Oid oid = new Oid(keyAlgorithm, OidGroup.PublicKeyAlgorithm, true);
					this.m_publicKey = new PublicKey(oid, new AsnEncodedData(oid, keyAlgorithmParameters), new AsnEncodedData(oid, publicKey));
				}
				return this.m_publicKey;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x000BDEA9 File Offset: 0x000BC0A9
		public byte[] RawData
		{
			get
			{
				return this.GetRawCertData();
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x060029CF RID: 10703 RVA: 0x000BDEB1 File Offset: 0x000BC0B1
		public string SerialNumber
		{
			get
			{
				return this.GetSerialNumberString();
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x060029D0 RID: 10704 RVA: 0x000BDEBC File Offset: 0x000BC0BC
		public unsafe X500DistinguishedName SubjectName
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_subjectName == null)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)this.m_safeCertContext.DangerousGetHandle();
					CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
					this.m_subjectName = new X500DistinguishedName(cert_INFO.Subject);
				}
				return this.m_subjectName;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x060029D1 RID: 10705 RVA: 0x000BDF3C File Offset: 0x000BC13C
		public Oid SignatureAlgorithm
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_signatureAlgorithm == null)
				{
					this.m_signatureAlgorithm = X509Certificate2.GetSignatureAlgorithm(this.m_safeCertContext);
				}
				return this.m_signatureAlgorithm;
			}
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x060029D2 RID: 10706 RVA: 0x000BDF8A File Offset: 0x000BC18A
		public string Thumbprint
		{
			get
			{
				return this.GetCertHashString();
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x060029D3 RID: 10707 RVA: 0x000BDF94 File Offset: 0x000BC194
		public int Version
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_version == 0)
				{
					this.m_version = (int)X509Certificate2.GetVersion(this.m_safeCertContext);
				}
				return this.m_version;
			}
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x000BDFE4 File Offset: 0x000BC1E4
		public unsafe string GetNameInfo(X509NameType nameType, bool forIssuer)
		{
			uint num = (forIssuer ? 1U : 0U);
			uint num2 = X509Utils.MapNameType(nameType);
			if (num2 == 1U)
			{
				return CAPI.GetCertNameInfo(this.m_safeCertContext, num, num2);
			}
			if (num2 == 4U)
			{
				return CAPI.GetCertNameInfo(this.m_safeCertContext, num, num2);
			}
			string text = string.Empty;
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)this.m_safeCertContext.DangerousGetHandle();
			CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
			IntPtr[] array = new IntPtr[]
			{
				CAPISafe.CertFindExtension(forIssuer ? "2.5.29.8" : "2.5.29.7", cert_INFO.cExtension, cert_INFO.rgExtension),
				CAPISafe.CertFindExtension(forIssuer ? "2.5.29.18" : "2.5.29.17", cert_INFO.cExtension, cert_INFO.rgExtension)
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != IntPtr.Zero)
				{
					CAPIBase.CERT_EXTENSION cert_EXTENSION = (CAPIBase.CERT_EXTENSION)Marshal.PtrToStructure(array[i], typeof(CAPIBase.CERT_EXTENSION));
					byte[] array2 = new byte[cert_EXTENSION.Value.cbData];
					Marshal.Copy(cert_EXTENSION.Value.pbData, array2, 0, array2.Length);
					uint num3 = 0U;
					SafeLocalAllocHandle safeLocalAllocHandle = null;
					SafeLocalAllocHandle safeLocalAllocHandle2 = X509Utils.StringToAnsiPtr(cert_EXTENSION.pszObjId);
					bool flag = CAPI.DecodeObject(safeLocalAllocHandle2.DangerousGetHandle(), array2, out safeLocalAllocHandle, out num3);
					safeLocalAllocHandle2.Dispose();
					if (flag)
					{
						CAPIBase.CERT_ALT_NAME_INFO cert_ALT_NAME_INFO = (CAPIBase.CERT_ALT_NAME_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_ALT_NAME_INFO));
						int num4 = 0;
						while ((long)num4 < (long)((ulong)cert_ALT_NAME_INFO.cAltEntry))
						{
							IntPtr intPtr = new IntPtr((long)cert_ALT_NAME_INFO.rgAltEntry + (long)(num4 * Marshal.SizeOf(typeof(CAPIBase.CERT_ALT_NAME_ENTRY))));
							CAPIBase.CERT_ALT_NAME_ENTRY cert_ALT_NAME_ENTRY = (CAPIBase.CERT_ALT_NAME_ENTRY)Marshal.PtrToStructure(intPtr, typeof(CAPIBase.CERT_ALT_NAME_ENTRY));
							switch (num2)
							{
							case 6U:
								if (cert_ALT_NAME_ENTRY.dwAltNameChoice == 3U)
								{
									text = Marshal.PtrToStringUni(cert_ALT_NAME_ENTRY.Value.pwszDNSName);
								}
								break;
							case 7U:
								if (cert_ALT_NAME_ENTRY.dwAltNameChoice == 7U)
								{
									text = Marshal.PtrToStringUni(cert_ALT_NAME_ENTRY.Value.pwszURL);
								}
								break;
							case 8U:
								if (cert_ALT_NAME_ENTRY.dwAltNameChoice == 1U)
								{
									CAPIBase.CERT_OTHER_NAME cert_OTHER_NAME = (CAPIBase.CERT_OTHER_NAME)Marshal.PtrToStructure(cert_ALT_NAME_ENTRY.Value.pOtherName, typeof(CAPIBase.CERT_OTHER_NAME));
									if (cert_OTHER_NAME.pszObjId == "1.3.6.1.4.1.311.20.2.3")
									{
										uint num5 = 0U;
										SafeLocalAllocHandle safeLocalAllocHandle3 = null;
										flag = CAPI.DecodeObject(new IntPtr(24L), X509Utils.PtrToByte(cert_OTHER_NAME.Value.pbData, cert_OTHER_NAME.Value.cbData), out safeLocalAllocHandle3, out num5);
										if (flag)
										{
											CAPIBase.CERT_NAME_VALUE cert_NAME_VALUE = (CAPIBase.CERT_NAME_VALUE)Marshal.PtrToStructure(safeLocalAllocHandle3.DangerousGetHandle(), typeof(CAPIBase.CERT_NAME_VALUE));
											if (X509Utils.IsCertRdnCharString(cert_NAME_VALUE.dwValueType))
											{
												text = Marshal.PtrToStringUni(cert_NAME_VALUE.Value.pbData);
											}
											safeLocalAllocHandle3.Dispose();
										}
									}
								}
								break;
							}
							num4++;
						}
						safeLocalAllocHandle.Dispose();
					}
				}
			}
			if (nameType == X509NameType.DnsName && (text == null || text.Length == 0))
			{
				text = CAPI.GetCertNameInfo(this.m_safeCertContext, num, 3U);
			}
			return text;
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x000BE313 File Offset: 0x000BC513
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Import(byte[] rawData)
		{
			this.Reset();
			base.Import(rawData);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x000BE333 File Offset: 0x000BC533
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Import(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			base.Import(rawData, password, keyStorageFlags);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x000BE355 File Offset: 0x000BC555
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Import(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			base.Import(rawData, password, keyStorageFlags);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x000BE377 File Offset: 0x000BC577
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Import(string fileName)
		{
			this.Reset();
			base.Import(fileName);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x000BE397 File Offset: 0x000BC597
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Import(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			base.Import(fileName, password, keyStorageFlags);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x000BE3B9 File Offset: 0x000BC5B9
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Import(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			base.Import(fileName, password, keyStorageFlags);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x000BE3DC File Offset: 0x000BC5DC
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Reset()
		{
			this.m_version = 0;
			this.m_notBefore = DateTime.MinValue;
			this.m_notAfter = DateTime.MinValue;
			this.m_privateKey = null;
			this.m_publicKey = null;
			this.m_extensions = null;
			this.m_signatureAlgorithm = null;
			this.m_subjectName = null;
			this.m_issuerName = null;
			if (!this.m_safeCertContext.IsInvalid)
			{
				this.m_safeCertContext.Dispose();
				this.m_safeCertContext = SafeCertContextHandle.InvalidHandle;
			}
			base.Reset();
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x000BE45C File Offset: 0x000BC65C
		public bool Verify()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
			}
			int num = X509Utils.VerifyCertificate(this.CertContext, null, null, X509RevocationMode.Online, X509RevocationFlag.ExcludeRoot, DateTime.Now, new TimeSpan(0, 0, 0), null, new IntPtr(1L), IntPtr.Zero);
			return num == 0;
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x000BE4BC File Offset: 0x000BC6BC
		public static X509ContentType GetCertContentType(byte[] rawData)
		{
			if (rawData == null || rawData.Length == 0)
			{
				throw new ArgumentException(SR.GetString("Arg_EmptyOrNullArray"), "rawData");
			}
			uint num = X509Certificate2.QueryCertBlobType(rawData);
			return X509Utils.MapContentType(num);
		}

		// Token: 0x060029DE RID: 10718 RVA: 0x000BE4F4 File Offset: 0x000BC6F4
		public static X509ContentType GetCertContentType(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			string fullPath = Path.GetFullPath(fileName);
			new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();
			uint num = X509Certificate2.QueryCertFileType(fileName);
			return X509Utils.MapContentType(num);
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060029DF RID: 10719 RVA: 0x000BE52F File Offset: 0x000BC72F
		internal new SafeCertContextHandle CertContext
		{
			get
			{
				return this.m_safeCertContext;
			}
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x000BE538 File Offset: 0x000BC738
		internal static bool GetPrivateKeyInfo(SafeCertContextHandle safeCertContext, ref CspParameters parameters)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			uint num = 0U;
			if (!CAPISafe.CertGetCertificateContextProperty(safeCertContext, 2U, safeLocalAllocHandle, ref num))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == -2146885628)
				{
					return false;
				}
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			else
			{
				safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
				if (CAPISafe.CertGetCertificateContextProperty(safeCertContext, 2U, safeLocalAllocHandle, ref num))
				{
					CAPIBase.CRYPT_KEY_PROV_INFO crypt_KEY_PROV_INFO = (CAPIBase.CRYPT_KEY_PROV_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPT_KEY_PROV_INFO));
					parameters.ProviderName = crypt_KEY_PROV_INFO.pwszProvName;
					parameters.KeyContainerName = crypt_KEY_PROV_INFO.pwszContainerName;
					parameters.ProviderType = (int)crypt_KEY_PROV_INFO.dwProvType;
					parameters.KeyNumber = (int)crypt_KEY_PROV_INFO.dwKeySpec;
					parameters.Flags = (((crypt_KEY_PROV_INFO.dwFlags & 32U) == 32U) ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
					safeLocalAllocHandle.Dispose();
					return true;
				}
				int lastWin32Error2 = Marshal.GetLastWin32Error();
				if (lastWin32Error2 == -2146885628)
				{
					return false;
				}
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x000BE61C File Offset: 0x000BC81C
		private void AppendPrivateKeyInfo(StringBuilder sb)
		{
			if (!this.HasPrivateKey)
			{
				return;
			}
			CspKeyContainerInfo cspKeyContainerInfo = null;
			try
			{
				CspParameters cspParameters = new CspParameters();
				if (X509Certificate2.GetPrivateKeyInfo(this.m_safeCertContext, ref cspParameters))
				{
					cspKeyContainerInfo = new CspKeyContainerInfo(cspParameters);
				}
			}
			catch (SecurityException)
			{
			}
			catch (CryptographicException)
			{
			}
			sb.Append(Environment.NewLine + Environment.NewLine + "[Private Key]");
			if (cspKeyContainerInfo == null)
			{
				return;
			}
			sb.Append(Environment.NewLine + "  Key Store: ");
			sb.Append(cspKeyContainerInfo.MachineKeyStore ? "Machine" : "User");
			sb.Append(Environment.NewLine + "  Provider Name: ");
			sb.Append(cspKeyContainerInfo.ProviderName);
			sb.Append(Environment.NewLine + "  Provider type: ");
			sb.Append(cspKeyContainerInfo.ProviderType);
			sb.Append(Environment.NewLine + "  Key Spec: ");
			sb.Append(cspKeyContainerInfo.KeyNumber);
			sb.Append(Environment.NewLine + "  Key Container Name: ");
			sb.Append(cspKeyContainerInfo.KeyContainerName);
			try
			{
				string uniqueKeyContainerName = cspKeyContainerInfo.UniqueKeyContainerName;
				sb.Append(Environment.NewLine + "  Unique Key Container Name: ");
				sb.Append(uniqueKeyContainerName);
			}
			catch (CryptographicException)
			{
			}
			catch (NotSupportedException)
			{
			}
			try
			{
				bool flag = cspKeyContainerInfo.HardwareDevice;
				sb.Append(Environment.NewLine + "  Hardware Device: ");
				sb.Append(flag);
			}
			catch (CryptographicException)
			{
			}
			try
			{
				bool flag = cspKeyContainerInfo.Removable;
				sb.Append(Environment.NewLine + "  Removable: ");
				sb.Append(flag);
			}
			catch (CryptographicException)
			{
			}
			try
			{
				bool flag = cspKeyContainerInfo.Protected;
				sb.Append(Environment.NewLine + "  Protected: ");
				sb.Append(flag);
			}
			catch (CryptographicException)
			{
			}
			catch (NotSupportedException)
			{
			}
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x000BE850 File Offset: 0x000BCA50
		private unsafe static Oid GetSignatureAlgorithm(SafeCertContextHandle safeCertContextHandle)
		{
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)safeCertContextHandle.DangerousGetHandle();
			CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
			return new Oid(cert_INFO.SignatureAlgorithm.pszObjId, OidGroup.SignatureAlgorithm, false);
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000BE89C File Offset: 0x000BCA9C
		private unsafe static uint GetVersion(SafeCertContextHandle safeCertContextHandle)
		{
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)safeCertContextHandle.DangerousGetHandle();
			CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
			return cert_INFO.dwVersion + 1U;
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x000BE8E0 File Offset: 0x000BCAE0
		private unsafe static uint QueryCertBlobType(byte[] rawData)
		{
			uint num = 0U;
			if (!CAPI.CryptQueryObject(2U, rawData, 16382U, 14U, 0U, IntPtr.Zero, new IntPtr((void*)(&num)), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return num;
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x000BE930 File Offset: 0x000BCB30
		private unsafe static uint QueryCertFileType(string fileName)
		{
			uint num = 0U;
			if (!CAPI.CryptQueryObject(1U, fileName, 16382U, 14U, 0U, IntPtr.Zero, new IntPtr((void*)(&num)), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return num;
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x000BE980 File Offset: 0x000BCB80
		private unsafe static void SetFriendlyNameExtendedProperty(SafeCertContextHandle safeCertContextHandle, string name)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = X509Utils.StringToUniPtr(name);
			using (safeLocalAllocHandle)
			{
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
				cryptoapi_BLOB.cbData = (uint)(2 * (name.Length + 1));
				cryptoapi_BLOB.pbData = safeLocalAllocHandle.DangerousGetHandle();
				if (!CAPI.CertSetCertificateContextProperty(safeCertContextHandle, 11U, 0U, new IntPtr((void*)(&cryptoapi_BLOB))))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x000BE9F8 File Offset: 0x000BCBF8
		private static void SetPrivateKeyProperty(SafeCertContextHandle safeCertContextHandle, ICspAsymmetricAlgorithm asymmetricAlgorithm)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (asymmetricAlgorithm != null)
			{
				CAPIBase.CRYPT_KEY_PROV_INFO crypt_KEY_PROV_INFO = default(CAPIBase.CRYPT_KEY_PROV_INFO);
				crypt_KEY_PROV_INFO.pwszContainerName = asymmetricAlgorithm.CspKeyContainerInfo.KeyContainerName;
				crypt_KEY_PROV_INFO.pwszProvName = asymmetricAlgorithm.CspKeyContainerInfo.ProviderName;
				crypt_KEY_PROV_INFO.dwProvType = (uint)asymmetricAlgorithm.CspKeyContainerInfo.ProviderType;
				crypt_KEY_PROV_INFO.dwFlags = (asymmetricAlgorithm.CspKeyContainerInfo.MachineKeyStore ? 32U : 0U);
				crypt_KEY_PROV_INFO.cProvParam = 0U;
				crypt_KEY_PROV_INFO.rgProvParam = IntPtr.Zero;
				crypt_KEY_PROV_INFO.dwKeySpec = (uint)asymmetricAlgorithm.CspKeyContainerInfo.KeyNumber;
				safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CRYPT_KEY_PROV_INFO))));
				Marshal.StructureToPtr(crypt_KEY_PROV_INFO, safeLocalAllocHandle.DangerousGetHandle(), false);
			}
			try
			{
				if (!CAPI.CertSetCertificateContextProperty(safeCertContextHandle, 2U, 0U, safeLocalAllocHandle))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			finally
			{
				if (!safeLocalAllocHandle.IsInvalid)
				{
					Marshal.DestroyStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPT_KEY_PROV_INFO));
					safeLocalAllocHandle.Dispose();
				}
			}
		}

		// Token: 0x040025B3 RID: 9651
		private int m_version;

		// Token: 0x040025B4 RID: 9652
		private DateTime m_notBefore;

		// Token: 0x040025B5 RID: 9653
		private DateTime m_notAfter;

		// Token: 0x040025B6 RID: 9654
		private AsymmetricAlgorithm m_privateKey;

		// Token: 0x040025B7 RID: 9655
		private PublicKey m_publicKey;

		// Token: 0x040025B8 RID: 9656
		private X509ExtensionCollection m_extensions;

		// Token: 0x040025B9 RID: 9657
		private Oid m_signatureAlgorithm;

		// Token: 0x040025BA RID: 9658
		private X500DistinguishedName m_subjectName;

		// Token: 0x040025BB RID: 9659
		private X500DistinguishedName m_issuerName;

		// Token: 0x040025BC RID: 9660
		private SafeCertContextHandle m_safeCertContext = SafeCertContextHandle.InvalidHandle;

		// Token: 0x040025BD RID: 9661
		private static int s_publicKeyOffset;

		// Token: 0x040025BE RID: 9662
		internal const X509KeyStorageFlags KeyStorageFlags47 = X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserProtected | X509KeyStorageFlags.PersistKeySet;

		// Token: 0x040025BF RID: 9663
		internal new const X509KeyStorageFlags KeyStorageFlagsAll = X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserProtected | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.EphemeralKeySet;
	}
}
