using System;
using System.Globalization;

namespace System.Net
{
	// Token: 0x020000DE RID: 222
	internal class CredentialKey
	{
		// Token: 0x0600078A RID: 1930 RVA: 0x00029DC1 File Offset: 0x00027FC1
		internal CredentialKey(Uri uriPrefix, string authenticationType)
		{
			this.UriPrefix = uriPrefix;
			this.UriPrefixLength = this.UriPrefix.ToString().Length;
			this.AuthenticationType = authenticationType;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00029DF4 File Offset: 0x00027FF4
		internal bool Match(Uri uri, string authenticationType)
		{
			return !(uri == null) && authenticationType != null && string.Compare(authenticationType, this.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && this.IsPrefix(uri, this.UriPrefix);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00029E24 File Offset: 0x00028024
		internal bool IsPrefix(Uri uri, Uri prefixUri)
		{
			if (prefixUri.Scheme != uri.Scheme || prefixUri.Host != uri.Host || prefixUri.Port != uri.Port)
			{
				return false;
			}
			int num = prefixUri.AbsolutePath.LastIndexOf('/');
			return num <= uri.AbsolutePath.LastIndexOf('/') && string.Compare(uri.AbsolutePath, 0, prefixUri.AbsolutePath, 0, num, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00029E9F File Offset: 0x0002809F
		public override int GetHashCode()
		{
			if (!this.m_ComputedHashCode)
			{
				this.m_HashCode = this.AuthenticationType.ToUpperInvariant().GetHashCode() + this.UriPrefixLength + this.UriPrefix.GetHashCode();
				this.m_ComputedHashCode = true;
			}
			return this.m_HashCode;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00029EE0 File Offset: 0x000280E0
		public override bool Equals(object comparand)
		{
			CredentialKey credentialKey = comparand as CredentialKey;
			return comparand != null && string.Compare(this.AuthenticationType, credentialKey.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && this.UriPrefix.Equals(credentialKey.UriPrefix);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00029F24 File Offset: 0x00028124
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.UriPrefixLength.ToString(NumberFormatInfo.InvariantInfo),
				"]:",
				ValidationHelper.ToString(this.UriPrefix),
				":",
				ValidationHelper.ToString(this.AuthenticationType)
			});
		}

		// Token: 0x04000D24 RID: 3364
		internal Uri UriPrefix;

		// Token: 0x04000D25 RID: 3365
		internal int UriPrefixLength = -1;

		// Token: 0x04000D26 RID: 3366
		internal string AuthenticationType;

		// Token: 0x04000D27 RID: 3367
		private int m_HashCode;

		// Token: 0x04000D28 RID: 3368
		private bool m_ComputedHashCode;
	}
}
