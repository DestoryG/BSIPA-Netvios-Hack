using System;

namespace System.Security.Cryptography
{
	// Token: 0x0200044D RID: 1101
	public class AsnEncodedData
	{
		// Token: 0x060028B7 RID: 10423 RVA: 0x000BA92A File Offset: 0x000B8B2A
		internal AsnEncodedData(Oid oid)
		{
			this.m_oid = oid;
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000BA939 File Offset: 0x000B8B39
		internal AsnEncodedData(string oid, CAPIBase.CRYPTOAPI_BLOB encodedBlob)
			: this(oid, CAPI.BlobToByteArray(encodedBlob))
		{
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x000BA948 File Offset: 0x000B8B48
		internal AsnEncodedData(Oid oid, CAPIBase.CRYPTOAPI_BLOB encodedBlob)
			: this(oid, CAPI.BlobToByteArray(encodedBlob))
		{
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x000BA957 File Offset: 0x000B8B57
		protected AsnEncodedData()
		{
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x000BA95F File Offset: 0x000B8B5F
		public AsnEncodedData(byte[] rawData)
		{
			this.Reset(null, rawData);
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x000BA96F File Offset: 0x000B8B6F
		public AsnEncodedData(string oid, byte[] rawData)
		{
			this.Reset(new Oid(oid), rawData);
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x000BA984 File Offset: 0x000B8B84
		public AsnEncodedData(Oid oid, byte[] rawData)
		{
			this.Reset(oid, rawData);
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x000BA994 File Offset: 0x000B8B94
		public AsnEncodedData(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			this.Reset(asnEncodedData.m_oid, asnEncodedData.m_rawData);
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x000BA9BC File Offset: 0x000B8BBC
		// (set) Token: 0x060028C0 RID: 10432 RVA: 0x000BA9C4 File Offset: 0x000B8BC4
		public Oid Oid
		{
			get
			{
				return this.m_oid;
			}
			set
			{
				if (value == null)
				{
					this.m_oid = null;
					return;
				}
				this.m_oid = new Oid(value);
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060028C1 RID: 10433 RVA: 0x000BA9DD File Offset: 0x000B8BDD
		// (set) Token: 0x060028C2 RID: 10434 RVA: 0x000BA9E5 File Offset: 0x000B8BE5
		public byte[] RawData
		{
			get
			{
				return this.m_rawData;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_rawData = (byte[])value.Clone();
			}
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x000BAA06 File Offset: 0x000B8C06
		public virtual void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			this.Reset(asnEncodedData.m_oid, asnEncodedData.m_rawData);
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000BAA28 File Offset: 0x000B8C28
		public virtual string Format(bool multiLine)
		{
			if (this.m_rawData == null || this.m_rawData.Length == 0)
			{
				return string.Empty;
			}
			string text = string.Empty;
			if (this.m_oid != null && this.m_oid.Value != null)
			{
				text = this.m_oid.Value;
			}
			return CAPI.CryptFormatObject(1U, multiLine ? 1U : 0U, text, this.m_rawData);
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x000BAA87 File Offset: 0x000B8C87
		private void Reset(Oid oid, byte[] rawData)
		{
			this.Oid = oid;
			this.RawData = rawData;
		}

		// Token: 0x04002273 RID: 8819
		internal Oid m_oid;

		// Token: 0x04002274 RID: 8820
		internal byte[] m_rawData;
	}
}
