using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000477 RID: 1143
	public sealed class X509KeyUsageExtension : X509Extension
	{
		// Token: 0x06002A5E RID: 10846 RVA: 0x000C1208 File Offset: 0x000BF408
		public X509KeyUsageExtension()
			: base("2.5.29.15")
		{
			this.m_decoded = true;
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000C121C File Offset: 0x000BF41C
		public X509KeyUsageExtension(X509KeyUsageFlags keyUsages, bool critical)
			: base("2.5.29.15", X509KeyUsageExtension.EncodeExtension(keyUsages), critical)
		{
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000C1230 File Offset: 0x000BF430
		public X509KeyUsageExtension(AsnEncodedData encodedKeyUsage, bool critical)
			: base("2.5.29.15", encodedKeyUsage.RawData, critical)
		{
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06002A61 RID: 10849 RVA: 0x000C1244 File Offset: 0x000BF444
		public X509KeyUsageFlags KeyUsages
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				return (X509KeyUsageFlags)this.m_keyUsages;
			}
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000C125A File Offset: 0x000BF45A
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000C126C File Offset: 0x000BF46C
		private void DecodeExtension()
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (!CAPI.DecodeObject(new IntPtr(14L), this.m_rawData, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = (CAPIBase.CRYPTOAPI_BLOB)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPTOAPI_BLOB));
			if (cryptoapi_BLOB.cbData > 4U)
			{
				cryptoapi_BLOB.cbData = 4U;
			}
			byte[] array = new byte[4];
			if (cryptoapi_BLOB.pbData != IntPtr.Zero)
			{
				Marshal.Copy(cryptoapi_BLOB.pbData, array, 0, (int)cryptoapi_BLOB.cbData);
			}
			this.m_keyUsages = BitConverter.ToUInt32(array, 0);
			this.m_decoded = true;
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000C1318 File Offset: 0x000BF518
		private unsafe static byte[] EncodeExtension(X509KeyUsageFlags keyUsages)
		{
			CAPIBase.CRYPT_BIT_BLOB crypt_BIT_BLOB = default(CAPIBase.CRYPT_BIT_BLOB);
			crypt_BIT_BLOB.cbData = 2U;
			crypt_BIT_BLOB.pbData = new IntPtr((void*)(&keyUsages));
			crypt_BIT_BLOB.cUnusedBits = 0U;
			byte[] array = null;
			if (!CAPI.EncodeObject("2.5.29.15", new IntPtr((void*)(&crypt_BIT_BLOB)), out array))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return array;
		}

		// Token: 0x0400262A RID: 9770
		private uint m_keyUsages;

		// Token: 0x0400262B RID: 9771
		private bool m_decoded;
	}
}
