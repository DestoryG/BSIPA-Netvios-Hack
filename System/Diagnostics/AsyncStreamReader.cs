using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x020004BF RID: 1215
	internal class AsyncStreamReader : IDisposable
	{
		// Token: 0x06002D56 RID: 11606 RVA: 0x000CC084 File Offset: 0x000CA284
		internal AsyncStreamReader(Process process, Stream stream, UserCallBack callback, Encoding encoding)
			: this(process, stream, callback, encoding, 1024)
		{
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000CC096 File Offset: 0x000CA296
		internal AsyncStreamReader(Process process, Stream stream, UserCallBack callback, Encoding encoding, int bufferSize)
		{
			this.Init(process, stream, callback, encoding, bufferSize);
			this.messageQueue = new Queue();
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000CC0B8 File Offset: 0x000CA2B8
		private void Init(Process process, Stream stream, UserCallBack callback, Encoding encoding, int bufferSize)
		{
			this.process = process;
			this.stream = stream;
			this.encoding = encoding;
			this.userCallBack = callback;
			this.decoder = encoding.GetDecoder();
			if (bufferSize < 128)
			{
				bufferSize = 128;
			}
			this.byteBuffer = new byte[bufferSize];
			this._maxCharsPerBuffer = encoding.GetMaxCharCount(bufferSize);
			this.charBuffer = new char[this._maxCharsPerBuffer];
			this.cancelOperation = false;
			this.eofEvent = new ManualResetEvent(false);
			this.sb = null;
			this.bLastCarriageReturn = false;
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x000CC14D File Offset: 0x000CA34D
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000CC156 File Offset: 0x000CA356
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000CC168 File Offset: 0x000CA368
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.stream != null)
			{
				this.stream.Close();
			}
			if (this.stream != null)
			{
				this.stream = null;
				this.encoding = null;
				this.decoder = null;
				this.byteBuffer = null;
				this.charBuffer = null;
			}
			if (this.eofEvent != null)
			{
				this.eofEvent.Close();
				this.eofEvent = null;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06002D5C RID: 11612 RVA: 0x000CC1D0 File Offset: 0x000CA3D0
		public virtual Encoding CurrentEncoding
		{
			get
			{
				return this.encoding;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06002D5D RID: 11613 RVA: 0x000CC1D8 File Offset: 0x000CA3D8
		public virtual Stream BaseStream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000CC1E0 File Offset: 0x000CA3E0
		internal void BeginReadLine()
		{
			if (this.cancelOperation)
			{
				this.cancelOperation = false;
			}
			if (this.sb == null)
			{
				this.sb = new StringBuilder(1024);
				this.stream.BeginRead(this.byteBuffer, 0, this.byteBuffer.Length, new AsyncCallback(this.ReadBuffer), null);
				return;
			}
			this.FlushMessageQueue();
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x000CC243 File Offset: 0x000CA443
		internal void CancelOperation()
		{
			this.cancelOperation = true;
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x000CC24C File Offset: 0x000CA44C
		private void ReadBuffer(IAsyncResult ar)
		{
			int num;
			try
			{
				num = this.stream.EndRead(ar);
			}
			catch (IOException)
			{
				num = 0;
			}
			catch (OperationCanceledException)
			{
				num = 0;
			}
			if (num == 0)
			{
				Queue queue = this.messageQueue;
				lock (queue)
				{
					if (this.sb.Length != 0)
					{
						this.messageQueue.Enqueue(this.sb.ToString());
						this.sb.Length = 0;
					}
					this.messageQueue.Enqueue(null);
				}
				try
				{
					this.FlushMessageQueue();
					return;
				}
				finally
				{
					this.eofEvent.Set();
				}
			}
			int chars = this.decoder.GetChars(this.byteBuffer, 0, num, this.charBuffer, 0);
			this.sb.Append(this.charBuffer, 0, chars);
			this.GetLinesFromStringBuilder();
			this.stream.BeginRead(this.byteBuffer, 0, this.byteBuffer.Length, new AsyncCallback(this.ReadBuffer), null);
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x000CC374 File Offset: 0x000CA574
		private void GetLinesFromStringBuilder()
		{
			int i = this.currentLinePos;
			int num = 0;
			int length = this.sb.Length;
			if (this.bLastCarriageReturn && length > 0 && this.sb[0] == '\n')
			{
				i = 1;
				num = 1;
				this.bLastCarriageReturn = false;
			}
			while (i < length)
			{
				char c = this.sb[i];
				if (c == '\r' || c == '\n')
				{
					string text = this.sb.ToString(num, i - num);
					num = i + 1;
					if (c == '\r' && num < length && this.sb[num] == '\n')
					{
						num++;
						i++;
					}
					Queue queue = this.messageQueue;
					lock (queue)
					{
						this.messageQueue.Enqueue(text);
					}
				}
				i++;
			}
			if (length > 0 && this.sb[length - 1] == '\r')
			{
				this.bLastCarriageReturn = true;
			}
			if (num < length)
			{
				if (num == 0)
				{
					this.currentLinePos = i;
				}
				else
				{
					this.sb.Remove(0, num);
					this.currentLinePos = 0;
				}
			}
			else
			{
				this.sb.Length = 0;
				this.currentLinePos = 0;
			}
			this.FlushMessageQueue();
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x000CC4C0 File Offset: 0x000CA6C0
		private void FlushMessageQueue()
		{
			while (this.messageQueue.Count > 0)
			{
				Queue queue = this.messageQueue;
				lock (queue)
				{
					if (this.messageQueue.Count > 0)
					{
						string text = (string)this.messageQueue.Dequeue();
						if (!this.cancelOperation)
						{
							this.userCallBack(text);
						}
					}
					continue;
				}
				break;
			}
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x000CC53C File Offset: 0x000CA73C
		internal void WaitUtilEOF()
		{
			if (this.eofEvent != null)
			{
				this.eofEvent.WaitOne();
				this.eofEvent.Close();
				this.eofEvent = null;
			}
		}

		// Token: 0x0400270A RID: 9994
		internal const int DefaultBufferSize = 1024;

		// Token: 0x0400270B RID: 9995
		private const int MinBufferSize = 128;

		// Token: 0x0400270C RID: 9996
		private Stream stream;

		// Token: 0x0400270D RID: 9997
		private Encoding encoding;

		// Token: 0x0400270E RID: 9998
		private Decoder decoder;

		// Token: 0x0400270F RID: 9999
		private byte[] byteBuffer;

		// Token: 0x04002710 RID: 10000
		private char[] charBuffer;

		// Token: 0x04002711 RID: 10001
		private int _maxCharsPerBuffer;

		// Token: 0x04002712 RID: 10002
		private Process process;

		// Token: 0x04002713 RID: 10003
		private UserCallBack userCallBack;

		// Token: 0x04002714 RID: 10004
		private bool cancelOperation;

		// Token: 0x04002715 RID: 10005
		private ManualResetEvent eofEvent;

		// Token: 0x04002716 RID: 10006
		private Queue messageQueue;

		// Token: 0x04002717 RID: 10007
		private StringBuilder sb;

		// Token: 0x04002718 RID: 10008
		private bool bLastCarriageReturn;

		// Token: 0x04002719 RID: 10009
		private int currentLinePos;
	}
}
