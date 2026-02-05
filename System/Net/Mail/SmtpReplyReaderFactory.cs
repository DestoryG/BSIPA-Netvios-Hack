using System;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000294 RID: 660
	internal class SmtpReplyReaderFactory
	{
		// Token: 0x0600188E RID: 6286 RVA: 0x0007CA7A File Offset: 0x0007AC7A
		internal SmtpReplyReaderFactory(Stream stream)
		{
			this.bufferedStream = new BufferedReadStream(stream);
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x0600188F RID: 6287 RVA: 0x0007CA8E File Offset: 0x0007AC8E
		internal SmtpReplyReader CurrentReader
		{
			get
			{
				return this.currentReader;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001890 RID: 6288 RVA: 0x0007CA96 File Offset: 0x0007AC96
		internal SmtpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x0007CAA0 File Offset: 0x0007ACA0
		internal IAsyncResult BeginReadLines(SmtpReplyReader caller, AsyncCallback callback, object state)
		{
			SmtpReplyReaderFactory.ReadLinesAsyncResult readLinesAsyncResult = new SmtpReplyReaderFactory.ReadLinesAsyncResult(this, callback, state);
			readLinesAsyncResult.Read(caller);
			return readLinesAsyncResult;
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x0007CAC0 File Offset: 0x0007ACC0
		internal IAsyncResult BeginReadLine(SmtpReplyReader caller, AsyncCallback callback, object state)
		{
			SmtpReplyReaderFactory.ReadLinesAsyncResult readLinesAsyncResult = new SmtpReplyReaderFactory.ReadLinesAsyncResult(this, callback, state, true);
			readLinesAsyncResult.Read(caller);
			return readLinesAsyncResult;
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0007CAE0 File Offset: 0x0007ACE0
		internal void Close(SmtpReplyReader caller)
		{
			if (this.currentReader == caller)
			{
				if (this.readState != SmtpReplyReaderFactory.ReadState.Done)
				{
					if (this.byteBuffer == null)
					{
						this.byteBuffer = new byte[256];
					}
					while (this.Read(caller, this.byteBuffer, 0, this.byteBuffer.Length) != 0)
					{
					}
				}
				this.currentReader = null;
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0007CB36 File Offset: 0x0007AD36
		internal LineInfo[] EndReadLines(IAsyncResult result)
		{
			return SmtpReplyReaderFactory.ReadLinesAsyncResult.End(result);
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x0007CB40 File Offset: 0x0007AD40
		internal LineInfo EndReadLine(IAsyncResult result)
		{
			LineInfo[] array = SmtpReplyReaderFactory.ReadLinesAsyncResult.End(result);
			if (array != null && array.Length != 0)
			{
				return array[0];
			}
			return default(LineInfo);
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x0007CB6C File Offset: 0x0007AD6C
		internal SmtpReplyReader GetNextReplyReader()
		{
			if (this.currentReader != null)
			{
				this.currentReader.Close();
			}
			this.readState = SmtpReplyReaderFactory.ReadState.Status0;
			this.currentReader = new SmtpReplyReader(this);
			return this.currentReader;
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x0007CB9C File Offset: 0x0007AD9C
		private unsafe int ProcessRead(byte[] buffer, int offset, int read, bool readLine)
		{
			if (read == 0)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { "net_io_connectionclosed" }));
			}
			byte* ptr;
			if (buffer == null || buffer.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &buffer[0];
			}
			byte* ptr2 = ptr + offset;
			byte* ptr3 = ptr2;
			byte* ptr4 = ptr3 + read;
			switch (this.readState)
			{
			case SmtpReplyReaderFactory.ReadState.Status0:
				goto IL_007C;
			case SmtpReplyReaderFactory.ReadState.Status1:
				goto IL_00C1;
			case SmtpReplyReaderFactory.ReadState.Status2:
				goto IL_010D;
			case SmtpReplyReaderFactory.ReadState.ContinueFlag:
				goto IL_0156;
			case SmtpReplyReaderFactory.ReadState.ContinueCR:
				break;
			case SmtpReplyReaderFactory.ReadState.ContinueLF:
				goto IL_01A9;
			case SmtpReplyReaderFactory.ReadState.LastCR:
				goto IL_01F1;
			case SmtpReplyReaderFactory.ReadState.LastLF:
				goto IL_01FF;
			case SmtpReplyReaderFactory.ReadState.Done:
				goto IL_0227;
			default:
				goto IL_023A;
			}
			IL_0198:
			while (ptr3 < ptr4)
			{
				if (*(ptr3++) == 13)
				{
					goto IL_01A9;
				}
			}
			this.readState = SmtpReplyReaderFactory.ReadState.ContinueCR;
			goto IL_023A;
			IL_01F1:
			while (ptr3 < ptr4)
			{
				if (*(ptr3++) == 13)
				{
					goto IL_01FF;
				}
			}
			this.readState = SmtpReplyReaderFactory.ReadState.LastCR;
			goto IL_023A;
			IL_007C:
			if (ptr3 >= ptr4)
			{
				this.readState = SmtpReplyReaderFactory.ReadState.Status0;
				goto IL_023A;
			}
			byte b = *(ptr3++);
			if (b < 48 && b > 57)
			{
				throw new FormatException(SR.GetString("SmtpInvalidResponse"));
			}
			this.statusCode = (SmtpStatusCode)(100 * (b - 48));
			IL_00C1:
			if (ptr3 >= ptr4)
			{
				this.readState = SmtpReplyReaderFactory.ReadState.Status1;
				goto IL_023A;
			}
			byte b2 = *(ptr3++);
			if (b2 < 48 && b2 > 57)
			{
				throw new FormatException(SR.GetString("SmtpInvalidResponse"));
			}
			this.statusCode += (int)(10 * (b2 - 48));
			IL_010D:
			if (ptr3 >= ptr4)
			{
				this.readState = SmtpReplyReaderFactory.ReadState.Status2;
				goto IL_023A;
			}
			byte b3 = *(ptr3++);
			if (b3 < 48 && b3 > 57)
			{
				throw new FormatException(SR.GetString("SmtpInvalidResponse"));
			}
			this.statusCode += (int)(b3 - 48);
			IL_0156:
			if (ptr3 >= ptr4)
			{
				this.readState = SmtpReplyReaderFactory.ReadState.ContinueFlag;
				goto IL_023A;
			}
			byte b4 = *(ptr3++);
			if (b4 == 32)
			{
				goto IL_01F1;
			}
			if (b4 != 45)
			{
				throw new FormatException(SR.GetString("SmtpInvalidResponse"));
			}
			goto IL_0198;
			IL_01A9:
			if (ptr3 >= ptr4)
			{
				this.readState = SmtpReplyReaderFactory.ReadState.ContinueLF;
				goto IL_023A;
			}
			if (*(ptr3++) != 10)
			{
				throw new FormatException(SR.GetString("SmtpInvalidResponse"));
			}
			if (readLine)
			{
				this.readState = SmtpReplyReaderFactory.ReadState.Status0;
				return (int)((long)(ptr3 - ptr2));
			}
			goto IL_007C;
			IL_01FF:
			if (ptr3 >= ptr4)
			{
				this.readState = SmtpReplyReaderFactory.ReadState.LastLF;
				goto IL_023A;
			}
			if (*(ptr3++) != 10)
			{
				throw new FormatException(SR.GetString("SmtpInvalidResponse"));
			}
			IL_0227:
			int num = (int)((long)(ptr3 - ptr2));
			this.readState = SmtpReplyReaderFactory.ReadState.Done;
			return num;
			IL_023A:
			return (int)((long)(ptr3 - ptr2));
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0007CDEC File Offset: 0x0007AFEC
		internal int Read(SmtpReplyReader caller, byte[] buffer, int offset, int count)
		{
			if (count == 0 || this.currentReader != caller || this.readState == SmtpReplyReaderFactory.ReadState.Done)
			{
				return 0;
			}
			int num = this.bufferedStream.Read(buffer, offset, count);
			int num2 = this.ProcessRead(buffer, offset, num, false);
			if (num2 < num)
			{
				this.bufferedStream.Push(buffer, offset + num2, num - num2);
			}
			return num2;
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x0007CE44 File Offset: 0x0007B044
		internal LineInfo ReadLine(SmtpReplyReader caller)
		{
			LineInfo[] array = this.ReadLines(caller, true);
			if (array != null && array.Length != 0)
			{
				return array[0];
			}
			return default(LineInfo);
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0007CE72 File Offset: 0x0007B072
		internal LineInfo[] ReadLines(SmtpReplyReader caller)
		{
			return this.ReadLines(caller, false);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0007CE7C File Offset: 0x0007B07C
		internal LineInfo[] ReadLines(SmtpReplyReader caller, bool oneLine)
		{
			if (caller != this.currentReader || this.readState == SmtpReplyReaderFactory.ReadState.Done)
			{
				return new LineInfo[0];
			}
			if (this.byteBuffer == null)
			{
				this.byteBuffer = new byte[256];
			}
			StringBuilder stringBuilder = new StringBuilder();
			ArrayList arrayList = new ArrayList();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (;;)
			{
				if (num2 == num3)
				{
					num3 = this.bufferedStream.Read(this.byteBuffer, 0, this.byteBuffer.Length);
					num2 = 0;
				}
				int num4 = this.ProcessRead(this.byteBuffer, num2, num3 - num2, true);
				if (num < 4)
				{
					int num5 = Math.Min(4 - num, num4);
					num += num5;
					num2 += num5;
					num4 -= num5;
					if (num4 == 0)
					{
						continue;
					}
				}
				stringBuilder.Append(Encoding.UTF8.GetString(this.byteBuffer, num2, num4));
				num2 += num4;
				if (this.readState == SmtpReplyReaderFactory.ReadState.Status0)
				{
					num = 0;
					arrayList.Add(new LineInfo(this.statusCode, stringBuilder.ToString(0, stringBuilder.Length - 2)));
					if (oneLine)
					{
						break;
					}
					stringBuilder = new StringBuilder();
				}
				else if (this.readState == SmtpReplyReaderFactory.ReadState.Done)
				{
					goto Block_7;
				}
			}
			this.bufferedStream.Push(this.byteBuffer, num2, num3 - num2);
			return (LineInfo[])arrayList.ToArray(typeof(LineInfo));
			Block_7:
			arrayList.Add(new LineInfo(this.statusCode, stringBuilder.ToString(0, stringBuilder.Length - 2)));
			this.bufferedStream.Push(this.byteBuffer, num2, num3 - num2);
			return (LineInfo[])arrayList.ToArray(typeof(LineInfo));
		}

		// Token: 0x0400185E RID: 6238
		private BufferedReadStream bufferedStream;

		// Token: 0x0400185F RID: 6239
		private byte[] byteBuffer;

		// Token: 0x04001860 RID: 6240
		private SmtpReplyReader currentReader;

		// Token: 0x04001861 RID: 6241
		private const int DefaultBufferSize = 256;

		// Token: 0x04001862 RID: 6242
		private SmtpReplyReaderFactory.ReadState readState;

		// Token: 0x04001863 RID: 6243
		private SmtpStatusCode statusCode;

		// Token: 0x020007A2 RID: 1954
		private enum ReadState
		{
			// Token: 0x040033BB RID: 13243
			Status0,
			// Token: 0x040033BC RID: 13244
			Status1,
			// Token: 0x040033BD RID: 13245
			Status2,
			// Token: 0x040033BE RID: 13246
			ContinueFlag,
			// Token: 0x040033BF RID: 13247
			ContinueCR,
			// Token: 0x040033C0 RID: 13248
			ContinueLF,
			// Token: 0x040033C1 RID: 13249
			LastCR,
			// Token: 0x040033C2 RID: 13250
			LastLF,
			// Token: 0x040033C3 RID: 13251
			Done
		}

		// Token: 0x020007A3 RID: 1955
		private class ReadLinesAsyncResult : LazyAsyncResult
		{
			// Token: 0x06004301 RID: 17153 RVA: 0x00118E87 File Offset: 0x00117087
			internal ReadLinesAsyncResult(SmtpReplyReaderFactory parent, AsyncCallback callback, object state)
				: base(null, state, callback)
			{
				this.parent = parent;
			}

			// Token: 0x06004302 RID: 17154 RVA: 0x00118E99 File Offset: 0x00117099
			internal ReadLinesAsyncResult(SmtpReplyReaderFactory parent, AsyncCallback callback, object state, bool oneLine)
				: base(null, state, callback)
			{
				this.oneLine = oneLine;
				this.parent = parent;
			}

			// Token: 0x06004303 RID: 17155 RVA: 0x00118EB4 File Offset: 0x001170B4
			internal void Read(SmtpReplyReader caller)
			{
				if (this.parent.currentReader != caller || this.parent.readState == SmtpReplyReaderFactory.ReadState.Done)
				{
					base.InvokeCallback();
					return;
				}
				if (this.parent.byteBuffer == null)
				{
					this.parent.byteBuffer = new byte[256];
				}
				this.builder = new StringBuilder();
				this.lines = new ArrayList();
				this.Read();
			}

			// Token: 0x06004304 RID: 17156 RVA: 0x00118F24 File Offset: 0x00117124
			internal static LineInfo[] End(IAsyncResult result)
			{
				SmtpReplyReaderFactory.ReadLinesAsyncResult readLinesAsyncResult = (SmtpReplyReaderFactory.ReadLinesAsyncResult)result;
				readLinesAsyncResult.InternalWaitForCompletion();
				return (LineInfo[])readLinesAsyncResult.lines.ToArray(typeof(LineInfo));
			}

			// Token: 0x06004305 RID: 17157 RVA: 0x00118F5C File Offset: 0x0011715C
			private void Read()
			{
				for (;;)
				{
					IAsyncResult asyncResult = this.parent.bufferedStream.BeginRead(this.parent.byteBuffer, 0, this.parent.byteBuffer.Length, SmtpReplyReaderFactory.ReadLinesAsyncResult.readCallback, this);
					if (!asyncResult.CompletedSynchronously)
					{
						break;
					}
					this.read = this.parent.bufferedStream.EndRead(asyncResult);
					if (!this.ProcessRead())
					{
						return;
					}
				}
			}

			// Token: 0x06004306 RID: 17158 RVA: 0x00118FC4 File Offset: 0x001171C4
			private static void ReadCallback(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					Exception ex = null;
					SmtpReplyReaderFactory.ReadLinesAsyncResult readLinesAsyncResult = (SmtpReplyReaderFactory.ReadLinesAsyncResult)result.AsyncState;
					try
					{
						readLinesAsyncResult.read = readLinesAsyncResult.parent.bufferedStream.EndRead(result);
						if (readLinesAsyncResult.ProcessRead())
						{
							readLinesAsyncResult.Read();
						}
					}
					catch (Exception ex2)
					{
						ex = ex2;
					}
					if (ex != null)
					{
						readLinesAsyncResult.InvokeCallback(ex);
					}
				}
			}

			// Token: 0x06004307 RID: 17159 RVA: 0x00119030 File Offset: 0x00117230
			private bool ProcessRead()
			{
				if (this.read == 0)
				{
					throw new IOException(SR.GetString("net_io_readfailure", new object[] { "net_io_connectionclosed" }));
				}
				int num = 0;
				while (num != this.read)
				{
					int num2 = this.parent.ProcessRead(this.parent.byteBuffer, num, this.read - num, true);
					if (this.statusRead < 4)
					{
						int num3 = Math.Min(4 - this.statusRead, num2);
						this.statusRead += num3;
						num += num3;
						num2 -= num3;
						if (num2 == 0)
						{
							continue;
						}
					}
					this.builder.Append(Encoding.UTF8.GetString(this.parent.byteBuffer, num, num2));
					num += num2;
					if (this.parent.readState == SmtpReplyReaderFactory.ReadState.Status0)
					{
						this.lines.Add(new LineInfo(this.parent.statusCode, this.builder.ToString(0, this.builder.Length - 2)));
						this.builder = new StringBuilder();
						this.statusRead = 0;
						if (this.oneLine)
						{
							this.parent.bufferedStream.Push(this.parent.byteBuffer, num, this.read - num);
							base.InvokeCallback();
							return false;
						}
					}
					else if (this.parent.readState == SmtpReplyReaderFactory.ReadState.Done)
					{
						this.lines.Add(new LineInfo(this.parent.statusCode, this.builder.ToString(0, this.builder.Length - 2)));
						this.parent.bufferedStream.Push(this.parent.byteBuffer, num, this.read - num);
						base.InvokeCallback();
						return false;
					}
				}
				return true;
			}

			// Token: 0x040033C4 RID: 13252
			private StringBuilder builder;

			// Token: 0x040033C5 RID: 13253
			private ArrayList lines;

			// Token: 0x040033C6 RID: 13254
			private SmtpReplyReaderFactory parent;

			// Token: 0x040033C7 RID: 13255
			private static AsyncCallback readCallback = new AsyncCallback(SmtpReplyReaderFactory.ReadLinesAsyncResult.ReadCallback);

			// Token: 0x040033C8 RID: 13256
			private int read;

			// Token: 0x040033C9 RID: 13257
			private int statusRead;

			// Token: 0x040033CA RID: 13258
			private bool oneLine;
		}
	}
}
