using System;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000046 RID: 70
	internal class MimeVersionHeader : MimeHeader
	{
		// Token: 0x06000525 RID: 1317 RVA: 0x00018C8E File Offset: 0x00016E8E
		public MimeVersionHeader(string value)
			: base("mime-version", value)
		{
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00018C9C File Offset: 0x00016E9C
		public string Version
		{
			get
			{
				if (this.version == null && base.Value != null)
				{
					this.ParseValue();
				}
				return this.version;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00018CBC File Offset: 0x00016EBC
		private void ParseValue()
		{
			if (base.Value == "1.0")
			{
				this.version = "1.0";
				return;
			}
			int num = 0;
			if (!MailBnfHelper.SkipCFWS(base.Value, ref num))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("MIME version header is invalid.")));
			}
			StringBuilder stringBuilder = new StringBuilder();
			MailBnfHelper.ReadDigits(base.Value, ref num, stringBuilder);
			if (!MailBnfHelper.SkipCFWS(base.Value, ref num) || num >= base.Value.Length || base.Value[num++] != '.' || !MailBnfHelper.SkipCFWS(base.Value, ref num))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("MIME version header is invalid.")));
			}
			stringBuilder.Append('.');
			MailBnfHelper.ReadDigits(base.Value, ref num, stringBuilder);
			this.version = stringBuilder.ToString();
		}

		// Token: 0x0400022D RID: 557
		public static readonly MimeVersionHeader Default = new MimeVersionHeader("1.0");

		// Token: 0x0400022E RID: 558
		private string version;
	}
}
