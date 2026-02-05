using System;
using IniParser.Model;
using IniParser.Parser;

namespace IniParser
{
	// Token: 0x02000004 RID: 4
	[Obsolete("Use class IniDataParser instead. See remarks comments in this class.")]
	public class StringIniParser
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002321 File Offset: 0x00000521
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002329 File Offset: 0x00000529
		public IniDataParser Parser { get; protected set; }

		// Token: 0x06000012 RID: 18 RVA: 0x00002332 File Offset: 0x00000532
		public StringIniParser()
			: this(new IniDataParser())
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002341 File Offset: 0x00000541
		public StringIniParser(IniDataParser parser)
		{
			this.Parser = parser;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002354 File Offset: 0x00000554
		public IniData ParseString(string dataStr)
		{
			return this.Parser.Parse(dataStr);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002374 File Offset: 0x00000574
		public string WriteString(IniData iniData)
		{
			return iniData.ToString();
		}
	}
}
