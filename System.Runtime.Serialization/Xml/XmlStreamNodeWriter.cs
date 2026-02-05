using System;
using System.IO;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Xml
{
	// Token: 0x02000056 RID: 86
	internal abstract class XmlStreamNodeWriter : XmlNodeWriter
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x0001C10B File Offset: 0x0001A30B
		protected XmlStreamNodeWriter()
		{
			this.buffer = new byte[512];
			this.encoding = XmlStreamNodeWriter.UTF8Encoding;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001C12E File Offset: 0x0001A32E
		protected void SetOutput(Stream stream, bool ownsStream, Encoding encoding)
		{
			this.stream = stream;
			this.ownsStream = ownsStream;
			this.offset = 0;
			if (encoding != null)
			{
				this.encoding = encoding;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x0001C14F File Offset: 0x0001A34F
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x0001C157 File Offset: 0x0001A357
		public Stream Stream
		{
			get
			{
				return this.stream;
			}
			set
			{
				this.stream = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001C160 File Offset: 0x0001A360
		public byte[] StreamBuffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x0001C168 File Offset: 0x0001A368
		public int BufferOffset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0001C170 File Offset: 0x0001A370
		public int Position
		{
			get
			{
				return (int)this.stream.Position + this.offset;
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001C188 File Offset: 0x0001A388
		protected byte[] GetBuffer(int count, out int offset)
		{
			int num = this.offset;
			if (num + count <= 512)
			{
				offset = num;
			}
			else
			{
				this.FlushBuffer();
				offset = 0;
			}
			return this.buffer;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001C1BC File Offset: 0x0001A3BC
		internal AsyncCompletionResult GetBufferAsync(XmlStreamNodeWriter.GetBufferAsyncEventArgs getBufferState)
		{
			int count = getBufferState.Arguments.Count;
			int num = this.offset;
			int num2;
			if (num + count <= 512)
			{
				num2 = num;
			}
			else
			{
				if (XmlStreamNodeWriter.onGetFlushComplete == null)
				{
					XmlStreamNodeWriter.onGetFlushComplete = new AsyncEventArgsCallback(XmlStreamNodeWriter.GetBufferFlushComplete);
				}
				if (this.flushBufferState == null)
				{
					this.flushBufferState = new AsyncEventArgs<object>();
				}
				this.flushBufferState.Set(XmlStreamNodeWriter.onGetFlushComplete, getBufferState, this);
				if (this.FlushBufferAsync(this.flushBufferState) != 1)
				{
					return 0;
				}
				num2 = 0;
				this.flushBufferState.Complete(true);
			}
			getBufferState.Result = getBufferState.Result ?? new XmlStreamNodeWriter.GetBufferEventResult();
			getBufferState.Result.Buffer = this.buffer;
			getBufferState.Result.Offset = num2;
			return 1;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001C280 File Offset: 0x0001A480
		private static void GetBufferFlushComplete(IAsyncEventArgs completionState)
		{
			XmlStreamNodeWriter xmlStreamNodeWriter = (XmlStreamNodeWriter)completionState.AsyncState;
			XmlStreamNodeWriter.GetBufferAsyncEventArgs getBufferAsyncEventArgs = (XmlStreamNodeWriter.GetBufferAsyncEventArgs)xmlStreamNodeWriter.flushBufferState.Arguments;
			getBufferAsyncEventArgs.Result = getBufferAsyncEventArgs.Result ?? new XmlStreamNodeWriter.GetBufferEventResult();
			getBufferAsyncEventArgs.Result.Buffer = xmlStreamNodeWriter.buffer;
			getBufferAsyncEventArgs.Result.Offset = 0;
			getBufferAsyncEventArgs.Complete(false, completionState.Exception);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001C2E8 File Offset: 0x0001A4E8
		private AsyncCompletionResult FlushBufferAsync(AsyncEventArgs<object> state)
		{
			if (Interlocked.CompareExchange(ref this.hasPendingWrite, 1, 0) != 0)
			{
				throw FxTrace.Exception.AsError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Flush buffer is already in use.")));
			}
			if (this.offset != 0)
			{
				if (XmlStreamNodeWriter.onFlushBufferComplete == null)
				{
					XmlStreamNodeWriter.onFlushBufferComplete = new AsyncCallback(XmlStreamNodeWriter.OnFlushBufferCompete);
				}
				IAsyncResult asyncResult = this.stream.BeginWrite(this.buffer, 0, this.offset, XmlStreamNodeWriter.onFlushBufferComplete, this);
				if (!asyncResult.CompletedSynchronously)
				{
					return 0;
				}
				this.stream.EndWrite(asyncResult);
				this.offset = 0;
			}
			if (Interlocked.CompareExchange(ref this.hasPendingWrite, 0, 1) != 1)
			{
				throw FxTrace.Exception.AsError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("No async write operation is pending.")));
			}
			return 1;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001C3A8 File Offset: 0x0001A5A8
		private static void OnFlushBufferCompete(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			XmlStreamNodeWriter xmlStreamNodeWriter = (XmlStreamNodeWriter)result.AsyncState;
			Exception ex = null;
			try
			{
				xmlStreamNodeWriter.stream.EndWrite(result);
				xmlStreamNodeWriter.offset = 0;
				if (Interlocked.CompareExchange(ref xmlStreamNodeWriter.hasPendingWrite, 0, 1) != 1)
				{
					throw FxTrace.Exception.AsError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("No async write operation is pending.")));
				}
			}
			catch (Exception ex2)
			{
				if (Fx.IsFatal(ex2))
				{
					throw;
				}
				ex = ex2;
			}
			xmlStreamNodeWriter.flushBufferState.Complete(false, ex);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001C438 File Offset: 0x0001A638
		protected IAsyncResult BeginGetBuffer(int count, AsyncCallback callback, object state)
		{
			return new XmlStreamNodeWriter.GetBufferAsyncResult(count, this, callback, state);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001C443 File Offset: 0x0001A643
		protected byte[] EndGetBuffer(IAsyncResult result, out int offset)
		{
			return XmlStreamNodeWriter.GetBufferAsyncResult.End(result, out offset);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001C44C File Offset: 0x0001A64C
		protected void Advance(int count)
		{
			this.offset += count;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001C45C File Offset: 0x0001A65C
		private void EnsureByte()
		{
			if (this.offset >= 512)
			{
				this.FlushBuffer();
			}
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001C474 File Offset: 0x0001A674
		protected void WriteByte(byte b)
		{
			this.EnsureByte();
			byte[] array = this.buffer;
			int num = this.offset;
			this.offset = num + 1;
			array[num] = b;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001C4A0 File Offset: 0x0001A6A0
		protected void WriteByte(char ch)
		{
			this.WriteByte((byte)ch);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001C4AC File Offset: 0x0001A6AC
		protected void WriteBytes(byte b1, byte b2)
		{
			byte[] array = this.buffer;
			int num = this.offset;
			if (num + 1 >= 512)
			{
				this.FlushBuffer();
				num = 0;
			}
			array[num] = b1;
			array[num + 1] = b2;
			this.offset += 2;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0001C4EF File Offset: 0x0001A6EF
		protected void WriteBytes(char ch1, char ch2)
		{
			this.WriteBytes((byte)ch1, (byte)ch2);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001C4FC File Offset: 0x0001A6FC
		public void WriteBytes(byte[] byteBuffer, int byteOffset, int byteCount)
		{
			if (byteCount < 512)
			{
				int num;
				byte[] array = this.GetBuffer(byteCount, out num);
				Buffer.BlockCopy(byteBuffer, byteOffset, array, num, byteCount);
				this.Advance(byteCount);
				return;
			}
			this.FlushBuffer();
			this.stream.Write(byteBuffer, byteOffset, byteCount);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0001C541 File Offset: 0x0001A741
		public IAsyncResult BeginWriteBytes(byte[] byteBuffer, int byteOffset, int byteCount, AsyncCallback callback, object state)
		{
			return new XmlStreamNodeWriter.WriteBytesAsyncResult(byteBuffer, byteOffset, byteCount, this, callback, state);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001C550 File Offset: 0x0001A750
		public void EndWriteBytes(IAsyncResult result)
		{
			XmlStreamNodeWriter.WriteBytesAsyncResult.End(result);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001C558 File Offset: 0x0001A758
		[SecurityCritical]
		protected unsafe void UnsafeWriteBytes(byte* bytes, int byteCount)
		{
			this.FlushBuffer();
			byte[] array = this.buffer;
			while (byteCount > 512)
			{
				for (int i = 0; i < 512; i++)
				{
					array[i] = bytes[i];
				}
				this.stream.Write(array, 0, 512);
				bytes += 512;
				byteCount -= 512;
			}
			if (byteCount > 0)
			{
				for (int j = 0; j < byteCount; j++)
				{
					array[j] = bytes[j];
				}
				this.stream.Write(array, 0, byteCount);
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001C5DC File Offset: 0x0001A7DC
		[SecuritySafeCritical]
		protected unsafe void WriteUTF8Char(int ch)
		{
			if (ch < 128)
			{
				this.WriteByte((byte)ch);
				return;
			}
			if (ch <= 65535)
			{
				char* ptr = stackalloc char[(UIntPtr)2];
				*ptr = (char)ch;
				this.UnsafeWriteUTF8Chars(ptr, 1);
				return;
			}
			SurrogateChar surrogateChar = new SurrogateChar(ch);
			char* ptr2 = stackalloc char[(UIntPtr)4];
			*ptr2 = surrogateChar.HighChar;
			ptr2[1] = surrogateChar.LowChar;
			this.UnsafeWriteUTF8Chars(ptr2, 2);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001C640 File Offset: 0x0001A840
		protected void WriteUTF8Chars(byte[] chars, int charOffset, int charCount)
		{
			if (charCount < 512)
			{
				int num;
				byte[] array = this.GetBuffer(charCount, out num);
				Buffer.BlockCopy(chars, charOffset, array, num, charCount);
				this.Advance(charCount);
				return;
			}
			this.FlushBuffer();
			this.stream.Write(chars, charOffset, charCount);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001C688 File Offset: 0x0001A888
		[SecuritySafeCritical]
		protected unsafe void WriteUTF8Chars(string value)
		{
			int length = value.Length;
			if (length > 0)
			{
				fixed (string text = value)
				{
					char* ptr = text;
					if (ptr != null)
					{
						ptr += RuntimeHelpers.OffsetToStringData / 2;
					}
					this.UnsafeWriteUTF8Chars(ptr, length);
				}
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001C6BC File Offset: 0x0001A8BC
		[SecurityCritical]
		protected unsafe void UnsafeWriteUTF8Chars(char* chars, int charCount)
		{
			while (charCount > 170)
			{
				int num = 170;
				if ((chars[num - 1] & 'ﰀ') == '\ud800')
				{
					num--;
				}
				int num2;
				byte[] array = this.GetBuffer(num * 3, out num2);
				this.Advance(this.UnsafeGetUTF8Chars(chars, num, array, num2));
				charCount -= num;
				chars += num;
			}
			if (charCount > 0)
			{
				int num3;
				byte[] array2 = this.GetBuffer(charCount * 3, out num3);
				this.Advance(this.UnsafeGetUTF8Chars(chars, charCount, array2, num3));
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001C740 File Offset: 0x0001A940
		[SecurityCritical]
		protected unsafe void UnsafeWriteUnicodeChars(char* chars, int charCount)
		{
			while (charCount > 256)
			{
				int num = 256;
				if ((chars[num - 1] & 'ﰀ') == '\ud800')
				{
					num--;
				}
				int num2;
				byte[] array = this.GetBuffer(num * 2, out num2);
				this.Advance(this.UnsafeGetUnicodeChars(chars, num, array, num2));
				charCount -= num;
				chars += num;
			}
			if (charCount > 0)
			{
				int num3;
				byte[] array2 = this.GetBuffer(charCount * 2, out num3);
				this.Advance(this.UnsafeGetUnicodeChars(chars, charCount, array2, num3));
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001C7C4 File Offset: 0x0001A9C4
		[SecurityCritical]
		protected unsafe int UnsafeGetUnicodeChars(char* chars, int charCount, byte[] buffer, int offset)
		{
			char* ptr = chars + charCount;
			while (chars < ptr)
			{
				char c = *(chars++);
				buffer[offset++] = (byte)c;
				c >>= 8;
				buffer[offset++] = (byte)c;
			}
			return charCount * 2;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001C804 File Offset: 0x0001AA04
		[SecurityCritical]
		protected unsafe int UnsafeGetUTF8Length(char* chars, int charCount)
		{
			char* ptr = chars + charCount;
			while (chars < ptr && *chars < '\u0080')
			{
				chars++;
			}
			if (chars == ptr)
			{
				return charCount;
			}
			return (int)((long)(chars - (ptr - charCount))) + this.encoding.GetByteCount(chars, (int)((long)(ptr - chars)));
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001C854 File Offset: 0x0001AA54
		[SecurityCritical]
		protected unsafe int UnsafeGetUTF8Chars(char* chars, int charCount, byte[] buffer, int offset)
		{
			if (charCount > 0)
			{
				fixed (byte* ptr = &buffer[offset])
				{
					byte* ptr2 = ptr;
					byte* ptr3 = ptr2;
					byte* ptr4 = ptr3 + (buffer.Length - offset);
					char* ptr5 = chars + charCount;
					do
					{
						if (chars < ptr5)
						{
							char c = *chars;
							if (c < '\u0080')
							{
								*ptr3 = (byte)c;
								ptr3++;
								chars++;
								continue;
							}
						}
						if (chars >= ptr5)
						{
							break;
						}
						char* ptr6 = chars;
						while (chars < ptr5 && *chars >= '\u0080')
						{
							chars++;
						}
						ptr3 += this.encoding.GetBytes(ptr6, (int)((long)(chars - ptr6)), ptr3, (int)((long)(ptr4 - ptr3)));
					}
					while (chars < ptr5);
					return (int)((long)(ptr3 - ptr2));
				}
			}
			return 0;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001C8F6 File Offset: 0x0001AAF6
		protected virtual void FlushBuffer()
		{
			if (this.offset != 0)
			{
				this.stream.Write(this.buffer, 0, this.offset);
				this.offset = 0;
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001C91F File Offset: 0x0001AB1F
		protected virtual IAsyncResult BeginFlushBuffer(AsyncCallback callback, object state)
		{
			return new XmlStreamNodeWriter.FlushBufferAsyncResult(this, callback, state);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001C929 File Offset: 0x0001AB29
		protected virtual void EndFlushBuffer(IAsyncResult result)
		{
			XmlStreamNodeWriter.FlushBufferAsyncResult.End(result);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001C931 File Offset: 0x0001AB31
		public override void Flush()
		{
			this.FlushBuffer();
			this.stream.Flush();
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001C944 File Offset: 0x0001AB44
		public override void Close()
		{
			if (this.stream != null)
			{
				if (this.ownsStream)
				{
					this.stream.Close();
				}
				this.stream = null;
			}
		}

		// Token: 0x04000293 RID: 659
		private Stream stream;

		// Token: 0x04000294 RID: 660
		private byte[] buffer;

		// Token: 0x04000295 RID: 661
		private int offset;

		// Token: 0x04000296 RID: 662
		private bool ownsStream;

		// Token: 0x04000297 RID: 663
		private const int bufferLength = 512;

		// Token: 0x04000298 RID: 664
		private const int maxEntityLength = 32;

		// Token: 0x04000299 RID: 665
		private const int maxBytesPerChar = 3;

		// Token: 0x0400029A RID: 666
		private Encoding encoding;

		// Token: 0x0400029B RID: 667
		private int hasPendingWrite;

		// Token: 0x0400029C RID: 668
		private AsyncEventArgs<object> flushBufferState;

		// Token: 0x0400029D RID: 669
		private static UTF8Encoding UTF8Encoding = new UTF8Encoding(false, true);

		// Token: 0x0400029E RID: 670
		private static AsyncCallback onFlushBufferComplete;

		// Token: 0x0400029F RID: 671
		private static AsyncEventArgsCallback onGetFlushComplete;

		// Token: 0x02000162 RID: 354
		private class GetBufferAsyncResult : AsyncResult
		{
			// Token: 0x060013BC RID: 5052 RVA: 0x0004FAAC File Offset: 0x0004DCAC
			public GetBufferAsyncResult(int count, XmlStreamNodeWriter writer, AsyncCallback callback, object state)
				: base(callback, state)
			{
				this.count = count;
				this.writer = writer;
				int num = writer.offset;
				bool flag;
				if (num + count <= 512)
				{
					this.offset = num;
					flag = true;
				}
				else
				{
					IAsyncResult asyncResult = writer.BeginFlushBuffer(base.PrepareAsyncCompletion(XmlStreamNodeWriter.GetBufferAsyncResult.onComplete), this);
					flag = base.SyncContinue(asyncResult);
				}
				if (flag)
				{
					base.Complete(true);
				}
			}

			// Token: 0x060013BD RID: 5053 RVA: 0x0004FB13 File Offset: 0x0004DD13
			private static bool OnComplete(IAsyncResult result)
			{
				return ((XmlStreamNodeWriter.GetBufferAsyncResult)result.AsyncState).HandleFlushBuffer(result);
			}

			// Token: 0x060013BE RID: 5054 RVA: 0x0004FB26 File Offset: 0x0004DD26
			private bool HandleFlushBuffer(IAsyncResult result)
			{
				this.writer.EndFlushBuffer(result);
				this.offset = 0;
				return true;
			}

			// Token: 0x060013BF RID: 5055 RVA: 0x0004FB3C File Offset: 0x0004DD3C
			public static byte[] End(IAsyncResult result, out int offset)
			{
				XmlStreamNodeWriter.GetBufferAsyncResult getBufferAsyncResult = AsyncResult.End<XmlStreamNodeWriter.GetBufferAsyncResult>(result);
				offset = getBufferAsyncResult.offset;
				return getBufferAsyncResult.writer.buffer;
			}

			// Token: 0x0400098A RID: 2442
			private XmlStreamNodeWriter writer;

			// Token: 0x0400098B RID: 2443
			private int offset;

			// Token: 0x0400098C RID: 2444
			private int count;

			// Token: 0x0400098D RID: 2445
			private static AsyncResult.AsyncCompletion onComplete = new AsyncResult.AsyncCompletion(XmlStreamNodeWriter.GetBufferAsyncResult.OnComplete);
		}

		// Token: 0x02000163 RID: 355
		private class WriteBytesAsyncResult : AsyncResult
		{
			// Token: 0x060013C1 RID: 5057 RVA: 0x0004FB78 File Offset: 0x0004DD78
			public WriteBytesAsyncResult(byte[] byteBuffer, int byteOffset, int byteCount, XmlStreamNodeWriter writer, AsyncCallback callback, object state)
				: base(callback, state)
			{
				this.byteBuffer = byteBuffer;
				this.byteOffset = byteOffset;
				this.byteCount = byteCount;
				this.writer = writer;
				bool flag;
				if (byteCount < 512)
				{
					flag = this.HandleGetBuffer(null);
				}
				else
				{
					flag = this.HandleFlushBuffer(null);
				}
				if (flag)
				{
					base.Complete(true);
				}
			}

			// Token: 0x060013C2 RID: 5058 RVA: 0x0004FBD2 File Offset: 0x0004DDD2
			private static bool OnHandleGetBufferComplete(IAsyncResult result)
			{
				return ((XmlStreamNodeWriter.WriteBytesAsyncResult)result.AsyncState).HandleGetBuffer(result);
			}

			// Token: 0x060013C3 RID: 5059 RVA: 0x0004FBE5 File Offset: 0x0004DDE5
			private static bool OnHandleFlushBufferComplete(IAsyncResult result)
			{
				return ((XmlStreamNodeWriter.WriteBytesAsyncResult)result.AsyncState).HandleFlushBuffer(result);
			}

			// Token: 0x060013C4 RID: 5060 RVA: 0x0004FBF8 File Offset: 0x0004DDF8
			private static bool OnHandleWrite(IAsyncResult result)
			{
				return ((XmlStreamNodeWriter.WriteBytesAsyncResult)result.AsyncState).HandleWrite(result);
			}

			// Token: 0x060013C5 RID: 5061 RVA: 0x0004FC0C File Offset: 0x0004DE0C
			private bool HandleGetBuffer(IAsyncResult result)
			{
				if (result == null)
				{
					result = this.writer.BeginGetBuffer(this.byteCount, base.PrepareAsyncCompletion(XmlStreamNodeWriter.WriteBytesAsyncResult.onHandleGetBufferComplete), this);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				int num;
				byte[] array = this.writer.EndGetBuffer(result, out num);
				Buffer.BlockCopy(this.byteBuffer, this.byteOffset, array, num, this.byteCount);
				this.writer.Advance(this.byteCount);
				return true;
			}

			// Token: 0x060013C6 RID: 5062 RVA: 0x0004FC7F File Offset: 0x0004DE7F
			private bool HandleFlushBuffer(IAsyncResult result)
			{
				if (result == null)
				{
					result = this.writer.BeginFlushBuffer(base.PrepareAsyncCompletion(XmlStreamNodeWriter.WriteBytesAsyncResult.onHandleFlushBufferComplete), this);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				this.writer.EndFlushBuffer(result);
				return this.HandleWrite(null);
			}

			// Token: 0x060013C7 RID: 5063 RVA: 0x0004FCBC File Offset: 0x0004DEBC
			private bool HandleWrite(IAsyncResult result)
			{
				if (result == null)
				{
					result = this.writer.stream.BeginWrite(this.byteBuffer, this.byteOffset, this.byteCount, base.PrepareAsyncCompletion(XmlStreamNodeWriter.WriteBytesAsyncResult.onHandleWrite), this);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				this.writer.stream.EndWrite(result);
				return true;
			}

			// Token: 0x060013C8 RID: 5064 RVA: 0x0004FD18 File Offset: 0x0004DF18
			public static void End(IAsyncResult result)
			{
				AsyncResult.End<XmlStreamNodeWriter.WriteBytesAsyncResult>(result);
			}

			// Token: 0x0400098E RID: 2446
			private static AsyncResult.AsyncCompletion onHandleGetBufferComplete = new AsyncResult.AsyncCompletion(XmlStreamNodeWriter.WriteBytesAsyncResult.OnHandleGetBufferComplete);

			// Token: 0x0400098F RID: 2447
			private static AsyncResult.AsyncCompletion onHandleFlushBufferComplete = new AsyncResult.AsyncCompletion(XmlStreamNodeWriter.WriteBytesAsyncResult.OnHandleFlushBufferComplete);

			// Token: 0x04000990 RID: 2448
			private static AsyncResult.AsyncCompletion onHandleWrite = new AsyncResult.AsyncCompletion(XmlStreamNodeWriter.WriteBytesAsyncResult.OnHandleWrite);

			// Token: 0x04000991 RID: 2449
			private byte[] byteBuffer;

			// Token: 0x04000992 RID: 2450
			private int byteOffset;

			// Token: 0x04000993 RID: 2451
			private int byteCount;

			// Token: 0x04000994 RID: 2452
			private XmlStreamNodeWriter writer;
		}

		// Token: 0x02000164 RID: 356
		private class FlushBufferAsyncResult : AsyncResult
		{
			// Token: 0x060013CA RID: 5066 RVA: 0x0004FD58 File Offset: 0x0004DF58
			public FlushBufferAsyncResult(XmlStreamNodeWriter writer, AsyncCallback callback, object state)
				: base(callback, state)
			{
				this.writer = writer;
				bool flag = true;
				if (writer.offset != 0)
				{
					flag = this.HandleFlushBuffer(null);
				}
				if (flag)
				{
					base.Complete(true);
				}
			}

			// Token: 0x060013CB RID: 5067 RVA: 0x0004FD90 File Offset: 0x0004DF90
			private static bool OnComplete(IAsyncResult result)
			{
				return ((XmlStreamNodeWriter.FlushBufferAsyncResult)result.AsyncState).HandleFlushBuffer(result);
			}

			// Token: 0x060013CC RID: 5068 RVA: 0x0004FDA4 File Offset: 0x0004DFA4
			private bool HandleFlushBuffer(IAsyncResult result)
			{
				if (result == null)
				{
					result = this.writer.stream.BeginWrite(this.writer.buffer, 0, this.writer.offset, base.PrepareAsyncCompletion(XmlStreamNodeWriter.FlushBufferAsyncResult.onComplete), this);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				this.writer.stream.EndWrite(result);
				this.writer.offset = 0;
				return true;
			}

			// Token: 0x060013CD RID: 5069 RVA: 0x0004FE11 File Offset: 0x0004E011
			public static void End(IAsyncResult result)
			{
				AsyncResult.End<XmlStreamNodeWriter.FlushBufferAsyncResult>(result);
			}

			// Token: 0x04000995 RID: 2453
			private static AsyncResult.AsyncCompletion onComplete = new AsyncResult.AsyncCompletion(XmlStreamNodeWriter.FlushBufferAsyncResult.OnComplete);

			// Token: 0x04000996 RID: 2454
			private XmlStreamNodeWriter writer;
		}

		// Token: 0x02000165 RID: 357
		internal class GetBufferArgs
		{
			// Token: 0x170003F9 RID: 1017
			// (get) Token: 0x060013CF RID: 5071 RVA: 0x0004FE2D File Offset: 0x0004E02D
			// (set) Token: 0x060013D0 RID: 5072 RVA: 0x0004FE35 File Offset: 0x0004E035
			public int Count { get; set; }
		}

		// Token: 0x02000166 RID: 358
		internal class GetBufferEventResult
		{
			// Token: 0x170003FA RID: 1018
			// (get) Token: 0x060013D2 RID: 5074 RVA: 0x0004FE46 File Offset: 0x0004E046
			// (set) Token: 0x060013D3 RID: 5075 RVA: 0x0004FE4E File Offset: 0x0004E04E
			internal byte[] Buffer { get; set; }

			// Token: 0x170003FB RID: 1019
			// (get) Token: 0x060013D4 RID: 5076 RVA: 0x0004FE57 File Offset: 0x0004E057
			// (set) Token: 0x060013D5 RID: 5077 RVA: 0x0004FE5F File Offset: 0x0004E05F
			internal int Offset { get; set; }
		}

		// Token: 0x02000167 RID: 359
		internal class GetBufferAsyncEventArgs : AsyncEventArgs<XmlStreamNodeWriter.GetBufferArgs, XmlStreamNodeWriter.GetBufferEventResult>
		{
		}
	}
}
