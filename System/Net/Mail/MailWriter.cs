using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x02000271 RID: 625
	internal class MailWriter : BaseWriter
	{
		// Token: 0x06001784 RID: 6020 RVA: 0x00077E70 File Offset: 0x00076070
		internal MailWriter(Stream stream)
			: base(stream, true)
		{
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00077E7C File Offset: 0x0007607C
		internal override void WriteHeaders(NameValueCollection headers, bool allowUnicode)
		{
			if (headers == null)
			{
				throw new ArgumentNullException("headers");
			}
			foreach (object obj in headers)
			{
				string text = (string)obj;
				string[] values = headers.GetValues(text);
				foreach (string text2 in values)
				{
					base.WriteHeader(text, text2, allowUnicode);
				}
			}
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00077F08 File Offset: 0x00076108
		internal override void Close()
		{
			this.bufferBuilder.Append(BaseWriter.CRLF);
			base.Flush(null);
			this.stream.Close();
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00077F2C File Offset: 0x0007612C
		protected override void OnClose(object sender, EventArgs args)
		{
			this.contentStream.Flush();
			this.contentStream = null;
		}
	}
}
