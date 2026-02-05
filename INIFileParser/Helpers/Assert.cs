using System;

namespace IniParser.Helpers
{
	// Token: 0x02000013 RID: 19
	internal static class Assert
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x000044B8 File Offset: 0x000026B8
		internal static bool StringHasNoBlankSpaces(string s)
		{
			return !s.Contains(" ");
		}
	}
}
