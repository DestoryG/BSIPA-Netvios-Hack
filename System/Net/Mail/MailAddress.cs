using System;
using System.Globalization;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x0200026A RID: 618
	public class MailAddress
	{
		// Token: 0x06001730 RID: 5936 RVA: 0x00076779 File Offset: 0x00074979
		internal MailAddress(string displayName, string userName, string domain)
		{
			this.host = domain;
			this.userName = userName;
			this.displayName = displayName;
			this.displayNameEncoding = Encoding.GetEncoding("utf-8");
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x000767A6 File Offset: 0x000749A6
		public MailAddress(string address)
			: this(address, null, null)
		{
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x000767B1 File Offset: 0x000749B1
		public MailAddress(string address, string displayName)
			: this(address, displayName, null)
		{
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x000767BC File Offset: 0x000749BC
		public MailAddress(string address, string displayName, Encoding displayNameEncoding)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (address == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "address" }), "address");
			}
			this.displayNameEncoding = displayNameEncoding ?? Encoding.GetEncoding("utf-8");
			this.displayName = displayName ?? string.Empty;
			if (!string.IsNullOrEmpty(this.displayName))
			{
				this.displayName = MailAddressParser.NormalizeOrThrow(this.displayName);
				if (this.displayName.Length >= 2 && this.displayName[0] == '"' && this.displayName[this.displayName.Length - 1] == '"')
				{
					this.displayName = this.displayName.Substring(1, this.displayName.Length - 2);
				}
			}
			MailAddress mailAddress = MailAddressParser.ParseAddress(address);
			this.host = mailAddress.host;
			this.userName = mailAddress.userName;
			if (string.IsNullOrEmpty(this.displayName))
			{
				this.displayName = mailAddress.displayName;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x000768E1 File Offset: 0x00074AE1
		public string DisplayName
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x000768E9 File Offset: 0x00074AE9
		public string User
		{
			get
			{
				return this.userName;
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x000768F1 File Offset: 0x00074AF1
		private string GetUser(bool allowUnicode)
		{
			if (!allowUnicode && !MimeBasePart.IsAscii(this.userName, true))
			{
				throw new SmtpException(SR.GetString("SmtpNonAsciiUserNotSupported", new object[] { this.Address }));
			}
			return this.userName;
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x00076929 File Offset: 0x00074B29
		public string Host
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00076934 File Offset: 0x00074B34
		private string GetHost(bool allowUnicode)
		{
			string ascii = this.host;
			if (!allowUnicode && !MimeBasePart.IsAscii(ascii, true))
			{
				IdnMapping idnMapping = new IdnMapping();
				try
				{
					ascii = idnMapping.GetAscii(ascii);
				}
				catch (ArgumentException ex)
				{
					throw new SmtpException(SR.GetString("SmtpInvalidHostName", new object[] { this.Address }), ex);
				}
			}
			if (!ServicePointManager.AllowFullDomainLiterals && ascii.IndexOfAny(MailAddress.s_newLines) >= 0)
			{
				throw new SmtpException("SmtpInvalidHostName", this.Address);
			}
			return ascii;
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x000769BC File Offset: 0x00074BBC
		public string Address
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}@{1}", new object[] { this.userName, this.host });
			}
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x000769E5 File Offset: 0x00074BE5
		private string GetAddress(bool allowUnicode)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}@{1}", new object[]
			{
				this.GetUser(allowUnicode),
				this.GetHost(allowUnicode)
			});
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x00076A10 File Offset: 0x00074C10
		private string SmtpAddress
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "<{0}>", new object[] { this.Address });
			}
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00076A30 File Offset: 0x00074C30
		internal string GetSmtpAddress(bool allowUnicode)
		{
			return string.Format(CultureInfo.InvariantCulture, "<{0}>", new object[] { this.GetAddress(allowUnicode) });
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00076A51 File Offset: 0x00074C51
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.DisplayName))
			{
				return this.Address;
			}
			return string.Format("\"{0}\" {1}", this.DisplayName, this.SmtpAddress);
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x00076A7D File Offset: 0x00074C7D
		public override bool Equals(object value)
		{
			return value != null && this.ToString().Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x00076A96 File Offset: 0x00074C96
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x00076AA4 File Offset: 0x00074CA4
		internal string Encode(int charsConsumed, bool allowUnicode)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.displayName))
			{
				if (MimeBasePart.IsAscii(this.displayName, false) || allowUnicode)
				{
					text = string.Format(CultureInfo.InvariantCulture, "\"{0}\"", new object[] { this.displayName });
				}
				else
				{
					IEncodableStream encoderForHeader = MailAddress.encoderFactory.GetEncoderForHeader(this.displayNameEncoding, false, charsConsumed);
					byte[] bytes = this.displayNameEncoding.GetBytes(this.displayName);
					encoderForHeader.EncodeBytes(bytes, 0, bytes.Length);
					text = encoderForHeader.GetEncodedString();
				}
				text = text + " " + this.GetSmtpAddress(allowUnicode);
			}
			else
			{
				text = this.GetAddress(allowUnicode);
			}
			return text;
		}

		// Token: 0x040017A0 RID: 6048
		private static readonly char[] s_newLines = new char[] { '\r', '\n' };

		// Token: 0x040017A1 RID: 6049
		private readonly Encoding displayNameEncoding;

		// Token: 0x040017A2 RID: 6050
		private readonly string displayName;

		// Token: 0x040017A3 RID: 6051
		private readonly string userName;

		// Token: 0x040017A4 RID: 6052
		private readonly string host;

		// Token: 0x040017A5 RID: 6053
		private static EncodedStreamFactory encoderFactory = new EncodedStreamFactory();
	}
}
