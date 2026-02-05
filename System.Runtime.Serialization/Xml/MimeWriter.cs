using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x0200004F RID: 79
	internal class MimeWriter
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x0001B170 File Offset: 0x00019370
		internal MimeWriter(Stream stream, string boundary)
		{
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
			}
			if (boundary == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("boundary");
			}
			this.stream = stream;
			this.boundaryBytes = MimeWriter.GetBoundaryBytes(boundary);
			this.state = MimeWriterState.Start;
			this.bufferedWrite = new BufferedWrite();
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001B1C4 File Offset: 0x000193C4
		internal static int GetHeaderSize(string name, string value, int maxSizeInBytes)
		{
			if (name == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("name");
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			int num = XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, 0, MimeGlobals.COLONSPACE.Length + MimeGlobals.CRLF.Length);
			num += XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, num, name.Length);
			return num + XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, num, value.Length);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001B228 File Offset: 0x00019428
		internal static byte[] GetBoundaryBytes(string boundary)
		{
			byte[] array = new byte[boundary.Length + MimeGlobals.BoundaryPrefix.Length];
			for (int i = 0; i < MimeGlobals.BoundaryPrefix.Length; i++)
			{
				array[i] = MimeGlobals.BoundaryPrefix[i];
			}
			Encoding.ASCII.GetBytes(boundary, 0, boundary.Length, array, MimeGlobals.BoundaryPrefix.Length);
			return array;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x0001B281 File Offset: 0x00019481
		internal MimeWriterState WriteState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001B289 File Offset: 0x00019489
		internal int GetBoundarySize()
		{
			return this.boundaryBytes.Length;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001B293 File Offset: 0x00019493
		internal void StartPreface()
		{
			if (this.state != MimeWriterState.Start)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("MIME writer is at invalid state for starting preface.", new object[] { this.state.ToString() })));
			}
			this.state = MimeWriterState.StartPreface;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001B2D4 File Offset: 0x000194D4
		internal void StartPart()
		{
			MimeWriterState mimeWriterState = this.state;
			if (mimeWriterState == MimeWriterState.StartPart || mimeWriterState == MimeWriterState.Closed)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("MIME writer is at invalid state for starting a part.", new object[] { this.state.ToString() })));
			}
			this.state = MimeWriterState.StartPart;
			if (this.contentStream != null)
			{
				this.contentStream.Flush();
				this.contentStream = null;
			}
			this.bufferedWrite.Write(this.boundaryBytes);
			this.bufferedWrite.Write(MimeGlobals.CRLF);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001B364 File Offset: 0x00019564
		internal void Close()
		{
			MimeWriterState mimeWriterState = this.state;
			if (mimeWriterState == MimeWriterState.Closed)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("MIME writer is at invalid state for closing.", new object[] { this.state.ToString() })));
			}
			this.state = MimeWriterState.Closed;
			if (this.contentStream != null)
			{
				this.contentStream.Flush();
				this.contentStream = null;
			}
			this.bufferedWrite.Write(this.boundaryBytes);
			this.bufferedWrite.Write(MimeGlobals.DASHDASH);
			this.bufferedWrite.Write(MimeGlobals.CRLF);
			this.Flush();
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001B403 File Offset: 0x00019603
		private void Flush()
		{
			if (this.bufferedWrite.Length > 0)
			{
				this.stream.Write(this.bufferedWrite.GetBuffer(), 0, this.bufferedWrite.Length);
				this.bufferedWrite.Reset();
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001B440 File Offset: 0x00019640
		internal void WriteHeader(string name, string value)
		{
			if (name == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("name");
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			MimeWriterState mimeWriterState = this.state;
			if (mimeWriterState == MimeWriterState.Start || mimeWriterState - MimeWriterState.Content <= 1)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("MIME writer is at invalid state for header.", new object[] { this.state.ToString() })));
			}
			this.state = MimeWriterState.Header;
			this.bufferedWrite.Write(name);
			this.bufferedWrite.Write(MimeGlobals.COLONSPACE);
			this.bufferedWrite.Write(value);
			this.bufferedWrite.Write(MimeGlobals.CRLF);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001B4E8 File Offset: 0x000196E8
		internal Stream GetContentStream()
		{
			MimeWriterState mimeWriterState = this.state;
			if (mimeWriterState == MimeWriterState.Start || mimeWriterState - MimeWriterState.Content <= 1)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("MIME writer is at invalid state for content.", new object[] { this.state.ToString() })));
			}
			this.state = MimeWriterState.Content;
			this.bufferedWrite.Write(MimeGlobals.CRLF);
			this.Flush();
			this.contentStream = this.stream;
			return this.contentStream;
		}

		// Token: 0x0400027B RID: 635
		private Stream stream;

		// Token: 0x0400027C RID: 636
		private byte[] boundaryBytes;

		// Token: 0x0400027D RID: 637
		private MimeWriterState state;

		// Token: 0x0400027E RID: 638
		private BufferedWrite bufferedWrite;

		// Token: 0x0400027F RID: 639
		private Stream contentStream;
	}
}
