using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200047C RID: 1148
	public sealed class X509ExtensionCollection : ICollection, IEnumerable
	{
		// Token: 0x06002A82 RID: 10882 RVA: 0x000C1C54 File Offset: 0x000BFE54
		public X509ExtensionCollection()
		{
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000C1C68 File Offset: 0x000BFE68
		internal unsafe X509ExtensionCollection(SafeCertContextHandle safeCertContextHandle)
		{
			using (SafeCertContextHandle safeCertContextHandle2 = CAPI.CertDuplicateCertificateContext(safeCertContextHandle))
			{
				CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)safeCertContextHandle2.DangerousGetHandle();
				CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
				uint cExtension = cert_INFO.cExtension;
				IntPtr rgExtension = cert_INFO.rgExtension;
				for (uint num = 0U; num < cExtension; num += 1U)
				{
					X509Extension x509Extension = new X509Extension(new IntPtr((long)rgExtension + (long)((ulong)num * (ulong)((long)Marshal.SizeOf(typeof(CAPIBase.CERT_EXTENSION))))));
					X509Extension x509Extension2 = CryptoConfig.CreateFromName(x509Extension.Oid.Value) as X509Extension;
					if (x509Extension2 != null)
					{
						x509Extension2.CopyFrom(x509Extension);
						x509Extension = x509Extension2;
					}
					this.Add(x509Extension);
				}
			}
		}

		// Token: 0x17000A52 RID: 2642
		public X509Extension this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumNotStarted"));
				}
				if (index >= this.m_list.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
				}
				return (X509Extension)this.m_list[index];
			}
		}

		// Token: 0x17000A53 RID: 2643
		public X509Extension this[string oid]
		{
			get
			{
				string text = X509Utils.FindOidInfoWithFallback(2U, oid, OidGroup.ExtensionOrAttribute);
				if (text == null)
				{
					text = oid;
				}
				foreach (object obj in this.m_list)
				{
					X509Extension x509Extension = (X509Extension)obj;
					if (string.Compare(x509Extension.Oid.Value, text, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return x509Extension;
					}
				}
				return null;
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06002A86 RID: 10886 RVA: 0x000C1E28 File Offset: 0x000C0028
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000C1E35 File Offset: 0x000C0035
		public int Add(X509Extension extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			return this.m_list.Add(extension);
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000C1E51 File Offset: 0x000C0051
		public X509ExtensionEnumerator GetEnumerator()
		{
			return new X509ExtensionEnumerator(this);
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000C1E59 File Offset: 0x000C0059
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new X509ExtensionEnumerator(this);
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000C1E64 File Offset: 0x000C0064
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000C1EFE File Offset: 0x000C00FE
		public void CopyTo(X509Extension[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06002A8C RID: 10892 RVA: 0x000C1F08 File Offset: 0x000C0108
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06002A8D RID: 10893 RVA: 0x000C1F0B File Offset: 0x000C010B
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04002638 RID: 9784
		private ArrayList m_list = new ArrayList();
	}
}
