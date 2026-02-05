using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200003E RID: 62
	internal class MimeReader
	{
		// Token: 0x060004FC RID: 1276 RVA: 0x000180A8 File Offset: 0x000162A8
		public MimeReader(Stream stream, string boundary)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			if (boundary == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("boundary");
			}
			this.reader = new DelimittedStreamReader(stream);
			this.boundaryBytes = MimeWriter.GetBoundaryBytes(boundary);
			this.reader.Push(this.boundaryBytes, 0, 2);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001810E File Offset: 0x0001630E
		public void Close()
		{
			this.reader.Close();
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0001811C File Offset: 0x0001631C
		public string Preface
		{
			get
			{
				if (this.content == null)
				{
					Stream nextStream = this.reader.GetNextStream(this.boundaryBytes);
					this.content = new StreamReader(nextStream, Encoding.ASCII, false, 256).ReadToEnd();
					nextStream.Close();
					if (this.content == null)
					{
						this.content = string.Empty;
					}
				}
				return this.content;
			}
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001817E File Offset: 0x0001637E
		public Stream GetContentStream()
		{
			this.mimeHeaderReader.Close();
			return this.reader.GetNextStream(this.boundaryBytes);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001819C File Offset: 0x0001639C
		public bool ReadNextPart()
		{
			string preface = this.Preface;
			if (this.currentStream != null)
			{
				this.currentStream.Close();
				this.currentStream = null;
			}
			Stream nextStream = this.reader.GetNextStream(MimeReader.CRLFCRLF);
			if (nextStream == null)
			{
				return false;
			}
			if (this.BlockRead(nextStream, this.scratch, 0, 2) == 2)
			{
				if (this.scratch[0] == 13 && this.scratch[1] == 10)
				{
					if (this.mimeHeaderReader == null)
					{
						this.mimeHeaderReader = new MimeHeaderReader(nextStream);
					}
					else
					{
						this.mimeHeaderReader.Reset(nextStream);
					}
					return true;
				}
				if (this.scratch[0] == 45 && this.scratch[1] == 45 && (this.BlockRead(nextStream, this.scratch, 0, 2) < 2 || (this.scratch[0] == 13 && this.scratch[1] == 10)))
				{
					return false;
				}
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("MIME parts are truncated.")));
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001828C File Offset: 0x0001648C
		public MimeHeaders ReadHeaders(int maxBuffer, ref int remaining)
		{
			MimeHeaders mimeHeaders = new MimeHeaders();
			while (this.mimeHeaderReader.Read(maxBuffer, ref remaining))
			{
				mimeHeaders.Add(this.mimeHeaderReader.Name, this.mimeHeaderReader.Value, ref remaining);
			}
			return mimeHeaders;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x000182D0 File Offset: 0x000164D0
		private int BlockRead(Stream stream, byte[] buffer, int offset, int count)
		{
			int num = 0;
			do
			{
				int num2 = stream.Read(buffer, offset + num, count - num);
				if (num2 == 0)
				{
					break;
				}
				num += num2;
			}
			while (num < count);
			return num;
		}

		// Token: 0x0400020E RID: 526
		private static byte[] CRLFCRLF = new byte[] { 13, 10, 13, 10 };

		// Token: 0x0400020F RID: 527
		private byte[] boundaryBytes;

		// Token: 0x04000210 RID: 528
		private string content;

		// Token: 0x04000211 RID: 529
		private Stream currentStream;

		// Token: 0x04000212 RID: 530
		private MimeHeaderReader mimeHeaderReader;

		// Token: 0x04000213 RID: 531
		private DelimittedStreamReader reader;

		// Token: 0x04000214 RID: 532
		private byte[] scratch = new byte[2];
	}
}
