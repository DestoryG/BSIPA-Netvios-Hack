using System;
using System.IO;
using System.Net.Mail;

namespace System.Net.Mime
{
	// Token: 0x0200024C RID: 588
	internal class MimePart : MimeBasePart, IDisposable
	{
		// Token: 0x06001642 RID: 5698 RVA: 0x00073622 File Offset: 0x00071822
		internal MimePart()
		{
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x0007362A File Offset: 0x0007182A
		public void Dispose()
		{
			if (this.stream != null)
			{
				this.stream.Close();
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001644 RID: 5700 RVA: 0x0007363F File Offset: 0x0007183F
		internal Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001645 RID: 5701 RVA: 0x00073647 File Offset: 0x00071847
		// (set) Token: 0x06001646 RID: 5702 RVA: 0x0007364F File Offset: 0x0007184F
		internal ContentDisposition ContentDisposition
		{
			get
			{
				return this.contentDisposition;
			}
			set
			{
				this.contentDisposition = value;
				if (value == null)
				{
					((HeaderCollection)base.Headers).InternalRemove(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition));
					return;
				}
				this.contentDisposition.PersistIfNeeded((HeaderCollection)base.Headers, true);
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001647 RID: 5703 RVA: 0x0007368C File Offset: 0x0007188C
		// (set) Token: 0x06001648 RID: 5704 RVA: 0x000736EC File Offset: 0x000718EC
		internal TransferEncoding TransferEncoding
		{
			get
			{
				string text = base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)];
				if (text.Equals("base64", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.Base64;
				}
				if (text.Equals("quoted-printable", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.QuotedPrintable;
				}
				if (text.Equals("7bit", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.SevenBit;
				}
				if (text.Equals("8bit", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.EightBit;
				}
				return TransferEncoding.Unknown;
			}
			set
			{
				if (value == TransferEncoding.Base64)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "base64";
					return;
				}
				if (value == TransferEncoding.QuotedPrintable)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "quoted-printable";
					return;
				}
				if (value == TransferEncoding.SevenBit)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "7bit";
					return;
				}
				if (value == TransferEncoding.EightBit)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "8bit";
					return;
				}
				throw new NotSupportedException(SR.GetString("MimeTransferEncodingNotSupported", new object[] { value }));
			}
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x00073784 File Offset: 0x00071984
		internal void SetContent(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.streamSet)
			{
				this.stream.Close();
				this.stream = null;
				this.streamSet = false;
			}
			this.stream = stream;
			this.streamSet = true;
			this.streamUsedOnce = false;
			this.TransferEncoding = TransferEncoding.Base64;
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x000737DC File Offset: 0x000719DC
		internal void SetContent(Stream stream, string name, string mimeType)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (mimeType != null && mimeType != string.Empty)
			{
				this.contentType = new ContentType(mimeType);
			}
			if (name != null && name != string.Empty)
			{
				base.ContentType.Name = name;
			}
			this.SetContent(stream);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00073836 File Offset: 0x00071A36
		internal void SetContent(Stream stream, ContentType contentType)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.contentType = contentType;
			this.SetContent(stream);
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00073854 File Offset: 0x00071A54
		internal void Complete(IAsyncResult result, Exception e)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			if (mimePartContext.completed)
			{
				throw e;
			}
			try
			{
				if (mimePartContext.outputStream != null)
				{
					mimePartContext.outputStream.Close();
				}
			}
			catch (Exception ex)
			{
				if (e == null)
				{
					e = ex;
				}
			}
			mimePartContext.completed = true;
			mimePartContext.result.InvokeCallback(e);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000738BC File Offset: 0x00071ABC
		internal void ReadCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.ReadCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00073908 File Offset: 0x00071B08
		internal void ReadCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			mimePartContext.bytesLeft = this.Stream.EndRead(result);
			if (mimePartContext.bytesLeft > 0)
			{
				IAsyncResult asyncResult = mimePartContext.outputStream.BeginWrite(mimePartContext.buffer, 0, mimePartContext.bytesLeft, this.writeCallback, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.WriteCallbackHandler(asyncResult);
					return;
				}
			}
			else
			{
				this.Complete(result, null);
			}
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00073974 File Offset: 0x00071B74
		internal void WriteCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.WriteCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x000739C0 File Offset: 0x00071BC0
		internal void WriteCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			mimePartContext.outputStream.EndWrite(result);
			IAsyncResult asyncResult = this.Stream.BeginRead(mimePartContext.buffer, 0, mimePartContext.buffer.Length, this.readCallback, mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ReadCallbackHandler(asyncResult);
			}
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00073A18 File Offset: 0x00071C18
		internal Stream GetEncodedStream(Stream stream)
		{
			Stream stream2 = stream;
			if (this.TransferEncoding == TransferEncoding.Base64)
			{
				stream2 = new Base64Stream(stream2, new Base64WriteStateInfo());
			}
			else if (this.TransferEncoding == TransferEncoding.QuotedPrintable)
			{
				stream2 = new QuotedPrintableStream(stream2, true);
			}
			else if (this.TransferEncoding == TransferEncoding.SevenBit || this.TransferEncoding == TransferEncoding.EightBit)
			{
				stream2 = new EightBitStream(stream2);
			}
			return stream2;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00073A6C File Offset: 0x00071C6C
		internal void ContentStreamCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			Stream stream = mimePartContext.writer.EndGetContentStream(result);
			mimePartContext.outputStream = this.GetEncodedStream(stream);
			this.readCallback = new AsyncCallback(this.ReadCallback);
			this.writeCallback = new AsyncCallback(this.WriteCallback);
			IAsyncResult asyncResult = this.Stream.BeginRead(mimePartContext.buffer, 0, mimePartContext.buffer.Length, this.readCallback, mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ReadCallbackHandler(asyncResult);
			}
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00073AF4 File Offset: 0x00071CF4
		internal void ContentStreamCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.ContentStreamCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00073B40 File Offset: 0x00071D40
		internal override IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, bool allowUnicode, object state)
		{
			base.PrepareHeaders(allowUnicode);
			writer.WriteHeaders(base.Headers, allowUnicode);
			MimeBasePart.MimePartAsyncResult mimePartAsyncResult = new MimeBasePart.MimePartAsyncResult(this, state, callback);
			MimePart.MimePartContext mimePartContext = new MimePart.MimePartContext(writer, mimePartAsyncResult);
			this.ResetStream();
			this.streamUsedOnce = true;
			IAsyncResult asyncResult = writer.BeginGetContentStream(new AsyncCallback(this.ContentStreamCallback), mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ContentStreamCallbackHandler(asyncResult);
			}
			return mimePartAsyncResult;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00073BA4 File Offset: 0x00071DA4
		internal override void Send(BaseWriter writer, bool allowUnicode)
		{
			if (this.Stream != null)
			{
				byte[] array = new byte[17408];
				base.PrepareHeaders(allowUnicode);
				writer.WriteHeaders(base.Headers, allowUnicode);
				Stream stream = writer.GetContentStream();
				stream = this.GetEncodedStream(stream);
				this.ResetStream();
				this.streamUsedOnce = true;
				int num;
				while ((num = this.Stream.Read(array, 0, 17408)) > 0)
				{
					stream.Write(array, 0, num);
				}
				stream.Close();
			}
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00073C1C File Offset: 0x00071E1C
		internal void ResetStream()
		{
			if (!this.streamUsedOnce)
			{
				return;
			}
			if (this.Stream.CanSeek)
			{
				this.Stream.Seek(0L, SeekOrigin.Begin);
				this.streamUsedOnce = false;
				return;
			}
			throw new InvalidOperationException(SR.GetString("MimePartCantResetStream"));
		}

		// Token: 0x0400172B RID: 5931
		private Stream stream;

		// Token: 0x0400172C RID: 5932
		private bool streamSet;

		// Token: 0x0400172D RID: 5933
		private bool streamUsedOnce;

		// Token: 0x0400172E RID: 5934
		private AsyncCallback readCallback;

		// Token: 0x0400172F RID: 5935
		private AsyncCallback writeCallback;

		// Token: 0x04001730 RID: 5936
		private const int maxBufferSize = 17408;

		// Token: 0x02000798 RID: 1944
		internal class MimePartContext
		{
			// Token: 0x060042D4 RID: 17108 RVA: 0x00117D5F File Offset: 0x00115F5F
			internal MimePartContext(BaseWriter writer, LazyAsyncResult result)
			{
				this.writer = writer;
				this.result = result;
				this.buffer = new byte[17408];
			}

			// Token: 0x04003387 RID: 13191
			internal Stream outputStream;

			// Token: 0x04003388 RID: 13192
			internal LazyAsyncResult result;

			// Token: 0x04003389 RID: 13193
			internal int bytesLeft;

			// Token: 0x0400338A RID: 13194
			internal BaseWriter writer;

			// Token: 0x0400338B RID: 13195
			internal byte[] buffer;

			// Token: 0x0400338C RID: 13196
			internal bool completed;

			// Token: 0x0400338D RID: 13197
			internal bool completedSynchronously = true;
		}
	}
}
