using System;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x0200031E RID: 798
	internal class RangeStream : BaseWrapperStream, ICloseEx
	{
		// Token: 0x06001C98 RID: 7320 RVA: 0x000879C8 File Offset: 0x00085BC8
		internal RangeStream(Stream parentStream, long offset, long size)
			: base(parentStream)
		{
			this.m_Offset = offset;
			this.m_Size = size;
			if (base.WrappedStream.CanSeek)
			{
				base.WrappedStream.Position = offset;
				this.m_Position = offset;
				return;
			}
			throw new NotSupportedException(SR.GetString("net_cache_non_seekable_stream_not_supported"));
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06001C99 RID: 7321 RVA: 0x00087A1A File Offset: 0x00085C1A
		public override bool CanRead
		{
			get
			{
				return base.WrappedStream.CanRead;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06001C9A RID: 7322 RVA: 0x00087A27 File Offset: 0x00085C27
		public override bool CanSeek
		{
			get
			{
				return base.WrappedStream.CanSeek;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x00087A34 File Offset: 0x00085C34
		public override bool CanWrite
		{
			get
			{
				return base.WrappedStream.CanWrite;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x00087A44 File Offset: 0x00085C44
		public override long Length
		{
			get
			{
				long length = base.WrappedStream.Length;
				return this.m_Size;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x00087A63 File Offset: 0x00085C63
		// (set) Token: 0x06001C9E RID: 7326 RVA: 0x00087A77 File Offset: 0x00085C77
		public override long Position
		{
			get
			{
				return base.WrappedStream.Position - this.m_Offset;
			}
			set
			{
				value += this.m_Offset;
				if (value > this.m_Offset + this.m_Size)
				{
					value = this.m_Offset + this.m_Size;
				}
				base.WrappedStream.Position = value;
			}
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x00087AB0 File Offset: 0x00085CB0
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin != SeekOrigin.Begin)
			{
				if (origin != SeekOrigin.End)
				{
					if (this.m_Position + offset > this.m_Offset + this.m_Size)
					{
						offset = this.m_Offset + this.m_Size - this.m_Position;
					}
					if (this.m_Position + offset < this.m_Offset)
					{
						offset = this.m_Offset - this.m_Position;
					}
				}
				else
				{
					offset -= this.m_Offset + this.m_Size;
					if (offset > 0L)
					{
						offset = 0L;
					}
					if (offset < -this.m_Size)
					{
						offset = -this.m_Size;
					}
				}
			}
			else
			{
				offset += this.m_Offset;
				if (offset > this.m_Offset + this.m_Size)
				{
					offset = this.m_Offset + this.m_Size;
				}
				if (offset < this.m_Offset)
				{
					offset = this.m_Offset;
				}
			}
			this.m_Position = base.WrappedStream.Seek(offset, origin);
			return this.m_Position - this.m_Offset;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x00087BA0 File Offset: 0x00085DA0
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_cache_unsupported_partial_stream"));
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x00087BB4 File Offset: 0x00085DB4
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.m_Position + (long)count > this.m_Offset + this.m_Size)
			{
				throw new NotSupportedException(SR.GetString("net_cache_unsupported_partial_stream"));
			}
			base.WrappedStream.Write(buffer, offset, count);
			this.m_Position += (long)count;
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x00087C06 File Offset: 0x00085E06
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.m_Position + (long)offset > this.m_Offset + this.m_Size)
			{
				throw new NotSupportedException(SR.GetString("net_cache_unsupported_partial_stream"));
			}
			return base.WrappedStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x00087C42 File Offset: 0x00085E42
		public override void EndWrite(IAsyncResult asyncResult)
		{
			base.WrappedStream.EndWrite(asyncResult);
			this.m_Position = base.WrappedStream.Position;
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x00087C61 File Offset: 0x00085E61
		public override void Flush()
		{
			base.WrappedStream.Flush();
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x00087C70 File Offset: 0x00085E70
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.m_Position >= this.m_Offset + this.m_Size)
			{
				return 0;
			}
			if (this.m_Position + (long)count > this.m_Offset + this.m_Size)
			{
				count = (int)(this.m_Offset + this.m_Size - this.m_Position);
			}
			int num = base.WrappedStream.Read(buffer, offset, count);
			this.m_Position += (long)num;
			return num;
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x00087CE4 File Offset: 0x00085EE4
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.m_Position >= this.m_Offset + this.m_Size)
			{
				count = 0;
			}
			else if (this.m_Position + (long)count > this.m_Offset + this.m_Size)
			{
				count = (int)(this.m_Offset + this.m_Size - this.m_Position);
			}
			return base.WrappedStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x00087D4C File Offset: 0x00085F4C
		public override int EndRead(IAsyncResult asyncResult)
		{
			int num = base.WrappedStream.EndRead(asyncResult);
			this.m_Position += (long)num;
			return num;
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x00087D76 File Offset: 0x00085F76
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x00087D80 File Offset: 0x00085F80
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			this.Dispose(true, closeState);
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001CAA RID: 7338 RVA: 0x00087D8A File Offset: 0x00085F8A
		public override bool CanTimeout
		{
			get
			{
				return base.WrappedStream.CanTimeout;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001CAB RID: 7339 RVA: 0x00087D97 File Offset: 0x00085F97
		// (set) Token: 0x06001CAC RID: 7340 RVA: 0x00087DA4 File Offset: 0x00085FA4
		public override int ReadTimeout
		{
			get
			{
				return base.WrappedStream.ReadTimeout;
			}
			set
			{
				base.WrappedStream.ReadTimeout = value;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06001CAD RID: 7341 RVA: 0x00087DB2 File Offset: 0x00085FB2
		// (set) Token: 0x06001CAE RID: 7342 RVA: 0x00087DBF File Offset: 0x00085FBF
		public override int WriteTimeout
		{
			get
			{
				return base.WrappedStream.WriteTimeout;
			}
			set
			{
				base.WrappedStream.WriteTimeout = value;
			}
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x00087DD0 File Offset: 0x00085FD0
		protected virtual void Dispose(bool disposing, CloseExState closeState)
		{
			try
			{
				if (disposing)
				{
					ICloseEx closeEx = base.WrappedStream as ICloseEx;
					if (closeEx != null)
					{
						closeEx.CloseEx(closeState);
					}
					else
					{
						base.WrappedStream.Close();
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x04001BA2 RID: 7074
		private long m_Offset;

		// Token: 0x04001BA3 RID: 7075
		private long m_Size;

		// Token: 0x04001BA4 RID: 7076
		private long m_Position;
	}
}
