using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000462 RID: 1122
	public sealed class X500DistinguishedName : AsnEncodedData
	{
		// Token: 0x0600299C RID: 10652 RVA: 0x000BC99B File Offset: 0x000BAB9B
		internal X500DistinguishedName(CAPIBase.CRYPTOAPI_BLOB encodedDistinguishedNameBlob)
			: base(new Oid(), encodedDistinguishedNameBlob)
		{
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x000BC9A9 File Offset: 0x000BABA9
		public X500DistinguishedName(byte[] encodedDistinguishedName)
			: base(new Oid(), encodedDistinguishedName)
		{
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000BC9B7 File Offset: 0x000BABB7
		public X500DistinguishedName(AsnEncodedData encodedDistinguishedName)
			: base(encodedDistinguishedName)
		{
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x000BC9C0 File Offset: 0x000BABC0
		public X500DistinguishedName(X500DistinguishedName distinguishedName)
			: base(distinguishedName)
		{
			this.m_distinguishedName = distinguishedName.Name;
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x000BC9D5 File Offset: 0x000BABD5
		public X500DistinguishedName(string distinguishedName)
			: this(distinguishedName, X500DistinguishedNameFlags.Reversed)
		{
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000BC9DF File Offset: 0x000BABDF
		public X500DistinguishedName(string distinguishedName, X500DistinguishedNameFlags flag)
			: base(new Oid(), X500DistinguishedName.Encode(distinguishedName, flag))
		{
			this.m_distinguishedName = distinguishedName;
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060029A2 RID: 10658 RVA: 0x000BC9FA File Offset: 0x000BABFA
		public string Name
		{
			get
			{
				if (this.m_distinguishedName == null)
				{
					this.m_distinguishedName = this.Decode(X500DistinguishedNameFlags.Reversed);
				}
				return this.m_distinguishedName;
			}
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x000BCA18 File Offset: 0x000BAC18
		public unsafe string Decode(X500DistinguishedNameFlags flag)
		{
			uint num = 3U | X500DistinguishedName.MapNameToStrFlag(flag);
			byte[] rawData = this.m_rawData;
			byte[] array;
			byte* ptr;
			if ((array = rawData) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB;
			IntPtr intPtr = new IntPtr((void*)(&cryptoapi_BLOB));
			cryptoapi_BLOB.cbData = (uint)rawData.Length;
			cryptoapi_BLOB.pbData = new IntPtr((void*)ptr);
			uint num2 = CAPISafe.CertNameToStrW(65537U, intPtr, num, SafeLocalAllocHandle.InvalidHandle, 0U);
			if (num2 == 0U)
			{
				throw new CryptographicException(-2146762476);
			}
			string text;
			using (SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)(2U * num2)))))
			{
				if (CAPISafe.CertNameToStrW(65537U, intPtr, num, safeLocalAllocHandle, num2) == 0U)
				{
					throw new CryptographicException(-2146762476);
				}
				text = Marshal.PtrToStringUni(safeLocalAllocHandle.DangerousGetHandle());
			}
			return text;
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x000BCAF4 File Offset: 0x000BACF4
		public override string Format(bool multiLine)
		{
			if (this.m_rawData == null || this.m_rawData.Length == 0)
			{
				return string.Empty;
			}
			return CAPI.CryptFormatObject(1U, multiLine ? 1U : 0U, new IntPtr(7L), this.m_rawData);
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x000BCB28 File Offset: 0x000BAD28
		private unsafe static byte[] Encode(string distinguishedName, X500DistinguishedNameFlags flag)
		{
			if (distinguishedName == null)
			{
				throw new ArgumentNullException("distinguishedName");
			}
			uint num = 0U;
			uint num2 = 3U | X500DistinguishedName.MapNameToStrFlag(flag);
			if (!CAPISafe.CertStrToNameW(65537U, distinguishedName, num2, IntPtr.Zero, IntPtr.Zero, ref num, IntPtr.Zero))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			byte[] array = new byte[num];
			byte[] array2;
			byte* ptr;
			if ((array2 = array) == null || array2.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array2[0];
			}
			if (!CAPISafe.CertStrToNameW(65537U, distinguishedName, num2, IntPtr.Zero, new IntPtr((void*)ptr), ref num, IntPtr.Zero))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			array2 = null;
			return array;
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x000BCBCC File Offset: 0x000BADCC
		private static uint MapNameToStrFlag(X500DistinguishedNameFlags flag)
		{
			uint num = 29169U;
			if ((flag & (X500DistinguishedNameFlags)(~(X500DistinguishedNameFlags)num)) != X500DistinguishedNameFlags.None)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { "flag" }));
			}
			uint num2 = 0U;
			if (flag != X500DistinguishedNameFlags.None)
			{
				if ((flag & X500DistinguishedNameFlags.Reversed) == X500DistinguishedNameFlags.Reversed)
				{
					num2 |= 33554432U;
				}
				if ((flag & X500DistinguishedNameFlags.UseSemicolons) == X500DistinguishedNameFlags.UseSemicolons)
				{
					num2 |= 1073741824U;
				}
				else if ((flag & X500DistinguishedNameFlags.UseCommas) == X500DistinguishedNameFlags.UseCommas)
				{
					num2 |= 67108864U;
				}
				else if ((flag & X500DistinguishedNameFlags.UseNewLines) == X500DistinguishedNameFlags.UseNewLines)
				{
					num2 |= 134217728U;
				}
				if ((flag & X500DistinguishedNameFlags.DoNotUsePlusSign) == X500DistinguishedNameFlags.DoNotUsePlusSign)
				{
					num2 |= 536870912U;
				}
				if ((flag & X500DistinguishedNameFlags.DoNotUseQuotes) == X500DistinguishedNameFlags.DoNotUseQuotes)
				{
					num2 |= 268435456U;
				}
				if ((flag & X500DistinguishedNameFlags.ForceUTF8Encoding) == X500DistinguishedNameFlags.ForceUTF8Encoding)
				{
					num2 |= 524288U;
				}
				if ((flag & X500DistinguishedNameFlags.UseUTF8Encoding) == X500DistinguishedNameFlags.UseUTF8Encoding)
				{
					num2 |= 262144U;
				}
				else if ((flag & X500DistinguishedNameFlags.UseT61Encoding) == X500DistinguishedNameFlags.UseT61Encoding)
				{
					num2 |= 131072U;
				}
			}
			return num2;
		}

		// Token: 0x040025A0 RID: 9632
		private string m_distinguishedName;
	}
}
