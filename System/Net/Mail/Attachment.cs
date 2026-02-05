using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000257 RID: 599
	public class Attachment : AttachmentBase
	{
		// Token: 0x060016D3 RID: 5843 RVA: 0x00075B44 File Offset: 0x00073D44
		internal Attachment()
		{
			base.MimePart.ContentDisposition = new ContentDisposition();
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00075B5C File Offset: 0x00073D5C
		public Attachment(string fileName)
			: base(fileName)
		{
			this.Name = AttachmentBase.ShortNameFromFile(fileName);
			base.MimePart.ContentDisposition = new ContentDisposition();
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00075B81 File Offset: 0x00073D81
		public Attachment(string fileName, string mediaType)
			: base(fileName, mediaType)
		{
			this.Name = AttachmentBase.ShortNameFromFile(fileName);
			base.MimePart.ContentDisposition = new ContentDisposition();
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00075BA8 File Offset: 0x00073DA8
		public Attachment(string fileName, ContentType contentType)
			: base(fileName, contentType)
		{
			if (contentType.Name == null || contentType.Name == string.Empty)
			{
				this.Name = AttachmentBase.ShortNameFromFile(fileName);
			}
			else
			{
				this.Name = contentType.Name;
			}
			base.MimePart.ContentDisposition = new ContentDisposition();
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00075C01 File Offset: 0x00073E01
		public Attachment(Stream contentStream, string name)
			: base(contentStream, null, null)
		{
			this.Name = name;
			base.MimePart.ContentDisposition = new ContentDisposition();
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x00075C23 File Offset: 0x00073E23
		public Attachment(Stream contentStream, string name, string mediaType)
			: base(contentStream, null, mediaType)
		{
			this.Name = name;
			base.MimePart.ContentDisposition = new ContentDisposition();
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00075C45 File Offset: 0x00073E45
		public Attachment(Stream contentStream, ContentType contentType)
			: base(contentStream, contentType)
		{
			this.Name = contentType.Name;
			base.MimePart.ContentDisposition = new ContentDisposition();
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00075C6C File Offset: 0x00073E6C
		internal void SetContentTypeName(bool allowUnicode)
		{
			if (!allowUnicode && this.name != null && this.name.Length != 0 && !MimeBasePart.IsAscii(this.name, false))
			{
				Encoding encoding = this.NameEncoding;
				if (encoding == null)
				{
					encoding = Encoding.GetEncoding("utf-8");
				}
				base.MimePart.ContentType.Name = MimeBasePart.EncodeHeaderValue(this.name, encoding, MimeBasePart.ShouldUseBase64Encoding(encoding));
				return;
			}
			base.MimePart.ContentType.Name = this.name;
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060016DB RID: 5851 RVA: 0x00075CED File Offset: 0x00073EED
		// (set) Token: 0x060016DC RID: 5852 RVA: 0x00075CF8 File Offset: 0x00073EF8
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				Encoding encoding = MimeBasePart.DecodeEncoding(value);
				if (encoding != null)
				{
					this.nameEncoding = encoding;
					this.name = MimeBasePart.DecodeHeaderValue(value);
					base.MimePart.ContentType.Name = value;
					return;
				}
				this.name = value;
				this.SetContentTypeName(true);
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x00075D42 File Offset: 0x00073F42
		// (set) Token: 0x060016DE RID: 5854 RVA: 0x00075D4A File Offset: 0x00073F4A
		public Encoding NameEncoding
		{
			get
			{
				return this.nameEncoding;
			}
			set
			{
				this.nameEncoding = value;
				if (this.name != null && this.name != string.Empty)
				{
					this.SetContentTypeName(true);
				}
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x00075D74 File Offset: 0x00073F74
		public ContentDisposition ContentDisposition
		{
			get
			{
				return base.MimePart.ContentDisposition;
			}
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x00075D81 File Offset: 0x00073F81
		internal override void PrepareForSending(bool allowUnicode)
		{
			if (this.name != null && this.name != string.Empty)
			{
				this.SetContentTypeName(allowUnicode);
			}
			base.PrepareForSending(allowUnicode);
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00075DAC File Offset: 0x00073FAC
		public static Attachment CreateAttachmentFromString(string content, string name)
		{
			Attachment attachment = new Attachment();
			attachment.SetContentFromString(content, null, string.Empty);
			attachment.Name = name;
			return attachment;
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00075DD4 File Offset: 0x00073FD4
		public static Attachment CreateAttachmentFromString(string content, string name, Encoding contentEncoding, string mediaType)
		{
			Attachment attachment = new Attachment();
			attachment.SetContentFromString(content, contentEncoding, mediaType);
			attachment.Name = name;
			return attachment;
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00075DF8 File Offset: 0x00073FF8
		public static Attachment CreateAttachmentFromString(string content, ContentType contentType)
		{
			Attachment attachment = new Attachment();
			attachment.SetContentFromString(content, contentType);
			attachment.Name = contentType.Name;
			return attachment;
		}

		// Token: 0x04001766 RID: 5990
		private string name;

		// Token: 0x04001767 RID: 5991
		private Encoding nameEncoding;
	}
}
