using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x02000025 RID: 37
	public class CountingStream : Stream
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00006358 File Offset: 0x00004558
		public CountingStream(Stream stream)
		{
			this._s = stream;
			try
			{
				this._initialOffset = this._s.Position;
			}
			catch
			{
				this._initialOffset = 0L;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000063A0 File Offset: 0x000045A0
		public Stream WrappedStream
		{
			get
			{
				return this._s;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000063A8 File Offset: 0x000045A8
		public long BytesWritten
		{
			get
			{
				return this._bytesWritten;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000063B0 File Offset: 0x000045B0
		public long BytesRead
		{
			get
			{
				return this._bytesRead;
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000063B8 File Offset: 0x000045B8
		public void Adjust(long delta)
		{
			this._bytesWritten -= delta;
			if (this._bytesWritten < 0L)
			{
				throw new InvalidOperationException();
			}
			if (this._s is CountingStream)
			{
				((CountingStream)this._s).Adjust(delta);
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000063F8 File Offset: 0x000045F8
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this._s.Read(buffer, offset, count);
			this._bytesRead += (long)num;
			return num;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006424 File Offset: 0x00004624
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			this._s.Write(buffer, offset, count);
			this._bytesWritten += (long)count;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00006447 File Offset: 0x00004647
		public override bool CanRead
		{
			get
			{
				return this._s.CanRead;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00006454 File Offset: 0x00004654
		public override bool CanSeek
		{
			get
			{
				return this._s.CanSeek;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00006461 File Offset: 0x00004661
		public override bool CanWrite
		{
			get
			{
				return this._s.CanWrite;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000646E File Offset: 0x0000466E
		public override void Flush()
		{
			this._s.Flush();
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600012E RID: 302 RVA: 0x0000647B File Offset: 0x0000467B
		public override long Length
		{
			get
			{
				return this._s.Length;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00006488 File Offset: 0x00004688
		public long ComputedPosition
		{
			get
			{
				return this._initialOffset + this._bytesWritten;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006497 File Offset: 0x00004697
		// (set) Token: 0x06000131 RID: 305 RVA: 0x000064A4 File Offset: 0x000046A4
		public override long Position
		{
			get
			{
				return this._s.Position;
			}
			set
			{
				this._s.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000064B4 File Offset: 0x000046B4
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._s.Seek(offset, origin);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000064C3 File Offset: 0x000046C3
		public override void SetLength(long value)
		{
			this._s.SetLength(value);
		}

		// Token: 0x040000B6 RID: 182
		private Stream _s;

		// Token: 0x040000B7 RID: 183
		private long _bytesWritten;

		// Token: 0x040000B8 RID: 184
		private long _bytesRead;

		// Token: 0x040000B9 RID: 185
		private long _initialOffset;
	}
}
