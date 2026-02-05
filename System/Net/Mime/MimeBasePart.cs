using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x02000249 RID: 585
	internal class MimeBasePart
	{
		// Token: 0x06001621 RID: 5665 RVA: 0x00072C8A File Offset: 0x00070E8A
		internal MimeBasePart()
		{
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00072C92 File Offset: 0x00070E92
		internal static bool ShouldUseBase64Encoding(Encoding encoding)
		{
			return encoding == Encoding.Unicode || encoding == Encoding.UTF8 || encoding == Encoding.UTF32 || encoding == Encoding.BigEndianUnicode;
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00072CB7 File Offset: 0x00070EB7
		internal static string EncodeHeaderValue(string value, Encoding encoding, bool base64Encoding)
		{
			return MimeBasePart.EncodeHeaderValue(value, encoding, base64Encoding, 0);
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00072CC4 File Offset: 0x00070EC4
		internal static string EncodeHeaderValue(string value, Encoding encoding, bool base64Encoding, int headerLength)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (MimeBasePart.IsAscii(value, false))
			{
				return value;
			}
			if (encoding == null)
			{
				encoding = Encoding.GetEncoding("utf-8");
			}
			EncodedStreamFactory encodedStreamFactory = new EncodedStreamFactory();
			IEncodableStream encoderForHeader = encodedStreamFactory.GetEncoderForHeader(encoding, base64Encoding, headerLength);
			byte[] bytes = encoding.GetBytes(value);
			encoderForHeader.EncodeBytes(bytes, 0, bytes.Length);
			return encoderForHeader.GetEncodedString();
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00072D1C File Offset: 0x00070F1C
		internal static string DecodeHeaderValue(string value)
		{
			if (value == null || value.Length == 0)
			{
				return string.Empty;
			}
			string text = string.Empty;
			string[] array = value.Split(new char[] { '\r', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text2 in array)
			{
				string[] array3 = text2.Split(new char[] { '?' });
				if (array3.Length != 5 || array3[0] != "=" || array3[4] != "=")
				{
					return value;
				}
				string text3 = array3[1];
				bool flag = array3[2] == "B";
				byte[] bytes = Encoding.ASCII.GetBytes(array3[3]);
				EncodedStreamFactory encodedStreamFactory = new EncodedStreamFactory();
				IEncodableStream encoderForHeader = encodedStreamFactory.GetEncoderForHeader(Encoding.GetEncoding(text3), flag, 0);
				int num = encoderForHeader.DecodeBytes(bytes, 0, bytes.Length);
				Encoding encoding = Encoding.GetEncoding(text3);
				text += encoding.GetString(bytes, 0, num);
			}
			return text;
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00072E20 File Offset: 0x00071020
		internal static Encoding DecodeEncoding(string value)
		{
			if (value == null || value.Length == 0)
			{
				return null;
			}
			string[] array = value.Split(new char[] { '?', '\r', '\n' });
			if (array.Length < 5 || array[0] != "=" || array[4] != "=")
			{
				return null;
			}
			string text = array[1];
			return Encoding.GetEncoding(text);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00072E84 File Offset: 0x00071084
		internal static bool IsAscii(string value, bool permitCROrLF)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			foreach (char c in value)
			{
				if (c > '\u007f')
				{
					return false;
				}
				if (!permitCROrLF && (c == '\r' || c == '\n'))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00072ED4 File Offset: 0x000710D4
		internal static bool IsAnsi(string value, bool permitCROrLF)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			foreach (char c in value)
			{
				if (c > 'ÿ')
				{
					return false;
				}
				if (!permitCROrLF && (c == '\r' || c == '\n'))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001629 RID: 5673 RVA: 0x00072F24 File Offset: 0x00071124
		// (set) Token: 0x0600162A RID: 5674 RVA: 0x00072F37 File Offset: 0x00071137
		internal string ContentID
		{
			get
			{
				return this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentID)];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.ContentID));
					return;
				}
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentID)] = value;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600162B RID: 5675 RVA: 0x00072F65 File Offset: 0x00071165
		// (set) Token: 0x0600162C RID: 5676 RVA: 0x00072F78 File Offset: 0x00071178
		internal string ContentLocation
		{
			get
			{
				return this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentLocation)];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.ContentLocation));
					return;
				}
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentLocation)] = value;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x0600162D RID: 5677 RVA: 0x00072FA8 File Offset: 0x000711A8
		internal NameValueCollection Headers
		{
			get
			{
				if (this.headers == null)
				{
					this.headers = new HeaderCollection();
				}
				if (this.contentType == null)
				{
					this.contentType = new ContentType();
				}
				this.contentType.PersistIfNeeded(this.headers, false);
				if (this.contentDisposition != null)
				{
					this.contentDisposition.PersistIfNeeded(this.headers, false);
				}
				return this.headers;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x0600162E RID: 5678 RVA: 0x0007300D File Offset: 0x0007120D
		// (set) Token: 0x0600162F RID: 5679 RVA: 0x00073028 File Offset: 0x00071228
		internal ContentType ContentType
		{
			get
			{
				if (this.contentType == null)
				{
					this.contentType = new ContentType();
				}
				return this.contentType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.contentType = value;
				this.contentType.PersistIfNeeded((HeaderCollection)this.Headers, true);
			}
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x00073058 File Offset: 0x00071258
		internal void PrepareHeaders(bool allowUnicode)
		{
			this.contentType.PersistIfNeeded((HeaderCollection)this.Headers, false);
			this.headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentType), this.contentType.Encode(allowUnicode));
			if (this.contentDisposition != null)
			{
				this.contentDisposition.PersistIfNeeded((HeaderCollection)this.Headers, false);
				this.headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.contentDisposition.Encode(allowUnicode));
			}
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x000730D5 File Offset: 0x000712D5
		internal virtual void Send(BaseWriter writer, bool allowUnicode)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x000730DC File Offset: 0x000712DC
		internal virtual IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, bool allowUnicode, object state)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x000730E4 File Offset: 0x000712E4
		internal void EndSend(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as MimeBasePart.MimePartAsyncResult;
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndSend" }));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			lazyAsyncResult.EndCalled = true;
			if (lazyAsyncResult.Result is Exception)
			{
				throw (Exception)lazyAsyncResult.Result;
			}
		}

		// Token: 0x0400171D RID: 5917
		protected ContentType contentType;

		// Token: 0x0400171E RID: 5918
		protected ContentDisposition contentDisposition;

		// Token: 0x0400171F RID: 5919
		private HeaderCollection headers;

		// Token: 0x04001720 RID: 5920
		internal const string defaultCharSet = "utf-8";

		// Token: 0x02000796 RID: 1942
		internal class MimePartAsyncResult : LazyAsyncResult
		{
			// Token: 0x060042D2 RID: 17106 RVA: 0x00117D30 File Offset: 0x00115F30
			internal MimePartAsyncResult(MimeBasePart part, object state, AsyncCallback callback)
				: base(part, state, callback)
			{
			}
		}
	}
}
