using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000479 RID: 1145
	public sealed class X509EnhancedKeyUsageExtension : X509Extension
	{
		// Token: 0x06002A6E RID: 10862 RVA: 0x000C15B9 File Offset: 0x000BF7B9
		public X509EnhancedKeyUsageExtension()
			: base("2.5.29.37")
		{
			this.m_enhancedKeyUsages = new OidCollection();
			this.m_decoded = true;
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x000C15D8 File Offset: 0x000BF7D8
		public X509EnhancedKeyUsageExtension(OidCollection enhancedKeyUsages, bool critical)
			: base("2.5.29.37", X509EnhancedKeyUsageExtension.EncodeExtension(enhancedKeyUsages), critical)
		{
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x000C15EC File Offset: 0x000BF7EC
		public X509EnhancedKeyUsageExtension(AsnEncodedData encodedEnhancedKeyUsages, bool critical)
			: base("2.5.29.37", encodedEnhancedKeyUsages.RawData, critical)
		{
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06002A71 RID: 10865 RVA: 0x000C1600 File Offset: 0x000BF800
		public OidCollection EnhancedKeyUsages
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				OidCollection oidCollection = new OidCollection();
				foreach (Oid oid in this.m_enhancedKeyUsages)
				{
					oidCollection.Add(oid);
				}
				return oidCollection;
			}
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x000C1647 File Offset: 0x000BF847
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000C1658 File Offset: 0x000BF858
		private void DecodeExtension()
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (!CAPI.DecodeObject(new IntPtr(36L), this.m_rawData, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPIBase.CERT_ENHKEY_USAGE cert_ENHKEY_USAGE = (CAPIBase.CERT_ENHKEY_USAGE)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_ENHKEY_USAGE));
			this.m_enhancedKeyUsages = new OidCollection();
			int num2 = 0;
			while ((long)num2 < (long)((ulong)cert_ENHKEY_USAGE.cUsageIdentifier))
			{
				IntPtr intPtr = Marshal.ReadIntPtr(new IntPtr((long)cert_ENHKEY_USAGE.rgpszUsageIdentifier + (long)(num2 * Marshal.SizeOf(typeof(IntPtr)))));
				string text = Marshal.PtrToStringAnsi(intPtr);
				Oid oid = new Oid(text, OidGroup.ExtensionOrAttribute, false);
				this.m_enhancedKeyUsages.Add(oid);
				num2++;
			}
			this.m_decoded = true;
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x000C1728 File Offset: 0x000BF928
		private unsafe static byte[] EncodeExtension(OidCollection enhancedKeyUsages)
		{
			if (enhancedKeyUsages == null)
			{
				throw new ArgumentNullException("enhancedKeyUsages");
			}
			SafeLocalAllocHandle safeLocalAllocHandle = X509Utils.CopyOidsToUnmanagedMemory(enhancedKeyUsages);
			byte[] array = null;
			using (safeLocalAllocHandle)
			{
				CAPIBase.CERT_ENHKEY_USAGE cert_ENHKEY_USAGE = default(CAPIBase.CERT_ENHKEY_USAGE);
				cert_ENHKEY_USAGE.cUsageIdentifier = (uint)enhancedKeyUsages.Count;
				cert_ENHKEY_USAGE.rgpszUsageIdentifier = safeLocalAllocHandle.DangerousGetHandle();
				if (!CAPI.EncodeObject("2.5.29.37", new IntPtr((void*)(&cert_ENHKEY_USAGE)), out array))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			return array;
		}

		// Token: 0x04002630 RID: 9776
		private OidCollection m_enhancedKeyUsages;

		// Token: 0x04002631 RID: 9777
		private bool m_decoded;
	}
}
