using System;
using System.Collections;

namespace System.Security.Cryptography
{
	// Token: 0x02000460 RID: 1120
	public sealed class OidEnumerator : IEnumerator
	{
		// Token: 0x06002996 RID: 10646 RVA: 0x000BC926 File Offset: 0x000BAB26
		private OidEnumerator()
		{
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000BC92E File Offset: 0x000BAB2E
		internal OidEnumerator(OidCollection oids)
		{
			this.m_oids = oids;
			this.m_current = -1;
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06002998 RID: 10648 RVA: 0x000BC944 File Offset: 0x000BAB44
		public Oid Current
		{
			get
			{
				return this.m_oids[this.m_current];
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06002999 RID: 10649 RVA: 0x000BC957 File Offset: 0x000BAB57
		object IEnumerator.Current
		{
			get
			{
				return this.m_oids[this.m_current];
			}
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000BC96A File Offset: 0x000BAB6A
		public bool MoveNext()
		{
			if (this.m_current == this.m_oids.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x000BC992 File Offset: 0x000BAB92
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04002593 RID: 9619
		private OidCollection m_oids;

		// Token: 0x04002594 RID: 9620
		private int m_current;
	}
}
