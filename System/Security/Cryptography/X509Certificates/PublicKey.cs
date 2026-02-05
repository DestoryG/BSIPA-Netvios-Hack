using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000465 RID: 1125
	public sealed class PublicKey
	{
		// Token: 0x060029A7 RID: 10663 RVA: 0x000BCCCA File Offset: 0x000BAECA
		private PublicKey()
		{
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x000BCCD2 File Offset: 0x000BAED2
		public PublicKey(Oid oid, AsnEncodedData parameters, AsnEncodedData keyValue)
		{
			this.m_oid = new Oid(oid);
			this.m_encodedParameters = new AsnEncodedData(parameters);
			this.m_encodedKeyValue = new AsnEncodedData(keyValue);
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x000BCCFE File Offset: 0x000BAEFE
		internal PublicKey(PublicKey publicKey)
		{
			this.m_oid = new Oid(publicKey.m_oid);
			this.m_encodedParameters = new AsnEncodedData(publicKey.m_encodedParameters);
			this.m_encodedKeyValue = new AsnEncodedData(publicKey.m_encodedKeyValue);
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x000BCD39 File Offset: 0x000BAF39
		internal uint AlgorithmId
		{
			get
			{
				if (this.m_aiPubKey == 0U)
				{
					this.m_aiPubKey = X509Utils.OidToAlgId(this.m_oid.Value);
				}
				return this.m_aiPubKey;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x000BCD5F File Offset: 0x000BAF5F
		private byte[] CspBlobData
		{
			get
			{
				if (this.m_cspBlobData == null)
				{
					PublicKey.DecodePublicKeyObject(this.AlgorithmId, this.m_encodedKeyValue.RawData, this.m_encodedParameters.RawData, out this.m_cspBlobData);
				}
				return this.m_cspBlobData;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x000BCD98 File Offset: 0x000BAF98
		public AsymmetricAlgorithm Key
		{
			get
			{
				if (this.m_key == null)
				{
					uint algorithmId = this.AlgorithmId;
					if (algorithmId != 8704U)
					{
						if (algorithmId != 9216U && algorithmId != 41984U)
						{
							throw new NotSupportedException(SR.GetString("NotSupported_KeyAlgorithm"));
						}
						RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
						rsacryptoServiceProvider.ImportCspBlob(this.CspBlobData);
						this.m_key = rsacryptoServiceProvider;
					}
					else
					{
						DSACryptoServiceProvider dsacryptoServiceProvider = new DSACryptoServiceProvider();
						dsacryptoServiceProvider.ImportCspBlob(this.CspBlobData);
						this.m_key = dsacryptoServiceProvider;
					}
				}
				return this.m_key;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060029AD RID: 10669 RVA: 0x000BCE18 File Offset: 0x000BB018
		public Oid Oid
		{
			get
			{
				return new Oid(this.m_oid);
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x000BCE25 File Offset: 0x000BB025
		public AsnEncodedData EncodedKeyValue
		{
			get
			{
				return this.m_encodedKeyValue;
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060029AF RID: 10671 RVA: 0x000BCE2D File Offset: 0x000BB02D
		public AsnEncodedData EncodedParameters
		{
			get
			{
				return this.m_encodedParameters;
			}
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x000BCE38 File Offset: 0x000BB038
		private static void DecodePublicKeyObject(uint aiPubKey, byte[] encodedKeyValue, byte[] encodedParameters, out byte[] decodedData)
		{
			decodedData = null;
			IntPtr zero = IntPtr.Zero;
			if (aiPubKey <= 9216U)
			{
				if (aiPubKey == 8704U)
				{
					zero = new IntPtr(38L);
					goto IL_006F;
				}
				if (aiPubKey != 9216U)
				{
					goto IL_005F;
				}
			}
			else if (aiPubKey != 41984U)
			{
				if (aiPubKey - 43521U > 1U)
				{
					goto IL_005F;
				}
				throw new NotSupportedException(SR.GetString("NotSupported_KeyAlgorithm"));
			}
			zero = new IntPtr(19L);
			goto IL_006F;
			IL_005F:
			throw new NotSupportedException(SR.GetString("NotSupported_KeyAlgorithm"));
			IL_006F:
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			uint num = 0U;
			if (!CAPI.DecodeObject(zero, encodedKeyValue, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if ((int)zero == 19)
			{
				decodedData = new byte[num];
				Marshal.Copy(safeLocalAllocHandle.DangerousGetHandle(), decodedData, 0, decodedData.Length);
			}
			else if ((int)zero == 38)
			{
				SafeLocalAllocHandle safeLocalAllocHandle2 = null;
				uint num2 = 0U;
				if (!CAPI.DecodeObject(new IntPtr(39L), encodedParameters, out safeLocalAllocHandle2, out num2))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				decodedData = PublicKey.ConstructDSSPubKeyCspBlob(safeLocalAllocHandle, safeLocalAllocHandle2);
				safeLocalAllocHandle2.Dispose();
			}
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000BCF40 File Offset: 0x000BB140
		private static byte[] ConstructDSSPubKeyCspBlob(SafeLocalAllocHandle decodedKeyValue, SafeLocalAllocHandle decodedParameters)
		{
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = (CAPIBase.CRYPTOAPI_BLOB)Marshal.PtrToStructure(decodedKeyValue.DangerousGetHandle(), typeof(CAPIBase.CRYPTOAPI_BLOB));
			CAPIBase.CERT_DSS_PARAMETERS cert_DSS_PARAMETERS = (CAPIBase.CERT_DSS_PARAMETERS)Marshal.PtrToStructure(decodedParameters.DangerousGetHandle(), typeof(CAPIBase.CERT_DSS_PARAMETERS));
			uint cbData = cert_DSS_PARAMETERS.p.cbData;
			if (cbData == 0U)
			{
				throw new CryptographicException(-2146893803);
			}
			uint num = 16U + cbData + 20U + cbData + cbData + 24U;
			MemoryStream memoryStream = new MemoryStream((int)num);
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(6);
			binaryWriter.Write(2);
			binaryWriter.Write(0);
			binaryWriter.Write(8704U);
			binaryWriter.Write(827544388U);
			binaryWriter.Write(cbData * 8U);
			byte[] array = new byte[cert_DSS_PARAMETERS.p.cbData];
			Marshal.Copy(cert_DSS_PARAMETERS.p.pbData, array, 0, array.Length);
			binaryWriter.Write(array);
			uint num2 = cert_DSS_PARAMETERS.q.cbData;
			if (num2 == 0U || num2 > 20U)
			{
				throw new CryptographicException(-2146893803);
			}
			byte[] array2 = new byte[cert_DSS_PARAMETERS.q.cbData];
			Marshal.Copy(cert_DSS_PARAMETERS.q.pbData, array2, 0, array2.Length);
			binaryWriter.Write(array2);
			if (20U > num2)
			{
				binaryWriter.Write(new byte[20U - num2]);
			}
			num2 = cert_DSS_PARAMETERS.g.cbData;
			if (num2 == 0U || num2 > cbData)
			{
				throw new CryptographicException(-2146893803);
			}
			byte[] array3 = new byte[cert_DSS_PARAMETERS.g.cbData];
			Marshal.Copy(cert_DSS_PARAMETERS.g.pbData, array3, 0, array3.Length);
			binaryWriter.Write(array3);
			if (cbData > num2)
			{
				binaryWriter.Write(new byte[cbData - num2]);
			}
			num2 = cryptoapi_BLOB.cbData;
			if (num2 == 0U || num2 > cbData)
			{
				throw new CryptographicException(-2146893803);
			}
			byte[] array4 = new byte[cryptoapi_BLOB.cbData];
			Marshal.Copy(cryptoapi_BLOB.pbData, array4, 0, array4.Length);
			binaryWriter.Write(array4);
			if (cbData > num2)
			{
				binaryWriter.Write(new byte[cbData - num2]);
			}
			binaryWriter.Write(uint.MaxValue);
			binaryWriter.Write(new byte[20]);
			return memoryStream.ToArray();
		}

		// Token: 0x040025AD RID: 9645
		private AsnEncodedData m_encodedKeyValue;

		// Token: 0x040025AE RID: 9646
		private AsnEncodedData m_encodedParameters;

		// Token: 0x040025AF RID: 9647
		private Oid m_oid;

		// Token: 0x040025B0 RID: 9648
		private uint m_aiPubKey;

		// Token: 0x040025B1 RID: 9649
		private byte[] m_cspBlobData;

		// Token: 0x040025B2 RID: 9650
		private AsymmetricAlgorithm m_key;
	}
}
