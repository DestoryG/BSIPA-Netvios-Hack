using System;
using System.IO;

namespace Google.Protobuf
{
	// Token: 0x02000019 RID: 25
	public sealed class InvalidProtocolBufferException : IOException
	{
		// Token: 0x0600014C RID: 332 RVA: 0x00006303 File Offset: 0x00004503
		internal InvalidProtocolBufferException(string message)
			: base(message)
		{
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000630C File Offset: 0x0000450C
		internal InvalidProtocolBufferException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00006316 File Offset: 0x00004516
		internal static InvalidProtocolBufferException MoreDataAvailable()
		{
			return new InvalidProtocolBufferException("Completed reading a message while more data was available in the stream.");
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006322 File Offset: 0x00004522
		internal static InvalidProtocolBufferException TruncatedMessage()
		{
			return new InvalidProtocolBufferException("While parsing a protocol message, the input ended unexpectedly in the middle of a field.  This could mean either that the input has been truncated or that an embedded message misreported its own length.");
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000632E File Offset: 0x0000452E
		internal static InvalidProtocolBufferException NegativeSize()
		{
			return new InvalidProtocolBufferException("CodedInputStream encountered an embedded string or message which claimed to have negative size.");
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000633A File Offset: 0x0000453A
		internal static InvalidProtocolBufferException MalformedVarint()
		{
			return new InvalidProtocolBufferException("CodedInputStream encountered a malformed varint.");
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006346 File Offset: 0x00004546
		internal static InvalidProtocolBufferException InvalidTag()
		{
			return new InvalidProtocolBufferException("Protocol message contained an invalid tag (zero).");
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006352 File Offset: 0x00004552
		internal static InvalidProtocolBufferException InvalidWireType()
		{
			return new InvalidProtocolBufferException("Protocol message contained a tag with an invalid wire type.");
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000635E File Offset: 0x0000455E
		internal static InvalidProtocolBufferException InvalidBase64(Exception innerException)
		{
			return new InvalidProtocolBufferException("Invalid base64 data", innerException);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000636B File Offset: 0x0000456B
		internal static InvalidProtocolBufferException InvalidEndTag()
		{
			return new InvalidProtocolBufferException("Protocol message end-group tag did not match expected tag.");
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006377 File Offset: 0x00004577
		internal static InvalidProtocolBufferException RecursionLimitExceeded()
		{
			return new InvalidProtocolBufferException("Protocol message had too many levels of nesting.  May be malicious.  Use CodedInputStream.SetRecursionLimit() to increase the depth limit.");
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006383 File Offset: 0x00004583
		internal static InvalidProtocolBufferException JsonRecursionLimitExceeded()
		{
			return new InvalidProtocolBufferException("Protocol message had too many levels of nesting.  May be malicious.  Use JsonParser.Settings to increase the depth limit.");
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000638F File Offset: 0x0000458F
		internal static InvalidProtocolBufferException SizeLimitExceeded()
		{
			return new InvalidProtocolBufferException("Protocol message was too large.  May be malicious.  Use CodedInputStream.SetSizeLimit() to increase the size limit.");
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000639B File Offset: 0x0000459B
		internal static InvalidProtocolBufferException InvalidMessageStreamTag()
		{
			return new InvalidProtocolBufferException("Stream of protocol messages had invalid tag. Expected tag is length-delimited field 1.");
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000063A7 File Offset: 0x000045A7
		internal static InvalidProtocolBufferException MissingFields()
		{
			return new InvalidProtocolBufferException("Message was missing required fields");
		}
	}
}
