using System;
using IniParser.Model;
using IniParser.Model.Configuration;

namespace IniParser.Parser
{
	// Token: 0x02000007 RID: 7
	public class ConcatenateDuplicatedKeysIniDataParser : IniDataParser
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002B4C File Offset: 0x00000D4C
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002B69 File Offset: 0x00000D69
		public new ConcatenateDuplicatedKeysIniParserConfiguration Configuration
		{
			get
			{
				return (ConcatenateDuplicatedKeysIniParserConfiguration)base.Configuration;
			}
			set
			{
				base.Configuration = value;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002B74 File Offset: 0x00000D74
		public ConcatenateDuplicatedKeysIniDataParser()
			: this(new ConcatenateDuplicatedKeysIniParserConfiguration())
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B83 File Offset: 0x00000D83
		public ConcatenateDuplicatedKeysIniDataParser(ConcatenateDuplicatedKeysIniParserConfiguration parserConfiguration)
			: base(parserConfiguration)
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002B90 File Offset: 0x00000D90
		protected override void HandleDuplicatedKeyInCollection(string key, string value, KeyDataCollection keyDataCollection, string sectionName)
		{
			keyDataCollection[key] = keyDataCollection[key] + this.Configuration.ConcatenateSeparator + value;
		}
	}
}
