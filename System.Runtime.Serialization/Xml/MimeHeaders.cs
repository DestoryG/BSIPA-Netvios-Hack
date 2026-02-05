using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Xml
{
	// Token: 0x02000040 RID: 64
	internal class MimeHeaders
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00018644 File Offset: 0x00016844
		public ContentTypeHeader ContentType
		{
			get
			{
				MimeHeader mimeHeader;
				if (this.headers.TryGetValue("content-type", out mimeHeader))
				{
					return mimeHeader as ContentTypeHeader;
				}
				return null;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x00018670 File Offset: 0x00016870
		public ContentIDHeader ContentID
		{
			get
			{
				MimeHeader mimeHeader;
				if (this.headers.TryGetValue("content-id", out mimeHeader))
				{
					return mimeHeader as ContentIDHeader;
				}
				return null;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0001869C File Offset: 0x0001689C
		public ContentTransferEncodingHeader ContentTransferEncoding
		{
			get
			{
				MimeHeader mimeHeader;
				if (this.headers.TryGetValue("content-transfer-encoding", out mimeHeader))
				{
					return mimeHeader as ContentTransferEncodingHeader;
				}
				return null;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x000186C8 File Offset: 0x000168C8
		public MimeVersionHeader MimeVersion
		{
			get
			{
				MimeHeader mimeHeader;
				if (this.headers.TryGetValue("mime-version", out mimeHeader))
				{
					return mimeHeader as MimeVersionHeader;
				}
				return null;
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000186F4 File Offset: 0x000168F4
		public void Add(string name, string value, ref int remaining)
		{
			if (name == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("name");
			}
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			if (!(name == "content-type"))
			{
				if (!(name == "content-id"))
				{
					if (!(name == "content-transfer-encoding"))
					{
						if (!(name == "mime-version"))
						{
							remaining += value.Length * 2;
						}
						else
						{
							this.Add(new MimeVersionHeader(value));
						}
					}
					else
					{
						this.Add(new ContentTransferEncodingHeader(value));
					}
				}
				else
				{
					this.Add(new ContentIDHeader(name, value));
				}
			}
			else
			{
				this.Add(new ContentTypeHeader(value));
			}
			remaining += name.Length * 2;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x000187A8 File Offset: 0x000169A8
		public void Add(MimeHeader header)
		{
			if (header == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("header");
			}
			MimeHeader mimeHeader;
			if (this.headers.TryGetValue(header.Name, out mimeHeader))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("MIME header '{0}' already exists.", new object[] { header.Name })));
			}
			this.headers.Add(header.Name, header);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00018810 File Offset: 0x00016A10
		public void Release(ref int remaining)
		{
			foreach (MimeHeader mimeHeader in this.headers.Values)
			{
				remaining += mimeHeader.Value.Length * 2;
			}
		}

		// Token: 0x0400021B RID: 539
		private Dictionary<string, MimeHeader> headers = new Dictionary<string, MimeHeader>();

		// Token: 0x0200015C RID: 348
		private static class Constants
		{
			// Token: 0x04000972 RID: 2418
			public const string ContentTransferEncoding = "content-transfer-encoding";

			// Token: 0x04000973 RID: 2419
			public const string ContentID = "content-id";

			// Token: 0x04000974 RID: 2420
			public const string ContentType = "content-type";

			// Token: 0x04000975 RID: 2421
			public const string MimeVersion = "mime-version";
		}
	}
}
