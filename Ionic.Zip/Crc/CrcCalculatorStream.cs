using System;
using System.IO;

namespace Ionic.Crc
{
	// Token: 0x02000071 RID: 113
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x060004E8 RID: 1256 RVA: 0x00022346 File Offset: 0x00020546
		public CrcCalculatorStream(Stream stream)
			: this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00022356 File Offset: 0x00020556
		public CrcCalculatorStream(Stream stream, bool leaveOpen)
			: this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00022366 File Offset: 0x00020566
		public CrcCalculatorStream(Stream stream, long length)
			: this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00022382 File Offset: 0x00020582
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen)
			: this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0002239E File Offset: 0x0002059E
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32)
			: this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000223BB File Offset: 0x000205BB
		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			this._innerStream = stream;
			this._Crc32 = crc32 ?? new CRC32();
			this._lengthLimit = length;
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x000223F2 File Offset: 0x000205F2
		public long TotalBytesSlurped
		{
			get
			{
				return this._Crc32.TotalBytesRead;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x000223FF File Offset: 0x000205FF
		public int Crc
		{
			get
			{
				return this._Crc32.Crc32Result;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x0002240C File Offset: 0x0002060C
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x00022414 File Offset: 0x00020614
		public bool LeaveOpen
		{
			get
			{
				return this._leaveOpen;
			}
			set
			{
				this._leaveOpen = value;
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00022420 File Offset: 0x00020620
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = count;
			if (this._lengthLimit != CrcCalculatorStream.UnsetLengthLimit)
			{
				if (this._Crc32.TotalBytesRead >= this._lengthLimit)
				{
					return 0;
				}
				long num2 = this._lengthLimit - this._Crc32.TotalBytesRead;
				if (num2 < (long)count)
				{
					num = (int)num2;
				}
			}
			int num3 = this._innerStream.Read(buffer, offset, num);
			if (num3 > 0)
			{
				this._Crc32.SlurpBlock(buffer, offset, num3);
			}
			return num3;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0002248E File Offset: 0x0002068E
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._Crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x000224B0 File Offset: 0x000206B0
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x000224BD File Offset: 0x000206BD
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x000224C0 File Offset: 0x000206C0
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000224CD File Offset: 0x000206CD
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x000224DA File Offset: 0x000206DA
		public override long Length
		{
			get
			{
				if (this._lengthLimit == CrcCalculatorStream.UnsetLengthLimit)
				{
					return this._innerStream.Length;
				}
				return this._lengthLimit;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x000224FB File Offset: 0x000206FB
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x00022508 File Offset: 0x00020708
		public override long Position
		{
			get
			{
				return this._Crc32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0002250F File Offset: 0x0002070F
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00022516 File Offset: 0x00020716
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0002251D File Offset: 0x0002071D
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00022525 File Offset: 0x00020725
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x040003D9 RID: 985
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x040003DA RID: 986
		internal Stream _innerStream;

		// Token: 0x040003DB RID: 987
		private CRC32 _Crc32;

		// Token: 0x040003DC RID: 988
		private long _lengthLimit = -99L;

		// Token: 0x040003DD RID: 989
		private bool _leaveOpen;
	}
}
