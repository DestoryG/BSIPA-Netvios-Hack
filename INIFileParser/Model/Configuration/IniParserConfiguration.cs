using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IniParser.Model.Configuration
{
	// Token: 0x0200000F RID: 15
	public class IniParserConfiguration : ICloneable
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00003C28 File Offset: 0x00001E28
		public IniParserConfiguration()
		{
			this.CommentString = ";";
			this.SectionStartChar = '[';
			this.SectionEndChar = ']';
			this.KeyValueAssigmentChar = '=';
			this.AssigmentSpacer = " ";
			this.NewLineStr = Environment.NewLine;
			this.ConcatenateDuplicateKeys = false;
			this.AllowKeysWithoutSection = true;
			this.AllowDuplicateKeys = false;
			this.AllowDuplicateSections = false;
			this.AllowCreateSectionsOnFly = true;
			this.ThrowExceptionsOnError = true;
			this.SkipInvalidLines = false;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003CB4 File Offset: 0x00001EB4
		public IniParserConfiguration(IniParserConfiguration ori)
		{
			this.AllowDuplicateKeys = ori.AllowDuplicateKeys;
			this.OverrideDuplicateKeys = ori.OverrideDuplicateKeys;
			this.AllowDuplicateSections = ori.AllowDuplicateSections;
			this.AllowKeysWithoutSection = ori.AllowKeysWithoutSection;
			this.AllowCreateSectionsOnFly = ori.AllowCreateSectionsOnFly;
			this.SectionStartChar = ori.SectionStartChar;
			this.SectionEndChar = ori.SectionEndChar;
			this.CommentString = ori.CommentString;
			this.ThrowExceptionsOnError = ori.ThrowExceptionsOnError;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003D3E File Offset: 0x00001F3E
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00003D46 File Offset: 0x00001F46
		public Regex CommentRegex { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003D4F File Offset: 0x00001F4F
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00003D57 File Offset: 0x00001F57
		public Regex SectionRegex { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003D60 File Offset: 0x00001F60
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00003D78 File Offset: 0x00001F78
		public char SectionStartChar
		{
			get
			{
				return this._sectionStartChar;
			}
			set
			{
				this._sectionStartChar = value;
				this.RecreateSectionRegex(this._sectionStartChar);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003D90 File Offset: 0x00001F90
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003DA8 File Offset: 0x00001FA8
		public char SectionEndChar
		{
			get
			{
				return this._sectionEndChar;
			}
			set
			{
				this._sectionEndChar = value;
				this.RecreateSectionRegex(this._sectionEndChar);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003DBF File Offset: 0x00001FBF
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003DC7 File Offset: 0x00001FC7
		public bool CaseInsensitive { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00003DD0 File Offset: 0x00001FD0
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00003DEE File Offset: 0x00001FEE
		[Obsolete("Please use the CommentString property")]
		public char CommentChar
		{
			get
			{
				return this.CommentString[0];
			}
			set
			{
				this.CommentString = value.ToString();
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003E00 File Offset: 0x00002000
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00003E24 File Offset: 0x00002024
		public string CommentString
		{
			get
			{
				return this._commentString ?? string.Empty;
			}
			set
			{
				foreach (char c in "[]\\^$.|?*+()")
				{
					value = value.Replace(new string(c, 1), "\\" + c.ToString());
				}
				this.CommentRegex = new Regex(string.Format("^{0}(.*)", value));
				this._commentString = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003E92 File Offset: 0x00002092
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00003E9A File Offset: 0x0000209A
		public string NewLineStr { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003EA3 File Offset: 0x000020A3
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003EAB File Offset: 0x000020AB
		public char KeyValueAssigmentChar { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003EB4 File Offset: 0x000020B4
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00003EBC File Offset: 0x000020BC
		public string AssigmentSpacer { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003EC5 File Offset: 0x000020C5
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00003ECD File Offset: 0x000020CD
		public bool AllowKeysWithoutSection { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003ED6 File Offset: 0x000020D6
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00003EDE File Offset: 0x000020DE
		public bool AllowDuplicateKeys { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003EE7 File Offset: 0x000020E7
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00003EEF File Offset: 0x000020EF
		public bool OverrideDuplicateKeys { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003EF8 File Offset: 0x000020F8
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00003F00 File Offset: 0x00002100
		public bool ConcatenateDuplicateKeys { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003F09 File Offset: 0x00002109
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00003F11 File Offset: 0x00002111
		public bool ThrowExceptionsOnError { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00003F1A File Offset: 0x0000211A
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00003F22 File Offset: 0x00002122
		public bool AllowDuplicateSections { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00003F2B File Offset: 0x0000212B
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00003F33 File Offset: 0x00002133
		public bool AllowCreateSectionsOnFly { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00003F3C File Offset: 0x0000213C
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00003F44 File Offset: 0x00002144
		public bool SkipInvalidLines { get; set; }

		// Token: 0x060000B4 RID: 180 RVA: 0x00003F50 File Offset: 0x00002150
		private void RecreateSectionRegex(char value)
		{
			bool flag = char.IsControl(value) || char.IsWhiteSpace(value) || this.CommentString.Contains(new string(new char[] { value })) || value == this.KeyValueAssigmentChar;
			if (flag)
			{
				throw new Exception(string.Format("Invalid character for section delimiter: '{0}", value));
			}
			string text = "^(\\s*?)";
			bool flag2 = "[]\\^$.|?*+()".Contains(new string(this._sectionStartChar, 1));
			if (flag2)
			{
				text = text + "\\" + this._sectionStartChar.ToString();
			}
			else
			{
				text += this._sectionStartChar.ToString();
			}
			text += "{1}\\s*[\\p{L}\\p{P}\\p{M}_\\\"\\'\\{\\}\\#\\+\\;\\*\\%\\(\\)\\=\\?\\&\\$\\,\\:\\/\\.\\-\\w\\d\\s\\\\\\~]+\\s*";
			bool flag3 = "[]\\^$.|?*+()".Contains(new string(this._sectionEndChar, 1));
			if (flag3)
			{
				text = text + "\\" + this._sectionEndChar.ToString();
			}
			else
			{
				text += this._sectionEndChar.ToString();
			}
			text += "(\\s*?)$";
			this.SectionRegex = new Regex(text);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004068 File Offset: 0x00002268
		public override int GetHashCode()
		{
			int num = 27;
			foreach (PropertyInfo propertyInfo in base.GetType().GetProperties())
			{
				num = num * 7 + propertyInfo.GetValue(this, null).GetHashCode();
			}
			return num;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000040B4 File Offset: 0x000022B4
		public override bool Equals(object obj)
		{
			IniParserConfiguration iniParserConfiguration = obj as IniParserConfiguration;
			bool flag = iniParserConfiguration == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				Type type = base.GetType();
				try
				{
					foreach (PropertyInfo propertyInfo in type.GetProperties())
					{
						bool flag3 = propertyInfo.GetValue(iniParserConfiguration, null).Equals(propertyInfo.GetValue(this, null));
						if (flag3)
						{
							return false;
						}
					}
				}
				catch
				{
					return false;
				}
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004144 File Offset: 0x00002344
		public IniParserConfiguration Clone()
		{
			return base.MemberwiseClone() as IniParserConfiguration;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004164 File Offset: 0x00002364
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x04000028 RID: 40
		private char _sectionStartChar;

		// Token: 0x04000029 RID: 41
		private char _sectionEndChar;

		// Token: 0x0400002A RID: 42
		private string _commentString;

		// Token: 0x0400002B RID: 43
		protected const string _strCommentRegex = "^{0}(.*)";

		// Token: 0x0400002C RID: 44
		protected const string _strSectionRegexStart = "^(\\s*?)";

		// Token: 0x0400002D RID: 45
		protected const string _strSectionRegexMiddle = "{1}\\s*[\\p{L}\\p{P}\\p{M}_\\\"\\'\\{\\}\\#\\+\\;\\*\\%\\(\\)\\=\\?\\&\\$\\,\\:\\/\\.\\-\\w\\d\\s\\\\\\~]+\\s*";

		// Token: 0x0400002E RID: 46
		protected const string _strSectionRegexEnd = "(\\s*?)$";

		// Token: 0x0400002F RID: 47
		protected const string _strKeyRegex = "^(\\s*[_\\.\\d\\w]*\\s*)";

		// Token: 0x04000030 RID: 48
		protected const string _strValueRegex = "([\\s\\d\\w\\W\\.]*)$";

		// Token: 0x04000031 RID: 49
		protected const string _strSpecialRegexChars = "[]\\^$.|?*+()";
	}
}
