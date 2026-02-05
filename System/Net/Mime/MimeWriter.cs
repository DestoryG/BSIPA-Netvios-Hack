using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x0200024D RID: 589
	internal class MimeWriter : BaseWriter
	{
		// Token: 0x06001657 RID: 5719 RVA: 0x00073C5A File Offset: 0x00071E5A
		internal MimeWriter(Stream stream, string boundary)
			: base(stream, false)
		{
			if (boundary == null)
			{
				throw new ArgumentNullException("boundary");
			}
			this.boundaryBytes = Encoding.ASCII.GetBytes(boundary);
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00073C8C File Offset: 0x00071E8C
		internal override void WriteHeaders(NameValueCollection headers, bool allowUnicode)
		{
			if (headers == null)
			{
				throw new ArgumentNullException("headers");
			}
			foreach (object obj in headers)
			{
				string text = (string)obj;
				base.WriteHeader(text, headers[text], allowUnicode);
			}
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00073CF8 File Offset: 0x00071EF8
		internal IAsyncResult BeginClose(AsyncCallback callback, object state)
		{
			MultiAsyncResult multiAsyncResult = new MultiAsyncResult(this, callback, state);
			this.Close(multiAsyncResult);
			multiAsyncResult.CompleteSequence();
			return multiAsyncResult;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00073D1C File Offset: 0x00071F1C
		internal void EndClose(IAsyncResult result)
		{
			MultiAsyncResult.End(result);
			this.stream.Close();
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00073D30 File Offset: 0x00071F30
		internal override void Close()
		{
			this.Close(null);
			this.stream.Close();
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00073D44 File Offset: 0x00071F44
		private void Close(MultiAsyncResult multiResult)
		{
			this.bufferBuilder.Append(BaseWriter.CRLF);
			this.bufferBuilder.Append(MimeWriter.DASHDASH);
			this.bufferBuilder.Append(this.boundaryBytes);
			this.bufferBuilder.Append(MimeWriter.DASHDASH);
			this.bufferBuilder.Append(BaseWriter.CRLF);
			base.Flush(multiResult);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00073DA9 File Offset: 0x00071FA9
		protected override void OnClose(object sender, EventArgs args)
		{
			if (this.contentStream != sender)
			{
				return;
			}
			this.contentStream.Flush();
			this.contentStream = null;
			this.writeBoundary = true;
			this.isInContent = false;
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00073DD8 File Offset: 0x00071FD8
		protected override void CheckBoundary()
		{
			if (this.writeBoundary)
			{
				this.bufferBuilder.Append(BaseWriter.CRLF);
				this.bufferBuilder.Append(MimeWriter.DASHDASH);
				this.bufferBuilder.Append(this.boundaryBytes);
				this.bufferBuilder.Append(BaseWriter.CRLF);
				this.writeBoundary = false;
			}
		}

		// Token: 0x04001731 RID: 5937
		private static byte[] DASHDASH = new byte[] { 45, 45 };

		// Token: 0x04001732 RID: 5938
		private byte[] boundaryBytes;

		// Token: 0x04001733 RID: 5939
		private bool writeBoundary = true;
	}
}
