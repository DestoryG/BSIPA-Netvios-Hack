using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200047D RID: 1149
	public sealed class X509ExtensionEnumerator : IEnumerator
	{
		// Token: 0x06002A8E RID: 10894 RVA: 0x000C1F0E File Offset: 0x000C010E
		private X509ExtensionEnumerator()
		{
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000C1F16 File Offset: 0x000C0116
		internal X509ExtensionEnumerator(X509ExtensionCollection extensions)
		{
			this.m_extensions = extensions;
			this.m_current = -1;
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x000C1F2C File Offset: 0x000C012C
		public X509Extension Current
		{
			get
			{
				return this.m_extensions[this.m_current];
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06002A91 RID: 10897 RVA: 0x000C1F3F File Offset: 0x000C013F
		object IEnumerator.Current
		{
			get
			{
				return this.m_extensions[this.m_current];
			}
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000C1F52 File Offset: 0x000C0152
		public bool MoveNext()
		{
			if (this.m_current == this.m_extensions.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000C1F7A File Offset: 0x000C017A
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04002639 RID: 9785
		private X509ExtensionCollection m_extensions;

		// Token: 0x0400263A RID: 9786
		private int m_current;
	}
}
