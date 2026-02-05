using System;
using System.Collections;

namespace System.Security.Cryptography
{
	// Token: 0x0200044E RID: 1102
	public sealed class AsnEncodedDataCollection : ICollection, IEnumerable
	{
		// Token: 0x060028C6 RID: 10438 RVA: 0x000BAA97 File Offset: 0x000B8C97
		public AsnEncodedDataCollection()
		{
			this.m_list = new ArrayList();
			this.m_oid = null;
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x000BAAB1 File Offset: 0x000B8CB1
		public AsnEncodedDataCollection(AsnEncodedData asnEncodedData)
			: this()
		{
			this.m_list.Add(asnEncodedData);
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x000BAAC8 File Offset: 0x000B8CC8
		public int Add(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			if (this.m_oid != null)
			{
				string value = this.m_oid.Value;
				string value2 = asnEncodedData.Oid.Value;
				if (value != null && value2 != null)
				{
					if (string.Compare(value, value2, StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw new CryptographicException(SR.GetString("Cryptography_Asn_MismatchedOidInCollection"));
					}
				}
				else if (value != null || value2 != null)
				{
					throw new CryptographicException(SR.GetString("Cryptography_Asn_MismatchedOidInCollection"));
				}
			}
			return this.m_list.Add(asnEncodedData);
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x000BAB45 File Offset: 0x000B8D45
		public void Remove(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			this.m_list.Remove(asnEncodedData);
		}

		// Token: 0x17000A04 RID: 2564
		public AsnEncodedData this[int index]
		{
			get
			{
				return (AsnEncodedData)this.m_list[index];
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060028CB RID: 10443 RVA: 0x000BAB74 File Offset: 0x000B8D74
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x000BAB81 File Offset: 0x000B8D81
		public AsnEncodedDataEnumerator GetEnumerator()
		{
			return new AsnEncodedDataEnumerator(this);
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000BAB89 File Offset: 0x000B8D89
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new AsnEncodedDataEnumerator(this);
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x000BAB94 File Offset: 0x000B8D94
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

		// Token: 0x060028CF RID: 10447 RVA: 0x000BAC2E File Offset: 0x000B8E2E
		public void CopyTo(AsnEncodedData[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060028D0 RID: 10448 RVA: 0x000BAC38 File Offset: 0x000B8E38
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x000BAC3B File Offset: 0x000B8E3B
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04002275 RID: 8821
		private ArrayList m_list;

		// Token: 0x04002276 RID: 8822
		private Oid m_oid;
	}
}
