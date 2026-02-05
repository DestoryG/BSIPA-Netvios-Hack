using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000256 RID: 598
	public abstract class AttachmentBase : IDisposable
	{
		// Token: 0x060016B9 RID: 5817 RVA: 0x0007556D File Offset: 0x0007376D
		internal AttachmentBase()
		{
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x00075580 File Offset: 0x00073780
		protected AttachmentBase(string fileName)
		{
			this.SetContentFromFile(fileName, string.Empty);
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0007559F File Offset: 0x0007379F
		protected AttachmentBase(string fileName, string mediaType)
		{
			this.SetContentFromFile(fileName, mediaType);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x000755BA File Offset: 0x000737BA
		protected AttachmentBase(string fileName, ContentType contentType)
		{
			this.SetContentFromFile(fileName, contentType);
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x000755D5 File Offset: 0x000737D5
		protected AttachmentBase(Stream contentStream)
		{
			this.part.SetContent(contentStream);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000755F4 File Offset: 0x000737F4
		protected AttachmentBase(Stream contentStream, string mediaType)
		{
			this.part.SetContent(contentStream, null, mediaType);
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x00075615 File Offset: 0x00073815
		internal AttachmentBase(Stream contentStream, string name, string mediaType)
		{
			this.part.SetContent(contentStream, name, mediaType);
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x00075636 File Offset: 0x00073836
		protected AttachmentBase(Stream contentStream, ContentType contentType)
		{
			this.part.SetContent(contentStream, contentType);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x00075656 File Offset: 0x00073856
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x0007565F File Offset: 0x0007385F
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this.disposed)
			{
				this.disposed = true;
				this.part.Dispose();
			}
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00075680 File Offset: 0x00073880
		internal static string ShortNameFromFile(string fileName)
		{
			int num = fileName.LastIndexOfAny(new char[] { '\\', ':' }, fileName.Length - 1, fileName.Length);
			string text;
			if (num > 0)
			{
				text = fileName.Substring(num + 1, fileName.Length - num - 1);
			}
			else
			{
				text = fileName;
			}
			return text;
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x000756D0 File Offset: 0x000738D0
		internal void SetContentFromFile(string fileName, ContentType contentType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "fileName" }), "fileName");
			}
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.part.SetContent(stream, contentType);
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x00075734 File Offset: 0x00073934
		internal void SetContentFromFile(string fileName, string mediaType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "fileName" }), "fileName");
			}
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.part.SetContent(stream, null, mediaType);
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00075798 File Offset: 0x00073998
		internal void SetContentFromString(string contentString, ContentType contentType)
		{
			if (contentString == null)
			{
				throw new ArgumentNullException("content");
			}
			if (this.part.Stream != null)
			{
				this.part.Stream.Close();
			}
			Encoding encoding;
			if (contentType != null && contentType.CharSet != null)
			{
				encoding = Encoding.GetEncoding(contentType.CharSet);
			}
			else if (MimeBasePart.IsAscii(contentString, false))
			{
				encoding = Encoding.ASCII;
			}
			else
			{
				encoding = Encoding.GetEncoding("utf-8");
			}
			byte[] bytes = encoding.GetBytes(contentString);
			this.part.SetContent(new MemoryStream(bytes), contentType);
			if (MimeBasePart.ShouldUseBase64Encoding(encoding))
			{
				this.part.TransferEncoding = TransferEncoding.Base64;
				return;
			}
			this.part.TransferEncoding = TransferEncoding.QuotedPrintable;
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00075840 File Offset: 0x00073A40
		internal void SetContentFromString(string contentString, Encoding encoding, string mediaType)
		{
			if (contentString == null)
			{
				throw new ArgumentNullException("content");
			}
			if (this.part.Stream != null)
			{
				this.part.Stream.Close();
			}
			if (mediaType == null || mediaType == string.Empty)
			{
				mediaType = "text/plain";
			}
			int num = 0;
			try
			{
				string text = MailBnfHelper.ReadToken(mediaType, ref num, null);
				if (text.Length == 0 || num >= mediaType.Length || mediaType[num++] != '/')
				{
					throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
				}
				text = MailBnfHelper.ReadToken(mediaType, ref num, null);
				if (text.Length == 0 || num < mediaType.Length)
				{
					throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
				}
			}
			catch (FormatException)
			{
				throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
			}
			ContentType contentType = new ContentType(mediaType);
			if (encoding == null)
			{
				if (MimeBasePart.IsAscii(contentString, false))
				{
					encoding = Encoding.ASCII;
				}
				else
				{
					encoding = Encoding.GetEncoding("utf-8");
				}
			}
			contentType.CharSet = encoding.BodyName;
			byte[] bytes = encoding.GetBytes(contentString);
			this.part.SetContent(new MemoryStream(bytes), contentType);
			if (MimeBasePart.ShouldUseBase64Encoding(encoding))
			{
				this.part.TransferEncoding = TransferEncoding.Base64;
				return;
			}
			this.part.TransferEncoding = TransferEncoding.QuotedPrintable;
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x00075998 File Offset: 0x00073B98
		internal virtual void PrepareForSending(bool allowUnicode)
		{
			this.part.ResetStream();
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x000759A5 File Offset: 0x00073BA5
		public Stream ContentStream
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				return this.part.Stream;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060016CA RID: 5834 RVA: 0x000759CC File Offset: 0x00073BCC
		// (set) Token: 0x060016CB RID: 5835 RVA: 0x00075A44 File Offset: 0x00073C44
		public string ContentId
		{
			get
			{
				string text = this.part.ContentID;
				if (string.IsNullOrEmpty(text))
				{
					text = Guid.NewGuid().ToString();
					this.ContentId = text;
					return text;
				}
				if (text.Length >= 2 && text[0] == '<' && text[text.Length - 1] == '>')
				{
					return text.Substring(1, text.Length - 2);
				}
				return text;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.part.ContentID = null;
					return;
				}
				if (value.IndexOfAny(new char[] { '<', '>' }) != -1)
				{
					throw new ArgumentException(SR.GetString("MailHeaderInvalidCID"), "value");
				}
				this.part.ContentID = "<" + value + ">";
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060016CC RID: 5836 RVA: 0x00075AAF File Offset: 0x00073CAF
		// (set) Token: 0x060016CD RID: 5837 RVA: 0x00075ABC File Offset: 0x00073CBC
		public ContentType ContentType
		{
			get
			{
				return this.part.ContentType;
			}
			set
			{
				this.part.ContentType = value;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060016CE RID: 5838 RVA: 0x00075ACA File Offset: 0x00073CCA
		// (set) Token: 0x060016CF RID: 5839 RVA: 0x00075AD7 File Offset: 0x00073CD7
		public TransferEncoding TransferEncoding
		{
			get
			{
				return this.part.TransferEncoding;
			}
			set
			{
				this.part.TransferEncoding = value;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060016D0 RID: 5840 RVA: 0x00075AE8 File Offset: 0x00073CE8
		// (set) Token: 0x060016D1 RID: 5841 RVA: 0x00075B0D File Offset: 0x00073D0D
		internal Uri ContentLocation
		{
			get
			{
				Uri uri;
				if (!Uri.TryCreate(this.part.ContentLocation, UriKind.RelativeOrAbsolute, out uri))
				{
					return null;
				}
				return uri;
			}
			set
			{
				this.part.ContentLocation = ((value == null) ? null : (value.IsAbsoluteUri ? value.AbsoluteUri : value.OriginalString));
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060016D2 RID: 5842 RVA: 0x00075B3C File Offset: 0x00073D3C
		internal MimePart MimePart
		{
			get
			{
				return this.part;
			}
		}

		// Token: 0x04001764 RID: 5988
		internal bool disposed;

		// Token: 0x04001765 RID: 5989
		private MimePart part = new MimePart();
	}
}
