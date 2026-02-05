using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x0200023F RID: 575
	public class ContentDisposition
	{
		// Token: 0x060015C6 RID: 5574 RVA: 0x00070BB0 File Offset: 0x0006EDB0
		static ContentDisposition()
		{
			ContentDisposition.validators.Add("creation-date", ContentDisposition.dateParser);
			ContentDisposition.validators.Add("modification-date", ContentDisposition.dateParser);
			ContentDisposition.validators.Add("read-date", ContentDisposition.dateParser);
			ContentDisposition.validators.Add("size", ContentDisposition.longParser);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00070C41 File Offset: 0x0006EE41
		public ContentDisposition()
		{
			this.isChanged = true;
			this.dispositionType = "attachment";
			this.disposition = this.dispositionType;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x00070C67 File Offset: 0x0006EE67
		public ContentDisposition(string disposition)
		{
			if (disposition == null)
			{
				throw new ArgumentNullException("disposition");
			}
			this.isChanged = true;
			this.disposition = disposition;
			this.ParseValue();
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x00070C94 File Offset: 0x0006EE94
		internal DateTime GetDateParameter(string parameterName)
		{
			SmtpDateTime smtpDateTime = ((TrackingValidationObjectDictionary)this.Parameters).InternalGet(parameterName) as SmtpDateTime;
			if (smtpDateTime == null)
			{
				return DateTime.MinValue;
			}
			return smtpDateTime.Date;
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x00070CC7 File Offset: 0x0006EEC7
		// (set) Token: 0x060015CB RID: 5579 RVA: 0x00070CCF File Offset: 0x0006EECF
		public string DispositionType
		{
			get
			{
				return this.dispositionType;
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
				this.isChanged = true;
				this.dispositionType = value;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x00070D0F File Offset: 0x0006EF0F
		public StringDictionary Parameters
		{
			get
			{
				if (this.parameters == null)
				{
					this.parameters = new TrackingValidationObjectDictionary(ContentDisposition.validators);
				}
				return this.parameters;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x00070D2F File Offset: 0x0006EF2F
		// (set) Token: 0x060015CE RID: 5582 RVA: 0x00070D41 File Offset: 0x0006EF41
		public string FileName
		{
			get
			{
				return this.Parameters["filename"];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Parameters.Remove("filename");
					return;
				}
				this.Parameters["filename"] = value;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x00070D6D File Offset: 0x0006EF6D
		// (set) Token: 0x060015D0 RID: 5584 RVA: 0x00070D7C File Offset: 0x0006EF7C
		public DateTime CreationDate
		{
			get
			{
				return this.GetDateParameter("creation-date");
			}
			set
			{
				SmtpDateTime smtpDateTime = new SmtpDateTime(value);
				((TrackingValidationObjectDictionary)this.Parameters).InternalSet("creation-date", smtpDateTime);
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060015D1 RID: 5585 RVA: 0x00070DA6 File Offset: 0x0006EFA6
		// (set) Token: 0x060015D2 RID: 5586 RVA: 0x00070DB4 File Offset: 0x0006EFB4
		public DateTime ModificationDate
		{
			get
			{
				return this.GetDateParameter("modification-date");
			}
			set
			{
				SmtpDateTime smtpDateTime = new SmtpDateTime(value);
				((TrackingValidationObjectDictionary)this.Parameters).InternalSet("modification-date", smtpDateTime);
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x00070DDE File Offset: 0x0006EFDE
		// (set) Token: 0x060015D4 RID: 5588 RVA: 0x00070DF0 File Offset: 0x0006EFF0
		public bool Inline
		{
			get
			{
				return this.dispositionType == "inline";
			}
			set
			{
				this.isChanged = true;
				if (value)
				{
					this.dispositionType = "inline";
					return;
				}
				this.dispositionType = "attachment";
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060015D5 RID: 5589 RVA: 0x00070E13 File Offset: 0x0006F013
		// (set) Token: 0x060015D6 RID: 5590 RVA: 0x00070E20 File Offset: 0x0006F020
		public DateTime ReadDate
		{
			get
			{
				return this.GetDateParameter("read-date");
			}
			set
			{
				SmtpDateTime smtpDateTime = new SmtpDateTime(value);
				((TrackingValidationObjectDictionary)this.Parameters).InternalSet("read-date", smtpDateTime);
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060015D7 RID: 5591 RVA: 0x00070E4C File Offset: 0x0006F04C
		// (set) Token: 0x060015D8 RID: 5592 RVA: 0x00070E7B File Offset: 0x0006F07B
		public long Size
		{
			get
			{
				object obj = ((TrackingValidationObjectDictionary)this.Parameters).InternalGet("size");
				if (obj == null)
				{
					return -1L;
				}
				return (long)obj;
			}
			set
			{
				((TrackingValidationObjectDictionary)this.Parameters).InternalSet("size", value);
			}
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00070E98 File Offset: 0x0006F098
		internal void Set(string contentDisposition, HeaderCollection headers)
		{
			this.disposition = contentDisposition;
			this.ParseValue();
			headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.ToString());
			this.isPersisted = true;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00070EC0 File Offset: 0x0006F0C0
		internal void PersistIfNeeded(HeaderCollection headers, bool forcePersist)
		{
			if (this.IsChanged || !this.isPersisted || forcePersist)
			{
				headers.InternalSet(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition), this.ToString());
				this.isPersisted = true;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x00070EF3 File Offset: 0x0006F0F3
		internal bool IsChanged
		{
			get
			{
				return this.isChanged || (this.parameters != null && this.parameters.IsChanged);
			}
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x00070F14 File Offset: 0x0006F114
		public override string ToString()
		{
			if (this.disposition == null || this.isChanged || (this.parameters != null && this.parameters.IsChanged))
			{
				this.disposition = this.Encode(false);
				this.isChanged = false;
				this.parameters.IsChanged = false;
				this.isPersisted = false;
			}
			return this.disposition;
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x00070F74 File Offset: 0x0006F174
		internal string Encode(bool allowUnicode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.dispositionType);
			foreach (object obj in this.Parameters.Keys)
			{
				string text = (string)obj;
				stringBuilder.Append("; ");
				ContentDisposition.EncodeToBuffer(text, stringBuilder, allowUnicode);
				stringBuilder.Append('=');
				ContentDisposition.EncodeToBuffer(this.parameters[text], stringBuilder, allowUnicode);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x00071014 File Offset: 0x0006F214
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

		// Token: 0x060015DF RID: 5599 RVA: 0x0007108E File Offset: 0x0006F28E
		public override bool Equals(object rparam)
		{
			return rparam != null && string.Compare(this.ToString(), rparam.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x000710AA File Offset: 0x0006F2AA
		public override int GetHashCode()
		{
			return this.ToString().ToLowerInvariant().GetHashCode();
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x000710BC File Offset: 0x0006F2BC
		private void ParseValue()
		{
			int num = 0;
			try
			{
				this.dispositionType = MailBnfHelper.ReadToken(this.disposition, ref num, null);
				if (string.IsNullOrEmpty(this.dispositionType))
				{
					throw new FormatException(SR.GetString("MailHeaderFieldMalformedHeader"));
				}
				if (this.parameters == null)
				{
					this.parameters = new TrackingValidationObjectDictionary(ContentDisposition.validators);
				}
				else
				{
					this.parameters.Clear();
				}
				while (MailBnfHelper.SkipCFWS(this.disposition, ref num))
				{
					if (this.disposition[num++] != ';')
					{
						throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { this.disposition[num - 1] }));
					}
					if (!MailBnfHelper.SkipCFWS(this.disposition, ref num))
					{
						break;
					}
					string text = MailBnfHelper.ReadParameterAttribute(this.disposition, ref num, null);
					if (this.disposition[num++] != '=')
					{
						throw new FormatException(SR.GetString("MailHeaderFieldMalformedHeader"));
					}
					if (!MailBnfHelper.SkipCFWS(this.disposition, ref num))
					{
						throw new FormatException(SR.GetString("ContentDispositionInvalid"));
					}
					string text2;
					if (this.disposition[num] == '"')
					{
						text2 = MailBnfHelper.ReadQuotedString(this.disposition, ref num, null);
					}
					else
					{
						text2 = MailBnfHelper.ReadToken(this.disposition, ref num, null);
					}
					if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
					{
						throw new FormatException(SR.GetString("ContentDispositionInvalid"));
					}
					this.Parameters.Add(text, text2);
				}
			}
			catch (FormatException ex)
			{
				throw new FormatException(SR.GetString("ContentDispositionInvalid"), ex);
			}
			this.parameters.IsChanged = false;
		}

		// Token: 0x040016E1 RID: 5857
		private string dispositionType;

		// Token: 0x040016E2 RID: 5858
		private TrackingValidationObjectDictionary parameters;

		// Token: 0x040016E3 RID: 5859
		private bool isChanged;

		// Token: 0x040016E4 RID: 5860
		private bool isPersisted;

		// Token: 0x040016E5 RID: 5861
		private string disposition;

		// Token: 0x040016E6 RID: 5862
		private const string creationDate = "creation-date";

		// Token: 0x040016E7 RID: 5863
		private const string readDate = "read-date";

		// Token: 0x040016E8 RID: 5864
		private const string modificationDate = "modification-date";

		// Token: 0x040016E9 RID: 5865
		private const string size = "size";

		// Token: 0x040016EA RID: 5866
		private const string fileName = "filename";

		// Token: 0x040016EB RID: 5867
		private static readonly TrackingValidationObjectDictionary.ValidateAndParseValue dateParser = (object value) => new SmtpDateTime(value.ToString());

		// Token: 0x040016EC RID: 5868
		private static readonly TrackingValidationObjectDictionary.ValidateAndParseValue longParser = delegate(object value)
		{
			long num;
			if (!long.TryParse(value.ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out num))
			{
				throw new FormatException(SR.GetString("ContentDispositionInvalid"));
			}
			return num;
		};

		// Token: 0x040016ED RID: 5869
		private static readonly IDictionary<string, TrackingValidationObjectDictionary.ValidateAndParseValue> validators = new Dictionary<string, TrackingValidationObjectDictionary.ValidateAndParseValue>();
	}
}
