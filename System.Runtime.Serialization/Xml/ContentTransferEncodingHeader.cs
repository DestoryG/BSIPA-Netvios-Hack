using System;

namespace System.Xml
{
	// Token: 0x02000044 RID: 68
	internal class ContentTransferEncodingHeader : MimeHeader
	{
		// Token: 0x0600051E RID: 1310 RVA: 0x00018B54 File Offset: 0x00016D54
		public ContentTransferEncodingHeader(string value)
			: base("content-transfer-encoding", value.ToLowerInvariant())
		{
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00018B67 File Offset: 0x00016D67
		public ContentTransferEncodingHeader(ContentTransferEncoding contentTransferEncoding, string value)
			: base("content-transfer-encoding", null)
		{
			this.contentTransferEncoding = contentTransferEncoding;
			this.contentTransferEncodingValue = value;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00018B83 File Offset: 0x00016D83
		public ContentTransferEncoding ContentTransferEncoding
		{
			get
			{
				this.ParseValue();
				return this.contentTransferEncoding;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00018B91 File Offset: 0x00016D91
		public string ContentTransferEncodingValue
		{
			get
			{
				this.ParseValue();
				return this.contentTransferEncodingValue;
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00018BA0 File Offset: 0x00016DA0
		private void ParseValue()
		{
			if (this.contentTransferEncodingValue == null)
			{
				int num = 0;
				this.contentTransferEncodingValue = ((base.Value.Length == 0) ? base.Value : ((base.Value[0] == '"') ? MailBnfHelper.ReadQuotedString(base.Value, ref num, null) : MailBnfHelper.ReadToken(base.Value, ref num, null)));
				string text = this.contentTransferEncodingValue;
				if (text == "7bit")
				{
					this.contentTransferEncoding = ContentTransferEncoding.SevenBit;
					return;
				}
				if (text == "8bit")
				{
					this.contentTransferEncoding = ContentTransferEncoding.EightBit;
					return;
				}
				if (text == "binary")
				{
					this.contentTransferEncoding = ContentTransferEncoding.Binary;
					return;
				}
				this.contentTransferEncoding = ContentTransferEncoding.Other;
			}
		}

		// Token: 0x04000228 RID: 552
		private ContentTransferEncoding contentTransferEncoding;

		// Token: 0x04000229 RID: 553
		private string contentTransferEncodingValue;

		// Token: 0x0400022A RID: 554
		public static readonly ContentTransferEncodingHeader Binary = new ContentTransferEncodingHeader(ContentTransferEncoding.Binary, "binary");

		// Token: 0x0400022B RID: 555
		public static readonly ContentTransferEncodingHeader EightBit = new ContentTransferEncodingHeader(ContentTransferEncoding.EightBit, "8bit");

		// Token: 0x0400022C RID: 556
		public static readonly ContentTransferEncodingHeader SevenBit = new ContentTransferEncodingHeader(ContentTransferEncoding.SevenBit, "7bit");
	}
}
