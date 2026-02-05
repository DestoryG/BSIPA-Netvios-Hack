using System;
using System.IO;
using System.Runtime.Serialization;

namespace System.Xml
{
	// Token: 0x0200003D RID: 61
	internal class MimeMessageReader
	{
		// Token: 0x060004F8 RID: 1272 RVA: 0x00017FD0 File Offset: 0x000161D0
		public MimeMessageReader(Stream stream)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			this.reader = new DelimittedStreamReader(stream);
			this.mimeHeaderReader = new MimeHeaderReader(this.reader.GetNextStream(MimeMessageReader.CRLFCRLF));
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001800D File Offset: 0x0001620D
		public Stream GetContentStream()
		{
			if (this.getContentStreamCalled)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("On MimeMessage, GetContentStream method is already called.")));
			}
			this.mimeHeaderReader.Close();
			Stream nextStream = this.reader.GetNextStream(null);
			this.getContentStreamCalled = true;
			return nextStream;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001804C File Offset: 0x0001624C
		public MimeHeaders ReadHeaders(int maxBuffer, ref int remaining)
		{
			MimeHeaders mimeHeaders = new MimeHeaders();
			while (this.mimeHeaderReader.Read(maxBuffer, ref remaining))
			{
				mimeHeaders.Add(this.mimeHeaderReader.Name, this.mimeHeaderReader.Value, ref remaining);
			}
			return mimeHeaders;
		}

		// Token: 0x0400020A RID: 522
		private static byte[] CRLFCRLF = new byte[] { 13, 10, 13, 10 };

		// Token: 0x0400020B RID: 523
		private bool getContentStreamCalled;

		// Token: 0x0400020C RID: 524
		private MimeHeaderReader mimeHeaderReader;

		// Token: 0x0400020D RID: 525
		private DelimittedStreamReader reader;
	}
}
