using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x02000241 RID: 577
	public class ContentType
	{
		// Token: 0x060015E2 RID: 5602 RVA: 0x00071278 File Offset: 0x0006F478
		public ContentType()
			: this(ContentType.Default)
		{
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00071288 File Offset: 0x0006F488
		public ContentType(string contentType)
		{
			if (contentType == null)
			{
				throw new ArgumentNullException("contentType");
			}
			if (contentType == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "contentType" }), "contentType");
			}
			this.isChanged = true;
			this.type = contentType;
			this.ParseValue();
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x000712ED File Offset: 0x0006F4ED
		// (set) Token: 0x060015E5 RID: 5605 RVA: 0x000712FF File Offset: 0x0006F4FF
		public string Boundary
		{
			get
			{
				return this.Parameters["boundary"];
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("boundary");
					return;
				}
				this.Parameters["boundary"] = value;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060015E6 RID: 5606 RVA: 0x00071333 File Offset: 0x0006F533
		// (set) Token: 0x060015E7 RID: 5607 RVA: 0x00071345 File Offset: 0x0006F545
		public string CharSet
		{
			get
			{
				return this.Parameters["charset"];
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("charset");
					return;
				}
				this.Parameters["charset"] = value;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060015E8 RID: 5608 RVA: 0x00071379 File Offset: 0x0006F579
		// (set) Token: 0x060015E9 RID: 5609 RVA: 0x00071394 File Offset: 0x0006F594
		public string MediaType
		{
			get
			{
				return this.mediaType + "/" + this.subType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value == string.Empty)
				{
					throw new ArgumentException(SR.GetString("net_emptystringset"), "value");
				}
				int num = 0;
				this.mediaType = MailBnfHelper.ReadToken(value, ref num, null);
				if (this.mediaType.Length == 0 || num >= value.Length || value[num++] != '/')
				{
					throw new FormatException(SR.GetString("MediaTypeInvalid"));
				}
				this.subType = MailBnfHelper.ReadToken(value, ref num, null);
				if (this.subType.Length == 0 || num < value.Length)
				{
					throw new FormatException(SR.GetString("MediaTypeInvalid"));
				}
				this.isChanged = true;
				this.isPersisted = false;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x0007145C File Offset: 0x0006F65C
		// (set) Token: 0x060015EB RID: 5611 RVA: 0x0007148C File Offset: 0x0006F68C
		public string Name
		{
			get
			{
				string text = this.Parameters["name"];
				Encoding encoding = MimeBasePart.DecodeEncoding(text);
				if (encoding != null)
				{
					text = MimeBasePart.DecodeHeaderValue(text);
				}
				return text;
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					this.Parameters.Remove("name");
					return;
				}
				this.Parameters["name"] = value;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x000714C0 File Offset: 0x0006F6C0
		public StringDictionary Parameters
		{
			get
			{
				if (this.parameters == null && this.type == null)
				{
					this.parameters = new TrackingStringDictionary();
				}
				return this.parameters;
			}
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x000714E3 File Offset: 0x0006F6E3
		internal void Set(string contentType, HeaderCollection headers)
		{
			this.type = contentType;
			this.ParseValue();
			headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentType), this.ToString());
			this.isPersisted = true;
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0007150B File Offset: 0x0006F70B
		internal void PersistIfNeeded(HeaderCollection headers, bool forcePersist)
		{
			if (this.IsChanged || !this.isPersisted || forcePersist)
			{
				headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentType), this.ToString());
				this.isPersisted = true;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060015EF RID: 5615 RVA: 0x0007153E File Offset: 0x0006F73E
		internal bool IsChanged
		{
			get
			{
				return this.isChanged || (this.parameters != null && this.parameters.IsChanged);
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x0007155F File Offset: 0x0006F75F
		public override string ToString()
		{
			if (this.type == null || this.IsChanged)
			{
				this.type = this.Encode(false);
				this.isChanged = false;
				this.parameters.IsChanged = false;
				this.isPersisted = false;
			}
			return this.type;
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x000715A0 File Offset: 0x0006F7A0
		internal string Encode(bool allowUnicode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.mediaType);
			stringBuilder.Append('/');
			stringBuilder.Append(this.subType);
			foreach (object obj in this.Parameters.Keys)
			{
				string text = (string)obj;
				stringBuilder.Append("; ");
				ContentType.EncodeToBuffer(text, stringBuilder, allowUnicode);
				stringBuilder.Append('=');
				ContentType.EncodeToBuffer(this.parameters[text], stringBuilder, allowUnicode);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00071658 File Offset: 0x0006F858
		private static void EncodeToBuffer(string value, StringBuilder builder, bool allowUnicode)
		{
			Encoding encoding = MimeBasePart.DecodeEncoding(value);
			if (encoding != null)
			{
				builder.Append("\"" + value + "\"");
				return;
			}
			if ((allowUnicode && !MailBnfHelper.HasCROrLF(value)) || MimeBasePart.IsAscii(value, false))
			{
				MailBnfHelper.GetTokenOrQuotedString(value, builder, allowUnicode);
				return;
			}
			encoding = Encoding.GetEncoding("utf-8");
			builder.Append("\"" + MimeBasePart.EncodeHeaderValue(value, encoding, MimeBasePart.ShouldUseBase64Encoding(encoding)) + "\"");
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x000716D2 File Offset: 0x0006F8D2
		public override bool Equals(object rparam)
		{
			return rparam != null && string.Compare(this.ToString(), rparam.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x000716EE File Offset: 0x0006F8EE
		public override int GetHashCode()
		{
			return this.ToString().ToLowerInvariant().GetHashCode();
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x00071700 File Offset: 0x0006F900
		private void ParseValue()
		{
			int num = 0;
			Exception ex = null;
			this.parameters = new TrackingStringDictionary();
			try
			{
				this.mediaType = MailBnfHelper.ReadToken(this.type, ref num, null);
				if (this.mediaType == null || this.mediaType.Length == 0 || num >= this.type.Length || this.type[num++] != '/')
				{
					ex = new FormatException(SR.GetString("ContentTypeInvalid"));
				}
				if (ex == null)
				{
					this.subType = MailBnfHelper.ReadToken(this.type, ref num, null);
					if (this.subType == null || this.subType.Length == 0)
					{
						ex = new FormatException(SR.GetString("ContentTypeInvalid"));
					}
				}
				if (ex == null)
				{
					while (MailBnfHelper.SkipCFWS(this.type, ref num))
					{
						if (this.type[num++] != ';')
						{
							ex = new FormatException(SR.GetString("ContentTypeInvalid"));
							break;
						}
						if (!MailBnfHelper.SkipCFWS(this.type, ref num))
						{
							break;
						}
						string text = MailBnfHelper.ReadParameterAttribute(this.type, ref num, null);
						if (text == null || text.Length == 0)
						{
							ex = new FormatException(SR.GetString("ContentTypeInvalid"));
							break;
						}
						if (num >= this.type.Length || this.type[num++] != '=')
						{
							ex = new FormatException(SR.GetString("ContentTypeInvalid"));
							break;
						}
						if (!MailBnfHelper.SkipCFWS(this.type, ref num))
						{
							ex = new FormatException(SR.GetString("ContentTypeInvalid"));
							break;
						}
						string text2;
						if (this.type[num] == '"')
						{
							text2 = MailBnfHelper.ReadQuotedString(this.type, ref num, null);
						}
						else
						{
							text2 = MailBnfHelper.ReadToken(this.type, ref num, null);
						}
						if (text2 == null)
						{
							ex = new FormatException(SR.GetString("ContentTypeInvalid"));
							break;
						}
						this.parameters.Add(text, text2);
					}
				}
				this.parameters.IsChanged = false;
			}
			catch (FormatException)
			{
				throw new FormatException(SR.GetString("ContentTypeInvalid"));
			}
			if (ex != null)
			{
				throw new FormatException(SR.GetString("ContentTypeInvalid"));
			}
		}

		// Token: 0x040016F7 RID: 5879
		private string mediaType;

		// Token: 0x040016F8 RID: 5880
		private string subType;

		// Token: 0x040016F9 RID: 5881
		private bool isChanged;

		// Token: 0x040016FA RID: 5882
		private string type;

		// Token: 0x040016FB RID: 5883
		private bool isPersisted;

		// Token: 0x040016FC RID: 5884
		private TrackingStringDictionary parameters;

		// Token: 0x040016FD RID: 5885
		internal static readonly string Default = "application/octet-stream";
	}
}
