using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;

namespace System.Net.Mime
{
	// Token: 0x0200023E RID: 574
	internal abstract class BaseWriter
	{
		// Token: 0x060015B8 RID: 5560 RVA: 0x00070804 File Offset: 0x0006EA04
		protected BaseWriter(Stream stream, bool shouldEncodeLeadingDots)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.stream = stream;
			this.shouldEncodeLeadingDots = shouldEncodeLeadingDots;
			this.onCloseHandler = new EventHandler(this.OnClose);
			this.bufferBuilder = new BufferBuilder();
			this.lineLength = BaseWriter.DefaultLineLength;
		}

		// Token: 0x060015B9 RID: 5561
		internal abstract void WriteHeaders(NameValueCollection headers, bool allowUnicode);

		// Token: 0x060015BA RID: 5562 RVA: 0x0007085C File Offset: 0x0006EA5C
		internal void WriteHeader(string name, string value, bool allowUnicode)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.isInContent)
			{
				throw new InvalidOperationException(SR.GetString("MailWriterIsInContent"));
			}
			this.CheckBoundary();
			this.bufferBuilder.Append(name);
			this.bufferBuilder.Append(": ");
			this.WriteAndFold(value, name.Length + 2, allowUnicode);
			this.bufferBuilder.Append(BaseWriter.CRLF);
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x000708E0 File Offset: 0x0006EAE0
		private void WriteAndFold(string value, int charsAlreadyOnLine, bool allowUnicode)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < value.Length; i++)
			{
				if (MailBnfHelper.IsFWSAt(value, i))
				{
					i += 2;
					this.bufferBuilder.Append(value, num2, i - num2, allowUnicode);
					num2 = i;
					num = i;
					charsAlreadyOnLine = 0;
				}
				else if (i - num2 > this.lineLength - charsAlreadyOnLine && num != num2)
				{
					this.bufferBuilder.Append(value, num2, num - num2, allowUnicode);
					this.bufferBuilder.Append(BaseWriter.CRLF);
					num2 = num;
					charsAlreadyOnLine = 0;
				}
				else if (value[i] == MailBnfHelper.Space || value[i] == MailBnfHelper.Tab)
				{
					num = i;
				}
			}
			if (value.Length - num2 > 0)
			{
				this.bufferBuilder.Append(value, num2, value.Length - num2, allowUnicode);
			}
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x000709A7 File Offset: 0x0006EBA7
		internal Stream GetContentStream()
		{
			return this.GetContentStream(null);
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x000709B0 File Offset: 0x0006EBB0
		private Stream GetContentStream(MultiAsyncResult multiResult)
		{
			if (this.isInContent)
			{
				throw new InvalidOperationException(SR.GetString("MailWriterIsInContent"));
			}
			this.isInContent = true;
			this.CheckBoundary();
			this.bufferBuilder.Append(BaseWriter.CRLF);
			this.Flush(multiResult);
			Stream stream = new EightBitStream(this.stream, this.shouldEncodeLeadingDots);
			ClosableStream closableStream = new ClosableStream(stream, this.onCloseHandler);
			this.contentStream = closableStream;
			return closableStream;
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x00070A20 File Offset: 0x0006EC20
		internal IAsyncResult BeginGetContentStream(AsyncCallback callback, object state)
		{
			MultiAsyncResult multiAsyncResult = new MultiAsyncResult(this, callback, state);
			Stream stream = this.GetContentStream(multiAsyncResult);
			if (!(multiAsyncResult.Result is Exception))
			{
				multiAsyncResult.Result = stream;
			}
			multiAsyncResult.CompleteSequence();
			return multiAsyncResult;
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x00070A5C File Offset: 0x0006EC5C
		internal Stream EndGetContentStream(IAsyncResult result)
		{
			object obj = MultiAsyncResult.End(result);
			if (obj is Exception)
			{
				throw (Exception)obj;
			}
			return (Stream)obj;
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x00070A88 File Offset: 0x0006EC88
		protected void Flush(MultiAsyncResult multiResult)
		{
			if (this.bufferBuilder.Length > 0)
			{
				if (multiResult != null)
				{
					multiResult.Enter();
					IAsyncResult asyncResult = this.stream.BeginWrite(this.bufferBuilder.GetBuffer(), 0, this.bufferBuilder.Length, BaseWriter.onWrite, multiResult);
					if (asyncResult.CompletedSynchronously)
					{
						this.stream.EndWrite(asyncResult);
						multiResult.Leave();
					}
				}
				else
				{
					this.stream.Write(this.bufferBuilder.GetBuffer(), 0, this.bufferBuilder.Length);
				}
				this.bufferBuilder.Reset();
			}
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x00070B20 File Offset: 0x0006ED20
		protected static void OnWrite(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result.AsyncState;
				BaseWriter baseWriter = (BaseWriter)multiAsyncResult.Context;
				try
				{
					baseWriter.stream.EndWrite(result);
					multiAsyncResult.Leave();
				}
				catch (Exception ex)
				{
					multiAsyncResult.Leave(ex);
				}
			}
		}

		// Token: 0x060015C2 RID: 5570
		internal abstract void Close();

		// Token: 0x060015C3 RID: 5571
		protected abstract void OnClose(object sender, EventArgs args);

		// Token: 0x060015C4 RID: 5572 RVA: 0x00070B7C File Offset: 0x0006ED7C
		protected virtual void CheckBoundary()
		{
		}

		// Token: 0x040016D7 RID: 5847
		private static int DefaultLineLength = 76;

		// Token: 0x040016D8 RID: 5848
		private static AsyncCallback onWrite = new AsyncCallback(BaseWriter.OnWrite);

		// Token: 0x040016D9 RID: 5849
		protected static byte[] CRLF = new byte[] { 13, 10 };

		// Token: 0x040016DA RID: 5850
		protected BufferBuilder bufferBuilder;

		// Token: 0x040016DB RID: 5851
		protected Stream contentStream;

		// Token: 0x040016DC RID: 5852
		protected bool isInContent;

		// Token: 0x040016DD RID: 5853
		protected Stream stream;

		// Token: 0x040016DE RID: 5854
		private int lineLength;

		// Token: 0x040016DF RID: 5855
		private EventHandler onCloseHandler;

		// Token: 0x040016E0 RID: 5856
		private bool shouldEncodeLeadingDots;
	}
}
