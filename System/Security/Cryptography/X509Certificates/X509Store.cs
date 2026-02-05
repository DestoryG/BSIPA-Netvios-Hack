using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000481 RID: 1153
	public sealed class X509Store : IDisposable
	{
		// Token: 0x06002A94 RID: 10900 RVA: 0x000C1F83 File Offset: 0x000C0183
		public X509Store()
			: this("MY", StoreLocation.CurrentUser)
		{
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000C1F91 File Offset: 0x000C0191
		public X509Store(string storeName)
			: this(storeName, StoreLocation.CurrentUser)
		{
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000C1F9B File Offset: 0x000C019B
		public X509Store(StoreName storeName)
			: this(storeName, StoreLocation.CurrentUser)
		{
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000C1FA5 File Offset: 0x000C01A5
		public X509Store(StoreLocation storeLocation)
			: this("MY", storeLocation)
		{
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000C1FB4 File Offset: 0x000C01B4
		public X509Store(StoreName storeName, StoreLocation storeLocation)
		{
			this.m_safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			base..ctor();
			if (storeLocation != StoreLocation.CurrentUser && storeLocation != StoreLocation.LocalMachine)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "storeLocation" }));
			}
			switch (storeName)
			{
			case StoreName.AddressBook:
				this.m_storeName = "AddressBook";
				break;
			case StoreName.AuthRoot:
				this.m_storeName = "AuthRoot";
				break;
			case StoreName.CertificateAuthority:
				this.m_storeName = "CA";
				break;
			case StoreName.Disallowed:
				this.m_storeName = "Disallowed";
				break;
			case StoreName.My:
				this.m_storeName = "My";
				break;
			case StoreName.Root:
				this.m_storeName = "Root";
				break;
			case StoreName.TrustedPeople:
				this.m_storeName = "TrustedPeople";
				break;
			case StoreName.TrustedPublisher:
				this.m_storeName = "TrustedPublisher";
				break;
			default:
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "storeName" }));
			}
			this.m_location = storeLocation;
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000C20C8 File Offset: 0x000C02C8
		public X509Store(string storeName, StoreLocation storeLocation)
		{
			this.m_safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			base..ctor();
			if (storeLocation != StoreLocation.CurrentUser && storeLocation != StoreLocation.LocalMachine)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "storeLocation" }));
			}
			this.m_storeName = storeName;
			this.m_location = storeLocation;
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000C2124 File Offset: 0x000C0324
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public X509Store(IntPtr storeHandle)
		{
			this.m_safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			base..ctor();
			if (storeHandle == IntPtr.Zero)
			{
				throw new ArgumentNullException("storeHandle");
			}
			this.m_safeCertStoreHandle = CAPISafe.CertDuplicateStore(storeHandle);
			if (this.m_safeCertStoreHandle == null || this.m_safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidStoreHandle"), "storeHandle");
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06002A9B RID: 10907 RVA: 0x000C2190 File Offset: 0x000C0390
		public IntPtr StoreHandle
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (this.m_safeCertStoreHandle == null || this.m_safeCertStoreHandle.IsInvalid || this.m_safeCertStoreHandle.IsClosed)
				{
					throw new CryptographicException(SR.GetString("Cryptography_X509_StoreNotOpen"));
				}
				return this.m_safeCertStoreHandle.DangerousGetHandle();
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06002A9C RID: 10908 RVA: 0x000C21CF File Offset: 0x000C03CF
		public StoreLocation Location
		{
			get
			{
				return this.m_location;
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06002A9D RID: 10909 RVA: 0x000C21D7 File Offset: 0x000C03D7
		public string Name
		{
			get
			{
				return this.m_storeName;
			}
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x000C21E0 File Offset: 0x000C03E0
		public void Open(OpenFlags flags)
		{
			if (this.m_location != StoreLocation.CurrentUser && this.m_location != StoreLocation.LocalMachine)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "m_location" }));
			}
			uint num = X509Utils.MapX509StoreFlags(this.m_location, flags);
			if (!this.m_safeCertStoreHandle.IsInvalid)
			{
				this.m_safeCertStoreHandle.Dispose();
			}
			this.m_safeCertStoreHandle = CAPI.CertOpenStore(new IntPtr(10L), 65537U, IntPtr.Zero, num, this.m_storeName);
			if (this.m_safeCertStoreHandle == null || this.m_safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPISafe.CertControlStore(this.m_safeCertStoreHandle, 0U, 4U, IntPtr.Zero);
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x000C22A3 File Offset: 0x000C04A3
		public void Dispose()
		{
			this.Close();
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x000C22AB File Offset: 0x000C04AB
		public void Close()
		{
			if (this.m_safeCertStoreHandle != null && !this.m_safeCertStoreHandle.IsClosed)
			{
				this.m_safeCertStoreHandle.Dispose();
			}
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x000C22D0 File Offset: 0x000C04D0
		public void Add(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			if (this.m_safeCertStoreHandle == null || this.m_safeCertStoreHandle.IsInvalid || this.m_safeCertStoreHandle.IsClosed)
			{
				throw new CryptographicException(SR.GetString("Cryptography_X509_StoreNotOpen"));
			}
			if (!CAPI.CertAddCertificateContextToStore(this.m_safeCertStoreHandle, certificate.CertContext, 5U, SafeCertContextHandle.InvalidHandle))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x000C2344 File Offset: 0x000C0544
		public void AddRange(X509Certificate2Collection certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			int num = 0;
			try
			{
				foreach (X509Certificate2 x509Certificate in certificates)
				{
					this.Add(x509Certificate);
					num++;
				}
			}
			catch
			{
				for (int i = 0; i < num; i++)
				{
					this.Remove(certificates[i]);
				}
				throw;
			}
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x000C23B4 File Offset: 0x000C05B4
		public void Remove(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			X509Store.RemoveCertificateFromStore(this.m_safeCertStoreHandle, certificate.CertContext);
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x000C23D8 File Offset: 0x000C05D8
		public void RemoveRange(X509Certificate2Collection certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			int num = 0;
			try
			{
				foreach (X509Certificate2 x509Certificate in certificates)
				{
					this.Remove(x509Certificate);
					num++;
				}
			}
			catch
			{
				for (int i = 0; i < num; i++)
				{
					this.Add(certificates[i]);
				}
				throw;
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06002AA5 RID: 10917 RVA: 0x000C2448 File Offset: 0x000C0648
		public X509Certificate2Collection Certificates
		{
			get
			{
				if (this.m_safeCertStoreHandle.IsInvalid || this.m_safeCertStoreHandle.IsClosed)
				{
					return new X509Certificate2Collection();
				}
				return X509Utils.GetCertificates(this.m_safeCertStoreHandle);
			}
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x000C2478 File Offset: 0x000C0678
		private static void RemoveCertificateFromStore(SafeCertStoreHandle safeCertStoreHandle, SafeCertContextHandle safeCertContext)
		{
			if (safeCertContext == null || safeCertContext.IsInvalid)
			{
				return;
			}
			if (safeCertStoreHandle == null || safeCertStoreHandle.IsInvalid || safeCertStoreHandle.IsClosed)
			{
				throw new CryptographicException(SR.GetString("Cryptography_X509_StoreNotOpen"));
			}
			SafeCertContextHandle safeCertContextHandle = CAPI.CertFindCertificateInStore(safeCertStoreHandle, 65537U, 0U, 851968U, safeCertContext.DangerousGetHandle(), SafeCertContextHandle.InvalidHandle);
			if (safeCertContextHandle == null || safeCertContextHandle.IsInvalid)
			{
				return;
			}
			GC.SuppressFinalize(safeCertContextHandle);
			if (!CAPI.CertDeleteCertificateFromStore(safeCertContextHandle))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x0400264D RID: 9805
		private string m_storeName;

		// Token: 0x0400264E RID: 9806
		private StoreLocation m_location;

		// Token: 0x0400264F RID: 9807
		private SafeCertStoreHandle m_safeCertStoreHandle;
	}
}
