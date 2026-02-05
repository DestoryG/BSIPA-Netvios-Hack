using System;
using System.IO;
using System.Runtime.Serialization;

namespace System.Xml
{
	// Token: 0x02000048 RID: 72
	internal class BufferedReadStream : Stream
	{
		// Token: 0x06000533 RID: 1331 RVA: 0x00019203 File Offset: 0x00017403
		public BufferedReadStream(Stream stream)
			: this(stream, false)
		{
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001920D File Offset: 0x0001740D
		public BufferedReadStream(Stream stream, bool readMore)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			this.stream = stream;
			this.readMore = readMore;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00019231 File Offset: 0x00017431
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00019234 File Offset: 0x00017434
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00019237 File Offset: 0x00017437
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00019244 File Offset: 0x00017444
		public override long Length
		{
			get
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Seek operation is not supported on this Stream.", new object[] { this.stream.GetType().FullName })));
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x00019273 File Offset: 0x00017473
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x000192A2 File Offset: 0x000174A2
		public override long Position
		{
			get
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Seek operation is not supported on this Stream.", new object[] { this.stream.GetType().FullName })));
			}
			set
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Seek operation is not supported on this Stream.", new object[] { this.stream.GetType().FullName })));
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x000192D4 File Offset: 0x000174D4
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanRead)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Read operation is not supported on the Stream.", new object[] { this.stream.GetType().FullName })));
			}
			return this.stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00019329 File Offset: 0x00017529
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Write operation is not supported on this '{0}' Stream.", new object[] { this.stream.GetType().FullName })));
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00019358 File Offset: 0x00017558
		public override void Close()
		{
			this.stream.Close();
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00019368 File Offset: 0x00017568
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (!this.CanRead)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Read operation is not supported on the Stream.", new object[] { this.stream.GetType().FullName })));
			}
			return this.stream.EndRead(asyncResult);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x000193B7 File Offset: 0x000175B7
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Write operation is not supported on this '{0}' Stream.", new object[] { this.stream.GetType().FullName })));
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000193E6 File Offset: 0x000175E6
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000193F4 File Offset: 0x000175F4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!this.CanRead)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Read operation is not supported on the Stream.", new object[] { this.stream.GetType().FullName })));
			}
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { buffer.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { buffer.Length - offset })));
			}
			int num = 0;
			if (this.storedOffset < this.storedLength)
			{
				num = Math.Min(count, this.storedLength - this.storedOffset);
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, buffer, offset, num);
				this.storedOffset += num;
				if (num == count || !this.readMore)
				{
					return num;
				}
				offset += num;
				count -= num;
			}
			return num + this.stream.Read(buffer, offset, count);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00019558 File Offset: 0x00017758
		public override int ReadByte()
		{
			if (this.storedOffset < this.storedLength)
			{
				byte[] array = this.storedBuffer;
				int num = this.storedOffset;
				this.storedOffset = num + 1;
				return array[num];
			}
			return base.ReadByte();
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00019594 File Offset: 0x00017794
		public int ReadBlock(byte[] buffer, int offset, int count)
		{
			int num = 0;
			int num2;
			while (num < count && (num2 = this.Read(buffer, offset + num, count - num)) != 0)
			{
				num += num2;
			}
			return num;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000195C0 File Offset: 0x000177C0
		public void Push(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			if (this.storedOffset == this.storedLength)
			{
				if (this.storedBuffer == null || this.storedBuffer.Length < count)
				{
					this.storedBuffer = new byte[count];
				}
				this.storedOffset = 0;
				this.storedLength = count;
			}
			else if (count <= this.storedOffset)
			{
				this.storedOffset -= count;
			}
			else if (count <= this.storedBuffer.Length - this.storedLength + this.storedOffset)
			{
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, this.storedBuffer, count, this.storedLength - this.storedOffset);
				this.storedLength += count - this.storedOffset;
				this.storedOffset = 0;
			}
			else
			{
				byte[] array = new byte[count + this.storedLength - this.storedOffset];
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, array, count, this.storedLength - this.storedOffset);
				this.storedLength += count - this.storedOffset;
				this.storedOffset = 0;
				this.storedBuffer = array;
			}
			Buffer.BlockCopy(buffer, offset, this.storedBuffer, this.storedOffset, count);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000196F5 File Offset: 0x000178F5
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Seek operation is not supported on this Stream.", new object[] { this.stream.GetType().FullName })));
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00019724 File Offset: 0x00017924
		public override void SetLength(long value)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Seek operation is not supported on this Stream.", new object[] { this.stream.GetType().FullName })));
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00019753 File Offset: 0x00017953
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Write operation is not supported on this '{0}' Stream.", new object[] { this.stream.GetType().FullName })));
		}

		// Token: 0x04000236 RID: 566
		private Stream stream;

		// Token: 0x04000237 RID: 567
		private byte[] storedBuffer;

		// Token: 0x04000238 RID: 568
		private int storedLength;

		// Token: 0x04000239 RID: 569
		private int storedOffset;

		// Token: 0x0400023A RID: 570
		private bool readMore;
	}
}
