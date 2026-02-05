using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200046A RID: 1130
	public sealed class X509Certificate2Enumerator : IEnumerator
	{
		// Token: 0x06002A13 RID: 10771 RVA: 0x000C023A File Offset: 0x000BE43A
		private X509Certificate2Enumerator()
		{
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000C0242 File Offset: 0x000BE442
		internal X509Certificate2Enumerator(X509Certificate2Collection mappings)
		{
			this.baseEnumerator = ((IEnumerable)mappings).GetEnumerator();
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06002A15 RID: 10773 RVA: 0x000C0256 File Offset: 0x000BE456
		public X509Certificate2 Current
		{
			get
			{
				return (X509Certificate2)this.baseEnumerator.Current;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06002A16 RID: 10774 RVA: 0x000C0268 File Offset: 0x000BE468
		object IEnumerator.Current
		{
			get
			{
				return this.baseEnumerator.Current;
			}
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000C0275 File Offset: 0x000BE475
		public bool MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000C0282 File Offset: 0x000BE482
		bool IEnumerator.MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000C028F File Offset: 0x000BE48F
		public void Reset()
		{
			this.baseEnumerator.Reset();
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000C029C File Offset: 0x000BE49C
		void IEnumerator.Reset()
		{
			this.baseEnumerator.Reset();
		}

		// Token: 0x040025D3 RID: 9683
		private IEnumerator baseEnumerator;
	}
}
