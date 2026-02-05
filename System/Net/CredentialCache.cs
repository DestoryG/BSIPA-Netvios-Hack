using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020000DB RID: 219
	[global::__DynamicallyInvokable]
	public class CredentialCache : ICredentials, ICredentialsByHost, IEnumerable
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00029803 File Offset: 0x00027A03
		internal bool IsDefaultInCache
		{
			get
			{
				return this.m_NumbDefaultCredInCache != 0;
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0002980E File Offset: 0x00027A0E
		[global::__DynamicallyInvokable]
		public CredentialCache()
		{
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002982C File Offset: 0x00027A2C
		[global::__DynamicallyInvokable]
		public void Add(Uri uriPrefix, string authType, NetworkCredential cred)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			if (authType == null)
			{
				throw new ArgumentNullException("authType");
			}
			if (cred is SystemNetworkCredential && string.Compare(authType, "NTLM", StringComparison.OrdinalIgnoreCase) != 0 && (!DigestClient.WDigestAvailable || string.Compare(authType, "Digest", StringComparison.OrdinalIgnoreCase) != 0) && string.Compare(authType, "Kerberos", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(authType, "Negotiate", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(SR.GetString("net_nodefaultcreds", new object[] { authType }), "authType");
			}
			this.m_version++;
			CredentialKey credentialKey = new CredentialKey(uriPrefix, authType);
			this.cache.Add(credentialKey, cred);
			if (cred is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache++;
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x000298FC File Offset: 0x00027AFC
		[global::__DynamicallyInvokable]
		public void Add(string host, int port, string authenticationType, NetworkCredential credential)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (authenticationType == null)
			{
				throw new ArgumentNullException("authenticationType");
			}
			if (host.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "host" }));
			}
			if (port < 0)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (credential is SystemNetworkCredential && string.Compare(authenticationType, "NTLM", StringComparison.OrdinalIgnoreCase) != 0 && (!DigestClient.WDigestAvailable || string.Compare(authenticationType, "Digest", StringComparison.OrdinalIgnoreCase) != 0) && string.Compare(authenticationType, "Kerberos", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(authenticationType, "Negotiate", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(SR.GetString("net_nodefaultcreds", new object[] { authenticationType }), "authenticationType");
			}
			this.m_version++;
			CredentialHostKey credentialHostKey = new CredentialHostKey(host, port, authenticationType);
			this.cacheForHosts.Add(credentialHostKey, credential);
			if (credential is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache++;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00029A00 File Offset: 0x00027C00
		[global::__DynamicallyInvokable]
		public void Remove(Uri uriPrefix, string authType)
		{
			if (uriPrefix == null || authType == null)
			{
				return;
			}
			this.m_version++;
			CredentialKey credentialKey = new CredentialKey(uriPrefix, authType);
			if (this.cache[credentialKey] is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache--;
			}
			this.cache.Remove(credentialKey);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00029A60 File Offset: 0x00027C60
		[global::__DynamicallyInvokable]
		public void Remove(string host, int port, string authenticationType)
		{
			if (host == null || authenticationType == null)
			{
				return;
			}
			if (port < 0)
			{
				return;
			}
			this.m_version++;
			CredentialHostKey credentialHostKey = new CredentialHostKey(host, port, authenticationType);
			if (this.cacheForHosts[credentialHostKey] is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache--;
			}
			this.cacheForHosts.Remove(credentialHostKey);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00029AC0 File Offset: 0x00027CC0
		[global::__DynamicallyInvokable]
		public NetworkCredential GetCredential(Uri uriPrefix, string authType)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			if (authType == null)
			{
				throw new ArgumentNullException("authType");
			}
			int num = -1;
			NetworkCredential networkCredential = null;
			IDictionaryEnumerator enumerator = this.cache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				CredentialKey credentialKey = (CredentialKey)enumerator.Key;
				if (credentialKey.Match(uriPrefix, authType))
				{
					int uriPrefixLength = credentialKey.UriPrefixLength;
					if (uriPrefixLength > num)
					{
						num = uriPrefixLength;
						networkCredential = (NetworkCredential)enumerator.Value;
					}
				}
			}
			return networkCredential;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00029B3C File Offset: 0x00027D3C
		[global::__DynamicallyInvokable]
		public NetworkCredential GetCredential(string host, int port, string authenticationType)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (authenticationType == null)
			{
				throw new ArgumentNullException("authenticationType");
			}
			if (host.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "host" }));
			}
			if (port < 0)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			NetworkCredential networkCredential = null;
			IDictionaryEnumerator enumerator = this.cacheForHosts.GetEnumerator();
			while (enumerator.MoveNext())
			{
				CredentialHostKey credentialHostKey = (CredentialHostKey)enumerator.Key;
				if (credentialHostKey.Match(host, port, authenticationType))
				{
					networkCredential = (NetworkCredential)enumerator.Value;
				}
			}
			return networkCredential;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00029BD6 File Offset: 0x00027DD6
		[global::__DynamicallyInvokable]
		public IEnumerator GetEnumerator()
		{
			return new CredentialCache.CredentialEnumerator(this, this.cache, this.cacheForHosts, this.m_version);
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x00029BF0 File Offset: 0x00027DF0
		[global::__DynamicallyInvokable]
		public static ICredentials DefaultCredentials
		{
			[global::__DynamicallyInvokable]
			get
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME").Demand();
				return SystemNetworkCredential.defaultCredential;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x00029C07 File Offset: 0x00027E07
		[global::__DynamicallyInvokable]
		public static NetworkCredential DefaultNetworkCredentials
		{
			[global::__DynamicallyInvokable]
			get
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME").Demand();
				return SystemNetworkCredential.defaultCredential;
			}
		}

		// Token: 0x04000D1A RID: 3354
		private Hashtable cache = new Hashtable();

		// Token: 0x04000D1B RID: 3355
		private Hashtable cacheForHosts = new Hashtable();

		// Token: 0x04000D1C RID: 3356
		internal int m_version;

		// Token: 0x04000D1D RID: 3357
		private int m_NumbDefaultCredInCache;

		// Token: 0x020006F6 RID: 1782
		private class CredentialEnumerator : IEnumerator
		{
			// Token: 0x06004076 RID: 16502 RVA: 0x0010E600 File Offset: 0x0010C800
			internal CredentialEnumerator(CredentialCache cache, Hashtable table, Hashtable hostTable, int version)
			{
				this.m_cache = cache;
				this.m_array = new ICredentials[table.Count + hostTable.Count];
				table.Values.CopyTo(this.m_array, 0);
				hostTable.Values.CopyTo(this.m_array, table.Count);
				this.m_version = version;
			}

			// Token: 0x17000EE6 RID: 3814
			// (get) Token: 0x06004077 RID: 16503 RVA: 0x0010E66C File Offset: 0x0010C86C
			object IEnumerator.Current
			{
				get
				{
					if (this.m_index < 0 || this.m_index >= this.m_array.Length)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					if (this.m_version != this.m_cache.m_version)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
					}
					return this.m_array[this.m_index];
				}
			}

			// Token: 0x06004078 RID: 16504 RVA: 0x0010E6D4 File Offset: 0x0010C8D4
			bool IEnumerator.MoveNext()
			{
				if (this.m_version != this.m_cache.m_version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				int num = this.m_index + 1;
				this.m_index = num;
				if (num < this.m_array.Length)
				{
					return true;
				}
				this.m_index = this.m_array.Length;
				return false;
			}

			// Token: 0x06004079 RID: 16505 RVA: 0x0010E730 File Offset: 0x0010C930
			void IEnumerator.Reset()
			{
				this.m_index = -1;
			}

			// Token: 0x04003096 RID: 12438
			private CredentialCache m_cache;

			// Token: 0x04003097 RID: 12439
			private ICredentials[] m_array;

			// Token: 0x04003098 RID: 12440
			private int m_index = -1;

			// Token: 0x04003099 RID: 12441
			private int m_version;
		}
	}
}
