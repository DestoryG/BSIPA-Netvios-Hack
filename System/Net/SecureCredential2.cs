using System;
using System.Net.Security;

namespace System.Net
{
	// Token: 0x0200012F RID: 303
	internal struct SecureCredential2
	{
		// Token: 0x06000B3D RID: 2877 RVA: 0x0003DBD8 File Offset: 0x0003BDD8
		public SecureCredential2(SecureCredential2.Flags flags, SchProtocols protocols, EncryptionPolicy policy)
		{
			this.rootStore = (this.phMappers = IntPtr.Zero);
			this.pTlsParameters = null;
			this.certContextArray = null;
			this.cCreds = (this.cMappers = (this.cTlsParameters = (this.dwCredformat = 0)));
			this.dwSessionLifespan = 0;
			this.version = 5;
			this.dwFlags = flags;
			if (policy == EncryptionPolicy.AllowNoEncryption)
			{
				this.dwFlags |= SecureCredential2.Flags.AllowNullEencryption;
			}
		}

		// Token: 0x04001023 RID: 4131
		public const int CurrentVersion = 5;

		// Token: 0x04001024 RID: 4132
		public int version;

		// Token: 0x04001025 RID: 4133
		public int dwCredformat;

		// Token: 0x04001026 RID: 4134
		public int cCreds;

		// Token: 0x04001027 RID: 4135
		public unsafe void** certContextArray;

		// Token: 0x04001028 RID: 4136
		private readonly IntPtr rootStore;

		// Token: 0x04001029 RID: 4137
		public int cMappers;

		// Token: 0x0400102A RID: 4138
		private readonly IntPtr phMappers;

		// Token: 0x0400102B RID: 4139
		public int dwSessionLifespan;

		// Token: 0x0400102C RID: 4140
		public SecureCredential2.Flags dwFlags;

		// Token: 0x0400102D RID: 4141
		public int cTlsParameters;

		// Token: 0x0400102E RID: 4142
		public unsafe TlsParamaters* pTlsParameters;

		// Token: 0x0200070C RID: 1804
		[Flags]
		public enum Flags
		{
			// Token: 0x040030FA RID: 12538
			Zero = 0,
			// Token: 0x040030FB RID: 12539
			NoSystemMapper = 2,
			// Token: 0x040030FC RID: 12540
			NoNameCheck = 4,
			// Token: 0x040030FD RID: 12541
			ValidateManual = 8,
			// Token: 0x040030FE RID: 12542
			NoDefaultCred = 16,
			// Token: 0x040030FF RID: 12543
			ValidateAuto = 32,
			// Token: 0x04003100 RID: 12544
			SendAuxRecord = 2097152,
			// Token: 0x04003101 RID: 12545
			UseStrongCrypto = 4194304,
			// Token: 0x04003102 RID: 12546
			UsePresharedKeyOnly = 8388608,
			// Token: 0x04003103 RID: 12547
			AllowNullEencryption = 33554432
		}
	}
}
