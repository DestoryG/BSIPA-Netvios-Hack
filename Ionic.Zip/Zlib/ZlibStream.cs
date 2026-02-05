using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x0200006F RID: 111
	public class ZlibStream : Stream
	{
		// Token: 0x060004BB RID: 1211 RVA: 0x00021AFE File Offset: 0x0001FCFE
		public ZlibStream(Stream stream, CompressionMode mode)
			: this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00021B0A File Offset: 0x0001FD0A
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level)
			: this(stream, mode, level, false)
		{
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00021B16 File Offset: 0x0001FD16
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen)
			: this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00021B22 File Offset: 0x0001FD22
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00021B3F File Offset: 0x0001FD3F
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x00021B4C File Offset: 0x0001FD4C
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00021B6D File Offset: 0x0001FD6D
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x00021B7C File Offset: 0x0001FD7C
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00021BE8 File Offset: 0x0001FDE8
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00021BFA File Offset: 0x0001FDFA
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00021C0C File Offset: 0x0001FE0C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00021C58 File Offset: 0x0001FE58
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00021C7D File Offset: 0x0001FE7D
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00021C80 File Offset: 0x0001FE80
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00021CA5 File Offset: 0x0001FEA5
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00021CC5 File Offset: 0x0001FEC5
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00021CCC File Offset: 0x0001FECC
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00021D18 File Offset: 0x0001FF18
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00021D1F File Offset: 0x0001FF1F
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00021D42 File Offset: 0x0001FF42
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00021D49 File Offset: 0x0001FF49
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00021D50 File Offset: 0x0001FF50
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00021D74 File Offset: 0x0001FF74
		public static byte[] CompressString(string s)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream stream = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, stream);
				array = memoryStream.ToArray();
			}
			return array;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00021DBC File Offset: 0x0001FFBC
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream stream = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, stream);
				array = memoryStream.ToArray();
			}
			return array;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00021E04 File Offset: 0x00020004
		public static string UncompressString(byte[] compressed)
		{
			string text;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream stream = new ZlibStream(memoryStream, CompressionMode.Decompress);
				text = ZlibBaseStream.UncompressString(compressed, stream);
			}
			return text;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00021E48 File Offset: 0x00020048
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream stream = new ZlibStream(memoryStream, CompressionMode.Decompress);
				array = ZlibBaseStream.UncompressBuffer(compressed, stream);
			}
			return array;
		}

		// Token: 0x040003D1 RID: 977
		internal ZlibBaseStream _baseStream;

		// Token: 0x040003D2 RID: 978
		private bool _disposed;
	}
}
