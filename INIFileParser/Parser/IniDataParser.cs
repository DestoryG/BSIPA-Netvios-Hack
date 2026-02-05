using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IniParser.Exceptions;
using IniParser.Model;
using IniParser.Model.Configuration;

namespace IniParser.Parser
{
	// Token: 0x02000006 RID: 6
	public class IniDataParser
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002464 File Offset: 0x00000664
		public IniDataParser()
			: this(new IniParserConfiguration())
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002474 File Offset: 0x00000674
		public IniDataParser(IniParserConfiguration parserConfiguration)
		{
			bool flag = parserConfiguration == null;
			if (flag)
			{
				throw new ArgumentNullException("parserConfiguration");
			}
			this.Configuration = parserConfiguration;
			this._errorExceptions = new List<Exception>();
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000024BA File Offset: 0x000006BA
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000024C2 File Offset: 0x000006C2
		public virtual IniParserConfiguration Configuration { get; protected set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000024CC File Offset: 0x000006CC
		public bool HasError
		{
			get
			{
				return this._errorExceptions.Count > 0;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000024EC File Offset: 0x000006EC
		public ReadOnlyCollection<Exception> Errors
		{
			get
			{
				return this._errorExceptions.AsReadOnly();
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000250C File Offset: 0x0000070C
		public IniData Parse(string iniDataString)
		{
			IniData iniData = (this.Configuration.CaseInsensitive ? new IniDataCaseInsensitive() : new IniData());
			iniData.Configuration = this.Configuration.Clone();
			bool flag = string.IsNullOrEmpty(iniDataString);
			IniData iniData2;
			if (flag)
			{
				iniData2 = iniData;
			}
			else
			{
				this._errorExceptions.Clear();
				this._currentCommentListTemp.Clear();
				this._currentSectionNameTemp = null;
				try
				{
					string[] array = iniDataString.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						bool flag2 = text.Trim() == string.Empty;
						if (!flag2)
						{
							try
							{
								this.ProcessLine(text, iniData);
							}
							catch (Exception ex)
							{
								ParsingException ex2 = new ParsingException(ex.Message, i + 1, text, ex);
								bool throwExceptionsOnError = this.Configuration.ThrowExceptionsOnError;
								if (throwExceptionsOnError)
								{
									throw ex2;
								}
								this._errorExceptions.Add(ex2);
							}
						}
					}
					bool flag3 = this._currentCommentListTemp.Count > 0;
					if (flag3)
					{
						bool flag4 = iniData.Sections.Count > 0;
						if (flag4)
						{
							iniData.Sections.GetSectionData(this._currentSectionNameTemp).TrailingComments.AddRange(this._currentCommentListTemp);
						}
						else
						{
							bool flag5 = iniData.Global.Count > 0;
							if (flag5)
							{
								iniData.Global.GetLast().Comments.AddRange(this._currentCommentListTemp);
							}
						}
						this._currentCommentListTemp.Clear();
					}
				}
				catch (Exception ex3)
				{
					this._errorExceptions.Add(ex3);
					bool throwExceptionsOnError2 = this.Configuration.ThrowExceptionsOnError;
					if (throwExceptionsOnError2)
					{
						throw;
					}
				}
				bool hasError = this.HasError;
				if (hasError)
				{
					iniData2 = null;
				}
				else
				{
					iniData2 = (IniData)iniData.Clone();
				}
			}
			return iniData2;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002730 File Offset: 0x00000930
		protected virtual bool LineContainsAComment(string line)
		{
			return !string.IsNullOrEmpty(line) && this.Configuration.CommentRegex.Match(line).Success;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002764 File Offset: 0x00000964
		protected virtual bool LineMatchesASection(string line)
		{
			return !string.IsNullOrEmpty(line) && this.Configuration.SectionRegex.Match(line).Success;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002798 File Offset: 0x00000998
		protected virtual bool LineMatchesAKeyValuePair(string line)
		{
			return !string.IsNullOrEmpty(line) && line.Contains(this.Configuration.KeyValueAssigmentChar.ToString());
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000027D0 File Offset: 0x000009D0
		protected virtual string ExtractComment(string line)
		{
			string text = this.Configuration.CommentRegex.Match(line).Value.Trim();
			this._currentCommentListTemp.Add(text.Substring(1, text.Length - 1));
			return line.Replace(text, "").Trim();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000282C File Offset: 0x00000A2C
		protected virtual void ProcessLine(string currentLine, IniData currentIniData)
		{
			currentLine = currentLine.Trim();
			bool flag = this.LineContainsAComment(currentLine);
			if (flag)
			{
				currentLine = this.ExtractComment(currentLine);
			}
			bool flag2 = currentLine == string.Empty;
			if (!flag2)
			{
				bool flag3 = this.LineMatchesASection(currentLine);
				if (flag3)
				{
					this.ProcessSection(currentLine, currentIniData);
				}
				else
				{
					bool flag4 = this.LineMatchesAKeyValuePair(currentLine);
					if (flag4)
					{
						this.ProcessKeyValuePair(currentLine, currentIniData);
					}
					else
					{
						bool skipInvalidLines = this.Configuration.SkipInvalidLines;
						if (!skipInvalidLines)
						{
							throw new ParsingException("Unknown file format. Couldn't parse the line: '" + currentLine + "'.");
						}
					}
				}
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000028C0 File Offset: 0x00000AC0
		protected virtual void ProcessSection(string line, IniData currentIniData)
		{
			string text = this.Configuration.SectionRegex.Match(line).Value.Trim();
			text = text.Substring(1, text.Length - 2).Trim();
			bool flag = text == string.Empty;
			if (flag)
			{
				throw new ParsingException("Section name is empty");
			}
			this._currentSectionNameTemp = text;
			bool flag2 = currentIniData.Sections.ContainsSection(text);
			if (flag2)
			{
				bool allowDuplicateSections = this.Configuration.AllowDuplicateSections;
				if (!allowDuplicateSections)
				{
					throw new ParsingException(string.Format("Duplicate section with name '{0}' on line '{1}'", text, line));
				}
			}
			else
			{
				currentIniData.Sections.AddSection(text);
				currentIniData.Sections.GetSectionData(text).LeadingComments = this._currentCommentListTemp;
				this._currentCommentListTemp.Clear();
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002988 File Offset: 0x00000B88
		protected virtual void ProcessKeyValuePair(string line, IniData currentIniData)
		{
			string text = this.ExtractKey(line);
			bool flag = string.IsNullOrEmpty(text) && this.Configuration.SkipInvalidLines;
			if (!flag)
			{
				string text2 = this.ExtractValue(line);
				bool flag2 = string.IsNullOrEmpty(this._currentSectionNameTemp);
				if (flag2)
				{
					bool flag3 = !this.Configuration.AllowKeysWithoutSection;
					if (flag3)
					{
						throw new ParsingException("key value pairs must be enclosed in a section");
					}
					this.AddKeyToKeyValueCollection(text, text2, currentIniData.Global, "global");
				}
				else
				{
					SectionData sectionData = currentIniData.Sections.GetSectionData(this._currentSectionNameTemp);
					this.AddKeyToKeyValueCollection(text, text2, sectionData.Keys, this._currentSectionNameTemp);
				}
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002A34 File Offset: 0x00000C34
		protected virtual string ExtractKey(string s)
		{
			int num = s.IndexOf(this.Configuration.KeyValueAssigmentChar, 0);
			return s.Substring(0, num).Trim();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002A68 File Offset: 0x00000C68
		protected virtual string ExtractValue(string s)
		{
			int num = s.IndexOf(this.Configuration.KeyValueAssigmentChar, 0);
			return s.Substring(num + 1, s.Length - num - 1).Trim();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002AA8 File Offset: 0x00000CA8
		protected virtual void HandleDuplicatedKeyInCollection(string key, string value, KeyDataCollection keyDataCollection, string sectionName)
		{
			bool flag = !this.Configuration.AllowDuplicateKeys;
			if (flag)
			{
				throw new ParsingException(string.Format("Duplicated key '{0}' found in section '{1}", key, sectionName));
			}
			bool overrideDuplicateKeys = this.Configuration.OverrideDuplicateKeys;
			if (overrideDuplicateKeys)
			{
				keyDataCollection[key] = value;
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002AF8 File Offset: 0x00000CF8
		private void AddKeyToKeyValueCollection(string key, string value, KeyDataCollection keyDataCollection, string sectionName)
		{
			bool flag = keyDataCollection.ContainsKey(key);
			if (flag)
			{
				this.HandleDuplicatedKeyInCollection(key, value, keyDataCollection, sectionName);
			}
			else
			{
				keyDataCollection.AddKey(key, value);
			}
			keyDataCollection.GetKeyData(key).Comments = this._currentCommentListTemp;
			this._currentCommentListTemp.Clear();
		}

		// Token: 0x04000006 RID: 6
		private List<Exception> _errorExceptions;

		// Token: 0x04000008 RID: 8
		private readonly List<string> _currentCommentListTemp = new List<string>();

		// Token: 0x04000009 RID: 9
		private string _currentSectionNameTemp;
	}
}
