using System;

namespace System.Net
{
	// Token: 0x020000D3 RID: 211
	internal class CookieTokenizer
	{
		// Token: 0x06000720 RID: 1824 RVA: 0x00027334 File Offset: 0x00025534
		internal CookieTokenizer(string tokenStream)
		{
			this.m_length = tokenStream.Length;
			this.m_tokenStream = tokenStream;
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0002734F File Offset: 0x0002554F
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x00027357 File Offset: 0x00025557
		internal bool EndOfCookie
		{
			get
			{
				return this.m_eofCookie;
			}
			set
			{
				this.m_eofCookie = value;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00027360 File Offset: 0x00025560
		internal bool Eof
		{
			get
			{
				return this.m_index >= this.m_length;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x00027373 File Offset: 0x00025573
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x0002737B File Offset: 0x0002557B
		internal string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x00027384 File Offset: 0x00025584
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0002738C File Offset: 0x0002558C
		internal bool Quoted
		{
			get
			{
				return this.m_quoted;
			}
			set
			{
				this.m_quoted = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x00027395 File Offset: 0x00025595
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0002739D File Offset: 0x0002559D
		internal CookieToken Token
		{
			get
			{
				return this.m_token;
			}
			set
			{
				this.m_token = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x000273A6 File Offset: 0x000255A6
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x000273AE File Offset: 0x000255AE
		internal string Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x000273B8 File Offset: 0x000255B8
		internal string Extract()
		{
			string text = string.Empty;
			if (this.m_tokenLength != 0)
			{
				text = this.m_tokenStream.Substring(this.m_start, this.m_tokenLength);
				if (!this.Quoted)
				{
					text = text.Trim();
				}
			}
			return text;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000273FC File Offset: 0x000255FC
		internal CookieToken FindNext(bool ignoreComma, bool ignoreEquals)
		{
			this.m_tokenLength = 0;
			this.m_start = this.m_index;
			while (this.m_index < this.m_length && char.IsWhiteSpace(this.m_tokenStream[this.m_index]))
			{
				this.m_index++;
				this.m_start++;
			}
			CookieToken cookieToken = CookieToken.End;
			int num = 1;
			if (!this.Eof)
			{
				if (this.m_tokenStream[this.m_index] == '"')
				{
					this.Quoted = true;
					this.m_index++;
					bool flag = false;
					while (this.m_index < this.m_length)
					{
						char c = this.m_tokenStream[this.m_index];
						if (!flag && c == '"')
						{
							break;
						}
						if (flag)
						{
							flag = false;
						}
						else if (c == '\\')
						{
							flag = true;
						}
						this.m_index++;
					}
					if (this.m_index < this.m_length)
					{
						this.m_index++;
					}
					this.m_tokenLength = this.m_index - this.m_start;
					num = 0;
					ignoreComma = false;
				}
				while (this.m_index < this.m_length && this.m_tokenStream[this.m_index] != ';' && (ignoreEquals || this.m_tokenStream[this.m_index] != '=') && (ignoreComma || this.m_tokenStream[this.m_index] != ','))
				{
					if (this.m_tokenStream[this.m_index] == ',')
					{
						this.m_start = this.m_index + 1;
						this.m_tokenLength = -1;
						ignoreComma = false;
					}
					this.m_index++;
					this.m_tokenLength += num;
				}
				if (!this.Eof)
				{
					char c2 = this.m_tokenStream[this.m_index];
					if (c2 != ';')
					{
						if (c2 != '=')
						{
							cookieToken = CookieToken.EndCookie;
						}
						else
						{
							cookieToken = CookieToken.Equals;
						}
					}
					else
					{
						cookieToken = CookieToken.EndToken;
					}
					this.m_index++;
				}
			}
			return cookieToken;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00027600 File Offset: 0x00025800
		internal CookieToken Next(bool first, bool parseResponseCookies)
		{
			this.Reset();
			CookieToken cookieToken = this.FindNext(false, false);
			if (cookieToken == CookieToken.EndCookie)
			{
				this.EndOfCookie = true;
			}
			if (cookieToken == CookieToken.End || cookieToken == CookieToken.EndCookie)
			{
				if ((this.Name = this.Extract()).Length != 0)
				{
					this.Token = this.TokenFromName(parseResponseCookies);
					return CookieToken.Attribute;
				}
				return cookieToken;
			}
			else
			{
				this.Name = this.Extract();
				if (first)
				{
					this.Token = CookieToken.CookieName;
				}
				else
				{
					this.Token = this.TokenFromName(parseResponseCookies);
				}
				if (cookieToken == CookieToken.Equals)
				{
					cookieToken = this.FindNext(!first && this.Token == CookieToken.Expires, true);
					if (cookieToken == CookieToken.EndCookie)
					{
						this.EndOfCookie = true;
					}
					this.Value = this.Extract();
					return CookieToken.NameValuePair;
				}
				return CookieToken.Attribute;
			}
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x000276B2 File Offset: 0x000258B2
		internal void Reset()
		{
			this.m_eofCookie = false;
			this.m_name = string.Empty;
			this.m_quoted = false;
			this.m_start = this.m_index;
			this.m_token = CookieToken.Nothing;
			this.m_tokenLength = 0;
			this.m_value = string.Empty;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x000276F4 File Offset: 0x000258F4
		internal CookieToken TokenFromName(bool parseResponseCookies)
		{
			if (!parseResponseCookies)
			{
				for (int i = 0; i < CookieTokenizer.RecognizedServerAttributes.Length; i++)
				{
					if (CookieTokenizer.RecognizedServerAttributes[i].IsEqualTo(this.Name))
					{
						return CookieTokenizer.RecognizedServerAttributes[i].Token;
					}
				}
			}
			else
			{
				for (int j = 0; j < CookieTokenizer.RecognizedAttributes.Length; j++)
				{
					if (CookieTokenizer.RecognizedAttributes[j].IsEqualTo(this.Name))
					{
						return CookieTokenizer.RecognizedAttributes[j].Token;
					}
				}
			}
			return CookieToken.Unknown;
		}

		// Token: 0x04000CFA RID: 3322
		private bool m_eofCookie;

		// Token: 0x04000CFB RID: 3323
		private int m_index;

		// Token: 0x04000CFC RID: 3324
		private int m_length;

		// Token: 0x04000CFD RID: 3325
		private string m_name;

		// Token: 0x04000CFE RID: 3326
		private bool m_quoted;

		// Token: 0x04000CFF RID: 3327
		private int m_start;

		// Token: 0x04000D00 RID: 3328
		private CookieToken m_token;

		// Token: 0x04000D01 RID: 3329
		private int m_tokenLength;

		// Token: 0x04000D02 RID: 3330
		private string m_tokenStream;

		// Token: 0x04000D03 RID: 3331
		private string m_value;

		// Token: 0x04000D04 RID: 3332
		private static CookieTokenizer.RecognizedAttribute[] RecognizedAttributes = new CookieTokenizer.RecognizedAttribute[]
		{
			new CookieTokenizer.RecognizedAttribute("Path", CookieToken.Path),
			new CookieTokenizer.RecognizedAttribute("Max-Age", CookieToken.MaxAge),
			new CookieTokenizer.RecognizedAttribute("Expires", CookieToken.Expires),
			new CookieTokenizer.RecognizedAttribute("Version", CookieToken.Version),
			new CookieTokenizer.RecognizedAttribute("Domain", CookieToken.Domain),
			new CookieTokenizer.RecognizedAttribute("Secure", CookieToken.Secure),
			new CookieTokenizer.RecognizedAttribute("Discard", CookieToken.Discard),
			new CookieTokenizer.RecognizedAttribute("Port", CookieToken.Port),
			new CookieTokenizer.RecognizedAttribute("Comment", CookieToken.Comment),
			new CookieTokenizer.RecognizedAttribute("CommentURL", CookieToken.CommentUrl),
			new CookieTokenizer.RecognizedAttribute("HttpOnly", CookieToken.HttpOnly)
		};

		// Token: 0x04000D05 RID: 3333
		private static CookieTokenizer.RecognizedAttribute[] RecognizedServerAttributes = new CookieTokenizer.RecognizedAttribute[]
		{
			new CookieTokenizer.RecognizedAttribute("$Path", CookieToken.Path),
			new CookieTokenizer.RecognizedAttribute("$Version", CookieToken.Version),
			new CookieTokenizer.RecognizedAttribute("$Domain", CookieToken.Domain),
			new CookieTokenizer.RecognizedAttribute("$Port", CookieToken.Port),
			new CookieTokenizer.RecognizedAttribute("$HttpOnly", CookieToken.HttpOnly)
		};

		// Token: 0x020006F2 RID: 1778
		private struct RecognizedAttribute
		{
			// Token: 0x0600406C RID: 16492 RVA: 0x0010E453 File Offset: 0x0010C653
			internal RecognizedAttribute(string name, CookieToken token)
			{
				this.m_name = name;
				this.m_token = token;
			}

			// Token: 0x17000EE4 RID: 3812
			// (get) Token: 0x0600406D RID: 16493 RVA: 0x0010E463 File Offset: 0x0010C663
			internal CookieToken Token
			{
				get
				{
					return this.m_token;
				}
			}

			// Token: 0x0600406E RID: 16494 RVA: 0x0010E46B File Offset: 0x0010C66B
			internal bool IsEqualTo(string value)
			{
				return string.Compare(this.m_name, value, StringComparison.OrdinalIgnoreCase) == 0;
			}

			// Token: 0x0400308A RID: 12426
			private string m_name;

			// Token: 0x0400308B RID: 12427
			private CookieToken m_token;
		}
	}
}
