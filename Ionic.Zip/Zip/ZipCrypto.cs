using System;
using System.IO;
using Ionic.Crc;

namespace Ionic.Zip
{
	// Token: 0x02000026 RID: 38
	internal class ZipCrypto
	{
		// Token: 0x06000134 RID: 308 RVA: 0x000064E4 File Offset: 0x000046E4
		private ZipCrypto()
		{
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006510 File Offset: 0x00004710
		public static ZipCrypto ForWrite(string password)
		{
			ZipCrypto zipCrypto = new ZipCrypto();
			if (password == null)
			{
				throw new BadPasswordException("This entry requires a password.");
			}
			zipCrypto.InitCipher(password);
			return zipCrypto;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000653C File Offset: 0x0000473C
		public static ZipCrypto ForRead(string password, ZipEntry e)
		{
			Stream archiveStream = e._archiveStream;
			e._WeakEncryptionHeader = new byte[12];
			byte[] weakEncryptionHeader = e._WeakEncryptionHeader;
			ZipCrypto zipCrypto = new ZipCrypto();
			if (password == null)
			{
				throw new BadPasswordException("This entry requires a password.");
			}
			zipCrypto.InitCipher(password);
			ZipEntry.ReadWeakEncryptionHeader(archiveStream, weakEncryptionHeader);
			byte[] array = zipCrypto.DecryptMessage(weakEncryptionHeader, weakEncryptionHeader.Length);
			if (array[11] != (byte)((e._Crc32 >> 24) & 255))
			{
				if ((e._BitField & 8) != 8)
				{
					throw new BadPasswordException("The password did not match.");
				}
				if (array[11] != (byte)((e._TimeBlob >> 8) & 255))
				{
					throw new BadPasswordException("The password did not match.");
				}
			}
			return zipCrypto;
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000065E0 File Offset: 0x000047E0
		private byte MagicByte
		{
			get
			{
				ushort num = (ushort)(this._Keys[2] & 65535U) | 2;
				return (byte)(num * (num ^ 1) >> 8);
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00006608 File Offset: 0x00004808
		public byte[] DecryptMessage(byte[] cipherText, int length)
		{
			if (cipherText == null)
			{
				throw new ArgumentNullException("cipherText");
			}
			if (length > cipherText.Length)
			{
				throw new ArgumentOutOfRangeException("length", "Bad length during Decryption: the length parameter must be smaller than or equal to the size of the destination array.");
			}
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				byte b = cipherText[i] ^ this.MagicByte;
				this.UpdateKeys(b);
				array[i] = b;
			}
			return array;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006664 File Offset: 0x00004864
		public byte[] EncryptMessage(byte[] plainText, int length)
		{
			if (plainText == null)
			{
				throw new ArgumentNullException("plaintext");
			}
			if (length > plainText.Length)
			{
				throw new ArgumentOutOfRangeException("length", "Bad length during Encryption: The length parameter must be smaller than or equal to the size of the destination array.");
			}
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				byte b = plainText[i];
				array[i] = plainText[i] ^ this.MagicByte;
				this.UpdateKeys(b);
			}
			return array;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000066C4 File Offset: 0x000048C4
		public void InitCipher(string passphrase)
		{
			byte[] array = SharedUtilities.StringToByteArray(passphrase);
			for (int i = 0; i < passphrase.Length; i++)
			{
				this.UpdateKeys(array[i]);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000066F4 File Offset: 0x000048F4
		private void UpdateKeys(byte byteValue)
		{
			this._Keys[0] = (uint)this.crc32.ComputeCrc32((int)this._Keys[0], byteValue);
			this._Keys[1] = this._Keys[1] + (uint)((byte)this._Keys[0]);
			this._Keys[1] = this._Keys[1] * 134775813U + 1U;
			this._Keys[2] = (uint)this.crc32.ComputeCrc32((int)this._Keys[2], (byte)(this._Keys[1] >> 24));
		}

		// Token: 0x040000BA RID: 186
		private uint[] _Keys = new uint[] { 305419896U, 591751049U, 878082192U };

		// Token: 0x040000BB RID: 187
		private CRC32 crc32 = new CRC32();
	}
}
