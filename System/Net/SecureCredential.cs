using System;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x0200012E RID: 302
	internal struct SecureCredential
	{
		// Token: 0x06000B3B RID: 2875 RVA: 0x0003DAEC File Offset: 0x0003BCEC
		public SecureCredential(int version, X509Certificate certificate, SecureCredential.Flags flags, SchProtocols protocols, EncryptionPolicy policy)
		{
			this.rootStore = (this.phMappers = (this.palgSupportedAlgs = (this.certContextArray = IntPtr.Zero)));
			this.cCreds = (this.cMappers = (this.cSupportedAlgs = 0));
			if (policy == EncryptionPolicy.RequireEncryption)
			{
				this.dwMinimumCipherStrength = 0;
				this.dwMaximumCipherStrength = 0;
			}
			else if (policy == EncryptionPolicy.AllowNoEncryption)
			{
				this.dwMinimumCipherStrength = -1;
				this.dwMaximumCipherStrength = 0;
			}
			else
			{
				if (policy != EncryptionPolicy.NoEncryption)
				{
					throw new ArgumentException(global::System.SR.GetString("net_invalid_enum", new object[] { "EncryptionPolicy" }), "policy");
				}
				this.dwMinimumCipherStrength = -1;
				this.dwMaximumCipherStrength = -1;
			}
			this.dwSessionLifespan = (this.reserved = 0);
			this.version = version;
			this.dwFlags = flags;
			this.grbitEnabledProtocols = protocols;
			if (certificate != null)
			{
				this.certContextArray = certificate.Handle;
				this.cCreds = 1;
			}
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0003DBD5 File Offset: 0x0003BDD5
		[Conditional("TRAVE")]
		internal void DebugDump()
		{
		}

		// Token: 0x04001014 RID: 4116
		public const int CurrentVersion = 4;

		// Token: 0x04001015 RID: 4117
		public int version;

		// Token: 0x04001016 RID: 4118
		public int cCreds;

		// Token: 0x04001017 RID: 4119
		public IntPtr certContextArray;

		// Token: 0x04001018 RID: 4120
		private readonly IntPtr rootStore;

		// Token: 0x04001019 RID: 4121
		public int cMappers;

		// Token: 0x0400101A RID: 4122
		private readonly IntPtr phMappers;

		// Token: 0x0400101B RID: 4123
		public int cSupportedAlgs;

		// Token: 0x0400101C RID: 4124
		private readonly IntPtr palgSupportedAlgs;

		// Token: 0x0400101D RID: 4125
		public SchProtocols grbitEnabledProtocols;

		// Token: 0x0400101E RID: 4126
		public int dwMinimumCipherStrength;

		// Token: 0x0400101F RID: 4127
		public int dwMaximumCipherStrength;

		// Token: 0x04001020 RID: 4128
		public int dwSessionLifespan;

		// Token: 0x04001021 RID: 4129
		public SecureCredential.Flags dwFlags;

		// Token: 0x04001022 RID: 4130
		public int reserved;

		// Token: 0x0200070B RID: 1803
		[Flags]
		public enum Flags
		{
			// Token: 0x040030F1 RID: 12529
			Zero = 0,
			// Token: 0x040030F2 RID: 12530
			NoSystemMapper = 2,
			// Token: 0x040030F3 RID: 12531
			NoNameCheck = 4,
			// Token: 0x040030F4 RID: 12532
			ValidateManual = 8,
			// Token: 0x040030F5 RID: 12533
			NoDefaultCred = 16,
			// Token: 0x040030F6 RID: 12534
			ValidateAuto = 32,
			// Token: 0x040030F7 RID: 12535
			SendAuxRecord = 2097152,
			// Token: 0x040030F8 RID: 12536
			UseStrongCrypto = 4194304
		}
	}
}
