using System;
using System.Collections;

namespace System.Security.Cryptography
{
	// Token: 0x0200044F RID: 1103
	public sealed class AsnEncodedDataEnumerator : IEnumerator
	{
		// Token: 0x060028D2 RID: 10450 RVA: 0x000BAC3E File Offset: 0x000B8E3E
		private AsnEncodedDataEnumerator()
		{
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x000BAC46 File Offset: 0x000B8E46
		internal AsnEncodedDataEnumerator(AsnEncodedDataCollection asnEncodedDatas)
		{
			this.m_asnEncodedDatas = asnEncodedDatas;
			this.m_current = -1;
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x000BAC5C File Offset: 0x000B8E5C
		public AsnEncodedData Current
		{
			get
			{
				return this.m_asnEncodedDatas[this.m_current];
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060028D5 RID: 10453 RVA: 0x000BAC6F File Offset: 0x000B8E6F
		object IEnumerator.Current
		{
			get
			{
				return this.m_asnEncodedDatas[this.m_current];
			}
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000BAC82 File Offset: 0x000B8E82
		public bool MoveNext()
		{
			if (this.m_current == this.m_asnEncodedDatas.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000BACAA File Offset: 0x000B8EAA
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04002277 RID: 8823
		private AsnEncodedDataCollection m_asnEncodedDatas;

		// Token: 0x04002278 RID: 8824
		private int m_current;
	}
}
