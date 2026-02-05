using System;
using System.IO;
using System.Runtime.Serialization;

namespace System.Xml
{
	// Token: 0x0200003F RID: 63
	internal class DelimittedStreamReader
	{
		// Token: 0x06000504 RID: 1284 RVA: 0x00018313 File Offset: 0x00016513
		public DelimittedStreamReader(Stream stream)
		{
			this.stream = new BufferedReadStream(stream);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001832E File Offset: 0x0001652E
		public void Close()
		{
			this.stream.Close();
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001833C File Offset: 0x0001653C
		private void Close(DelimittedStreamReader.DelimittedReadStream caller)
		{
			if (this.currentStream == caller)
			{
				if (this.delimitter == null)
				{
					this.stream.Close();
				}
				else
				{
					if (this.scratch == null)
					{
						this.scratch = new byte[1024];
					}
					while (this.Read(caller, this.scratch, 0, this.scratch.Length) != 0)
					{
					}
				}
				this.currentStream = null;
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x000183A0 File Offset: 0x000165A0
		public Stream GetNextStream(byte[] delimitter)
		{
			if (this.currentStream != null)
			{
				this.currentStream.Close();
				this.currentStream = null;
			}
			if (!this.canGetNextStream)
			{
				return null;
			}
			this.delimitter = delimitter;
			this.canGetNextStream = delimitter != null;
			this.currentStream = new DelimittedStreamReader.DelimittedReadStream(this);
			return this.currentStream;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000183F4 File Offset: 0x000165F4
		private DelimittedStreamReader.MatchState MatchDelimitter(byte[] buffer, int start, int end)
		{
			if (this.delimitter.Length > end - start)
			{
				for (int i = end - start - 1; i >= 1; i--)
				{
					if (buffer[start + i] != this.delimitter[i])
					{
						return DelimittedStreamReader.MatchState.False;
					}
				}
				return DelimittedStreamReader.MatchState.InsufficientData;
			}
			for (int j = this.delimitter.Length - 1; j >= 1; j--)
			{
				if (buffer[start + j] != this.delimitter[j])
				{
					return DelimittedStreamReader.MatchState.False;
				}
			}
			return DelimittedStreamReader.MatchState.True;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00018458 File Offset: 0x00016658
		private int ProcessRead(byte[] buffer, int offset, int read)
		{
			if (read == 0)
			{
				return read;
			}
			int i = offset;
			int num = offset + read;
			while (i < num)
			{
				if (buffer[i] == this.delimitter[0])
				{
					switch (this.MatchDelimitter(buffer, i, num))
					{
					case DelimittedStreamReader.MatchState.True:
					{
						int num2 = i - offset;
						i += this.delimitter.Length;
						this.stream.Push(buffer, i, num - i);
						this.currentStream = null;
						return num2;
					}
					case DelimittedStreamReader.MatchState.InsufficientData:
					{
						int num3 = i - offset;
						if (num3 > 0)
						{
							this.stream.Push(buffer, i, num - i);
							return num3;
						}
						return -1;
					}
					}
				}
				i++;
			}
			return read;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000184E8 File Offset: 0x000166E8
		private int Read(DelimittedStreamReader.DelimittedReadStream caller, byte[] buffer, int offset, int count)
		{
			if (this.currentStream != caller)
			{
				return 0;
			}
			int num = this.stream.Read(buffer, offset, count);
			if (num == 0)
			{
				this.canGetNextStream = false;
				this.currentStream = null;
				return num;
			}
			if (this.delimitter == null)
			{
				return num;
			}
			int num2 = this.ProcessRead(buffer, offset, num);
			if (num2 < 0)
			{
				if (this.matchBuffer == null || this.matchBuffer.Length < this.delimitter.Length - num)
				{
					this.matchBuffer = new byte[this.delimitter.Length - num];
				}
				int num3 = this.stream.ReadBlock(this.matchBuffer, 0, this.delimitter.Length - num);
				if (this.MatchRemainder(num, num3))
				{
					this.currentStream = null;
					num2 = 0;
				}
				else
				{
					this.stream.Push(this.matchBuffer, 0, num3);
					int num4 = 1;
					while (num4 < num && buffer[num4] != this.delimitter[0])
					{
						num4++;
					}
					if (num4 < num)
					{
						this.stream.Push(buffer, offset + num4, num - num4);
					}
					num2 = num4;
				}
			}
			return num2;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000185E6 File Offset: 0x000167E6
		private bool MatchRemainder(int start, int count)
		{
			if (start + count != this.delimitter.Length)
			{
				return false;
			}
			for (count--; count >= 0; count--)
			{
				if (this.delimitter[start + count] != this.matchBuffer[count])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001861E File Offset: 0x0001681E
		internal void Push(byte[] buffer, int offset, int count)
		{
			this.stream.Push(buffer, offset, count);
		}

		// Token: 0x04000215 RID: 533
		private bool canGetNextStream = true;

		// Token: 0x04000216 RID: 534
		private DelimittedStreamReader.DelimittedReadStream currentStream;

		// Token: 0x04000217 RID: 535
		private byte[] delimitter;

		// Token: 0x04000218 RID: 536
		private byte[] matchBuffer;

		// Token: 0x04000219 RID: 537
		private byte[] scratch;

		// Token: 0x0400021A RID: 538
		private BufferedReadStream stream;

		// Token: 0x0200015A RID: 346
		private enum MatchState
		{
			// Token: 0x0400096E RID: 2414
			True,
			// Token: 0x0400096F RID: 2415
			False,
			// Token: 0x04000970 RID: 2416
			InsufficientData
		}

		// Token: 0x0200015B RID: 347
		private class DelimittedReadStream : Stream
		{
			// Token: 0x0600137B RID: 4987 RVA: 0x0004F62A File Offset: 0x0004D82A
			public DelimittedReadStream(DelimittedStreamReader reader)
			{
				if (reader == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("reader");
				}
				this.reader = reader;
			}

			// Token: 0x170003F4 RID: 1012
			// (get) Token: 0x0600137C RID: 4988 RVA: 0x0004F647 File Offset: 0x0004D847
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003F5 RID: 1013
			// (get) Token: 0x0600137D RID: 4989 RVA: 0x0004F64A File Offset: 0x0004D84A
			public override bool CanSeek
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003F6 RID: 1014
			// (get) Token: 0x0600137E RID: 4990 RVA: 0x0004F64D File Offset: 0x0004D84D
			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003F7 RID: 1015
			// (get) Token: 0x0600137F RID: 4991 RVA: 0x0004F650 File Offset: 0x0004D850
			public override long Length
			{
				get
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Seek operation is not supported on this Stream.", new object[] { base.GetType().FullName })));
				}
			}

			// Token: 0x170003F8 RID: 1016
			// (get) Token: 0x06001380 RID: 4992 RVA: 0x0004F67A File Offset: 0x0004D87A
			// (set) Token: 0x06001381 RID: 4993 RVA: 0x0004F6A4 File Offset: 0x0004D8A4
			public override long Position
			{
				get
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Seek operation is not supported on this Stream.", new object[] { base.GetType().FullName })));
				}
				set
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Seek operation is not supported on this Stream.", new object[] { base.GetType().FullName })));
				}
			}

			// Token: 0x06001382 RID: 4994 RVA: 0x0004F6CE File Offset: 0x0004D8CE
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Write operation is not supported on this '{0}' Stream.", new object[] { base.GetType().FullName })));
			}

			// Token: 0x06001383 RID: 4995 RVA: 0x0004F6F8 File Offset: 0x0004D8F8
			public override void Close()
			{
				this.reader.Close(this);
			}

			// Token: 0x06001384 RID: 4996 RVA: 0x0004F706 File Offset: 0x0004D906
			public override void EndWrite(IAsyncResult asyncResult)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Write operation is not supported on this '{0}' Stream.", new object[] { base.GetType().FullName })));
			}

			// Token: 0x06001385 RID: 4997 RVA: 0x0004F730 File Offset: 0x0004D930
			public override void Flush()
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Write operation is not supported on this '{0}' Stream.", new object[] { base.GetType().FullName })));
			}

			// Token: 0x06001386 RID: 4998 RVA: 0x0004F75C File Offset: 0x0004D95C
			public override int Read(byte[] buffer, int offset, int count)
			{
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
				return this.reader.Read(this, buffer, offset, count);
			}

			// Token: 0x06001387 RID: 4999 RVA: 0x0004F828 File Offset: 0x0004DA28
			public override long Seek(long offset, SeekOrigin origin)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Seek operation is not supported on this Stream.", new object[] { base.GetType().FullName })));
			}

			// Token: 0x06001388 RID: 5000 RVA: 0x0004F852 File Offset: 0x0004DA52
			public override void SetLength(long value)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Write operation is not supported on this '{0}' Stream.", new object[] { base.GetType().FullName })));
			}

			// Token: 0x06001389 RID: 5001 RVA: 0x0004F87C File Offset: 0x0004DA7C
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(global::System.Runtime.Serialization.SR.GetString("Write operation is not supported on this '{0}' Stream.", new object[] { base.GetType().FullName })));
			}

			// Token: 0x04000971 RID: 2417
			private DelimittedStreamReader reader;
		}
	}
}
