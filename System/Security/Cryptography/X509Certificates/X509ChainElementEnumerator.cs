using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000470 RID: 1136
	public sealed class X509ChainElementEnumerator : IEnumerator
	{
		// Token: 0x06002A40 RID: 10816 RVA: 0x000C0E9A File Offset: 0x000BF09A
		private X509ChainElementEnumerator()
		{
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000C0EA2 File Offset: 0x000BF0A2
		internal X509ChainElementEnumerator(X509ChainElementCollection chainElements)
		{
			this.m_chainElements = chainElements;
			this.m_current = -1;
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06002A42 RID: 10818 RVA: 0x000C0EB8 File Offset: 0x000BF0B8
		public X509ChainElement Current
		{
			get
			{
				return this.m_chainElements[this.m_current];
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06002A43 RID: 10819 RVA: 0x000C0ECB File Offset: 0x000BF0CB
		object IEnumerator.Current
		{
			get
			{
				return this.m_chainElements[this.m_current];
			}
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000C0EDE File Offset: 0x000BF0DE
		public bool MoveNext()
		{
			if (this.m_current == this.m_chainElements.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000C0F06 File Offset: 0x000BF106
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x040025FD RID: 9725
		private X509ChainElementCollection m_chainElements;

		// Token: 0x040025FE RID: 9726
		private int m_current;
	}
}
