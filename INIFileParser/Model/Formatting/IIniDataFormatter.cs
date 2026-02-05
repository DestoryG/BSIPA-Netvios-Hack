using System;
using IniParser.Model.Configuration;

namespace IniParser.Model.Formatting
{
	// Token: 0x02000012 RID: 18
	public interface IIniDataFormatter
	{
		// Token: 0x060000C6 RID: 198
		string IniDataToString(IniData iniData);

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C7 RID: 199
		// (set) Token: 0x060000C8 RID: 200
		IniParserConfiguration Configuration { get; set; }
	}
}
