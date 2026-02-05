using System;
using System.Collections;

namespace System.Net.Security
{
	// Token: 0x02000352 RID: 850
	internal static class SslSessionsCache
	{
		// Token: 0x06001E84 RID: 7812 RVA: 0x0008F954 File Offset: 0x0008DB54
		internal static SafeFreeCredentials TryCachedCredential(byte[] thumbPrint, SchProtocols allowedProtocols, EncryptionPolicy encryptionPolicy)
		{
			if (SslSessionsCache.s_CachedCreds.Count == 0)
			{
				return null;
			}
			object obj = new SslSessionsCache.SslCredKey(thumbPrint, allowedProtocols, encryptionPolicy);
			SafeCredentialReference safeCredentialReference = SslSessionsCache.s_CachedCreds[obj] as SafeCredentialReference;
			if (safeCredentialReference == null || safeCredentialReference.IsClosed || safeCredentialReference._Target.IsInvalid)
			{
				return null;
			}
			return safeCredentialReference._Target;
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0008F9B0 File Offset: 0x0008DBB0
		internal static void CacheCredential(SafeFreeCredentials creds, byte[] thumbPrint, SchProtocols allowedProtocols, EncryptionPolicy encryptionPolicy)
		{
			if (creds.IsInvalid)
			{
				return;
			}
			object obj = new SslSessionsCache.SslCredKey(thumbPrint, allowedProtocols, encryptionPolicy);
			SafeCredentialReference safeCredentialReference = SslSessionsCache.s_CachedCreds[obj] as SafeCredentialReference;
			if (safeCredentialReference == null || safeCredentialReference.IsClosed || safeCredentialReference._Target.IsInvalid)
			{
				Hashtable hashtable = SslSessionsCache.s_CachedCreds;
				lock (hashtable)
				{
					safeCredentialReference = SslSessionsCache.s_CachedCreds[obj] as SafeCredentialReference;
					if (safeCredentialReference == null || safeCredentialReference.IsClosed)
					{
						safeCredentialReference = SafeCredentialReference.CreateReference(creds);
						if (safeCredentialReference != null)
						{
							SslSessionsCache.s_CachedCreds[obj] = safeCredentialReference;
							if (SslSessionsCache.s_CachedCreds.Count % 32 == 0)
							{
								DictionaryEntry[] array = new DictionaryEntry[SslSessionsCache.s_CachedCreds.Count];
								SslSessionsCache.s_CachedCreds.CopyTo(array, 0);
								for (int i = 0; i < array.Length; i++)
								{
									safeCredentialReference = array[i].Value as SafeCredentialReference;
									if (safeCredentialReference != null)
									{
										creds = safeCredentialReference._Target;
										safeCredentialReference.Close();
										if (!creds.IsClosed && !creds.IsInvalid && (safeCredentialReference = SafeCredentialReference.CreateReference(creds)) != null)
										{
											SslSessionsCache.s_CachedCreds[array[i].Key] = safeCredentialReference;
										}
										else
										{
											SslSessionsCache.s_CachedCreds.Remove(array[i].Key);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04001CDE RID: 7390
		private const int c_CheckExpiredModulo = 32;

		// Token: 0x04001CDF RID: 7391
		private static Hashtable s_CachedCreds = new Hashtable(32);

		// Token: 0x020007CD RID: 1997
		private struct SslCredKey
		{
			// Token: 0x0600439B RID: 17307 RVA: 0x0011CF58 File Offset: 0x0011B158
			internal SslCredKey(byte[] thumbPrint, SchProtocols allowedProtocols, EncryptionPolicy encryptionPolicy)
			{
				this._CertThumbPrint = ((thumbPrint == null) ? SslSessionsCache.SslCredKey.s_EmptyArray : thumbPrint);
				this._HashCode = 0;
				if (thumbPrint != null)
				{
					this._HashCode ^= (int)this._CertThumbPrint[0];
					if (1 < this._CertThumbPrint.Length)
					{
						this._HashCode ^= (int)this._CertThumbPrint[1] << 8;
					}
					if (2 < this._CertThumbPrint.Length)
					{
						this._HashCode ^= (int)this._CertThumbPrint[2] << 16;
					}
					if (3 < this._CertThumbPrint.Length)
					{
						this._HashCode ^= (int)this._CertThumbPrint[3] << 24;
					}
				}
				this._HashCode ^= (int)allowedProtocols;
				this._HashCode ^= (int)encryptionPolicy;
				this._AllowedProtocols = allowedProtocols;
				this._EncryptionPolicy = encryptionPolicy;
			}

			// Token: 0x0600439C RID: 17308 RVA: 0x0011D027 File Offset: 0x0011B227
			public override int GetHashCode()
			{
				return this._HashCode;
			}

			// Token: 0x0600439D RID: 17309 RVA: 0x0011D02F File Offset: 0x0011B22F
			public static bool operator ==(SslSessionsCache.SslCredKey sslCredKey1, SslSessionsCache.SslCredKey sslCredKey2)
			{
				return sslCredKey1 == sslCredKey2 || (sslCredKey1 != null && sslCredKey2 != null && sslCredKey1.Equals(sslCredKey2));
			}

			// Token: 0x0600439E RID: 17310 RVA: 0x0011D066 File Offset: 0x0011B266
			public static bool operator !=(SslSessionsCache.SslCredKey sslCredKey1, SslSessionsCache.SslCredKey sslCredKey2)
			{
				return sslCredKey1 != sslCredKey2 && (sslCredKey1 == null || sslCredKey2 == null || !sslCredKey1.Equals(sslCredKey2));
			}

			// Token: 0x0600439F RID: 17311 RVA: 0x0011D0A0 File Offset: 0x0011B2A0
			public override bool Equals(object y)
			{
				SslSessionsCache.SslCredKey sslCredKey = (SslSessionsCache.SslCredKey)y;
				if (this._CertThumbPrint.Length != sslCredKey._CertThumbPrint.Length)
				{
					return false;
				}
				if (this._HashCode != sslCredKey._HashCode)
				{
					return false;
				}
				if (this._EncryptionPolicy != sslCredKey._EncryptionPolicy)
				{
					return false;
				}
				if (this._AllowedProtocols != sslCredKey._AllowedProtocols)
				{
					return false;
				}
				for (int i = 0; i < this._CertThumbPrint.Length; i++)
				{
					if (this._CertThumbPrint[i] != sslCredKey._CertThumbPrint[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x04003475 RID: 13429
			private static readonly byte[] s_EmptyArray = new byte[0];

			// Token: 0x04003476 RID: 13430
			private byte[] _CertThumbPrint;

			// Token: 0x04003477 RID: 13431
			private SchProtocols _AllowedProtocols;

			// Token: 0x04003478 RID: 13432
			private EncryptionPolicy _EncryptionPolicy;

			// Token: 0x04003479 RID: 13433
			private int _HashCode;
		}
	}
}
