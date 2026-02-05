using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Xml
{
	// Token: 0x02000042 RID: 66
	internal class ContentTypeHeader : MimeHeader
	{
		// Token: 0x06000518 RID: 1304 RVA: 0x000188A8 File Offset: 0x00016AA8
		public ContentTypeHeader(string value)
			: base("content-type", value)
		{
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x000188B6 File Offset: 0x00016AB6
		public string MediaType
		{
			get
			{
				if (this.mediaType == null && base.Value != null)
				{
					this.ParseValue();
				}
				return this.mediaType;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x000188D4 File Offset: 0x00016AD4
		public string MediaSubtype
		{
			get
			{
				if (this.subType == null && base.Value != null)
				{
					this.ParseValue();
				}
				return this.subType;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x000188F2 File Offset: 0x00016AF2
		public Dictionary<string, string> Parameters
		{
			get
			{
				if (this.parameters == null)
				{
					if (base.Value != null)
					{
						this.ParseValue();
					}
					else
					{
						this.parameters = new Dictionary<string, string>();
					}
				}
				return this.parameters;
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00018920 File Offset: 0x00016B20
		private void ParseValue()
		{
			if (this.parameters == null)
			{
				int num = 0;
				this.parameters = new Dictionary<string, string>();
				this.mediaType = MailBnfHelper.ReadToken(base.Value, ref num, null);
				if (num >= base.Value.Length || base.Value[num++] != '/')
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("MIME content type header is invalid.")));
				}
				this.subType = MailBnfHelper.ReadToken(base.Value, ref num, null);
				while (MailBnfHelper.SkipCFWS(base.Value, ref num))
				{
					if (num >= base.Value.Length || base.Value[num++] != ';')
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("MIME content type header is invalid.")));
					}
					if (!MailBnfHelper.SkipCFWS(base.Value, ref num))
					{
						break;
					}
					string text = MailBnfHelper.ReadParameterAttribute(base.Value, ref num, null);
					if (text == null || num >= base.Value.Length || base.Value[num++] != '=')
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("MIME content type header is invalid.")));
					}
					string text2 = MailBnfHelper.ReadParameterValue(base.Value, ref num, null);
					this.parameters.Add(text.ToLowerInvariant(), text2);
				}
				if (this.parameters.ContainsKey(MtomGlobals.StartInfoParam))
				{
					string text3 = this.parameters[MtomGlobals.StartInfoParam];
					int num2 = text3.IndexOf(';');
					if (num2 > -1)
					{
						while (MailBnfHelper.SkipCFWS(text3, ref num2))
						{
							if (text3[num2] != ';')
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("MIME content type header is invalid.")));
							}
							num2++;
							string text4 = MailBnfHelper.ReadParameterAttribute(text3, ref num2, null);
							if (text4 == null || num2 >= text3.Length || text3[num2++] != '=')
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("MIME content type header is invalid.")));
							}
							string text5 = MailBnfHelper.ReadParameterValue(text3, ref num2, null);
							if (text4 == MtomGlobals.ActionParam)
							{
								this.parameters[MtomGlobals.ActionParam] = text5;
							}
						}
					}
				}
			}
		}

		// Token: 0x0400021E RID: 542
		public static readonly ContentTypeHeader Default = new ContentTypeHeader("application/octet-stream");

		// Token: 0x0400021F RID: 543
		private string mediaType;

		// Token: 0x04000220 RID: 544
		private string subType;

		// Token: 0x04000221 RID: 545
		private Dictionary<string, string> parameters;
	}
}
