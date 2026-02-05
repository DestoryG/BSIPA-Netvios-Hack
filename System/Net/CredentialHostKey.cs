using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020000DD RID: 221
	internal class CredentialHostKey
	{
		// Token: 0x06000785 RID: 1925 RVA: 0x00029C41 File Offset: 0x00027E41
		internal CredentialHostKey(string host, int port, string authenticationType)
		{
			this.Host = host;
			this.Port = port;
			this.AuthenticationType = authenticationType;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00029C5E File Offset: 0x00027E5E
		internal bool Match(string host, int port, string authenticationType)
		{
			return host != null && authenticationType != null && string.Compare(authenticationType, this.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(this.Host, host, StringComparison.OrdinalIgnoreCase) == 0 && port == this.Port;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00029C98 File Offset: 0x00027E98
		public override int GetHashCode()
		{
			if (!this.m_ComputedHashCode)
			{
				this.m_HashCode = this.AuthenticationType.ToUpperInvariant().GetHashCode() + this.Host.ToUpperInvariant().GetHashCode() + this.Port.GetHashCode();
				this.m_ComputedHashCode = true;
			}
			return this.m_HashCode;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00029CF0 File Offset: 0x00027EF0
		public override bool Equals(object comparand)
		{
			CredentialHostKey credentialHostKey = comparand as CredentialHostKey;
			return comparand != null && (string.Compare(this.AuthenticationType, credentialHostKey.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(this.Host, credentialHostKey.Host, StringComparison.OrdinalIgnoreCase) == 0) && this.Port == credentialHostKey.Port;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00029D44 File Offset: 0x00027F44
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.Host.Length.ToString(NumberFormatInfo.InvariantInfo),
				"]:",
				this.Host,
				":",
				this.Port.ToString(NumberFormatInfo.InvariantInfo),
				":",
				ValidationHelper.ToString(this.AuthenticationType)
			});
		}

		// Token: 0x04000D1F RID: 3359
		internal string Host;

		// Token: 0x04000D20 RID: 3360
		internal string AuthenticationType;

		// Token: 0x04000D21 RID: 3361
		internal int Port;

		// Token: 0x04000D22 RID: 3362
		private int m_HashCode;

		// Token: 0x04000D23 RID: 3363
		private bool m_ComputedHashCode;
	}
}
