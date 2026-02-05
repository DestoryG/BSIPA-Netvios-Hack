using System;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x02000244 RID: 580
	internal class EncodedStreamFactory
	{
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x00071B3A File Offset: 0x0006FD3A
		internal static int DefaultMaxLineLength
		{
			get
			{
				return 70;
			}
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00071B3E File Offset: 0x0006FD3E
		internal IEncodableStream GetEncoder(TransferEncoding encoding, Stream stream)
		{
			if (encoding == TransferEncoding.Base64)
			{
				return new Base64Stream(stream, new Base64WriteStateInfo());
			}
			if (encoding == TransferEncoding.QuotedPrintable)
			{
				return new QuotedPrintableStream(stream, true);
			}
			if (encoding == TransferEncoding.SevenBit || encoding == TransferEncoding.EightBit)
			{
				return new EightBitStream(stream);
			}
			throw new NotSupportedException("Encoding Stream");
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x00071B74 File Offset: 0x0006FD74
		internal IEncodableStream GetEncoderForHeader(Encoding encoding, bool useBase64Encoding, int headerTextLength)
		{
			byte[] array = this.CreateHeader(encoding, useBase64Encoding);
			byte[] array2 = this.CreateFooter();
			WriteStateInfoBase writeStateInfoBase;
			if (useBase64Encoding)
			{
				writeStateInfoBase = new Base64WriteStateInfo(1024, array, array2, EncodedStreamFactory.DefaultMaxLineLength, headerTextLength);
				return new Base64Stream((Base64WriteStateInfo)writeStateInfoBase);
			}
			writeStateInfoBase = new WriteStateInfoBase(1024, array, array2, EncodedStreamFactory.DefaultMaxLineLength, headerTextLength);
			return new QEncodedStream(writeStateInfoBase);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00071BCC File Offset: 0x0006FDCC
		protected byte[] CreateHeader(Encoding encoding, bool useBase64Encoding)
		{
			string text = string.Format("=?{0}?{1}?", encoding.HeaderName, useBase64Encoding ? "B" : "Q");
			return Encoding.ASCII.GetBytes(text);
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x00071C04 File Offset: 0x0006FE04
		protected byte[] CreateFooter()
		{
			return new byte[] { 63, 61 };
		}

		// Token: 0x04001702 RID: 5890
		private const int defaultMaxLineLength = 70;

		// Token: 0x04001703 RID: 5891
		private const int initialBufferSize = 1024;
	}
}
