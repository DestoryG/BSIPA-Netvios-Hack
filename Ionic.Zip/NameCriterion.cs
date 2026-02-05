using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000013 RID: 19
	internal class NameCriterion : SelectionCriterion
	{
		// Token: 0x17000042 RID: 66
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00003970 File Offset: 0x00001B70
		internal virtual string MatchingFileSpec
		{
			set
			{
				if (Directory.Exists(value))
				{
					this._MatchingFileSpec = ".\\" + value + "\\*.*";
				}
				else
				{
					this._MatchingFileSpec = value;
				}
				this._regexString = "^" + Regex.Escape(this._MatchingFileSpec).Replace("\\\\\\*\\.\\*", "\\\\([^\\.]+|.*\\.[^\\\\\\.]*)").Replace("\\.\\*", "\\.[^\\\\\\.]*")
					.Replace("\\*", ".*")
					.Replace("\\?", "[^\\\\\\.]") + "$";
				this._re = new Regex(this._regexString, RegexOptions.IgnoreCase);
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003A14 File Offset: 0x00001C14
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("name ").Append(EnumUtil.GetDescription(this.Operator)).Append(" '")
				.Append(this._MatchingFileSpec)
				.Append("'");
			return stringBuilder.ToString();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003A6D File Offset: 0x00001C6D
		internal override bool Evaluate(string filename)
		{
			return this._Evaluate(filename);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003A78 File Offset: 0x00001C78
		private bool _Evaluate(string fullpath)
		{
			string text = ((this._MatchingFileSpec.IndexOf('\\') == -1) ? Path.GetFileName(fullpath) : fullpath);
			bool flag = this._re.IsMatch(text);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003ABC File Offset: 0x00001CBC
		internal override bool Evaluate(ZipEntry entry)
		{
			string text = entry.FileName.Replace("/", "\\");
			return this._Evaluate(text);
		}

		// Token: 0x04000071 RID: 113
		private Regex _re;

		// Token: 0x04000072 RID: 114
		private string _regexString;

		// Token: 0x04000073 RID: 115
		internal ComparisonOperator Operator;

		// Token: 0x04000074 RID: 116
		private string _MatchingFileSpec;
	}
}
