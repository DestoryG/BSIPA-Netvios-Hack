using System;
using System.Text.RegularExpressions;

namespace Google.Protobuf
{
	// Token: 0x02000012 RID: 18
	internal static class FrameworkPortability
	{
		// Token: 0x0400003C RID: 60
		internal static readonly RegexOptions CompiledRegexWhereAvailable = (Enum.IsDefined(typeof(RegexOptions), 8) ? RegexOptions.Compiled : RegexOptions.None);
	}
}
