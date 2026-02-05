using System;
using System.IO;
using System.Security.Cryptography;

namespace Ionic.Zip
{
	// Token: 0x0200001B RID: 27
	internal class WinZipAesCrypto
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00005020 File Offset: 0x00003220
		private WinZipAesCrypto(string password, int KeyStrengthInBits)
		{
			this._Password = password;
			this._KeyStrengthInBits = KeyStrengthInBits;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005044 File Offset: 0x00003244
		public static WinZipAesCrypto Generate(string password, int KeyStrengthInBits)
		{
			WinZipAesCrypto winZipAesCrypto = new WinZipAesCrypto(password, KeyStrengthInBits);
			int num = winZipAesCrypto._KeyStrengthInBytes / 2;
			winZipAesCrypto._Salt = new byte[num];
			Random random = new Random();
			random.NextBytes(winZipAesCrypto._Salt);
			return winZipAesCrypto;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005084 File Offset: 0x00003284
		public static WinZipAesCrypto ReadFromStream(string password, int KeyStrengthInBits, Stream s)
		{
			WinZipAesCrypto winZipAesCrypto = new WinZipAesCrypto(password, KeyStrengthInBits);
			int num = winZipAesCrypto._KeyStrengthInBytes / 2;
			winZipAesCrypto._Salt = new byte[num];
			winZipAesCrypto._providedPv = new byte[2];
			s.Read(winZipAesCrypto._Salt, 0, winZipAesCrypto._Salt.Length);
			s.Read(winZipAesCrypto._providedPv, 0, winZipAesCrypto._providedPv.Length);
			winZipAesCrypto.PasswordVerificationStored = (short)((int)winZipAesCrypto._providedPv[0] + (int)winZipAesCrypto._providedPv[1] * 256);
			if (password != null)
			{
				winZipAesCrypto.PasswordVerificationGenerated = (short)((int)winZipAesCrypto.GeneratedPV[0] + (int)winZipAesCrypto.GeneratedPV[1] * 256);
				if (winZipAesCrypto.PasswordVerificationGenerated != winZipAesCrypto.PasswordVerificationStored)
				{
					throw new BadPasswordException("bad password");
				}
			}
			return winZipAesCrypto;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000513F File Offset: 0x0000333F
		public byte[] GeneratedPV
		{
			get
			{
				if (!this._cryptoGenerated)
				{
					this._GenerateCryptoBytes();
				}
				return this._generatedPv;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005155 File Offset: 0x00003355
		public byte[] Salt
		{
			get
			{
				return this._Salt;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000DA RID: 218 RVA: 0x0000515D File Offset: 0x0000335D
		private int _KeyStrengthInBytes
		{
			get
			{
				return this._KeyStrengthInBits / 8;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00005167 File Offset: 0x00003367
		public int SizeOfEncryptionMetadata
		{
			get
			{
				return this._KeyStrengthInBytes / 2 + 10 + 2;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000051C6 File Offset: 0x000033C6
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00005178 File Offset: 0x00003378
		public string Password
		{
			private get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				if (this._Password != null)
				{
					this.PasswordVerificationGenerated = (short)((int)this.GeneratedPV[0] + (int)this.GeneratedPV[1] * 256);
					if (this.PasswordVerificationGenerated != this.PasswordVerificationStored)
					{
						throw new BadPasswordException();
					}
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000051D0 File Offset: 0x000033D0
		private void _GenerateCryptoBytes()
		{
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(this._Password, this.Salt, this.Rfc2898KeygenIterations);
			this._keyBytes = rfc2898DeriveBytes.GetBytes(this._KeyStrengthInBytes);
			this._MacInitializationVector = rfc2898DeriveBytes.GetBytes(this._KeyStrengthInBytes);
			this._generatedPv = rfc2898DeriveBytes.GetBytes(2);
			this._cryptoGenerated = true;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000DF RID: 223 RVA: 0x0000522D File Offset: 0x0000342D
		public byte[] KeyBytes
		{
			get
			{
				if (!this._cryptoGenerated)
				{
					this._GenerateCryptoBytes();
				}
				return this._keyBytes;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005243 File Offset: 0x00003443
		public byte[] MacIv
		{
			get
			{
				if (!this._cryptoGenerated)
				{
					this._GenerateCryptoBytes();
				}
				return this._MacInitializationVector;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000525C File Offset: 0x0000345C
		public void ReadAndVerifyMac(Stream s)
		{
			bool flag = false;
			this._StoredMac = new byte[10];
			s.Read(this._StoredMac, 0, this._StoredMac.Length);
			if (this._StoredMac.Length != this.CalculatedMac.Length)
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = 0; i < this._StoredMac.Length; i++)
				{
					if (this._StoredMac[i] != this.CalculatedMac[i])
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				throw new BadStateException("The MAC does not match.");
			}
		}

		// Token: 0x04000088 RID: 136
		internal byte[] _Salt;

		// Token: 0x04000089 RID: 137
		internal byte[] _providedPv;

		// Token: 0x0400008A RID: 138
		internal byte[] _generatedPv;

		// Token: 0x0400008B RID: 139
		internal int _KeyStrengthInBits;

		// Token: 0x0400008C RID: 140
		private byte[] _MacInitializationVector;

		// Token: 0x0400008D RID: 141
		private byte[] _StoredMac;

		// Token: 0x0400008E RID: 142
		private byte[] _keyBytes;

		// Token: 0x0400008F RID: 143
		private short PasswordVerificationStored;

		// Token: 0x04000090 RID: 144
		private short PasswordVerificationGenerated;

		// Token: 0x04000091 RID: 145
		private int Rfc2898KeygenIterations = 1000;

		// Token: 0x04000092 RID: 146
		private string _Password;

		// Token: 0x04000093 RID: 147
		private bool _cryptoGenerated;

		// Token: 0x04000094 RID: 148
		public byte[] CalculatedMac;
	}
}
