using System;
using System.IO;
using System.Text;
using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;

namespace BS_Utils.Utilities
{
	// Token: 0x02000005 RID: 5
	public class IniFile
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003A1E File Offset: 0x00001C1E
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00003A26 File Offset: 0x00001C26
		public string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				Directory.CreateDirectory(global::System.IO.Path.GetDirectoryName(value));
				if (!File.Exists(value))
				{
					File.WriteAllText(value, "", Encoding.Unicode);
				}
				this._path = value;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003A54 File Offset: 0x00001C54
		public IniFile(string INIPath)
		{
			this.Path = INIPath;
			this.config.AllowCreateSectionsOnFly = true;
			this.config.AllowDuplicateKeys = true;
			this.config.AllowDuplicateSections = true;
			this.config.OverrideDuplicateKeys = true;
			this.config.SkipInvalidLines = true;
			this.config.ThrowExceptionsOnError = true;
			this.config.AllowKeysWithoutSection = true;
			this.dataParser = new IniDataParser(this.config);
			this.parser = new FileIniDataParser(this.dataParser);
			this.data = this.parser.ReadFile(this.Path);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003B14 File Offset: 0x00001D14
		public void IniWriteValue(string Section, string Key, string Value)
		{
			try
			{
				this.data[Section][Key] = Value;
				this.parser.WriteFile(this.Path, this.data, null);
			}
			catch
			{
				Logger.Log("IniWriteValue doesnt want to write the stuffs");
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003B6C File Offset: 0x00001D6C
		public string IniReadValue(string Section, string Key)
		{
			string text;
			try
			{
				this.data = this.parser.ReadFile(this.Path);
				if (!this.data[Section].ContainsKey(Key))
				{
					text = "";
				}
				else
				{
					string value = this.data[Section].GetKeyData(Key).Value;
					text = value;
				}
			}
			catch
			{
				Logger.Log("IniReadValue doesn't want to read the stuffs");
				text = "";
			}
			return text;
		}

		// Token: 0x0400002B RID: 43
		private string _path = "";

		// Token: 0x0400002C RID: 44
		internal IniParserConfiguration config = new IniParserConfiguration();

		// Token: 0x0400002D RID: 45
		internal FileIniDataParser parser;

		// Token: 0x0400002E RID: 46
		internal IniDataParser dataParser;

		// Token: 0x0400002F RID: 47
		internal IniData data;
	}
}
