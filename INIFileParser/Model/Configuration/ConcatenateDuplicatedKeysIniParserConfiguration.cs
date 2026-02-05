using System;

namespace IniParser.Model.Configuration
{
	// Token: 0x02000010 RID: 16
	public class ConcatenateDuplicatedKeysIniParserConfiguration : IniParserConfiguration
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000417C File Offset: 0x0000237C
		public new bool AllowDuplicateKeys
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000418F File Offset: 0x0000238F
		public ConcatenateDuplicatedKeysIniParserConfiguration()
		{
			this.ConcatenateSeparator = ";";
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000041A5 File Offset: 0x000023A5
		public ConcatenateDuplicatedKeysIniParserConfiguration(ConcatenateDuplicatedKeysIniParserConfiguration ori)
			: base(ori)
		{
			this.ConcatenateSeparator = ori.ConcatenateSeparator;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000041BD File Offset: 0x000023BD
		// (set) Token: 0x060000BD RID: 189 RVA: 0x000041C5 File Offset: 0x000023C5
		public string ConcatenateSeparator { get; set; }
	}
}
