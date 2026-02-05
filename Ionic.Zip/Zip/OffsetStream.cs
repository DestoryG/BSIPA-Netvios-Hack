using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x02000009 RID: 9
	internal class OffsetStream : Stream, IDisposable
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00003592 File Offset: 0x00001792
		public OffsetStream(Stream s)
		{
			this._originalPosition = s.Position;
			this._innerStream = s;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000035AD File Offset: 0x000017AD
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._innerStream.Read(buffer, offset, count);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000035BD File Offset: 0x000017BD
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000035C4 File Offset: 0x000017C4
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000035D1 File Offset: 0x000017D1
		public override bool CanSeek
		{
			get
			{
				return this._innerStream.CanSeek;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000035DE File Offset: 0x000017DE
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000035E1 File Offset: 0x000017E1
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000035EE File Offset: 0x000017EE
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000035FB File Offset: 0x000017FB
		// (set) Token: 0x06000091 RID: 145 RVA: 0x0000360F File Offset: 0x0000180F
		public override long Position
		{
			get
			{
				return this._innerStream.Position - this._originalPosition;
			}
			set
			{
				this._innerStream.Position = this._originalPosition + value;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003624 File Offset: 0x00001824
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(this._originalPosition + offset, origin) - this._originalPosition;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003641 File Offset: 0x00001841
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003648 File Offset: 0x00001848
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003650 File Offset: 0x00001850
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x04000049 RID: 73
		private long _originalPosition;

		// Token: 0x0400004A RID: 74
		private Stream _innerStream;
	}
}
