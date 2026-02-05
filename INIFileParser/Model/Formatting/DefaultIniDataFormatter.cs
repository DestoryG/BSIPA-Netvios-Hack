using System;
using System.Collections.Generic;
using System.Text;
using IniParser.Model.Configuration;

namespace IniParser.Model.Formatting
{
	// Token: 0x02000011 RID: 17
	public class DefaultIniDataFormatter : IIniDataFormatter
	{
		// Token: 0x060000BE RID: 190 RVA: 0x000041CE File Offset: 0x000023CE
		public DefaultIniDataFormatter()
			: this(new IniParserConfiguration())
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000041E0 File Offset: 0x000023E0
		public DefaultIniDataFormatter(IniParserConfiguration configuration)
		{
			bool flag = configuration == null;
			if (flag)
			{
				throw new ArgumentNullException("configuration");
			}
			this.Configuration = configuration;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004210 File Offset: 0x00002410
		public virtual string IniDataToString(IniData iniData)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool allowKeysWithoutSection = this.Configuration.AllowKeysWithoutSection;
			if (allowKeysWithoutSection)
			{
				this.WriteKeyValueData(iniData.Global, stringBuilder);
			}
			foreach (SectionData sectionData in iniData.Sections)
			{
				this.WriteSection(sectionData, stringBuilder);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004298 File Offset: 0x00002498
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x000042B0 File Offset: 0x000024B0
		public IniParserConfiguration Configuration
		{
			get
			{
				return this._configuration;
			}
			set
			{
				this._configuration = value.Clone();
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000042C0 File Offset: 0x000024C0
		private void WriteSection(SectionData section, StringBuilder sb)
		{
			bool flag = sb.Length > 0;
			if (flag)
			{
				sb.Append(this.Configuration.NewLineStr);
			}
			this.WriteComments(section.LeadingComments, sb);
			sb.Append(string.Format("{0}{1}{2}{3}", new object[]
			{
				this.Configuration.SectionStartChar,
				section.SectionName,
				this.Configuration.SectionEndChar,
				this.Configuration.NewLineStr
			}));
			this.WriteKeyValueData(section.Keys, sb);
			this.WriteComments(section.TrailingComments, sb);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000436C File Offset: 0x0000256C
		private void WriteKeyValueData(KeyDataCollection keyDataCollection, StringBuilder sb)
		{
			foreach (KeyData keyData in keyDataCollection)
			{
				bool flag = keyData.Comments.Count > 0;
				if (flag)
				{
					sb.Append(this.Configuration.NewLineStr);
				}
				this.WriteComments(keyData.Comments, sb);
				sb.Append(string.Format("{0}{3}{1}{3}{2}{4}", new object[]
				{
					keyData.KeyName,
					this.Configuration.KeyValueAssigmentChar,
					keyData.Value,
					this.Configuration.AssigmentSpacer,
					this.Configuration.NewLineStr
				}));
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004444 File Offset: 0x00002644
		private void WriteComments(List<string> comments, StringBuilder sb)
		{
			foreach (string text in comments)
			{
				sb.Append(string.Format("{0}{1}{2}", this.Configuration.CommentString, text, this.Configuration.NewLineStr));
			}
		}

		// Token: 0x04000033 RID: 51
		private IniParserConfiguration _configuration;
	}
}
