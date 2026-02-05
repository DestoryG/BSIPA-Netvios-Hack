using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x02000028 RID: 40
	internal class ZipCipherStream : Stream
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00006776 File Offset: 0x00004976
		public ZipCipherStream(Stream s, ZipCrypto cipher, CryptoMode mode)
		{
			this._cipher = cipher;
			this._s = s;
			this._mode = mode;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006794 File Offset: 0x00004994
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Encrypt)
			{
				throw new NotSupportedException("This stream does not encrypt via Read()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			byte[] array = new byte[count];
			int num = this._s.Read(array, 0, count);
			byte[] array2 = this._cipher.DecryptMessage(array, num);
			for (int i = 0; i < num; i++)
			{
				buffer[offset + i] = array2[i];
			}
			return num;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000067FC File Offset: 0x000049FC
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Decrypt)
			{
				throw new NotSupportedException("This stream does not Decrypt via Write()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count == 0)
			{
				return;
			}
			byte[] array;
			if (offset != 0)
			{
				array = new byte[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = buffer[offset + i];
				}
			}
			else
			{
				array = buffer;
			}
			byte[] array2 = this._cipher.EncryptMessage(array, count);
			this._s.Write(array2, 0, array2.Length);
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00006871 File Offset: 0x00004A71
		public override bool CanRead
		{
			get
			{
				return this._mode == CryptoMode.Decrypt;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000687C File Offset: 0x00004A7C
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000687F File Offset: 0x00004A7F
		public override bool CanWrite
		{
			get
			{
				return this._mode == CryptoMode.Encrypt;
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000688A File Offset: 0x00004A8A
		public override void Flush()
		{
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000688C File Offset: 0x00004A8C
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00006893 File Offset: 0x00004A93
		// (set) Token: 0x06000145 RID: 325 RVA: 0x0000689A File Offset: 0x00004A9A
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000068A1 File Offset: 0x00004AA1
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000068A8 File Offset: 0x00004AA8
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040000BF RID: 191
		private ZipCrypto _cipher;

		// Token: 0x040000C0 RID: 192
		private Stream _s;

		// Token: 0x040000C1 RID: 193
		private CryptoMode _mode;
	}
}
