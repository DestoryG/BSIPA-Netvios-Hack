using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x02000227 RID: 551
	internal class DelegatedStream : Stream
	{
		// Token: 0x06001440 RID: 5184 RVA: 0x0006B6B4 File Offset: 0x000698B4
		protected DelegatedStream()
		{
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0006B6BC File Offset: 0x000698BC
		protected DelegatedStream(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.stream = stream;
			this.netStream = stream as NetworkStream;
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x0006B6E5 File Offset: 0x000698E5
		protected Stream BaseStream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x0006B6ED File Offset: 0x000698ED
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x0006B6FA File Offset: 0x000698FA
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x0006B707 File Offset: 0x00069907
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x0006B714 File Offset: 0x00069914
		public override long Length
		{
			get
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.GetString("SeekNotSupported"));
				}
				return this.stream.Length;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x0006B739 File Offset: 0x00069939
		// (set) Token: 0x06001448 RID: 5192 RVA: 0x0006B75E File Offset: 0x0006995E
		public override long Position
		{
			get
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.GetString("SeekNotSupported"));
				}
				return this.stream.Position;
			}
			set
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.GetString("SeekNotSupported"));
				}
				this.stream.Position = value;
			}
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0006B784 File Offset: 0x00069984
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.GetString("ReadNotSupported"));
			}
			IAsyncResult asyncResult;
			if (this.netStream != null)
			{
				asyncResult = this.netStream.UnsafeBeginRead(buffer, offset, count, callback, state);
			}
			else
			{
				asyncResult = this.stream.BeginRead(buffer, offset, count, callback, state);
			}
			return asyncResult;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0006B7DC File Offset: 0x000699DC
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException(SR.GetString("WriteNotSupported"));
			}
			IAsyncResult asyncResult;
			if (this.netStream != null)
			{
				asyncResult = this.netStream.UnsafeBeginWrite(buffer, offset, count, callback, state);
			}
			else
			{
				asyncResult = this.stream.BeginWrite(buffer, offset, count, callback, state);
			}
			return asyncResult;
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0006B834 File Offset: 0x00069A34
		public override void Close()
		{
			this.stream.Close();
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0006B844 File Offset: 0x00069A44
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.GetString("ReadNotSupported"));
			}
			return this.stream.EndRead(asyncResult);
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0006B877 File Offset: 0x00069A77
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException(SR.GetString("WriteNotSupported"));
			}
			this.stream.EndWrite(asyncResult);
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0006B89D File Offset: 0x00069A9D
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0006B8AA File Offset: 0x00069AAA
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return this.stream.FlushAsync(cancellationToken);
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0006B8B8 File Offset: 0x00069AB8
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.GetString("ReadNotSupported"));
			}
			return this.stream.Read(buffer, offset, count);
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0006B8ED File Offset: 0x00069AED
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.GetString("ReadNotSupported"));
			}
			return this.stream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0006B918 File Offset: 0x00069B18
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (!this.CanSeek)
			{
				throw new NotSupportedException(SR.GetString("SeekNotSupported"));
			}
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0006B94C File Offset: 0x00069B4C
		public override void SetLength(long value)
		{
			if (!this.CanSeek)
			{
				throw new NotSupportedException(SR.GetString("SeekNotSupported"));
			}
			this.stream.SetLength(value);
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0006B972 File Offset: 0x00069B72
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException(SR.GetString("WriteNotSupported"));
			}
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0006B99A File Offset: 0x00069B9A
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException(SR.GetString("WriteNotSupported"));
			}
			return this.stream.WriteAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x04001620 RID: 5664
		private Stream stream;

		// Token: 0x04001621 RID: 5665
		private NetworkStream netStream;
	}
}
