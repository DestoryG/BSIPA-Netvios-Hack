using System;
using System.Security.Permissions;

namespace System.IO.Compression
{
	// Token: 0x0200042E RID: 1070
	[global::__DynamicallyInvokable]
	public class GZipStream : Stream
	{
		// Token: 0x06002815 RID: 10261 RVA: 0x000B82B5 File Offset: 0x000B64B5
		[global::__DynamicallyInvokable]
		public GZipStream(Stream stream, CompressionMode mode)
			: this(stream, mode, false)
		{
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000B82C0 File Offset: 0x000B64C0
		[global::__DynamicallyInvokable]
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen)
		{
			if (mode == CompressionMode.Decompress)
			{
				this.deflateStream = new DeflateStream(stream, leaveOpen, new GZipDecoder());
				return;
			}
			this.deflateStream = new DeflateStream(stream, mode, leaveOpen);
			this.deflateStream.SetFileFormatWriter(new GZipFormatter());
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000B82FC File Offset: 0x000B64FC
		[global::__DynamicallyInvokable]
		public GZipStream(Stream stream, CompressionLevel compressionLevel)
			: this(stream, compressionLevel, false)
		{
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000B8307 File Offset: 0x000B6507
		[global::__DynamicallyInvokable]
		public GZipStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen)
		{
			this.deflateStream = new DeflateStream(stream, compressionLevel, leaveOpen);
			this.deflateStream.SetFileFormatWriter(new GZipFormatter());
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06002819 RID: 10265 RVA: 0x000B832D File Offset: 0x000B652D
		[global::__DynamicallyInvokable]
		public override bool CanRead
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.deflateStream != null && this.deflateStream.CanRead;
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x0600281A RID: 10266 RVA: 0x000B8344 File Offset: 0x000B6544
		[global::__DynamicallyInvokable]
		public override bool CanWrite
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.deflateStream != null && this.deflateStream.CanWrite;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x0600281B RID: 10267 RVA: 0x000B835B File Offset: 0x000B655B
		[global::__DynamicallyInvokable]
		public override bool CanSeek
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.deflateStream != null && this.deflateStream.CanSeek;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x0600281C RID: 10268 RVA: 0x000B8372 File Offset: 0x000B6572
		[global::__DynamicallyInvokable]
		public override long Length
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x0600281D RID: 10269 RVA: 0x000B8383 File Offset: 0x000B6583
		// (set) Token: 0x0600281E RID: 10270 RVA: 0x000B8394 File Offset: 0x000B6594
		[global::__DynamicallyInvokable]
		public override long Position
		{
			[global::__DynamicallyInvokable]
			get
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
			[global::__DynamicallyInvokable]
			set
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x000B83A5 File Offset: 0x000B65A5
		[global::__DynamicallyInvokable]
		public override void Flush()
		{
			if (this.deflateStream == null)
			{
				throw new ObjectDisposedException(null, SR.GetString("ObjectDisposed_StreamClosed"));
			}
			this.deflateStream.Flush();
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000B83CB File Offset: 0x000B65CB
		[global::__DynamicallyInvokable]
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("NotSupported"));
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000B83DC File Offset: 0x000B65DC
		[global::__DynamicallyInvokable]
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("NotSupported"));
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000B83ED File Offset: 0x000B65ED
		[global::__DynamicallyInvokable]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			if (this.deflateStream == null)
			{
				throw new InvalidOperationException(SR.GetString("ObjectDisposed_StreamClosed"));
			}
			return this.deflateStream.BeginRead(array, offset, count, asyncCallback, asyncState);
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000B8419 File Offset: 0x000B6619
		[global::__DynamicallyInvokable]
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (this.deflateStream == null)
			{
				throw new InvalidOperationException(SR.GetString("ObjectDisposed_StreamClosed"));
			}
			return this.deflateStream.EndRead(asyncResult);
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x000B843F File Offset: 0x000B663F
		[global::__DynamicallyInvokable]
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			if (this.deflateStream == null)
			{
				throw new InvalidOperationException(SR.GetString("ObjectDisposed_StreamClosed"));
			}
			return this.deflateStream.BeginWrite(array, offset, count, asyncCallback, asyncState);
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x000B846B File Offset: 0x000B666B
		[global::__DynamicallyInvokable]
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (this.deflateStream == null)
			{
				throw new InvalidOperationException(SR.GetString("ObjectDisposed_StreamClosed"));
			}
			this.deflateStream.EndWrite(asyncResult);
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x000B8491 File Offset: 0x000B6691
		[global::__DynamicallyInvokable]
		public override int Read(byte[] array, int offset, int count)
		{
			if (this.deflateStream == null)
			{
				throw new ObjectDisposedException(null, SR.GetString("ObjectDisposed_StreamClosed"));
			}
			return this.deflateStream.Read(array, offset, count);
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x000B84BA File Offset: 0x000B66BA
		[global::__DynamicallyInvokable]
		public override void Write(byte[] array, int offset, int count)
		{
			if (this.deflateStream == null)
			{
				throw new ObjectDisposedException(null, SR.GetString("ObjectDisposed_StreamClosed"));
			}
			this.deflateStream.Write(array, offset, count);
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000B84E4 File Offset: 0x000B66E4
		[global::__DynamicallyInvokable]
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this.deflateStream != null)
				{
					this.deflateStream.Close();
				}
				this.deflateStream = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x000B8528 File Offset: 0x000B6728
		[global::__DynamicallyInvokable]
		public Stream BaseStream
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.deflateStream != null)
				{
					return this.deflateStream.BaseStream;
				}
				return null;
			}
		}

		// Token: 0x040021D1 RID: 8657
		private DeflateStream deflateStream;
	}
}
