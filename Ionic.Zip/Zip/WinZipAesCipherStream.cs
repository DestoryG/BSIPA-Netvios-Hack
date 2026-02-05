using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Ionic.Zip
{
	// Token: 0x0200001C RID: 28
	internal class WinZipAesCipherStream : Stream
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x000052DA File Offset: 0x000034DA
		internal WinZipAesCipherStream(Stream s, WinZipAesCrypto cryptoParams, long length, CryptoMode mode)
			: this(s, cryptoParams, mode)
		{
			this._length = length;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000052F0 File Offset: 0x000034F0
		internal WinZipAesCipherStream(Stream s, WinZipAesCrypto cryptoParams, CryptoMode mode)
		{
			this._params = cryptoParams;
			this._s = s;
			this._mode = mode;
			this._nonce = 1;
			if (this._params == null)
			{
				throw new BadPasswordException("Supply a password to use AES encryption.");
			}
			int num = this._params.KeyBytes.Length * 8;
			if (num != 256 && num != 128 && num != 192)
			{
				throw new ArgumentOutOfRangeException("keysize", "size of key must be 128, 192, or 256");
			}
			this._mac = new HMACSHA1(this._params.MacIv);
			this._aesCipher = new RijndaelManaged();
			this._aesCipher.BlockSize = 128;
			this._aesCipher.KeySize = num;
			this._aesCipher.Mode = CipherMode.ECB;
			this._aesCipher.Padding = PaddingMode.None;
			byte[] array = new byte[16];
			this._xform = this._aesCipher.CreateEncryptor(this._params.KeyBytes, array);
			if (this._mode == CryptoMode.Encrypt)
			{
				this._iobuf = new byte[2048];
				this._PendingWriteBlock = new byte[16];
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005430 File Offset: 0x00003630
		private void XorInPlace(byte[] buffer, int offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				buffer[offset + i] = this.counterOut[i] ^ buffer[offset + i];
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005460 File Offset: 0x00003660
		private void WriteTransformOneBlock(byte[] buffer, int offset)
		{
			Array.Copy(BitConverter.GetBytes(this._nonce++), 0, this.counter, 0, 4);
			this._xform.TransformBlock(this.counter, 0, 16, this.counterOut, 0);
			this.XorInPlace(buffer, offset, 16);
			this._mac.TransformBlock(buffer, offset, 16, null, 0);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000054CC File Offset: 0x000036CC
		private void WriteTransformBlocks(byte[] buffer, int offset, int count)
		{
			int num = offset;
			int num2 = count + offset;
			while (num < buffer.Length && num < num2)
			{
				this.WriteTransformOneBlock(buffer, num);
				num += 16;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000054F8 File Offset: 0x000036F8
		private void WriteTransformFinalBlock()
		{
			if (this._pendingCount == 0)
			{
				throw new InvalidOperationException("No bytes available.");
			}
			if (this._finalBlock)
			{
				throw new InvalidOperationException("The final block has already been transformed.");
			}
			Array.Copy(BitConverter.GetBytes(this._nonce++), 0, this.counter, 0, 4);
			this.counterOut = this._xform.TransformFinalBlock(this.counter, 0, 16);
			this.XorInPlace(this._PendingWriteBlock, 0, this._pendingCount);
			this._mac.TransformFinalBlock(this._PendingWriteBlock, 0, this._pendingCount);
			this._finalBlock = true;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000559C File Offset: 0x0000379C
		private int ReadTransformOneBlock(byte[] buffer, int offset, int last)
		{
			if (this._finalBlock)
			{
				throw new NotSupportedException();
			}
			int num = last - offset;
			int num2 = ((num > 16) ? 16 : num);
			Array.Copy(BitConverter.GetBytes(this._nonce++), 0, this.counter, 0, 4);
			if (num2 == num && this._length > 0L && this._totalBytesXferred + (long)last == this._length)
			{
				this._mac.TransformFinalBlock(buffer, offset, num2);
				this.counterOut = this._xform.TransformFinalBlock(this.counter, 0, 16);
				this._finalBlock = true;
			}
			else
			{
				this._mac.TransformBlock(buffer, offset, num2, null, 0);
				this._xform.TransformBlock(this.counter, 0, 16, this.counterOut, 0);
			}
			this.XorInPlace(buffer, offset, num2);
			return num2;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005674 File Offset: 0x00003874
		private void ReadTransformBlocks(byte[] buffer, int offset, int count)
		{
			int num = offset;
			int num2 = count + offset;
			while (num < buffer.Length && num < num2)
			{
				int num3 = this.ReadTransformOneBlock(buffer, num, num2);
				num += num3;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000056A4 File Offset: 0x000038A4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Encrypt)
			{
				throw new NotSupportedException();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Must not be less than zero.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Must not be less than zero.");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentException("The buffer is too small");
			}
			int num = count;
			if (this._totalBytesXferred >= this._length)
			{
				return 0;
			}
			long num2 = this._length - this._totalBytesXferred;
			if (num2 < (long)count)
			{
				num = (int)num2;
			}
			int num3 = this._s.Read(buffer, offset, num);
			this.ReadTransformBlocks(buffer, offset, num);
			this._totalBytesXferred += (long)num3;
			return num3;
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005758 File Offset: 0x00003958
		public byte[] FinalAuthentication
		{
			get
			{
				if (!this._finalBlock)
				{
					if (this._totalBytesXferred != 0L)
					{
						throw new BadStateException("The final hash has not been computed.");
					}
					byte[] array = new byte[0];
					this._mac.ComputeHash(array);
				}
				byte[] array2 = new byte[10];
				Array.Copy(this._mac.Hash, 0, array2, 0, 10);
				return array2;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000057B4 File Offset: 0x000039B4
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._finalBlock)
			{
				throw new InvalidOperationException("The final block has already been transformed.");
			}
			if (this._mode == CryptoMode.Decrypt)
			{
				throw new NotSupportedException();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Must not be less than zero.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Must not be less than zero.");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentException("The offset and count are too large");
			}
			if (count == 0)
			{
				return;
			}
			if (count + this._pendingCount <= 16)
			{
				Buffer.BlockCopy(buffer, offset, this._PendingWriteBlock, this._pendingCount, count);
				this._pendingCount += count;
				return;
			}
			int num = count;
			int num2 = offset;
			if (this._pendingCount != 0)
			{
				int num3 = 16 - this._pendingCount;
				if (num3 > 0)
				{
					Buffer.BlockCopy(buffer, offset, this._PendingWriteBlock, this._pendingCount, num3);
					num -= num3;
					num2 += num3;
				}
				this.WriteTransformOneBlock(this._PendingWriteBlock, 0);
				this._s.Write(this._PendingWriteBlock, 0, 16);
				this._totalBytesXferred += 16L;
				this._pendingCount = 0;
			}
			int num4 = (num - 1) / 16;
			this._pendingCount = num - num4 * 16;
			Buffer.BlockCopy(buffer, num2 + num - this._pendingCount, this._PendingWriteBlock, 0, this._pendingCount);
			num -= this._pendingCount;
			this._totalBytesXferred += (long)num;
			if (num4 > 0)
			{
				do
				{
					int num5 = this._iobuf.Length;
					if (num5 > num)
					{
						num5 = num;
					}
					Buffer.BlockCopy(buffer, num2, this._iobuf, 0, num5);
					this.WriteTransformBlocks(this._iobuf, 0, num5);
					this._s.Write(this._iobuf, 0, num5);
					num -= num5;
					num2 += num5;
				}
				while (num > 0);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000596C File Offset: 0x00003B6C
		public override void Close()
		{
			if (this._pendingCount > 0)
			{
				this.WriteTransformFinalBlock();
				this._s.Write(this._PendingWriteBlock, 0, this._pendingCount);
				this._totalBytesXferred += (long)this._pendingCount;
				this._pendingCount = 0;
			}
			this._s.Close();
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000059C6 File Offset: 0x00003BC6
		public override bool CanRead
		{
			get
			{
				return this._mode == CryptoMode.Decrypt;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000EF RID: 239 RVA: 0x000059D4 File Offset: 0x00003BD4
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000059D7 File Offset: 0x00003BD7
		public override bool CanWrite
		{
			get
			{
				return this._mode == CryptoMode.Encrypt;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000059E2 File Offset: 0x00003BE2
		public override void Flush()
		{
			this._s.Flush();
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000059EF File Offset: 0x00003BEF
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x000059F6 File Offset: 0x00003BF6
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x000059FD File Offset: 0x00003BFD
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005A04 File Offset: 0x00003C04
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005A0B File Offset: 0x00003C0B
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005A14 File Offset: 0x00003C14
		[Conditional("Trace")]
		private void TraceOutput(string format, params object[] varParams)
		{
			lock (this._outputLock)
			{
				int hashCode = Thread.CurrentThread.GetHashCode();
				Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
				Console.Write("{0:000} WZACS ", hashCode);
				Console.WriteLine(format, varParams);
				Console.ResetColor();
			}
		}

		// Token: 0x04000095 RID: 149
		private const int BLOCK_SIZE_IN_BYTES = 16;

		// Token: 0x04000096 RID: 150
		private WinZipAesCrypto _params;

		// Token: 0x04000097 RID: 151
		private Stream _s;

		// Token: 0x04000098 RID: 152
		private CryptoMode _mode;

		// Token: 0x04000099 RID: 153
		private int _nonce;

		// Token: 0x0400009A RID: 154
		private bool _finalBlock;

		// Token: 0x0400009B RID: 155
		internal HMACSHA1 _mac;

		// Token: 0x0400009C RID: 156
		internal RijndaelManaged _aesCipher;

		// Token: 0x0400009D RID: 157
		internal ICryptoTransform _xform;

		// Token: 0x0400009E RID: 158
		private byte[] counter = new byte[16];

		// Token: 0x0400009F RID: 159
		private byte[] counterOut = new byte[16];

		// Token: 0x040000A0 RID: 160
		private long _length;

		// Token: 0x040000A1 RID: 161
		private long _totalBytesXferred;

		// Token: 0x040000A2 RID: 162
		private byte[] _PendingWriteBlock;

		// Token: 0x040000A3 RID: 163
		private int _pendingCount;

		// Token: 0x040000A4 RID: 164
		private byte[] _iobuf;

		// Token: 0x040000A5 RID: 165
		private object _outputLock = new object();
	}
}
